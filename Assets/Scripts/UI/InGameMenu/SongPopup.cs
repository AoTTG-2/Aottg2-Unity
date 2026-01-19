using ApplicationManagers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class SongPopup : BasePopup
    {
        ushort ienumCount = 0;
        Text _nextSongPanelLabel;
        protected override float Width => 225f;
        protected override float Height => 65f;
        protected override bool DoublePanel => true;
        protected override bool DoublePanelDivider => false;
        protected override float VerticalSpacing => 0f;
        protected override int VerticalPadding => 0;
        protected override int HorizontalPadding => 0;
        protected override float TopBarHeight => 0;
        protected override float BottomBarHeight => 0;
        protected override float BottomBarSpacing => 0;
        protected override int BottomBarPadding => 0;
        protected override int TitleFontSize => 0;
        protected override int ButtonFontSize => 0;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);

            var doublePanelLeftLayout = DoublePanelLeft.GetComponent<LayoutElement>();
            doublePanelLeftLayout.preferredWidth = 55;
            doublePanelLeftLayout.preferredHeight = Height;
            var doublePanelLeftVerticalLayoutGroup = DoublePanelLeft.GetComponent<VerticalLayoutGroup>();
            doublePanelLeftVerticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            doublePanelLeftVerticalLayoutGroup.childControlHeight = false;
            doublePanelLeftVerticalLayoutGroup.childControlWidth = false;
            var doublePanelRightLayout = DoublePanelRight.GetComponent<LayoutElement>();
            doublePanelRightLayout.preferredWidth = 150;
            doublePanelRightLayout.preferredHeight = Height;
            var doublePanelRightVerticalLayoutGroup = DoublePanelRight.GetComponent<VerticalLayoutGroup>();
            doublePanelRightVerticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            DoublePanelLeft.transform.parent.GetComponent<HorizontalLayoutGroup>().spacing = 10;
            Parent = parent;
            var coverImage = ElementFactory.CreateRawImage(DoublePanelLeft.transform, new ElementStyle(), "Sprites/ost_cover").GetComponent<RawImage>();
            coverImage.GetComponent<RectTransform>().sizeDelta = new Vector2(55, 55);
            var _nextSongPanelLabelElement = ElementFactory.CreateDefaultLabel(DoublePanelRight.transform, new ElementStyle(), "");
            _nextSongPanelLabel = _nextSongPanelLabelElement.GetComponent<Text>();
            _nextSongPanelLabel.color = Color.white;
            _nextSongPanelLabel.fontSize = 16;
            ElementFactory.SetAnchor(coverImage.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(20, 0));
            ElementFactory.SetAnchor(gameObject, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-350f, -10f));
            ElementFactory.SetAnchor(_nextSongPanelLabelElement, TextAnchor.MiddleLeft, TextAnchor.MiddleLeft, new Vector2(250f, 0));
            Hide();
        }
        public void ChangeSongInfo(string name)
        {
            ParseMusicString(name, out var authors, out var song);
            var newText = song;
            for(var i = 0; i < authors.Count; i++)
            {
                if (i == 0)
                {
                    newText += " by " + authors[i];
                }
                else
                {
                    newText += " and " + authors[i];
                }
            }
            _nextSongPanelLabel.text = newText;
        }
        public static void ParseMusicString(string input, out List<string> authors, out string song)
        {
            authors = new List<string>();
            song = string.Empty;
            if (string.IsNullOrWhiteSpace(input))
                return;
            var parts = input.Split(new[] { '_' }, 2);

            string authorPart = parts[0];
            if (parts.Length > 1)
            {
                song = parts[1]
                    .Replace("-", " ")
                    .Trim();
            }
            var authorParts = authorPart.Split(new[] { "--" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var author in authorParts)
            {
                var cleanAuthor = author
                    .Replace("-", " ")
                    .Trim();

                if (!string.IsNullOrWhiteSpace(cleanAuthor))
                    authors.Add(cleanAuthor);
            }
        }
        public IEnumerator ShowNextSongPopup()
        {
            ChangeSongInfo(MusicManager.GetCurrentSong());
            ienumCount++;
            Show();
            yield return new WaitForSeconds(5f);
            ienumCount--;
            if (ienumCount == 0)
            {
                Hide();
            }
        }
    }
}