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

namespace Cameras
{
    class BaseCamera : MonoBehaviour
    {
        public Camera Camera;
        public BaseComponentCache Cache;
        public Skybox Skybox;

        protected virtual void Awake()
        {
            Camera = gameObject.GetComponent<Camera>();
            Skybox = gameObject.AddComponent<Skybox>();
            Cache = new BaseComponentCache(gameObject);
            AudioListener.volume = SettingsManager.SoundSettings.Volume.Value;
            Camera.fieldOfView = 50f;
            Camera.cullingMask &= ~(1 << PhysicsLayer.MinimapIcon);
        }

        public virtual void OnFinishLoading()
        {
            SetDefaultCameraPosition();
        }

        protected virtual void SetDefaultCameraPosition()
        {
        }
    }
}