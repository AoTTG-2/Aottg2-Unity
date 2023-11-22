using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UI;
using System.Collections;
using Utility;
using ApplicationManagers;
using GameManagers;
using CustomSkins;
using Events;
using Map;
using Photon.Realtime;
using Photon.Pun;

namespace Weather
{
    class WeatherManager : MonoBehaviourPunCallbacks
    {
        // consts
        static WeatherManager _instance;
        const float LerpDelay = 0.05f;
        const float SyncDelay = 5f;
        HashSet<WeatherEffect> LowEffects = new HashSet<WeatherEffect> { WeatherEffect.Daylight, WeatherEffect.AmbientLight,
            WeatherEffect.Flashlight, WeatherEffect.Skybox, WeatherEffect.DaylightIntensity };
        static Dictionary<string, Material> SkyboxMaterials = new Dictionary<string, Material>();
        static Dictionary<string, Dictionary<string, Material>> SkyboxBlendedMaterials = new Dictionary<string, Dictionary<string, Material>>();
        static Shader _blendedShader;

        // instance
        List<WeatherScheduleRunner> _scheduleRunners = new List<WeatherScheduleRunner>();
        List<Camera> _skyboxCameras = new List<Camera>();
        Dictionary<WeatherEffect, BaseWeatherEffect> _effects = new Dictionary<WeatherEffect, BaseWeatherEffect>();
        public WeatherSet _currentWeather = new WeatherSet();
        public WeatherSet _targetWeather = new WeatherSet();
        public WeatherSet _startWeather = new WeatherSet();
        public Dictionary<int, float> _targetWeatherStartTimes = new Dictionary<int, float>();
        public Dictionary<int, float> _targetWeatherEndTimes = new Dictionary<int, float>();
        List<WeatherEffect> _needApply = new List<WeatherEffect>();
        public float _currentTime;
        public bool _needSync;
        public Dictionary<WeatherScheduleRunner, float> _currentScheduleWait = new Dictionary<WeatherScheduleRunner, float>();
        float _currentLerpWait;
        float _currentSyncWait;
        bool _finishedLoading;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            ThunderWeatherEffect.OnFinishInit();
            LoadSkyboxes();
            EventManager.OnPreLoadScene += OnPreLoadScene;
        }

        public static void OnPreLoadScene(SceneName sceneName)
        {
            WindWeatherEffect.WindEnabled = false;
            foreach (List<LightningParticle> list in ThunderWeatherEffect.LightningPool)
            {
                foreach (LightningParticle particle in list)
                    particle.Disable();
            }
            _instance._finishedLoading = false;
        }

        public static void OnFinishLoading()
        {
            _instance.RestartWeather();
        }

        private static void LoadSkyboxes()
        {
            _blendedShader = Shader.Find("Skybox/Blended");
            string[] skyboxNames = Util.EnumToStringArray<WeatherSkybox>();
            string[] parts = Util.EnumToStringArray<SkyboxCustomSkinPartId>();
            foreach (string skyboxName in skyboxNames)
                SkyboxMaterials.Add(skyboxName, ResourceManager.InstantiateAsset<Material>(ResourcePaths.Weather, 
                    "Skyboxes/" + skyboxName.ToString()  + "/" + skyboxName.ToString() + "Skybox"));
            foreach (string skybox1 in skyboxNames)
            {
                SkyboxBlendedMaterials.Add(skybox1, new Dictionary<string, Material>());
                foreach (string skybox2 in skyboxNames)
                {
                    Material blend = CreateBlendedSkybox(_blendedShader, parts, skybox1, skybox2);
                    SkyboxBlendedMaterials[skybox1].Add(skybox2, blend);
                }
            }
        }

        public static void TakeFlashlight(Transform parent)
        {
            if (_instance._effects.ContainsKey(WeatherEffect.Flashlight) && _instance._effects[WeatherEffect.Flashlight] != null)
                _instance._effects[WeatherEffect.Flashlight].SetParent(parent);
        }

