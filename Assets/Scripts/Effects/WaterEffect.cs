using ApplicationManagers;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.ImageEffects;

public class WaterEffect : MonoBehaviour
{
    [SerializeField] GameObject PostProcessingVolume;
    Collider _collider;

    private PostProcessingManager _postProcessingManager;
    private PostProcessVolume _volume;
    private ColorGrading _colorGrading;
    private GlobalFog _globalFog;
    private bool _fogEnabled;

    void Start()
    {
        // Get gameobject with component PostProcessingMananger script
        if (_postProcessingManager == null)
            _postProcessingManager = FindFirstObjectByType<PostProcessingManager>();

        Camera cam = SceneLoader.CurrentCamera.Camera;
        _globalFog = cam.GetComponent<GlobalFog>();

        _collider = GetComponent<Collider>();
        _volume = PostProcessingVolume.GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _colorGrading);
        //_volume.profile.TryGetSettings(out _depthOfField);
        Settings.GraphicsSettings settings = SettingsManager.GraphicsSettings;

        if (settings != null)
            ApplySettings((WaterFXLevel)settings.WaterFX.Value);
    }

    public void ApplySettings(WaterFXLevel wfxl)
    {
        if (wfxl == WaterFXLevel.Off)
        {
            PostProcessingVolume.gameObject.SetActive(false);
            this.enabled = false;
        } 
        else {
            PostProcessingVolume.gameObject.SetActive(true);
            this.enabled = true;
        }



        switch (wfxl)
        {
            case WaterFXLevel.Off:
                _fogEnabled = false;
                _globalFog.enabled = false;
                _colorGrading.enabled.value = false;
                break;
            case WaterFXLevel.Low:
                _fogEnabled = false;
                _globalFog.enabled = false;
                _colorGrading.enabled.value = true;
                break;
            case WaterFXLevel.Medium:
                _fogEnabled = true;
                _colorGrading.enabled.value = true;
                break;
            case WaterFXLevel.High:
                _fogEnabled = true;
                _colorGrading.enabled.value = true;
                break;
        }
    }

    void FixedUpdate()
    {
        Camera cam = SceneLoader.CurrentCamera.Camera;

        // If the camera is inside this objects collider, enable the post processing volume
        if (_collider.bounds.Contains(cam.transform.position))
        {
            PostProcessingVolume.gameObject.SetActive(true);
            _postProcessingManager.SetState(false);
            if (_fogEnabled)
            {
                _globalFog.enabled = true;
            }
        }
        else
        {
            PostProcessingVolume.gameObject.SetActive(false);
            _postProcessingManager.SetState(true);
            _globalFog.enabled = false;
        }
    }
}
