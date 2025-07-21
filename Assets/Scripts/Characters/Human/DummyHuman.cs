using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomSkins;
using Settings;
using UI;
using ApplicationManagers;
using Weather;
using GameProgress;
using GameManagers;
using Controllers;

namespace Characters
{
    class DummyHuman: DummyCharacter
    {
        public HumanComponentCache Cache;
        public HumanSetup Setup;
        private HumanCustomSkinLoader _customSkinLoader;
        private bool _isLoadingSkins = false;

        protected override void Awake()
        {
            base.Awake();
            Cache = new HumanComponentCache(gameObject);
            Cache.Rigidbody.freezeRotation = true;
            Cache.Rigidbody.useGravity = false;
            Cache.Rigidbody.velocity = Vector3.zero;
            Setup = gameObject.GetComponent<HumanSetup>();
            if (Setup == null)
                Setup = gameObject.AddComponent<HumanSetup>();
        }
        
        protected void Start()
        {
            if (_customSkinLoader == null)
            {
                _customSkinLoader = gameObject.AddComponent<HumanCustomSkinLoader>();
            }
        }

        protected override string GetIdleAnimation()
        {
            bool male = Setup.CustomSet.Sex.Value == (int)HumanSex.Male;
            string animation;
            if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG)
                animation = male ? HumanAnimations.IdleAHSSM : HumanAnimations.IdleAHSSF;
            else if (Setup.Weapon == HumanWeapon.Thunderspear)
                animation = male ? HumanAnimations.IdleTSM : HumanAnimations.IdleTSF;
            else
                animation = male ? HumanAnimations.IdleM : HumanAnimations.IdleF;
            return animation;
        }

        protected override string GetEmoteAnimation(string emote)
        {
            string animation = HumanAnimations.EmoteSalute;
            if (emote == "Salute")
                animation = HumanAnimations.EmoteSalute;
            else if (emote == "Dance")
                animation = HumanAnimations.SpecialArmin;
            else if (emote == "Flip")
                animation = HumanAnimations.Dodge;
            else if (emote == "Wave")
                animation = HumanAnimations.EmoteWave;
            else if (emote == "Nod")
                animation = HumanAnimations.EmoteYes;
            else if (emote == "Shake")
                animation = HumanAnimations.EmoteNo;
            else if (emote == "Eat")
                animation = HumanAnimations.SpecialSasha;
            return animation;
        }

        public void LoadSkin()
        {
            if (_isLoadingSkins)
                return;
            StartCoroutine(LoadSkinCoroutine());
        }
        
