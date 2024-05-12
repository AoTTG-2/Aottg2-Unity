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

    public void Awake()
    {
        _postProcessingVolume = GetComponent<PostProcessVolume>();
        _postProcessingVolume.profile.TryGetSettings(out _ambientOcclusion);
        _postProcessingVolume.profile.TryGetSettings(out _bloom);
        _postProcessingVolume.profile.TryGetSettings(out _chromaticAberration);
        _postProcessingVolume.profile.TryGetSettings(out _colorGrading);
        _postProcessingVolume.profile.TryGetSettings(out _depthOfField);
        _postProcessingVolume.profile.TryGetSettings(out _motionBlur);
    }

    public void ApplySettings()
    {
        Settings.GraphicsSettings settings = SettingsManager.GraphicsSettings;

        if (settings == null)
            return;

        SetAmbientOcclusionQuality((AmbientOcclusionLevel)settings.AmbientOcclusion.Value);
        SetBloomQuality((BloomLevel)settings.Bloom.Value);
        SetChromaticAberrationQuality((ChromaticAberrationLevel)settings.ChromaticAberration.Value);
        SetColorGradingQuality((ColorGradingLevel)settings.ColorGrading.Value);
        SetDepthOfFieldQuality((DepthOfFieldLevel)settings.DepthOfField.Value);
        SetMotionBlurQuality((MotionBlurLevel)settings.MotionBlur.Value);

        // Find all objects with the WaterEffect script and apply settings
        WaterEffect[] waterEffects = FindObjectsOfType<WaterEffect>();
        foreach (WaterEffect waterEffect in waterEffects)
        {
            waterEffect.ApplySettings();
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