        private static Material CreateBlendedSkybox(Shader shader, string[] parts, string skybox1, string skybox2)
        {
            Material blend = new Material(shader);
            foreach (string part in parts)
            {
                string texName = "_" + part + "Tex";
                blend.SetTexture(texName, SkyboxMaterials[skybox1].GetTexture(texName));
                blend.SetTexture(texName + "2", SkyboxMaterials[skybox2].GetTexture(texName));
            }
            SetSkyboxBlend(blend, 0f);
            return blend;
        }

        private static void SetSkyboxBlend(Material skybox, float blend)
        {
            skybox.SetFloat("_Blend", blend);
        }

        private void Cache()
        {
            ResetCameras();   
        }

        private void ResetCameras()
        {
            _skyboxCameras.Clear();
            _instance._skyboxCameras.Add(SceneLoader.CurrentCamera.Camera);
            if (SceneLoader.SceneName == SceneName.InGame)
            {
                if (MinimapHandler.CameraTransform != null && MinimapHandler.CameraTransform.gameObject.activeSelf)
                    _instance._skyboxCameras.Add(MinimapHandler.CameraTransform.GetComponent<Camera>());
            }
        }

        private void ResetSkyboxColors()
        {
            foreach (string skybox1 in SkyboxBlendedMaterials.Keys)
            {
                foreach (string skybox2 in SkyboxBlendedMaterials[skybox1].Keys)
                {
                    SkyboxBlendedMaterials[skybox1][skybox2].SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
                }
            }
        }

        private void RestartWeather()
        {
            Cache();
            ResetSkyboxColors();
            _scheduleRunners.Clear();
            _effects.Clear();
            _currentWeather.SetDefault();
            _startWeather.SetDefault();
            _targetWeather.SetDefault();
            _targetWeatherStartTimes.Clear();
            _targetWeatherEndTimes.Clear();
            _needApply.Clear();
            _currentTime = 0f;
            _currentScheduleWait.Clear();
            CreateEffects();
            SetSceneWeather();
            ApplyCurrentWeather(firstStart: true, applyAll: true);
            if (SceneLoader.SceneName == SceneName.InGame && PhotonNetwork.IsMasterClient)
            {
                _currentWeather.Copy(SettingsManager.WeatherSettings.WeatherSets.Sets.GetItemAt(SettingsManager.InGameCurrent.WeatherIndex.Value));
                CreateScheduleRunners(_currentWeather.Schedule.Value);
                _currentWeather.Schedule.SetDefault();
                if (_currentWeather.UseSchedule.Value)
                {
                    foreach (WeatherScheduleRunner runner in _scheduleRunners)
                    {
                        runner.ProcessSchedule();
                        runner.ConsumeSchedule();
                    }
                }
                SyncWeather();
                _currentSyncWait = SyncDelay;
                _needSync = false;
            }
            _currentLerpWait = LerpDelay;
            _finishedLoading = true;
        }

        private void SetSceneWeather()
        {
            if (SceneLoader.SceneName == SceneName.CharacterEditor || SceneLoader.SceneName == SceneName.MapEditor)
                _currentWeather.AmbientLight.Value = new Color255(255, 255, 255);
        }

        private void CreateScheduleRunners(string schedule)
        {
            WeatherScheduleRunner runner = new WeatherScheduleRunner(this);
            WeatherSchedule mainSchedule = new WeatherSchedule(schedule);
            foreach (WeatherEvent ev in mainSchedule.Events)
            {
                if (ev.Action == WeatherAction.BeginSchedule)
                {
                    runner = new WeatherScheduleRunner(this);
                    _scheduleRunners.Add(runner);
                    _currentScheduleWait.Add(runner, 0f);
                }
                runner.Schedule.Events.Add(ev);
            }
        }

