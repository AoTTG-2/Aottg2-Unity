using UnityEngine;
using Utility;
using Settings;
using UI;
using Weather;
using System.Collections;
using GameProgress;
using Map;
using GameManagers;
using Events;
using ApplicationManagers;

namespace Cameras
{
    class BaseCamera : MonoBehaviour
    {
        public Camera Camera;
        public BaseComponentCache Cache;
        public Skybox Skybox;
        // public Camera BackgroundCamera;

        protected virtual void Awake()
        {
            Camera = gameObject.GetComponent<Camera>();
            Camera.depthTextureMode |= DepthTextureMode.Depth;
            // BackgroundCamera = gameObject.transform.Find("BackgroundCamera").GetComponent<Camera>();
            // Skybox = BackgroundCamera.gameObject.GetComponent<Skybox>();
            Skybox = gameObject.GetComponent<Skybox>();
            Cache = new BaseComponentCache(gameObject);
            FullscreenHandler.UpdateSound();
            Camera.fieldOfView = 50f;
        }

        public virtual void OnFinishLoading()
        {
            SetDefaultCameraPosition();
        }

        protected virtual void SetDefaultCameraPosition()
        {
        }

        protected virtual void LateUpdate()
        {
            /*
            BackgroundCamera.fieldOfView = Camera.fieldOfView;
            BackgroundCamera.transform.position = Vector3.up * Cache.Transform.position.y * 0.02f;
            BackgroundCamera.transform.rotation = Cache.Transform.rotation;
            */
        }
    }
}