using ApplicationManagers;
using GameManagers;
using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class CutsceneDialoguePanel: BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 600f;
        protected override float Height => 224f;
        protected override float TopBarHeight => 0f;
        protected override float BottomBarHeight => 0f;
        protected override int VerticalPadding => 10;
        protected override int HorizontalPadding => 20;
        protected override float VerticalSpacing => 10;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        protected override float AnimationTime => 0.5f;
        protected override string ThemePanel => "CutsceneDialoguePanel";
        private Text _contentLabel;
        private Text _titleLabel;
        private RawImage _image;
        private GameObject _labelRight;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(themePanel: ThemePanel);
            Transform group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
            ElementFactory.CreateHorizontalGroup(group, 0f);
            GameObject labelLeft = ElementFactory.CreateDefaultLabel(group, style, "", FontStyle.Normal, TextAnchor.MiddleLeft);
            GameObject labelRight = ElementFactory.CreateDefaultLabel(group, style, "Press " + SettingsManager.InputSettings.General.SkipCutscene.ToString() + " to skip", FontStyle.Normal, TextAnchor.MiddleRight);
            ElementFactory.CreateHorizontalGroup(group, 0f);
            labelLeft.GetComponent<LayoutElement>().preferredWidth = GetPanelWidth() / 2f - 30f;
            labelRight.GetComponent<LayoutElement>().preferredWidth = GetPanelWidth() / 2f - 30f;
            _labelRight = labelRight;
            _titleLabel = labelLeft.GetComponent<Text>();
            CreateHorizontalDivider(SinglePanel);
            ElementFactory.CreateHorizontalGroup(SinglePanel, 0f);
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 10f, TextAnchor.MiddleLeft).transform;
            _image = ElementFactory.CreateRawImage(group.transform, style, "Icons/Profile/Levi1Icon", 
                elementWidth: 128, elementHeight: 128).GetComponent<RawImage>();
            _contentLabel = ElementFactory.CreateDefaultLabel(group.transform, style, "", FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();
            _contentLabel.GetComponent<LayoutElement>().preferredHeight = 128f;
            _contentLabel.GetComponent<LayoutElement>().preferredWidth = GetPanelWidth() - HorizontalPadding * 2f - 128f - 10f;
        }

        public void Show(string icon, string title, string content, bool full)
        {
            SetTitle(title);
            icon = UIManager.GetProfileIcon(icon);
            _image.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Profile/" + icon, true);
            _titleLabel.text = title;
            _contentLabel.text = content;
            base.Show();
            _labelRight.SetActive(full);
        }
    }
}
