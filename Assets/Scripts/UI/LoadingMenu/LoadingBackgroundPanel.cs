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

        public void SetRandomBackground(bool loading, bool seasonal=false)
        {
            string seasonalPath = "EventBackgrounds";
            string path = "MainBackgrounds";
            if (seasonal && MainMenu.MainBackgroundInfo.HasKey(seasonalPath))
            {
                JSONNode seasonals = MainMenu.MainBackgroundInfo[seasonalPath];
                if (seasonals.AsArray.Count > 0)
                    path = seasonalPath;
            }
            JSONNode backgrounds = loading ? MainMenu.MainBackgroundInfo["LoadingBackgrounds"] : MainMenu.MainBackgroundInfo[path];
            int backgroundIndex = BackgroundIndex;
            while (backgroundIndex == BackgroundIndex)
                backgroundIndex = Random.Range(0, backgrounds.Count);
            SetBackground(loading, backgroundIndex, path);
        }

        public void SetBackground(bool loading, int backgroundIndex, string path= "MainBackgrounds")
        {
            BackgroundIndex = backgroundIndex;
            JSONNode backgrounds = loading ? MainMenu.MainBackgroundInfo["LoadingBackgrounds"] : MainMenu.MainBackgroundInfo[path];
            RawImage image = _background.transform.Find("Image").GetComponent<RawImage>();
            try
            {
                if (BackgroundIndex < 0)
                    image.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Backgrounds/MainBackgroundBlankTexture");
                else
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

        protected override float GetAnimationSpeed(float min, float max)
        {
            if (SettingsManager.UISettings.FadeLoadscreen.Value)
                return base.GetAnimationSpeed(min, max);
            return (max - min) / 0.01f;
        }
    }
}
