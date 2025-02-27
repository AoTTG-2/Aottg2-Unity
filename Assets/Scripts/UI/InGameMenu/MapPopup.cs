using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections.Generic;
using Cameras;
using ApplicationManagers;
using Characters;

namespace UI
{
    class MapPopup : BasePopup
    {
        protected override string Title => "Map";
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        protected override float MinFadeAlpha => 0.5f;
        protected override float Width => 900f;
        protected override float Height => 1030f;
        public string LocaleCategory = "MapPopup";
        private float _height = 2000f;
        private Dictionary<Transform, Transform> _icons = new Dictionary<Transform, Transform>();
        private RawImage _background;
        private Texture2D _texture;
        private const float SyncDelay = 1f;
        private float _syncTimeLeft = 1f;
        private Vector3 _syncPosition = Vector3.zero;
        private List<Transform> _iconsToRemove = new List<Transform>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();
            _background = ElementFactory.CreateRawImage(transform, new ElementStyle(), "", MinimapCamera.MapSize, MinimapCamera.MapSize).GetComponent<RawImage>();
            _background.GetComponent<LayoutElement>().ignoreLayout = true;
            _background.GetComponent<RectTransform>().sizeDelta = new Vector2(MinimapCamera.MapSize, MinimapCamera.MapSize);

        }

        public override void Show()
        {
            base.Show();
            Sync();
            _syncTimeLeft = SyncDelay;
        }

        private void Update()
        {
            _syncTimeLeft -= Time.deltaTime;
            if (_syncTimeLeft <= 0f)
            {
                Sync();
                _syncTimeLeft = SyncDelay;
            }
            UpdateIcons();
        }

        private void UpdateIcons()
        {
            _iconsToRemove.Clear();
            foreach (var transform in _icons.Keys)
            {
                var icon = _icons[transform];
                bool dead = false;
                if (transform != null)
                {
                    var character = transform.GetComponent<BaseCharacter>();
                    dead = character != null && character.Dead;
                }
                if (transform == null || dead)
                {
                    _iconsToRemove.Add(transform);
                    Destroy(icon.gameObject);
                }
                else
                {
                    var relativePosition = new Vector3(transform.position.x - _syncPosition.x, transform.position.z - _syncPosition.z, 0f);
                    relativePosition *= (MinimapCamera.MapSize / _height);
                    if (relativePosition.magnitude > (MinimapCamera.MapSize * 0.5f))
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

        private void Sync()
        {
            SyncIcons();
            if (SceneLoader.MinimapCamera.Ready())
            {
                if (_texture != null)
                    Destroy(_texture);
                _texture = new Texture2D(MinimapCamera.MapSize, MinimapCamera.MapSize, TextureFormat.RGB24, false);
                var camera = (InGameCamera)SceneLoader.CurrentCamera;
                var position = camera.Cache.Transform.position;
                SceneLoader.MinimapCamera.TakeSnapshot(new Vector3(position.x, _height, position.z), _height, _texture, false, true);
                _background.texture = _texture;
                _syncPosition = position;
            }
        }

        private void SyncIcons()
        {
            var icons = MinimapHandler.GetIcons();
            if (icons == null)
                return;
            foreach (var transform in icons.Keys)
            {
                if (transform == null || _icons.ContainsKey(transform))
                    continue;
                var go = Instantiate(icons[transform].gameObject);
                go.transform.SetParent(_background.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.rotation = Quaternion.identity;
                go.SetActive(false);
                _icons.Add(transform, go.transform);
            }
        }

        private void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, "Zoom In",
                    onClick: () => OnBottomBarButtonClick("Plus"));
            ElementFactory.CreateTextButton(BottomBar, style, "Zoom Out",
                    onClick: () => OnBottomBarButtonClick("Minus"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"),
                    onClick: () => OnBottomBarButtonClick("Back"));
        }

        private void OnBottomBarButtonClick(string name)
        {
            switch (name)
            {
                case "Back":
                    ((InGameMenu)UIManager.CurrentMenu).SetMapMenu(false, true);
                    break;
                case "Plus":
                    _height = Mathf.Max(_height - 500f, 100f);
                    Sync();
                    break;
                case "Minus":
                    _height = _height + 500f;
                    Sync();
                    break;
            }
        }
    }
}
