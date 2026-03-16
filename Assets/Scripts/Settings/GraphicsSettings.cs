using ApplicationManagers;
using Cameras;
using System;
using System.Reflection;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
        public IntSetting RenderDistance = new IntSetting(10000, minValue: 10, maxValue: 1000000);
        public IntSetting TextureQuality = new IntSetting((int)TextureQualityLevel.High);
        public IntSetting ShadowQuality = new IntSetting((int)ShadowQualityLevel.High);
        public IntSetting ShadowDistance = new IntSetting(1000, minValue: 0, maxValue: 3000);
        public IntSetting LightDistance = new IntSetting(1000, minValue: 0, maxValue: 3000);
        public IntSetting AntiAliasing = new IntSetting((int)AntiAliasingLevel.On, minValue: 0, maxValue: (int)Util.EnumMaxValue<AntiAliasingLevel>());
        public IntSetting AnisotropicFiltering = new IntSetting((int)AnisotropicLevel.Low);
        public IntSetting WeatherEffects = new IntSetting((int)WeatherEffectLevel.High);
        public IntSetting WeaponTrail = new IntSetting((int)WeaponTrailMode.All);
        public BoolSetting WeaponTrailHold = new BoolSetting(false);
        public BoolSetting WeaponFireEffect = new BoolSetting(true);
        public BoolSetting WindEffectEnabled = new BoolSetting(true);
        public BoolSetting BloodSplatterEnabled = new BoolSetting(true);
        public BoolSetting NapeBloodEnabled = new BoolSetting(true);
        public BoolSetting MipmapEnabled = new BoolSetting(true);
        public IntSetting ShadowCascades = new IntSetting((int)ShadowCascadesLevel.Four, minValue: 0, maxValue: (int)Util.EnumMaxValue<ShadowCascadesLevel>());
        public IntSetting AdditionalLightsMode = new IntSetting((int)AdditionalLightingMode.Default, minValue: 0, maxValue: (int)Util.EnumMaxValue<AdditionalLightingMode>());
        public BoolSetting VolumeUpdateEveryFrame = new BoolSetting(true);

        // Post Processing
        public IntSetting AmbientOcclusion = new IntSetting((int)AmbientOcclusionLevel.Off, minValue: 0, maxValue: (int)Util.EnumMaxValue<AmbientOcclusionLevel>());
        public IntSetting Bloom = new IntSetting((int)BloomLevel.Low, minValue: 0, maxValue: (int)Util.EnumMaxValue<BloomLevel>());
        public IntSetting ChromaticAberrationFX = new IntSetting((int)ChromaticAberrationLevel.Off, minValue: 0, maxValue: (int)Util.EnumMaxValue<ChromaticAberrationLevel>());
        public IntSetting ColorGrading = new IntSetting((int)ColorGradingLevel.Off, minValue: 0, maxValue: (int)Util.EnumMaxValue<ColorGradingLevel>());
        public IntSetting AutoExposure = new IntSetting((int)AutoExposureLevel.On, minValue: 0, maxValue: (int)Util.EnumMaxValue<AutoExposureLevel>());
        public IntSetting DepthOfField = new IntSetting((int)DepthOfFieldLevel.Off, minValue: 0, maxValue: (int)Util.EnumMaxValue<DepthOfFieldLevel>());
        public IntSetting MotionBlur = new IntSetting((int)MotionBlurLevel.Off, minValue: 0, maxValue: (int)Util.EnumMaxValue<MotionBlurLevel>());
        public IntSetting WaterFX = new IntSetting((int)WaterFXLevel.High, minValue: 0, maxValue: (int)Util.EnumMaxValue<WaterFXLevel>());
        public BoolSetting HDR = new BoolSetting(false);

        public override void Apply()
        {
            UniversalRenderPipelineAsset urpAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
            if (urpAsset == null)
                urpAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
            if (urpAsset != null)
            {
                if (ShadowQuality.Value == (int)ShadowQualityLevel.Off)
                {
                    urpAsset.shadowDistance = 0f;
                }
                else
                {
                    urpAsset.shadowDistance = ShadowDistance.Value;
                    var shadowResField = typeof(UniversalRenderPipelineAsset).GetField("m_MainLightShadowmapResolution", BindingFlags.NonPublic | BindingFlags.Instance);
                    int shadowResolution = ShadowQuality.Value == (int)ShadowQualityLevel.Low ? 512
                        : ShadowQuality.Value == (int)ShadowQualityLevel.Medium ? 1024 : 2048;
                    shadowResField?.SetValue(urpAsset, (UnityEngine.Rendering.Universal.ShadowResolution)shadowResolution);
                    switch (ShadowCascades.Value)
                    {
                        case (int)ShadowCascadesLevel.One:
                            urpAsset.shadowCascadeCount = 1;
                            break;
                        case (int)ShadowCascadesLevel.Two:
                            urpAsset.shadowCascadeCount = 2;
                            break;
                        case (int)ShadowCascadesLevel.Four:
                            urpAsset.shadowCascadeCount = 4;
                            break;
                    }
                }
                var additionalLightsField = typeof(UniversalRenderPipelineAsset).GetField("m_AdditionalLightsRenderingMode", BindingFlags.NonPublic | BindingFlags.Instance);
                LightRenderingMode lightMode = AdditionalLightsMode.Value == (int)AdditionalLightingMode.PerVertex ? LightRenderingMode.PerVertex
                    : AdditionalLightsMode.Value == (int)AdditionalLightingMode.Disabled ? LightRenderingMode.Disabled
                    : LightRenderingMode.PerPixel;
                additionalLightsField?.SetValue(urpAsset, lightMode);
            }
            QualitySettings.vSyncCount = Convert.ToInt32(VSync.Value);
            if (SceneLoader.SceneName == SceneName.InGame || SceneLoader.SceneName == SceneName.MapEditor)
                Application.targetFrameRate = FPSCap.Value > 0 ? FPSCap.Value : -1;
            else
                Application.targetFrameRate = MenuFPSCap.Value > 0 ? MenuFPSCap.Value : -1;
            QualitySettings.globalTextureMipmapLimit = 3 - TextureQuality.Value;
            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)AnisotropicFiltering.Value;
            QualitySettings.antiAliasing = 0;
            if (Camera.main != null)
                Camera.main.SetVolumeFrameworkUpdateMode(VolumeUpdateEveryFrame.Value ? VolumeFrameworkUpdateMode.EveryFrame : VolumeFrameworkUpdateMode.ViaScripting);
            if (SceneLoader.CurrentCamera is InGameCamera)
                ((InGameCamera)SceneLoader.CurrentCamera).ApplyGraphicsSettings();
            ScreenResolution.Value = FullscreenHandler.SanitizeResolutionSetting(ScreenResolution.Value);
            FullscreenHandler.Apply(ScreenResolution.Value, (FullScreenLevel)FullScreenMode.Value);
            PostProcessingManager postProcessingManager = GameObject.FindFirstObjectByType<PostProcessingManager>();
            if (postProcessingManager != null)
                postProcessingManager.ApplySettings(
                    (AmbientOcclusionLevel)AmbientOcclusion.Value,
                    (BloomLevel)Bloom.Value,
                    (ChromaticAberrationLevel)ChromaticAberrationFX.Value,
                    (ColorGradingLevel)ColorGrading.Value,
                    (AutoExposureLevel)AutoExposure.Value,
                    (DepthOfFieldLevel)DepthOfField.Value,
                    (MotionBlurLevel)MotionBlur.Value,
                    (WaterFXLevel)WaterFX.Value
                );

            if (UIManager.CurrentMenu != null)
            {
                if (UIManager.CurrentMenu is InGameMenu)
                {
                    InGameMenu igm = (InGameMenu)UIManager.CurrentMenu;
                    igm.ApplyUISettings();
                }
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
                ChromaticAberrationFX.Value = (int)ChromaticAberrationLevel.Off;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
                WaterFX.Value = (int)WaterFXLevel.Low;
                AutoExposure.Value = (int)AutoExposureLevel.Off;
                HDR.Value = false;
                RenderDistance.Value = 1000;
                ShadowCascades.Value = (int)ShadowCascadesLevel.One;
                AdditionalLightsMode.Value = (int)AdditionalLightingMode.Disabled;
                VolumeUpdateEveryFrame.Value = false;
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
                ChromaticAberrationFX.Value = (int)ChromaticAberrationLevel.Off;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
                WaterFX.Value = (int)WaterFXLevel.Low;
                AutoExposure.Value = (int)AutoExposureLevel.On;
                HDR.Value = false;
                RenderDistance.Value = 2000;
                ShadowCascades.Value = (int)ShadowCascadesLevel.One;
                AdditionalLightsMode.Value = (int)AdditionalLightingMode.PerVertex;
                VolumeUpdateEveryFrame.Value = false;
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
                ChromaticAberrationFX.Value = (int)ChromaticAberrationLevel.Off;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
                WaterFX.Value = (int)WaterFXLevel.Medium;
                AutoExposure.Value = (int)AutoExposureLevel.On;
                HDR.Value = false;
                RenderDistance.Value = 5000;
                ShadowCascades.Value = (int)ShadowCascadesLevel.Two;
                AdditionalLightsMode.Value = (int)AdditionalLightingMode.PerVertex;
                VolumeUpdateEveryFrame.Value = true;
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
                ChromaticAberrationFX.Value = (int)ChromaticAberrationLevel.Off;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
                WaterFX.Value = (int)WaterFXLevel.High;
                AutoExposure.Value = (int)AutoExposureLevel.On;
                HDR.Value = false;
                RenderDistance.Value = 10000;
                ShadowCascades.Value = (int)ShadowCascadesLevel.Four;
                AdditionalLightsMode.Value = (int)AdditionalLightingMode.Default;
                VolumeUpdateEveryFrame.Value = true;
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
                ChromaticAberrationFX.Value = (int)ChromaticAberrationLevel.Off;
                AmbientOcclusion.Value = (int)AmbientOcclusionLevel.Off;
                WaterFX.Value = (int)WaterFXLevel.High;
                AutoExposure.Value = (int)AutoExposureLevel.On;
                HDR.Value = false;
                RenderDistance.Value = 10000;
                ShadowCascades.Value = (int)ShadowCascadesLevel.Four;
                AdditionalLightsMode.Value = (int)AdditionalLightingMode.Default;
                VolumeUpdateEveryFrame.Value = true;
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

    public enum AutoExposureLevel
    {
        Off,
        On
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

    public enum WeaponTrailMode
    {
        Off,
        Mine,
        All
    }

    public enum ShadowCascadesLevel
    {
        One,
        Two,
        Four
    }

    public enum AdditionalLightingMode
    {
        Default,
        PerVertex,
        Disabled
    }
}
