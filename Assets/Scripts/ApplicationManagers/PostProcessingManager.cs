using ApplicationManagers;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using Utility;

public class PostProcessingManager : MonoBehaviour
{
    private PostProcessVolume _postProcessingVolume;
    private AmbientOcclusion _ambientOcclusion;
    private Bloom _bloom;
    private ChromaticAberration _chromaticAberration;
    private ColorGrading _colorGrading;
    private DepthOfField _depthOfField;
    private MotionBlur _motionBlur;
    private AutoExposure _autoExposure;

    public void Awake()
    {
        _postProcessingVolume = GetComponent<PostProcessVolume>();
        _postProcessingVolume.profile.TryGetSettings(out _ambientOcclusion);
        _postProcessingVolume.profile.TryGetSettings(out _bloom);
        _postProcessingVolume.profile.TryGetSettings(out _chromaticAberration);
        _postProcessingVolume.profile.TryGetSettings(out _colorGrading);
        _postProcessingVolume.profile.TryGetSettings(out _depthOfField);
        _postProcessingVolume.profile.TryGetSettings(out _motionBlur);
        _postProcessingVolume.profile.TryGetSettings(out _autoExposure);

        Settings.GraphicsSettings settings = SettingsManager.GraphicsSettings;

        if (settings != null)
            ApplySettings(
                (AmbientOcclusionLevel)settings.AmbientOcclusion.Value,
                (BloomLevel)settings.Bloom.Value,
                (ChromaticAberrationLevel)settings.ChromaticAberration.Value,
                (ColorGradingLevel)settings.ColorGrading.Value,
                (AutoExposureLevel)settings.AutoExposure.Value,
                (DepthOfFieldLevel)settings.DepthOfField.Value,
                (MotionBlurLevel)settings.MotionBlur.Value,
                (WaterFXLevel)settings.WaterFX.Value
            );

    }

    public void SetState(bool state)
    {
        // disable volume
        _postProcessingVolume.enabled = state;
    }

    public void ApplySettings(
        AmbientOcclusionLevel aol,
        BloomLevel bl,
        ChromaticAberrationLevel cal,
        ColorGradingLevel cgl,
        AutoExposureLevel ael,
        DepthOfFieldLevel dofl,
        MotionBlurLevel mbl,
        WaterFXLevel wfxl)
    {
        SetAmbientOcclusionQuality(aol);
        SetBloomQuality(bl);
        SetChromaticAberrationQuality(cal);
        SetColorGradingQuality(cgl);
        SetAutoExposureQuality(ael);
        SetDepthOfFieldQuality(dofl);
        SetMotionBlurQuality(mbl);

        // Find all objects with the WaterEffect script and apply settings
        WaterEffect[] waterEffects = FindObjectsByType<WaterEffect>(sortMode: FindObjectsSortMode.None);
        foreach (WaterEffect waterEffect in waterEffects)
        {
            waterEffect.ApplySettings(wfxl);
        }

    }

    public void SetAmbientOcclusionQuality(AmbientOcclusionLevel quality)
    {
        if (quality == AmbientOcclusionLevel.Off)
        {
            _ambientOcclusion.enabled.value = false;
            return;
        }

        _ambientOcclusion.enabled.value = true; 
        _ambientOcclusion.quality.value = (AmbientOcclusionQuality)((int)quality - 1);
    }

    public void SetBloomQuality(BloomLevel quality)
    {
        switch (quality)
        {
            case BloomLevel.Off:
                _bloom.enabled.value = false;
                break;
            case BloomLevel.Low:
                _bloom.enabled.value = true;
                _bloom.fastMode.value = true;
                break;
            case BloomLevel.High:
                _bloom.enabled.value = true;
                _bloom.fastMode.value = false;
                break;
        }
    }

    public void SetChromaticAberrationQuality(ChromaticAberrationLevel quality)
    {
        switch (quality)
        {
            case ChromaticAberrationLevel.Off:
                _chromaticAberration.enabled.value = false;
                break;
            case ChromaticAberrationLevel.Low:
                _chromaticAberration.enabled.value = true;
                _chromaticAberration.fastMode.value = true;
                break;
            case ChromaticAberrationLevel.High:
                _chromaticAberration.enabled.value = true;
                _chromaticAberration.fastMode.value = false;
                break;
        }
    }

    public void SetColorGradingQuality(ColorGradingLevel quality)
    {
        _colorGrading.enabled.value = quality != ColorGradingLevel.Off;
    }

    public void SetAutoExposureQuality(AutoExposureLevel quality)
    {
        _autoExposure.enabled.value = quality != AutoExposureLevel.Off;
    }
    
    public void SetDepthOfFieldQuality(DepthOfFieldLevel quality)
    {
        switch (quality)
        {
            case DepthOfFieldLevel.Off:
                _depthOfField.enabled.value = false;
                break;
            default:
                _depthOfField.enabled.value = true; // MaxBlurSize setting not exposed?
                break;
        }
    }

    public void SetMotionBlurQuality(MotionBlurLevel quality)
    {
        switch (quality)
        {
            case MotionBlurLevel.Off:
                _motionBlur.enabled.value = false;
                break;
            case MotionBlurLevel.Low:
                _motionBlur.enabled.value = true;
                _motionBlur.sampleCount.value = 4;
                break;
            case MotionBlurLevel.Medium:
                _motionBlur.enabled.value = true;
                _motionBlur.sampleCount.value = 8;
                break;
            case MotionBlurLevel.High:
                _motionBlur.enabled.value = true;
                _motionBlur.sampleCount.value = 16;
                break;
        }
    }


}