        private IEnumerator LoadSkinCoroutine()
        {
            _isLoadingSkins = true;
            yield return null;
            string skinUrlString = "";
            string[] skinUrls = new string[22];
            if (_customSkinLoader == null)
            {
                _customSkinLoader = gameObject.AddComponent<HumanCustomSkinLoader>();
            }
            if (Setup == null)
            {
                UnityEngine.Debug.LogWarning("HumanSetup not ready for skin loading on " + gameObject.name);
                _isLoadingSkins = false;
                yield break;
            }
            
            try
            {
                bool useGlobalOverrides = SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value;
                bool usePresetSkins = SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value;
                HumanCustomSet presetSet = Setup?.CustomSet;
                HumanCustomSkinSet globalSet = null;
                if (useGlobalOverrides)
                {
                    int globalPresetIndex = SettingsManager.CustomSkinSettings.Human.LastGlobalPresetIndex.Value;
                    var allSets = SettingsManager.CustomSkinSettings.Human.GetSets().GetItems();
                    if (globalPresetIndex >= 0 && globalPresetIndex < allSets.Count)
                    {
                        globalSet = (HumanCustomSkinSet)allSets[globalPresetIndex];
                    }
                }

                string GetSkinValue(string globalValue, string presetValue)
                {
                    if (usePresetSkins && presetSet != null)
                    {
                        if (useGlobalOverrides && globalSet != null && !string.IsNullOrEmpty(globalValue))
                        {
                            return globalValue;
                        }
                        if (!string.IsNullOrEmpty(presetValue))
                        {
                            return presetValue;
                        }
                    }
                    else if (useGlobalOverrides && globalSet != null)
                    {
                        return globalValue;
                    }
                    else if (usePresetSkins && presetSet != null && !string.IsNullOrEmpty(presetValue))
                    {
                        return presetValue;
                    }
                    return string.Empty;
                }
                float GetFloatValue(float globalValue, float presetValue)
                {
                    if (usePresetSkins && presetSet != null)
                    {
                        if (useGlobalOverrides && globalSet != null)
                        {
                            return globalValue;
                        }
                        return presetValue;
                    }
                    else if (useGlobalOverrides && globalSet != null)
                    {
                        return globalValue;
                    }
                    else if (usePresetSkins && presetSet != null)
                    {
                        return presetValue;
                    }
                    return 1f;
                }
                skinUrls = new string[] {
                    GetSkinValue(globalSet?.Horse.Value, presetSet?.SkinHorse.Value),
                    GetSkinValue(globalSet?.Hair.Value, presetSet?.SkinHair.Value),
                    GetSkinValue(globalSet?.Eye.Value, presetSet?.SkinEye.Value),
                    GetSkinValue(globalSet?.Glass.Value, presetSet?.SkinGlass.Value),
                    GetSkinValue(globalSet?.Face.Value, presetSet?.SkinFace.Value),
                    GetSkinValue(globalSet?.Skin.Value, presetSet?.SkinSkin.Value),
                    GetSkinValue(globalSet?.Costume.Value, presetSet?.SkinCostume.Value),
                    GetSkinValue(globalSet?.Logo.Value, presetSet?.SkinLogo.Value),
                    GetSkinValue(globalSet?.GearL.Value, presetSet?.SkinGearL.Value),
                    GetSkinValue(globalSet?.GearR.Value, presetSet?.SkinGearR.Value),
                    GetSkinValue(globalSet?.Gas.Value, presetSet?.SkinGas.Value),
                    GetSkinValue(globalSet?.Hoodie.Value, presetSet?.SkinHoodie.Value),
                    GetSkinValue(globalSet?.WeaponTrail.Value, presetSet?.SkinWeaponTrail.Value),
                    GetSkinValue(globalSet?.ThunderspearL.Value, presetSet?.SkinThunderspearL.Value),
                    GetSkinValue(globalSet?.ThunderspearR.Value, presetSet?.SkinThunderspearR.Value),
                    GetFloatValue(globalSet?.HookLTiling.Value ?? 1f, presetSet?.SkinHookLTiling.Value ?? 1f).ToString(),
                    GetSkinValue(globalSet?.HookL.Value, presetSet?.SkinHookL.Value),
                    GetFloatValue(globalSet?.HookRTiling.Value ?? 1f, presetSet?.SkinHookRTiling.Value ?? 1f).ToString(),
                    GetSkinValue(globalSet?.HookR.Value, presetSet?.SkinHookR.Value),
                    GetSkinValue(globalSet?.Hat.Value, presetSet?.SkinHat.Value),
                    GetSkinValue(globalSet?.Head.Value, presetSet?.SkinHead.Value),
                    GetSkinValue(globalSet?.Back.Value, presetSet?.SkinBack.Value)
                };
                skinUrlString = string.Join(",", skinUrls);
            }
            catch (System.Exception ex)
            {
                skinUrls = new string[22] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1", "", "1", "", "", "", "" };
                skinUrlString = string.Join(",", skinUrls);
            }
            
            _isLoadingSkins = false;
            bool hasAnySkins = skinUrls.Any(url => !string.IsNullOrEmpty(url) && url != "1");
            
            if (hasAnySkins)
            {
                yield return StartCoroutine(_customSkinLoader.LoadSkinsFromRPC(new object[] { -1, skinUrlString }));
            }
            else
            {
                Setup.Load(Setup.CustomSet, Setup.Weapon, false);
                if (_customSkinLoader != null)
                {
                    Destroy(_customSkinLoader);
                }
                _customSkinLoader = gameObject.AddComponent<HumanCustomSkinLoader>();
            }
        }
    }
}
