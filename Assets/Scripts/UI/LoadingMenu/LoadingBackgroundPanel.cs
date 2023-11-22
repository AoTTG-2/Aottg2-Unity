using ApplicationManagers;
using GameManagers;
using Settings;
using SimpleJSONFixed;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class LoadingBackgroundPanel : BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 0f;
        protected override float Height => 0f;
        protected override float TopBarHeight => 0f;
        protected override float BottomBarHeight => 0f;
        protected override bool ShowOnTop => false;
        protected GameObject _background;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        protected override float AnimationTime => 0.2f;
        public int BackgroundIndex = -1;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _background = ElementFactory.InstantiateAndBind(transform, "Prefabs/MainMenu/MainBackground");
        }

        public void SetRandomBackground(bool loading)
        {

            JSONNode backgrounds = loading ? MainMenu.MainBackgroundInfo["LoadingBackgrounds"] : MainMenu.MainBackgroundInfo["MainBackgrounds"];
            int backgroundIndex = BackgroundIndex;
            while (backgroundIndex == BackgroundIndex)
                backgroundIndex = Random.Range(0, backgrounds.Count);
            SetBackground(loading, backgroundIndex);
        }

        public void SetBackground(bool loading, int backgroundIndex)
        {
            BackgroundIndex = backgroundIndex;
            JSONNode backgrounds = loading ? MainMenu.MainBackgroundInfo["LoadingBackgrounds"] : MainMenu.MainBackgroundInfo["MainBackgrounds"];
            RawImage image = _background.transform.Find("Image").GetComponent<RawImage>();
            try
            {
                image.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Backgrounds/" + backgrounds[backgroundIndex].Value);
                float height = (1928f / image.texture.width) * image.texture.height;
                height = Mathf.Max(height, 1084f);
                image.GetComponent<RectTransform>().sizeDelta = new Vector2(1928f, height);
            }
            catch
            {
                Debug.Log("Error loading main background " + backgroundIndex.ToString());
            }
        }

        protected override float GetAnimmationSpeed(float min, float max)
        {
            if (SettingsManager.UISettings.FadeLoadscreen.Value)
                return base.GetAnimmationSpeed(min, max);
            return (max - min) / 0.01f;
        }
    }
}
