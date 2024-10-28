using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using Characters;
using GameManagers;
using ApplicationManagers;
using Utility;
using Cameras;
using UnityEngine.Rendering;

namespace UI
{
    class MinimapHandler : MonoBehaviour
    {
        public static Transform CameraTransform;
        private static Dictionary<string, Material> _cache = new Dictionary<string, Material>();
        private float _height;
        private static Color _mineColor = new Color(0.455f, 0.608f, 0.816f);
        private static Color _titanColor = new Color(1f, 1f, 0.44f);
        private static Color _humanColor = new Color(0.58f, 1f, 0.5f);
        private static Color _teamBlueColor = new Color(0f, 0.67f, 1f);
        private static Color _teamRedColor = new Color(0.87f, 0.25f, 0.25f);
        private GameObject _minimapPanel;
        private Text _positionLabel;
        private Text _compassLabel;

        private void Awake()
        {
            var go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Minimap/Prefabs/MinimapCamera", Vector3.zero, Quaternion.identity);
            CameraTransform = go.transform;
            _height = SettingsManager.GeneralSettings.MinimapHeight.Value;
            go.GetComponent<Camera>().orthographicSize = _height;
            go.GetComponent<Camera>().farClipPlane = _height + 1000f;
            go.AddComponent<MinimapCameraComponent>();
        }

        public void Disable()
        {
            CameraTransform.gameObject.SetActive(false);
        }

        public static void CreateMinimapIcon(Transform transform, string type)
        {
            if (!CameraTransform.gameObject.activeSelf)
                return;
            string texture = "Minimap/Textures/MinimapSupplyIcon";
            var go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Minimap/Prefabs/MinimapIcon", true);
            if (!_cache.ContainsKey(texture))
            {
                go.GetComponent<Renderer>().material.SetTexture("_MainTex", (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, texture, true));
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                _cache.Add(texture, go.GetComponent<Renderer>().material);
            }
            else
                go.GetComponent<Renderer>().material = _cache[texture];
            var follow = go.AddComponent<MinimapIconFollow>();
            follow.Init(CameraTransform, transform);
        }

        public static void CreateMinimapIcon(BaseCharacter character)
        {
            if (!CameraTransform.gameObject.activeSelf)
                return;
            string texture;
            Color color = Color.white;
            string team = character.Team;
            if (team == TeamInfo.None)
                team = character is Human ? TeamInfo.Human : TeamInfo.Titan;
            if (character.IsMainCharacter())
            {
                team = "Mine";
                color = _mineColor;
            }
            if (character is Human)
                texture = "Minimap/Textures/MinimapHumanIcon";
            else
                texture = "Minimap/Textures/MinimapTitanIcon";
            if (team == TeamInfo.Human)
                color = _humanColor;
            else if (team == TeamInfo.Titan)
                color = _titanColor;
            else if (team == TeamInfo.Blue)
                color = _teamBlueColor;
            else if (team == TeamInfo.Red)
                color = _teamRedColor;
            var go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Minimap/Prefabs/MinimapIcon", true);
            string hash = texture + team;
            if (!_cache.ContainsKey(hash))
            {
                go.GetComponent<Renderer>().material.SetTexture("_MainTex", (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, texture, true));
                go.GetComponent<Renderer>().material.SetColor("_Color", color);
                _cache.Add(hash, go.GetComponent<Renderer>().material);
            }
            else
                go.GetComponent<Renderer>().material = _cache[hash];
            var follow = go.AddComponent<MinimapIconFollow>();
            follow.Init(CameraTransform, character);
        }

        private void Update()
        {
            if (!CameraTransform.gameObject.activeSelf)
                return;
            var camera = (InGameCamera)SceneLoader.CurrentCamera;
            var position = camera.Cache.Transform.position;
            CameraTransform.position = new Vector3(position.x, position.y + _height, position.z);
            float y = camera.Cache.Transform.rotation.eulerAngles.y;
            CameraTransform.rotation = Quaternion.Euler(new Vector3(90f, y, 0f));
            if (_minimapPanel == null)
            {
                _minimapPanel = ((InGameMenu)UIManager.CurrentMenu)._minimapPanel;
                _positionLabel = _minimapPanel.transform.Find("PositionLabel").GetComponent<Text>();
                _compassLabel = _minimapPanel.transform.Find("CompassLabel").GetComponent<Text>();
            }
            if (_minimapPanel != null && _minimapPanel.activeSelf)
            {
                if (y < 0f)
                    y += 360f;
                if (y > 360f)
                    y -= 360f;
                string compass = "";
                if (y >= 22.5f && y <= 202.5f)
                {
                    if (y >= 22.5f && y <= 67.5f)
                        compass = "NE";
                    else if (y >= 67.5f && y <= 112.5f)
                        compass = "E";
                    else if (y >= 112.5f && y <= 157.5f)
                        compass = "SE";
                    else if (y >= 157.5f && y <= 202.5f)
                        compass = "S";
                }
                else if (y >= 202.5f && y <= 337.5f)
                {
                    if (y >= 202.5f && y <= 247.5f)
                        compass = "SW";
                    else if (y >= 247.5f && y <= 292.5f)
                        compass = "W";
                    else if (y >= 292.5f && y <= 337.5f)
                        compass = "NW";
                }
                else if (y >= 337.5f || y <= 22.5f)
                    compass = "N";
                _compassLabel.text = compass;
                if (SettingsManager.UISettings.Coordinates.Value == (int)CoordinateMode.Minimap)
                {
                    if (camera._follow != null)
                        position = camera._follow.Cache.Transform.position;
                    _positionLabel.text = position.x.ToString("F0") + ", " + position.y.ToString("F0") + ", " + position.z.ToString("F0");
                }
                else
                    _positionLabel.text = "";
            }
        }
    }
}
