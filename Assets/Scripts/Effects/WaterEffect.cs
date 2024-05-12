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

        ApplySettings();
    }

    public void ApplySettings()
    {
        Settings.GraphicsSettings settings = SettingsManager.GraphicsSettings;

        if (settings.ColorGrading.Value == 0 && settings.DepthOfField.Value == 0)
        {
            PostProcessingVolume.gameObject.SetActive(false);
            this.enabled = false;
            return;
        }
        this.enabled = true;

        _colorGrading.enabled.value = settings.ColorGrading.Value != 0;
        _depthOfField.enabled.value = settings.DepthOfField.Value != 0;
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
