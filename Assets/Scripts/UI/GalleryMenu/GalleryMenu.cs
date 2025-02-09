using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ApplicationManagers;
using Settings;
using Characters;
using GameManagers;
using System.Collections;
using Utility;

namespace UI
{
    class GalleryMenu: BaseMenu
    {
        private GalleryMainPanel _mainPanel;
        private GameObject _picture;
        private int _backgroundCount;
        private int _profileCount;
        public int TotalBackgroundCount;

        public override void Setup()
        {
            base.Setup();
            _backgroundCount = MainMenu.MainBackgroundInfo["AllBackgrounds"].AsInt;
            _profileCount = UIManager.AvailableProfileIcons.Count;
            TotalBackgroundCount = _backgroundCount + _profileCount;
            _mainPanel = ElementFactory.CreateHeadedPanel<GalleryMainPanel>(transform, enabled: true).GetComponent<GalleryMainPanel>();
            _picture = ElementFactory.InstantiateAndBind(transform, "Prefabs/Misc/GalleryImage");
            ElementFactory.SetAnchor(_picture.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(0f, -30f));
            ElementFactory.SetAnchor(_mainPanel.gameObject, TextAnchor.UpperCenter, TextAnchor.UpperCenter, new Vector2(0f, 0f));
            LoadGallery(0);
        }

        public void LoadGallery(int index)
        {
            bool isBackground = true;
            if (index >= _backgroundCount)
            {
                isBackground = false;
                index -= _backgroundCount;
            }
            if (isBackground)
            {
                _picture.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 1f);
                SetBackground("Backgrounds/MainBackground" + (index).ToString() + "Texture");
            }
            else
            {
                _picture.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 0f);
                SetBackground("Icons/Profile/FullSize/" + UIManager.AvailableProfileIcons[index] + "IconFull");
            }
        }

        public void SetBackground(string image)
        {
            RawImage background = _picture.transform.Find("Background").GetComponent<RawImage>();
            try
            {
                Resources.UnloadUnusedAssets();
                background.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, image);
                float maxHeight = 990f;
                float maxWidth = 1900f;
                float width = background.texture.width;
                float height = background.texture.height;
                if (width > maxWidth)
                {
                    height = height * (maxWidth / width);
                    width = maxWidth;
                }
                if (height > maxHeight)
                {
                    width = width * (maxHeight / height);
                    height = maxHeight;
                }
                _picture.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            }
            catch
            {
                Debug.Log("Error loading " + image);
            }
        }
    }
}