        private void CreateEffects()
        {
            _effects.Add(WeatherEffect.Rain, ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Weather, "Prefabs/RainEffect").AddComponent<RainWeatherEffect>());
            _effects.Add(WeatherEffect.Snow, ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Weather, "Prefabs/SnowEffect").AddComponent<SnowWeatherEffect>());
            _effects.Add(WeatherEffect.Wind, ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Weather, "Prefabs/WindEffect").AddComponent<WindWeatherEffect>());
            _effects.Add(WeatherEffect.Thunder, ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Weather, "Prefabs/ThunderEffect").AddComponent<ThunderWeatherEffect>());
            foreach (BaseWeatherEffect effect in _effects.Values)
            {
                effect.Setup(SceneLoader.CurrentCamera.Cache.Transform);
                effect.Randomize();
                effect.Disable(fadeOut: false);
            }
            CreateFlashlight();
        }

        private void CreateFlashlight()
        {
            _effects.Add(WeatherEffect.Flashlight, ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Weather, "Prefabs/FlashlightEffect").AddComponent<FlashlightWeatherEffect>());
            _effects[WeatherEffect.Flashlight].Setup(null);
            _effects[WeatherEffect.Flashlight].Disable(fadeOut: false);
            TakeFlashlight(SceneLoader.CurrentCamera.Cache.Transform);
        }

        private void FixedUpdate()
        {
            if (SceneLoader.SceneName != SceneName.InGame || !_finishedLoading)
                return;
            _currentTime += Time.fixedDeltaTime;
            if (_targetWeatherStartTimes.Count > 0)
            {
                _currentLerpWait -= Time.fixedDeltaTime;
                if (_currentLerpWait <= 0f)
                {
                    LerpCurrentWeatherToTarget();
                    ApplyCurrentWeather(firstStart: false, applyAll: false);
                    _currentLerpWait = LerpDelay;
                }
            }
            if (PhotonNetwork.IsMasterClient)
            {
                if (_currentWeather.UseSchedule.Value)
                {
                    foreach (WeatherScheduleRunner key in new List<WeatherScheduleRunner>(_currentScheduleWait.Keys))
                    {
                        _currentScheduleWait[key] -= Time.fixedDeltaTime;
                        if (_currentScheduleWait[key] <= 0f)
                            key.ConsumeSchedule();
                    }
                    _currentSyncWait -= Time.fixedDeltaTime;
                    if (_currentSyncWait <= 0f && _needSync)
                    {
                        LerpCurrentWeatherToTarget();
                        SyncWeather();
                        _needSync = false;
                        _currentSyncWait = SyncDelay;
                    }
                }
            }
        }

        private void SyncWeather()
        {
            ApplyCurrentWeather(firstStart: false, applyAll: true);
            if (PhotonNetwork.IsMasterClient)
            {
                RPCManager.PhotonView.RPC("SetWeatherRPC", RpcTarget.Others, new object[]{ StringCompression.Compress(_currentWeather.SerializeToJsonString()),
                    StringCompression.Compress(_startWeather.SerializeToJsonString()), StringCompression.Compress(_targetWeather.SerializeToJsonString()), 
                    _targetWeatherStartTimes, _targetWeatherEndTimes, _currentTime });
            }
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                RPCManager.PhotonView.RPC("SetWeatherRPC", player, new object[]{ StringCompression.Compress(_currentWeather.SerializeToJsonString()),
                    StringCompression.Compress(_startWeather.SerializeToJsonString()), StringCompression.Compress(_targetWeather.SerializeToJsonString()),
                    _targetWeatherStartTimes, _targetWeatherEndTimes, _currentTime });
            }
        }

        private void LerpCurrentWeatherToTarget()
        {
            List<int> remove = new List<int>();
            foreach (KeyValuePair<int, float> entry in _targetWeatherEndTimes)
            {
                float lerp;
                if (entry.Value <= _currentTime)
                {
                    remove.Add(entry.Key);
                    lerp = 1f;
                }
                else
                {
                    float startTime = _targetWeatherStartTimes[entry.Key];
                    float endTime = entry.Value;
                    lerp = (_currentTime - startTime) / Mathf.Max(endTime - startTime, 1f);
                    lerp = Mathf.Clamp(lerp, 0f, 1f);
                }
                string effectName = ((WeatherEffect)entry.Key).ToString();
                BaseSetting startSetting = (BaseSetting)_startWeather.Settings[effectName];
                BaseSetting currentSetting = (BaseSetting)_currentWeather.Settings[effectName];
                BaseSetting targetSetting = (BaseSetting)_targetWeather.Settings[effectName];
                switch ((WeatherEffect)entry.Key)
                {
                    case WeatherEffect.Daylight:
                    case WeatherEffect.AmbientLight:
                    case WeatherEffect.FogColor:
                    case WeatherEffect.Flashlight:
                    case WeatherEffect.SkyboxColor:
                        ((ColorSetting)currentSetting).Value = Color255.Lerp(((ColorSetting)startSetting).Value, ((ColorSetting)targetSetting).Value, lerp);
                        break;
                    case WeatherEffect.FogDensity:
                    case WeatherEffect.Rain:
                    case WeatherEffect.Thunder:
                    case WeatherEffect.Snow:
                    case WeatherEffect.Wind:
                    case WeatherEffect.DaylightIntensity:
                        ((FloatSetting)currentSetting).Value = Mathf.Lerp(((FloatSetting)startSetting).Value, ((FloatSetting)targetSetting).Value, lerp);
                        break;
                    case WeatherEffect.Skybox:
                        Material mat = GetBlendedSkybox(_currentWeather.Skybox.Value, _targetWeather.Skybox.Value);
                        if (mat != null)
                        {
                            if (lerp >= 1f)
                            {
                                ((StringSetting)currentSetting).Value = ((StringSetting)targetSetting).Value;
                            }
                            SetSkyboxBlend(mat, lerp);
                        }
                        break;
                }
                _needApply.Add((WeatherEffect)entry.Key);
            }
            foreach (int r in remove)
            {
                _targetWeatherStartTimes.Remove(r);
                _targetWeatherEndTimes.Remove(r);
            }
        }

        private void ApplyCurrentWeather(bool firstStart, bool applyAll)
        {
            if (!firstStart && !IsWeatherEnabled())
                return;
            if (applyAll)
                _needApply = Util.EnumToList<WeatherEffect>();
            WeatherEffectLevel level = (WeatherEffectLevel)SettingsManager.GraphicsSettings.WeatherEffects.Value;
            foreach (WeatherEffect effect in _needApply)
            {
                if (!firstStart && level == WeatherEffectLevel.Low && !LowEffects.Contains(effect))
                    continue;
                switch (effect)
                {
                    case WeatherEffect.Daylight:
                        foreach (Light light in MapLoader.Daylight)
                        {
                            light.color = _currentWeather.Daylight.Value.ToColor();
                        }
                        break;
                    case WeatherEffect.DaylightIntensity:
                        foreach (Light light in MapLoader.Daylight)
                            light.intensity = _currentWeather.DaylightIntensity.Value;
                        break;
                    case WeatherEffect.AmbientLight:
                        RenderSettings.ambientLight = _currentWeather.AmbientLight.Value.ToColor();
                        break;
                    case WeatherEffect.FogColor:
                        RenderSettings.fogColor = _currentWeather.FogColor.Value.ToColor();
                        break;
                    case WeatherEffect.FogDensity:
                        if (_currentWeather.FogDensity.Value > 0f)
                        {
                            RenderSettings.fog = true;
                            RenderSettings.fogMode = FogMode.Exponential;
                            RenderSettings.fogDensity = _currentWeather.FogDensity.Value * 0.02f;
                        }
                        else
                        {
                            RenderSettings.fog = false;
                        }
                        break;
                    case WeatherEffect.Flashlight:
                        ((FlashlightWeatherEffect)_effects[WeatherEffect.Flashlight]).SetColor(_currentWeather.Flashlight.Value.ToColor());
                        if (_currentWeather.Flashlight.Value.A > 0 && _currentWeather.Flashlight.Value.ToColor() != Color.black)
                        {
                            if (!_effects[WeatherEffect.Flashlight].gameObject.activeSelf)
                                _effects[WeatherEffect.Flashlight].Enable();
                        }
                        else
                            _effects[WeatherEffect.Flashlight].Disable();
                        break;
                    case WeatherEffect.Skybox:
                        StartCoroutine(WaitAndApplySkybox());
                        break;
                    case WeatherEffect.SkyboxColor:
                        Material mat = GetBlendedSkybox(_currentWeather.Skybox.Value, _targetWeather.Skybox.Value);
                        if (mat != null)
                        {
                            mat.SetColor("_Tint", _currentWeather.SkyboxColor.Value.ToColor());
                        }
                        break;
                    case WeatherEffect.Rain:
                    case WeatherEffect.Snow:
                    case WeatherEffect.Wind:
                    case WeatherEffect.Thunder:
                        float value = ((FloatSetting)_currentWeather.Settings[effect.ToString()]).Value;
                        _effects[effect].SetLevel(value);
                        if (value > 0f)
                        {
                            if (!_effects[effect].gameObject.activeSelf)
                            {
                                _effects[effect].Randomize();
                                _effects[effect].Enable();
                            }
                        }
                        else
                            _effects[effect].Disable(fadeOut: true);
                        break;
                }
            }
            _needApply.Clear();
        }

        private IEnumerator WaitAndApplySkybox()
        {
            yield return new WaitForEndOfFrame();
            Material mat;
            if (!IsWeatherEnabled())
                mat = GetBlendedSkybox("Day", "Day");
            else
                mat = GetBlendedSkybox(_currentWeather.Skybox.Value, _targetWeather.Skybox.Value);
            var skybox = SceneLoader.CurrentCamera.Skybox;
            if (mat != null && skybox.material != mat && SkyboxCustomSkinLoader.SkyboxMaterial == null)
            {
                mat.SetColor("_Tint", _currentWeather.SkyboxColor.Value.ToColor());
                foreach (var camera in _skyboxCameras)
                    camera.gameObject.GetComponent<Skybox>().material = mat;
            }
        }

        private Material GetBlendedSkybox(string skybox1, string skybox2)
        {
            if (SkyboxBlendedMaterials.ContainsKey(skybox1))
            {
                if (SkyboxBlendedMaterials[skybox1].ContainsKey(skybox2))
                    return SkyboxBlendedMaterials[skybox1][skybox2];
            }
            return null;
        }

        public static void OnSetWeatherRPC(byte[] currentWeatherJson, byte[] startWeatherJson, byte[] targetWeatherJson, 
            Dictionary<int, float> targetWeatherStartTimes, Dictionary<int, float> targetWeatherEndTimes, float currentTime, PhotonMessageInfo info)
        {
            if (info.Sender != PhotonNetwork.MasterClient)
                return;
            if (SettingsManager.GraphicsSettings.WeatherEffects.Value == (int)WeatherEffectLevel.Off)
                return;
            _instance.StartCoroutine(_instance.WaitAndFinishOnSetWeather(currentWeatherJson, startWeatherJson, targetWeatherJson, targetWeatherStartTimes,
                targetWeatherEndTimes, currentTime));
        }

        private IEnumerator WaitAndFinishOnSetWeather(byte[] currentWeatherJson, byte[] startWeatherJson, byte[] targetWeatherJson, 
            Dictionary<int, float> targetWeatherStartTimes, Dictionary<int, float> targetWeatherEndTimes, float currentTime)
        {
            while (!_finishedLoading)
                yield return null;
            _currentWeather.DeserializeFromJsonString(StringCompression.Decompress(currentWeatherJson));
            _startWeather.DeserializeFromJsonString(StringCompression.Decompress(startWeatherJson));
            _targetWeather.DeserializeFromJsonString(StringCompression.Decompress(targetWeatherJson));
            _targetWeatherStartTimes = targetWeatherStartTimes;
            _targetWeatherEndTimes = targetWeatherEndTimes;
            _currentTime = currentTime;
            LerpCurrentWeatherToTarget();
            ApplyCurrentWeather(firstStart: false, applyAll: true);
        }

        private bool IsWeatherEnabled()
        {
            return SettingsManager.GraphicsSettings.WeatherEffects.Value != (int)WeatherEffectLevel.Off;
        }
    }
}
