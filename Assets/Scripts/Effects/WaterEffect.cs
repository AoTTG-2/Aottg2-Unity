using ApplicationManagers;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;
using UnityStandardAssets.ImageEffects;
using Utility;

public class WaterEffect : MonoBehaviour
{
    [SerializeField] GameObject PostProcessingVolume;
    private PostProcessingManager _postProcessingManager;
    private PostProcessVolume _volume;
    private ColorGrading _colorGrading;
    private GlobalFog _globalFog;
    private BoxCollider _boxCollider;
    private bool _fogEnabled;

    void Start()
    {
        if (_postProcessingManager == null)
            _postProcessingManager = FindFirstObjectByType<PostProcessingManager>();

        Camera cam = SceneLoader.CurrentCamera.Camera;
        _globalFog = cam.GetComponent<GlobalFog>();
        _boxCollider = GetComponent<BoxCollider>();
        if (_boxCollider == null )
        {
            Debug.Log("Unsupported Object for Water Effect (must have box collider).");
            this.enabled = false;
            return;
        }
        _volume = PostProcessingVolume.GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _colorGrading);
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

    bool IsInsideBounds(Vector3 worldPos, BoxCollider bc)
    {
        Vector3 localPos = bc.transform.InverseTransformPoint(worldPos);
        Vector3 delta = localPos - bc.center + bc.size * 0.5f;
        return Vector3.Max(Vector3.zero, delta) == Vector3.Min(delta, bc.size);
    }

    private void FixedUpdate()
    {
        if (IsInsideBounds(SceneLoader.CurrentCamera.Camera.transform.position, _boxCollider))
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
