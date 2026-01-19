using Settings;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

class NextSongPanel : SimplePanel
{
    Text _nextSongPanelLabel;
    protected override float Width => 400f;
    protected override float Height=> 100f;
    protected override bool DoublePanel => true;
    protected override bool DoublePanelDivider => false;
    protected override float VerticalSpacing => 0f;
    protected override int VerticalPadding => 0;
    protected override int HorizontalPadding => 0;

    public override void Setup(BasePanel parent = null)
    {
        base.Setup(parent);

        var doublePanelLeftLayout = DoublePanelLeft.GetComponent<LayoutElement>();
        doublePanelLeftLayout.preferredWidth = 100f;
        doublePanelLeftLayout.preferredHeight = Height;
        var doublePanelLeftVerticalLayoutGroup = DoublePanelLeft.GetComponent<VerticalLayoutGroup>();
        doublePanelLeftVerticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        doublePanelLeftVerticalLayoutGroup.childControlHeight = false;
        doublePanelLeftVerticalLayoutGroup.childControlWidth = false;
        var doublePanelRightLayout = DoublePanelRight.GetComponent<LayoutElement>();
        doublePanelRightLayout.preferredWidth = 300f;
        doublePanelRightLayout.preferredHeight = Height;
        var doublePanelRightVerticalLayoutGroup = DoublePanelRight.GetComponent<VerticalLayoutGroup>();
        doublePanelRightVerticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        Parent = parent;
        var coverImage = ElementFactory.CreateRawImage(DoublePanelLeft.transform, new ElementStyle(), "Sprites/ost_cover").GetComponent<RawImage>();
        coverImage.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
        var _nextSongPanelLabelElement = ElementFactory.CreateDefaultLabel(DoublePanelRight.transform, new ElementStyle(), "");
        _nextSongPanelLabel = _nextSongPanelLabelElement.GetComponent<Text>();
        _nextSongPanelLabel.color = Color.white;
        ElementFactory.SetAnchor(coverImage.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(20, 0));
        ElementFactory.SetAnchor(gameObject, TextAnchor.LowerRight, TextAnchor.LowerRight, new Vector2(-30f, 40f));
        ElementFactory.SetAnchor(_nextSongPanelLabelElement, TextAnchor.MiddleLeft, TextAnchor.MiddleLeft, new Vector2(250f, 0));
        Hide();
    }
    public void ChangeSongText(string name)
    {
        _nextSongPanelLabel.text = name;
    }
}