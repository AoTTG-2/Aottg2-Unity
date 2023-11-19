using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class PromptPopup : BasePopup
    {
        protected override float TopBarHeight => 55f;
        protected override float BottomBarHeight => 55f;
        protected override int TitleFontSize => 26;
        protected override int ButtonFontSize => 22;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            GameObject dim = ElementFactory.InstantiateAndBind(transform, "Prefabs/Panels/BackgroundDim");
            dim.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.3f);
            dim.AddComponent<IgnoreScaler>();
            dim.transform.SetSiblingIndex(0);
            _staticTransforms.Add(dim.transform);
        }
    }
}
