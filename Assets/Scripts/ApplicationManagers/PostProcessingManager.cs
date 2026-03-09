using ApplicationManagers;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Utility;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;
    [SerializeField] private bool disable;

    [Header("Post Processing Profiles")]
    [SerializeField] private VolumeProfile postProfileMain;
    [SerializeField] private VolumeProfile underwaterProfile;
    [SerializeField] private VolumeProfile rainyProfile;
    [SerializeField] private VolumeProfile snowyProfile;
    [SerializeField] private VolumeProfile nightProfile;


    private Bloom _bloom;
    private ChromaticAberration _chromaticAberration;
    private ColorAdjustments _colorAdjustments;
    private DepthOfField _depthOfField;
    private MotionBlur _motionBlur;

    public void Awake()
    {
        postProcessingVolume = GetComponent<Volume>();
        if (postProcessingVolume == null)
        {
            Debug.LogError("PostProcessingManager: No PostProcessVolume component found on this object.");
            return;
        }

        postProcessingVolume.profile = postProfileMain;

        postProcessingVolume.profile.TryGet(out _bloom);
        postProcessingVolume.profile.TryGet(out _chromaticAberration);
        postProcessingVolume.profile.TryGet(out _colorAdjustments);
        postProcessingVolume.profile.TryGet(out _depthOfField);
        postProcessingVolume.profile.TryGet(out _motionBlur);

        Settings.GraphicsSettings settings = SettingsManager.GraphicsSettings;

        if (settings != null)
            ApplySettings(
                (AmbientOcclusionLevel)settings.AmbientOcclusion.Value,
                (BloomLevel)settings.Bloom.Value,
                (ChromaticAberrationLevel)settings.ChromaticAberrationFX.Value,
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
        if (postProcessingVolume == null)
        {
            Debug.LogError("PostProcessingManager: No PostProcessVolume component found on this object.");
            return;
        }
        postProcessingVolume.enabled = state;
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
        if (postProcessingVolume == null)
        {
            Debug.LogError("PostProcessingManager: No PostProcessVolume component found on this object.");
            return;
        }
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
        // AmbientOcclusion is a renderer feature in URP, not a volume component
    }

    public void SetBloomQuality(BloomLevel quality)
    {
        switch (quality)
        {
            case BloomLevel.Off:
                _bloom.active = false;
                break;
            case BloomLevel.Low:
                _bloom.active = true;
                _bloom.highQualityFiltering.value = false;
                break;
            case BloomLevel.High:
                _bloom.active = true;
                _bloom.highQualityFiltering.value = true;
                break;
        }
    }

    public void SetChromaticAberrationQuality(ChromaticAberrationLevel quality)
    {
        _chromaticAberration.active = quality != ChromaticAberrationLevel.Off;
    }

    public void SetColorGradingQuality(ColorGradingLevel quality)
    {
        _colorAdjustments.active = quality != ColorGradingLevel.Off;
    }

    public void SetAutoExposureQuality(AutoExposureLevel quality)
    {
        // AutoExposure is not available as a volume component in URP
    }
    
    public void SetDepthOfFieldQuality(DepthOfFieldLevel quality)
    {
        switch (quality)
        {
            case DepthOfFieldLevel.Off:
                _depthOfField.active = false;
                break;
            default:
                _depthOfField.active = true;
                break;
        }
    }

    public void SetMotionBlurQuality(MotionBlurLevel quality)
    {
        switch (quality)
        {
            case MotionBlurLevel.Off:
                _motionBlur.active = false;
                break;
            case MotionBlurLevel.Low:
                _motionBlur.active = true;
                _motionBlur.quality.value = MotionBlurQuality.Low;
                break;
            case MotionBlurLevel.Medium:
                _motionBlur.active = true;
                _motionBlur.quality.value = MotionBlurQuality.Medium;
                break;
            case MotionBlurLevel.High:
                _motionBlur.active = true;
                _motionBlur.quality.value = MotionBlurQuality.High;
                break;
        }
    }


}
