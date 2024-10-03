using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    public class TextButton : ButtonBase
    {
        private Text textComponent;

        protected virtual void Awake()
        {
            textComponent = GetComponent<Text>();
            if (textComponent == null)
            {
                Debug.LogError("TextButton requires a Text component!");
                return;
            }
            SetDefaultColor();
        }

        private void SetDefaultColor()
        {
            Color defaultColor = ColorPalette.White;
            defaultColor.a = 0.5f;
            textComponent.color = defaultColor;
        }

        protected override void OnHover()
        {
            textComponent.color = ColorPalette.PrimaryLight;
        }

        protected override void OnExit()
        {
            SetDefaultColor();
        }

        protected override void OnPress()
        {
            textComponent.color = ColorPalette.White;
        }

        protected override void OnRelease()
        {
            OnExit();
        }
    }
}