using ApplicationManagers;
using Cameras;
using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Settings
{
    class GraphicsSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "Graphics.json"; } }
        public IntSetting PresetQuality = new IntSetting((int)PresetQualityLevel.VeryHigh);
        public IntSetting FullScreenMode = new IntSetting((int)FullScreenLevel.Borderless);
        public IntSetting ScreenResolution = new IntSetting(0);
        public IntSetting FPSCap = new IntSetting(144, minValue: 0);
        public IntSetting MenuFPSCap = new IntSetting(60, minValue: 0);
        public BoolSetting VSync = new BoolSetting(false);
        public BoolSetting InterpolationEnabled = new BoolSetting(true);
        public BoolSetting ShowFPS = new BoolSetting(false);
        public IntSetting RenderDistance = new IntSetting(1500, minValue: 10, maxValue: 1000000);
        public IntSetting TextureQuality = new IntSetting((int)TextureQualityLevel.High);
        public IntSetting ShadowQuality = new IntSetting((int)ShadowQualityLevel.High);
        public IntSetting ShadowDistance = new IntSetting(1000, minValue: 0, maxValue: 3000);
        public IntSetting LightDistance = new IntSetting(1000, minValue: 0, maxValue: 3000);
        public IntSetting DetailDistance = new IntSetting(500, minValue: 0, maxValue: 1000);  // Added by Snake for Terrain Detail Slider 26 may 24
        public IntSetting DetailDensity = new IntSetting(500, minValue: 0, maxValue: 1000);  // Added by Snake for Terrain Detail Slider 27 may 24
        public IntSetting TreeDistance = new IntSetting(5000, minValue: 0, maxValue: 5000);  // Added by Snake for Terrain Detail Slider 28 may 24
        public IntSetting AntiAliasing = new IntSetting((int)AntiAliasingLevel.High);
        public IntSetting AnisotropicFiltering = new IntSetting((int)AnisotropicLevel.Low);
        public IntSetting WeatherEffects = new IntSetting((int)WeatherEffectLevel.High);
        public BoolSetting WeaponTrailEnabled = new BoolSetting(true);
        public BoolSetting WindEffectEnabled = new BoolSetting(true);
        public BoolSetting BloodSplatterEnabled = new BoolSetting(true);
        public BoolSetting NapeBloodEnabled = new BoolSetting(true);
        public BoolSetting MipmapEnabled = new BoolSetting(true);

        
        public override void Apply()
        {
            // Added by Snake for Terrain Detail Slider 28 may 24 
            SetTerrainDetails(DetailDistance.Value, DetailDensity.Value, TreeDistance.Value);

            if (ShadowQuality.Value == (int)ShadowQualityLevel.Off)
                QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
            else if (ShadowQuality.Value == (int)ShadowQualityLevel.Low)
            {
                QualitySettings.shadows = UnityEngine.ShadowQuality.HardOnly;
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                QualitySettings.shadowCascades = 0;
            }
            else if (ShadowQuality.Value == (int)ShadowQualityLevel.Medium)
            {
                QualitySettings.shadows = UnityEngine.ShadowQuality.All;
                QualitySettings.shadowResolution = ShadowResolution.High;
                QualitySettings.shadowCascades = 2;
            }
            else if (ShadowQuality.Value == (int)ShadowQualityLevel.High)
            {
                QualitySettings.shadows = UnityEngine.ShadowQuality.All;
                QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                QualitySettings.shadowCascades = 4;
            }
            QualitySettings.vSyncCount = Convert.ToInt32(VSync.Value);
            if (SceneLoader.SceneName == SceneName.InGame || SceneLoader.SceneName == SceneName.MapEditor)
                Application.targetFrameRate = FPSCap.Value > 0 ? FPSCap.Value : -1;
            else
                Application.targetFrameRate = MenuFPSCap.Value > 0 ? MenuFPSCap.Value : -1;
            QualitySettings.globalTextureMipmapLimit = 3 - TextureQuality.Value;
            QualitySettings.antiAliasing = AntiAliasing.Value == 0 ? 0 : (int)Mathf.Pow(2, AntiAliasing.Value);
            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)AnisotropicFiltering.Value;
            QualitySettings.shadowDistance = ShadowDistance.Value;
            if (SceneLoader.CurrentCamera is InGameCamera)
                ((InGameCamera)SceneLoader.CurrentCamera).ApplyGraphicsSettings();
            ScreenResolution.Value = FullscreenHandler.SanitizeResolutionSetting(ScreenResolution.Value);
            FullscreenHandler.Apply(ScreenResolution.Value, (FullScreenLevel)FullScreenMode.Value);
        }

        // Added by Snake for Terrain Detail Slider 28 may 24 
        public void SetTerrainDetails(int DetailDistance, int DetailDensity, int TreeDistance)
        {
            Terrain[] terrains = GameObject.FindObjectsOfType<Terrain>();
            foreach (Terrain terrain in terrains)
            {
                terrain.detailObjectDistance = DetailDistance;
                terrain.detailObjectDensity = DetailDensity /1000f; 
                terrain.treeDistance = TreeDistance;
            }
        }

        public void OnSelectPreset()
        {
            if (PresetQuality.Value == (int)PresetQualityLevel.VeryLow)
            {
                TextureQuality.Value = (int)TextureQualityLevel.VeryLow;
                ShadowQuality.Value = (int)ShadowQualityLevel.Off;
                AntiAliasing.Value = (int)AntiAliasingLevel.Off;
                AnisotropicFiltering.Value = (int)AnisotropicLevel.Off;
                WeatherEffects.Value = (int)WeatherEffectLevel.Off;
                ShadowDistance.Value = 500;
                LightDistance.Value = 250;
                DetailDistance.Value = 0;
                DetailDensity.Value = 0;
                TreeDistance.Value = 400;
            }
            else if (PresetQuality.Value == (int)PresetQualityLevel.Low)
            {
                TextureQuality.Value = (int)TextureQualityLevel.High;
                ShadowQuality.Value = (int)ShadowQualityLevel.Off;
                AntiAliasing.Value = (int)AntiAliasingLevel.Off;
                AnisotropicFiltering.Value = (int)AnisotropicLevel.Off;
                WeatherEffects.Value = (int)WeatherEffectLevel.Low;
                ShadowDistance.Value = 500;
                LightDistance.Value = 250;
                DetailDistance.Value = 200;
                DetailDensity.Value = 100;
                TreeDistance.Value = 400;
            }
            else if (PresetQuality.Value == (int)PresetQualityLevel.Medium)
            {
                TextureQuality.Value = (int)TextureQualityLevel.High;
                ShadowQuality.Value = (int)ShadowQualityLevel.Low;
                AntiAliasing.Value = (int)AntiAliasingLevel.Low;
                AnisotropicFiltering.Value = (int)AnisotropicLevel.Low;
                WeatherEffects.Value = (int)WeatherEffectLevel.Medium;
                ShadowDistance.Value = 500;
                LightDistance.Value = 500;
                DetailDistance.Value = 500;
                DetailDensity.Value = 250;
                TreeDistance.Value = 1000;
            }
            else if (PresetQuality.Value == (int)PresetQualityLevel.High)
            {
                TextureQuality.Value = (int)TextureQualityLevel.High;
                ShadowQuality.Value = (int)ShadowQualityLevel.Medium;
                AntiAliasing.Value = (int)AntiAliasingLevel.Medium;
                AnisotropicFiltering.Value = (int)AnisotropicLevel.High;
                WeatherEffects.Value = (int)WeatherEffectLevel.High;
                ShadowDistance.Value = 1000;
                LightDistance.Value = 500;
                DetailDistance.Value = 800;
                DetailDensity.Value = 380;
                TreeDistance.Value = 2500;
            }
            else if (PresetQuality.Value == (int)PresetQualityLevel.VeryHigh)
            {
                TextureQuality.Value = (int)TextureQualityLevel.High;
                ShadowQuality.Value = (int)ShadowQualityLevel.High;
                AntiAliasing.Value = (int)AntiAliasingLevel.High;
                AnisotropicFiltering.Value = (int)AnisotropicLevel.High;
                WeatherEffects.Value = (int)WeatherEffectLevel.High;
                ShadowDistance.Value = 1000;
                LightDistance.Value = 1000;
                DetailDistance.Value = 1000;
                DetailDensity.Value = 500;
                TreeDistance.Value = 5000;
            }
        }
    }

    public enum PresetQualityLevel
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh
    }

    public enum TextureQualityLevel
    {
        VeryLow,
        Low,
        Medium,
        High
    }

    public enum ShadowQualityLevel
    {
        Off,
        Low,
        Medium,
        High
    }

    public enum AntiAliasingLevel
    {
        Off,
        Low,
        Medium,
        High
    }

    public enum AnisotropicLevel
    {
        Off,
        Low,
        High
    }

    public enum WeatherEffectLevel
    {
        Off,
        Low,
        Medium,
        High
    }

    public enum TitanSpawnEffectLevel
    {
        Off,
        Quarter,
        Half,
        Full
    }

    public enum FullScreenLevel
    {
        Windowed,
        Borderless,
        Exclusive
    }
}
