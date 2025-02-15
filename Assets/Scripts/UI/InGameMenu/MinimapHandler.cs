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
using System.Collections;
using System;
using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace UI
{
    class MinimapHandler : MonoBehaviour
    {
        public static MinimapHandler Instance;
        private float _height;
        private static Color _mineColor = new Color(0.455f, 0.608f, 0.816f);
        private static Color _titanColor = new Color(1f, 1f, 0.44f);
        private static Color _humanColor = new Color(0.58f, 1f, 0.5f);
        private static Color _teamBlueColor = new Color(0f, 0.67f, 1f);
        private static Color _teamRedColor = new Color(0.87f, 0.25f, 0.25f);
        private GameObject _minimapPanel;
        private Text _positionLabel;
        private Text _compassLabel;
        private RawImage[] _images = new RawImage[9];
        private Transform _tileTransform;
        private Transform _maskTransform;
        private Dictionary<Tuple<int, int>, Texture2D> _tileTextures = new Dictionary<Tuple<int, int>, Texture2D>();
        private Queue<Tuple<int, int>> _createTileQueue = new Queue<Tuple<int, int>>();
        private HashSet<Tuple<int, int>> _finishedTiles = new HashSet<Tuple<int, int>>();
        private Tuple<int, int> _currentTile = new Tuple<int, int>(0, 0);
        private Dictionary<Transform, Transform> _icons = new Dictionary<Transform, Transform>();
        private List<Transform> _iconsToRemove = new List<Transform>();
        private Vector3 _currentTileCenter = new Vector3();
        private bool _needUpdateTiles = true;
        private const int MaxTilesFromCenter = 2;

        private void Awake()
        {
            Instance = this;
            _height = SettingsManager.GeneralSettings.MinimapCameraHeight.Value;
            _minimapPanel = ((InGameMenu)UIManager.CurrentMenu)._minimapPanel;
            _positionLabel = _minimapPanel.transform.Find("PositionLabel").GetComponent<Text>();
            _compassLabel = _minimapPanel.transform.Find("CompassLabel").GetComponent<Text>();
            _maskTransform = _minimapPanel.transform.Find("Mask").transform;
            _tileTransform = _maskTransform.Find("Tiles").transform;
            for (int i = 0; i < 9; i++)
                _images[i] = _tileTransform.Find("Image" + i.ToString()).GetComponent<RawImage>();
        }

        public static void CreateMinimapIcon(Transform transform, string type)
        {
            if (Instance == null)
                return;
            string texture = "Minimap/Textures/MinimapSupplyIcon";
            SetupIcon(texture, Color.white, transform);
        }

        public static void CreateMinimapIcon(BaseCharacter character)
        {
            if (Instance == null)
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
            SetupIcon(texture, color, character.transform);
        }

        public static Dictionary<Transform, Transform> GetIcons()
        {
            if (Instance == null)
                return null;
            return Instance._icons;
        }

        private static void SetupIcon(string texture, Color color, Transform transform)
        {
            var go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Minimap/Prefabs/MinimapIcon", true);
            var image = go.GetComponent<RawImage>();
            image.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, texture, true);
            image.color = color;
            Instance._icons.Add(transform, go.transform);
            go.transform.SetParent(Instance._maskTransform);
            go.transform.localPosition = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_minimapPanel.activeSelf)
            {
                var camera = (InGameCamera)SceneLoader.CurrentCamera;
                var position = camera.Cache.Transform.position;
                float y = camera.Cache.Transform.rotation.eulerAngles.y;
                UpdateTiles(position, y);
                UpdateIcons(position, y);
                UpdateCompass(camera, position, y);
            }
        }

        private void UpdateCompass(InGameCamera camera, Vector3 position, float y)
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

        private void UpdateIcons(Vector3 position, float y)
        {
            _iconsToRemove.Clear();
            foreach (var transform in _icons.Keys)
            {
                var icon = _icons[transform];
                if (transform == null)
                {
                    _iconsToRemove.Add(transform);
                    Destroy(icon.gameObject);
                }
                else
                {
                    var relativePosition = new Vector3(transform.position.x - position.x, transform.position.z - position.z, 0f);
                    relativePosition *= (MinimapCamera.MinimapSize / _height);
                    if (relativePosition.magnitude > (MinimapCamera.MinimapSize * 0.5f))
                    {
                        if (icon.gameObject.activeSelf)
                            icon.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (!icon.gameObject.activeSelf)
                            icon.gameObject.SetActive(true);
                        icon.localPosition = relativePosition;
                        icon.rotation = Quaternion.identity;
                    }
                }
            }
            foreach (var transfrom in _iconsToRemove)
            {
                _icons.Remove(transfrom);
            }
        }

        private void UpdateTiles(Vector3 position, float rotation)
        {
            var currentTile = GetCurrentTile(position);
            if (currentTile.Item1 != _currentTile.Item1 || currentTile.Item2 != _currentTile.Item2)
                _needUpdateTiles = true;
            if (_needUpdateTiles)
            {
                _needUpdateTiles = false;
                _currentTile = currentTile;
                _currentTileCenter = GetTilePosition(currentTile);
                var tilesToRemove = new List<Tuple<int, int>>();
                foreach (var tile in _tileTextures.Keys)
                {
                    if (Mathf.Abs(tile.Item1 - currentTile.Item1) > MaxTilesFromCenter || Mathf.Abs(tile.Item2 - currentTile.Item2) > MaxTilesFromCenter)
                        tilesToRemove.Add(tile);
                }
                foreach (var tile in tilesToRemove)
                {
                    var texture = _tileTextures[tile];
                    _tileTextures.Remove(tile);
                    Destroy(texture);
                    if (_finishedTiles.Contains(tile))
                        _finishedTiles.Remove(tile);
                }
                _createTileQueue.Clear();
                if (!_finishedTiles.Contains(currentTile))
                    _createTileQueue.Enqueue(currentTile);
                for (int i = 0; i < 9; i++)
                {
                    var tile = Tuple.Create(currentTile.Item1 + (i % 3) - 1, currentTile.Item2 + 1 - (i / 3));
                    if (!_tileTextures.ContainsKey(tile))
                        _tileTextures[tile] = new Texture2D(MinimapCamera.MinimapSize, MinimapCamera.MinimapSize, TextureFormat.RGB24, false);
                    if (!_finishedTiles.Contains(tile) && tile != currentTile)
                        _createTileQueue.Enqueue(tile);
                    var texture = _tileTextures[tile];
                    _images[i].texture = texture;
                }
            }
            float ratio = MinimapCamera.MinimapSize / _height;
            float x = ratio * (_currentTileCenter.x - position.x);
            float y = ratio * (_currentTileCenter.z - position.z);
            _tileTransform.localPosition = new Vector3(x, y, 0f);
            _maskTransform.localRotation = Quaternion.Euler(0f, 0f, rotation);
            var minimapCamera = SceneLoader.MinimapCamera;
            if (_createTileQueue.Count > 0 && minimapCamera.Ready())
                CreateTile(_createTileQueue.Dequeue());
        }

        private Tuple<int, int> GetCurrentTile(Vector3 position)
        {
            float halfHeight = 0.5f * _height;
            if (position.x > 0)
                position.x += halfHeight;
            else if (position.x < 0)
                position.x -= halfHeight;
            if (position.z > 0)
                position.z += halfHeight;
            else if (position.z < 0)
                position.z -= halfHeight;
            int x = (int)(position.x / _height);
            int z = (int)(position.z / _height);
            return Tuple.Create(x, z);
        }

        private Vector3 GetTilePosition(Tuple<int, int> tile)
        {
            float x = tile.Item1 * _height;
            float z = tile.Item2 * _height;
            return new Vector3(x, _height, z);
        }

        private void CreateTile(Tuple<int, int> tile)
        {
            if (_tileTextures.ContainsKey(tile))
            {
                var texture = _tileTextures[tile];
                if (texture == null)
                    return;
                SceneLoader.MinimapCamera.TakeSnapshot(GetTilePosition(tile), _height, texture, true, false);
                _finishedTiles.Add(tile);
            }
        }
    }
}
