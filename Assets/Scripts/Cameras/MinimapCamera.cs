using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using System.Collections;
using ApplicationManagers;
using Cameras;
using Characters;
using Utility;
using System;
using Map;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Photon.Pun;

namespace Cameras
{
    class MinimapCamera: MonoBehaviour
    {
        private Transform _cameraTransform;
        private Camera _camera;
        private bool _takingSnapshot;
        private RenderTexture _minimapRenderTexture;
        private RenderTexture _mapRenderTexture;
        public static int MinimapSize = 300;
        public static int MapSize = 900;

        public void Awake()
        {
            var go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Minimap/Prefabs/MinimapCamera");
            go.AddComponent<MinimapCameraRender>();
            _cameraTransform = go.transform;
            _camera = go.GetComponent<Camera>();
            go.SetActive(false);
            _minimapRenderTexture = ResourceManager.InstantiateAsset<RenderTexture>(ResourcePaths.UI, "Minimap/Textures/MinimapRenderTexture");
            _mapRenderTexture = ResourceManager.InstantiateAsset<RenderTexture>(ResourcePaths.UI, "Minimap/Textures/MapRenderTexture");
        }

        public bool Ready()
        {
            return !_takingSnapshot;
        }

        public void TakeSnapshot(Vector3 position, float height, Texture2D texture, bool minimap, bool immediate=false)
        {
            if (_takingSnapshot)
                return;
            if (immediate)
                TakeSnapshotImmediate(position, height, texture, minimap);
            else
            {
                _takingSnapshot = true;
                StartCoroutine(TakeSnapshotCoroutine(position, height, texture, minimap));
            }
        }

        private void TakeSnapshotImmediate(Vector3 position, float height, Texture2D texture, bool minimap)
        {
            if (texture == null)
                return;
            RTImage(position, height, texture, minimap);
            texture.Apply();
        }

        private IEnumerator TakeSnapshotCoroutine(Vector3 position, float height, Texture2D texture, bool minimap)
        {
            if (texture == null)
            {
                _takingSnapshot = false;
                yield break;
            }
            RTImage(position, height, texture, minimap);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (texture == null)
            {
                _takingSnapshot = false;
                yield break;
            }
            texture.Apply();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            _takingSnapshot = false;
        }

        private void RTImage(Vector3 position, float height, Texture2D texture, bool minimap)
        {
            _cameraTransform.gameObject.SetActive(true);
            _camera.orthographicSize = height * 0.5f;
            _camera.farClipPlane = height + 1000f;
            _cameraTransform.position = position;
            _cameraTransform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            try
            {
                RenderTexture active = RenderTexture.active;
                int imageSize = MinimapSize;
                if (minimap)
                    _camera.targetTexture = _minimapRenderTexture;
                else
                {
                    _camera.targetTexture = _mapRenderTexture;
                    imageSize = MapSize;
                }
                RenderTexture.active = _camera.targetTexture;
                _camera.Render();
                texture.ReadPixels(new Rect(0, 0, imageSize, imageSize), 0, 0);
                RenderTexture.active = active;
            }
            catch (Exception e)
            {
            }
            _cameraTransform.gameObject.SetActive(false);
        }
    }

    class MinimapCameraRender : MonoBehaviour
    {
        private List<MapLight> _disabledLights = new List<MapLight>();
        private Color _ambientLight;
        private float _ambientIntensity;
        private bool _fog;

        private void OnPreCull()
        {
            _disabledLights.Clear();
            foreach (var light in MapLoader.MapLights)
            {
                if (light.MinimapDisableLight())
                    _disabledLights.Add(light);
            }
            _ambientLight = RenderSettings.ambientLight;
            _ambientIntensity = RenderSettings.ambientIntensity;
            _fog = RenderSettings.fog;
            RenderSettings.ambientLight = Color.white;
            RenderSettings.ambientIntensity = 1f;
            RenderSettings.fog = false;
        }

        private void OnPostRender()
        {
            foreach (var light in _disabledLights)
            {
                if (light != null)
                    light.MinimapEnableLight();
            }
            RenderSettings.ambientLight = _ambientLight;
            RenderSettings.ambientIntensity = _ambientIntensity;
            RenderSettings.fog = _fog;
        }
    }
}
