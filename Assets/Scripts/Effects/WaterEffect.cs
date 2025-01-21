using ApplicationManagers;
using Settings;
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
    private bool _isInWater;

    void Start()
    {
        if (_postProcessingManager == null)
            _postProcessingManager = FindFirstObjectByType<PostProcessingManager>();

        Camera cam = SceneLoader.CurrentCamera.Camera;
        _globalFog = cam.GetComponent<GlobalFog>();

        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        
        _volume = PostProcessingVolume.GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _colorGrading);
        
        Settings.GraphicsSettings settings = SettingsManager.GraphicsSettings;

        if (settings != null)
            ApplySettings((WaterFXLevel)settings.WaterFX.Value);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            _isInWater = true;
            PostProcessingVolume.gameObject.SetActive(true);
            _postProcessingManager.SetState(false);
            if (_fogEnabled)
            {
                _globalFog.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("MainCamera"))
        {
            _isInWater = false;
            PostProcessingVolume.gameObject.SetActive(false);
            _postProcessingManager.SetState(true);
            _globalFog.enabled = false;
        }
    }

    public void ApplySettings(WaterFXLevel wfxl)
    {
        if (wfxl == WaterFXLevel.Off)
        {
            PostProcessingVolume.gameObject.SetActive(false);
            this.enabled = false;
        }
        else
        {
            PostProcessingVolume.gameObject.SetActive(_isInWater);
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
            case WaterFXLevel.High:
                _fogEnabled = true;
                _colorGrading.enabled.value = true;
                break;
        }
    }
}
