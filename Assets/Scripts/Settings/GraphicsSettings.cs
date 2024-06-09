﻿using ApplicationManagers;
using Cameras;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Settings
{
    class GraphicsSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "Graphics.json"; } }
        public IntSetting PresetQuality = new IntSetting((int)PresetQualityLevel.VeryHigh);
        public IntSetting FullScreenMode = new IntSetting((int)FullScreenLevel.Borderless);
        public IntSetting ScreenResolution = new IntSetting(0);
        public IntSetting MonitorLength = new IntSetting(0);
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
        public IntSetting AntiAliasing = new IntSetting((int)AntiAliasingLevel.High);
        public IntSetting AnisotropicFiltering = new IntSetting((int)AnisotropicLevel.Low);
        public IntSetting WeatherEffects = new IntSetting((int)WeatherEffectLevel.High);
        public BoolSetting WeaponTrailEnabled = new BoolSetting(true);
        public BoolSetting WindEffectEnabled = new BoolSetting(true);
        public BoolSetting BloodSplatterEnabled = new BoolSetting(true);
        public BoolSetting NapeBloodEnabled = new BoolSetting(true);
        public BoolSetting MipmapEnabled = new BoolSetting(true);

        // Post Processing
        public IntSetting AmbientOcclusion = new IntSetting((int)AmbientOcclusionLevel.High);
        public IntSetting Bloom = new IntSetting((int)BloomLevel.High);
        public IntSetting ChromaticAberration = new IntSetting((int)ChromaticAberrationLevel.High);
        public IntSetting ColorGrading = new IntSetting((int)ColorGradingLevel.On);
        public IntSetting DepthOfField = new IntSetting((int)DepthOfFieldLevel.Off);
        public IntSetting MotionBlur = new IntSetting((int)MotionBlurLevel.Off);
        

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
            QualitySettings.antiAliasing = AntiAliasing.Value == 0 ? 0 : (int)Mathf.Pow(2, AntiAliasing.Value);
            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)AnisotropicFiltering.Value;
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
                    (MotionBlurLevel)MotionBlur.Value
                );
            if(MonitorLength.Value == 0)
            {
                Display.displays[0].Activate();
            }
            else if(MonitorLength.Value == 1)
            {
                Display.displays[1].Activate();
            }
            else if (MonitorLength.Value == 2)
            {
                Display.displays[2].Activate();
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
                LightDistance.Value = 0;
                Bloom.Value = (int)BloomLevel.Off;
                MotionBlur.Value = (int)MotionBlurLevel.Off;
                ColorGrading.Value = (int)ColorGradingLevel.Off;
                DepthOfField.Value = (int)DepthOfFieldLevel.Off;
                ChromaticAberration.Value = (int)ChromaticAberrationLevel.Off;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
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
                Bloom.Value = (int)BloomLevel.Off;
                MotionBlur.Value = (int)MotionBlurLevel.Off;
                ColorGrading.Value = (int)ColorGradingLevel.On;
                DepthOfField.Value = (int)DepthOfFieldLevel.Off;
                ChromaticAberration.Value = (int)ChromaticAberrationLevel.Off;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
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
                Bloom.Value = (int)BloomLevel.Low;
                MotionBlur.Value = (int)MotionBlurLevel.Off;
                ColorGrading.Value = (int)ColorGradingLevel.On;
                DepthOfField.Value = (int)DepthOfFieldLevel.Off;
                ChromaticAberration.Value = (int)ChromaticAberrationLevel.Low;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Low;
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
                Bloom.Value = (int)BloomLevel.High;
                MotionBlur.Value = (int)MotionBlurLevel.Off;
                ColorGrading.Value = (int)ColorGradingLevel.On;
                DepthOfField.Value = (int)DepthOfFieldLevel.Off;
                ChromaticAberration.Value = (int)ChromaticAberrationLevel.High;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Medium;
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
                Bloom.Value = (int)BloomLevel.High;
                MotionBlur.Value = (int)MotionBlurLevel.Off;
                ColorGrading.Value = (int)ColorGradingLevel.On;
                DepthOfField.Value = (int)DepthOfFieldLevel.Off;
                ChromaticAberration.Value = (int)ChromaticAberrationLevel.High;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.High;
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
