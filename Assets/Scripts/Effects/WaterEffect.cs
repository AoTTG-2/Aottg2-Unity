using ApplicationManagers;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WaterEffect : MonoBehaviour
{
    [SerializeField] GameObject PostProcessingVolume;
    Collider _collider;

    private PostProcessVolume _volume;
    private ColorGrading _colorGrading;
    private DepthOfField _depthOfField;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _volume = PostProcessingVolume.GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _colorGrading);
        _volume.profile.TryGetSettings(out _depthOfField);
        Settings.GraphicsSettings settings = SettingsManager.GraphicsSettings;

        if (settings != null)
            ApplySettings((ColorGradingLevel)settings.ColorGrading.Value, (DepthOfFieldLevel)settings.DepthOfField.Value);
    }

    public void ApplySettings(ColorGradingLevel cgl, DepthOfFieldLevel dofl)
    {
        if (cgl == ColorGradingLevel.Off && dofl == DepthOfFieldLevel.Off)
        {
            PostProcessingVolume.gameObject.SetActive(false);
            this.enabled = false;
            return;
        }
        this.enabled = true;

        _colorGrading.enabled.value = cgl != ColorGradingLevel.Off;
        _depthOfField.enabled.value = dofl != DepthOfFieldLevel.Off;
    }

    void FixedUpdate()
    {
        Camera cam = SceneLoader.CurrentCamera.Camera;

        // If the camera is inside this objects collider, enable the post processing volume
        if (_collider.bounds.Contains(cam.transform.position))
        {
            PostProcessingVolume.gameObject.SetActive(true);
        }
        else
        {
            PostProcessingVolume.gameObject.SetActive(false);
        }
    }
}
