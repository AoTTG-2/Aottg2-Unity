using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using System.Collections;
using CustomSkins;
using Utility;

namespace UI
{
    class CrosshairHandler : MonoBehaviour
    {
        public RawImage _crosshairImageWhite;
        public RawImage _crosshairImageRed;
        public Text _crosshairLabelWhite;
        public Text _crosshairLabelRed;
        public Image _arrowLeft;
        public Image _arrowRight;

        private static Texture2D _crosshairSkinTexture;
        private static string _crosshairSkinURL;

        public void Awake()
        {
            _crosshairImageWhite = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/CrosshairImage").GetComponent<RawImage>();
            _crosshairImageRed = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/CrosshairImage").GetComponent<RawImage>();
            _crosshairImageRed.color = Color.red;
            _crosshairLabelWhite = _crosshairImageWhite.transform.Find("DefaultLabel").GetComponent<Text>();
            _crosshairLabelRed = _crosshairImageRed.transform.Find("DefaultLabel").GetComponent<Text>();
            _arrowLeft = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/HookArrowImage").GetComponent<Image>();
            _arrowRight = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/HookArrowImage").GetComponent<Image>();
            ElementFactory.SetAnchor(_crosshairImageWhite.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, Vector2.zero);
            ElementFactory.SetAnchor(_crosshairImageRed.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, Vector2.zero);
            _crosshairImageWhite.gameObject.AddComponent<CrosshairScaler>();
            _crosshairImageRed.gameObject.AddComponent<CrosshairScaler>();
            CursorManager.UpdateCrosshair(_crosshairImageWhite, _crosshairImageRed, _crosshairLabelWhite, _crosshairLabelRed, true);
            if (SettingsManager.UISettings.CrosshairSkin.Value != "")
                StartCoroutine(LoadSkin(SettingsManager.UISettings.CrosshairSkin.Value));
        }

        private IEnumerator LoadSkin(string url)
        {
            url = url.Trim();
            if (!TextureDownloader.ValidTextureURL(url))
                yield break;
            if (url != _crosshairSkinURL)
            {
                CoroutineWithData cwd = new CoroutineWithData(this, TextureDownloader.DownloadTexture(this, url, false, 1000 * 2000));
                yield return cwd.Coroutine;
                _crosshairSkinTexture = (Texture2D)cwd.Result;
                _crosshairSkinURL = url;
            }
            _crosshairImageWhite.texture = _crosshairSkinTexture;
            _crosshairImageRed.texture = _crosshairSkinTexture;
        }

        private void Update()
        {
            CursorManager.UpdateCrosshair(_crosshairImageWhite, _crosshairImageRed, _crosshairLabelWhite, _crosshairLabelRed);
            CursorManager.UpdateHookArrows(_arrowLeft, _arrowRight);
        }
    }
}
