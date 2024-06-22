using ApplicationManagers;
using Cameras;
using System;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Utility;

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
        public IntSetting AntiAliasing = new IntSetting((int)AntiAliasingLevel.On, minValue: 0, maxValue: (int)Util.EnumMaxValue<AntiAliasingLevel>());
        public IntSetting AnisotropicFiltering = new IntSetting((int)AnisotropicLevel.Low);
        public IntSetting WeatherEffects = new IntSetting((int)WeatherEffectLevel.High);
        public BoolSetting WeaponTrailEnabled = new BoolSetting(true);
        public BoolSetting WindEffectEnabled = new BoolSetting(true);
        public BoolSetting BloodSplatterEnabled = new BoolSetting(true);
        public BoolSetting NapeBloodEnabled = new BoolSetting(true);
        public BoolSetting MipmapEnabled = new BoolSetting(true);

        // Post Processing
        public IntSetting AmbientOcclusion = new IntSetting((int)AmbientOcclusionLevel.Off, minValue: 0, maxValue: (int)Util.EnumMaxValue<AmbientOcclusionLevel>());
        public IntSetting Bloom = new IntSetting((int)BloomLevel.Low, minValue: 0, maxValue: (int)Util.EnumMaxValue<BloomLevel>());
        public IntSetting ChromaticAberration = new IntSetting((int)ChromaticAberrationLevel.Low, minValue: 0, maxValue: (int)Util.EnumMaxValue<ChromaticAberrationLevel>());
        public IntSetting ColorGrading = new IntSetting((int)ColorGradingLevel.Off, minValue: 0, maxValue: (int)Util.EnumMaxValue<ColorGradingLevel>());
        public IntSetting DepthOfField = new IntSetting((int)DepthOfFieldLevel.Off, minValue: 0, maxValue: (int)Util.EnumMaxValue<DepthOfFieldLevel>());
        public IntSetting MotionBlur = new IntSetting((int)MotionBlurLevel.Off, minValue: 0, maxValue: (int)Util.EnumMaxValue<MotionBlurLevel>());
        public IntSetting WaterFX = new IntSetting((int)WaterFXLevel.High, minValue: 0, maxValue: (int)Util.EnumMaxValue<WaterFXLevel>());
        

        public override void Apply()
        {
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
            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)AnisotropicFiltering.Value;
            QualitySettings.antiAliasing = 0;
            QualitySettings.shadowDistance = ShadowDistance.Value;
            if (SceneLoader.CurrentCamera is InGameCamera)
                ((InGameCamera)SceneLoader.CurrentCamera).ApplyGraphicsSettings();
            ScreenResolution.Value = FullscreenHandler.SanitizeResolutionSetting(ScreenResolution.Value);
            FullscreenHandler.Apply(ScreenResolution.Value, (FullScreenLevel)FullScreenMode.Value);
            PostProcessingManager postProcessingManager = GameObject.FindFirstObjectByType<PostProcessingManager>();
            if (postProcessingManager != null)
                postProcessingManager.ApplySettings(
                    (AmbientOcclusionLevel)AmbientOcclusion.Value,
                    (BloomLevel)Bloom.Value,
                    (ChromaticAberrationLevel)ChromaticAberration.Value,
                    (ColorGradingLevel)ColorGrading.Value,
                    (DepthOfFieldLevel)DepthOfField.Value,
                    (MotionBlurLevel)MotionBlur.Value,
                    (WaterFXLevel)WaterFX.Value
                );
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
                LightDistance.Value = 0;
                Bloom.Value = (int)BloomLevel.Off;
                MotionBlur.Value = (int)MotionBlurLevel.Off;
                ColorGrading.Value = (int)ColorGradingLevel.Off;
                DepthOfField.Value = (int)DepthOfFieldLevel.Off;
                ChromaticAberration.Value = (int)ChromaticAberrationLevel.Off;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
                WaterFX.Value = (int)WaterFXLevel.Low;
            }
            else if (PresetQuality.Value == (int)PresetQualityLevel.Low)
            {
                TextureQuality.Value = (int)TextureQualityLevel.High;
                ShadowQuality.Value = (int)ShadowQualityLevel.Off;
                AntiAliasing.Value = (int)AntiAliasingLevel.Off;
                AnisotropicFiltering.Value = (int)AnisotropicLevel.Off;
                WeatherEffects.Value = (int)WeatherEffectLevel.Low;
                ShadowDistance.Value = 500;
                LightDistance.Value = 100;
                Bloom.Value = (int)BloomLevel.Off;
                MotionBlur.Value = (int)MotionBlurLevel.Off;
                ColorGrading.Value = (int)ColorGradingLevel.Off;
                DepthOfField.Value = (int)DepthOfFieldLevel.Off;
                ChromaticAberration.Value = (int)ChromaticAberrationLevel.Off;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
                WaterFX.Value = (int)WaterFXLevel.Low;
            }
            else if (PresetQuality.Value == (int)PresetQualityLevel.Medium)
            {
                TextureQuality.Value = (int)TextureQualityLevel.High;
                ShadowQuality.Value = (int)ShadowQualityLevel.Low;
                AntiAliasing.Value = (int)AntiAliasingLevel.On;
                AnisotropicFiltering.Value = (int)AnisotropicLevel.Low;
                WeatherEffects.Value = (int)WeatherEffectLevel.Medium;
                ShadowDistance.Value = 500;
                LightDistance.Value = 250;
                Bloom.Value = (int)BloomLevel.Low;
                MotionBlur.Value = (int)MotionBlurLevel.Off;
                ColorGrading.Value = (int)ColorGradingLevel.Off;
                DepthOfField.Value = (int)DepthOfFieldLevel.Off;
                ChromaticAberration.Value = (int)ChromaticAberrationLevel.Low;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
                WaterFX.Value = (int)WaterFXLevel.Medium;
            }
            else if (PresetQuality.Value == (int)PresetQualityLevel.High)
            {
                TextureQuality.Value = (int)TextureQualityLevel.High;
                ShadowQuality.Value = (int)ShadowQualityLevel.Medium;
                AntiAliasing.Value = (int)AntiAliasingLevel.On;
                AnisotropicFiltering.Value = (int)AnisotropicLevel.High;
                WeatherEffects.Value = (int)WeatherEffectLevel.High;
                ShadowDistance.Value = 1000;
                LightDistance.Value = 500;
                Bloom.Value = (int)BloomLevel.Low;
                MotionBlur.Value = (int)MotionBlurLevel.Off;
                ColorGrading.Value = (int)ColorGradingLevel.Off;
                DepthOfField.Value = (int)DepthOfFieldLevel.Off;
                ChromaticAberration.Value = (int)ChromaticAberrationLevel.Low;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
                WaterFX.Value = (int)WaterFXLevel.High;
            }
            else if (PresetQuality.Value == (int)PresetQualityLevel.VeryHigh)
            {
                TextureQuality.Value = (int)TextureQualityLevel.High;
                ShadowQuality.Value = (int)ShadowQualityLevel.High;
                AntiAliasing.Value = (int)AntiAliasingLevel.On;
                AnisotropicFiltering.Value = (int)AnisotropicLevel.High;
                WeatherEffects.Value = (int)WeatherEffectLevel.High;
                ShadowDistance.Value = 1000;
                LightDistance.Value = 1000;
                Bloom.Value = (int)BloomLevel.Low;
                MotionBlur.Value = (int)MotionBlurLevel.Off;
                ColorGrading.Value = (int)ColorGradingLevel.Off;
                DepthOfField.Value = (int)DepthOfFieldLevel.Off;
                ChromaticAberration.Value = (int)ChromaticAberrationLevel.Low;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
                WaterFX.Value = (int)WaterFXLevel.High;
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
        On
    }

    public enum AnisotropicLevel
    {
        Off,
        Low,
        High
    }

    public enum AmbientOcclusionLevel
    {
        Off,
        Lowest,
        Low,
        Medium,
        High,
        Ultra
    }

    public enum BloomLevel
    {
        Off,
        Low,
        High
    }

    public enum ChromaticAberrationLevel
    {
        Off,
        Low,
        High
    }

    public enum ColorGradingLevel
    {
        Off,
        On
    }

    public enum DepthOfFieldLevel
    {
        Off,
        Low,
        Medium,
        High,
    }

    public enum MotionBlurLevel
    {
        Off,
        Low,
        Medium,
        High
    }

    public enum WaterFXLevel
    {
        Off,
        Low,
        Medium,
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
