using ApplicationManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPPVolumeManager : MonoBehaviour
{
    [SerializeField]GameObject PostProcessingVolume;
    Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
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
