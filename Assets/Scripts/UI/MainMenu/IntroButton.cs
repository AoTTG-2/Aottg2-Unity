using ApplicationManagers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class IntroButton: Button
    {
        private float _fadeTime = 0.1f;
        private Image _hoverImage;

        protected override void Awake()
        {
            _hoverImage = transform.Find("HoverImage").GetComponent<Image>();
            _hoverImage.canvasRenderer.SetAlpha(0f);
            transition = Transition.ColorTint;
            targetGraphic = transform.Find("Label").GetComponent<Graphic>();
            if (gameObject.name.StartsWith("Settings") || gameObject.name.StartsWith("Quit") || gameObject.name.StartsWith("Profile"))
                targetGraphic.GetComponent<Text>().text = UIManager.GetLocaleCommon(gameObject.name.Replace("Button", string.Empty));
            else
                targetGraphic.GetComponent<Text>().text = UIManager.GetLocale("MainMenu", "Intro", gameObject.name);
            ColorBlock block = new ColorBlock
            {
                colorMultiplier = 1f,
                fadeDuration = _fadeTime,
                normalColor = new Color(0.2f, 0.2f, 0.2f),
                highlightedColor = new Color(1f, 1f, 1f),
                pressedColor = new Color(0.5f, 0.5f, 0.5f),
                disabledColor = new Color(0.5f, 0.5f, 0.5f)
            };
            colors = block;
            navigation = new Navigation { mode = Navigation.Mode.None };
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            var menu = ((MainMenu)UIManager.CurrentMenu);
            if (state == SelectionState.Highlighted)
                UIManager.PlaySound(UISound.Hover);
            if (state == SelectionState.Pressed || state == SelectionState.Highlighted)
            {
                _hoverImage.CrossFadeAlpha(1f, _fadeTime, true);
            }
            else if (state == SelectionState.Normal)
            {
                _hoverImage.CrossFadeAlpha(0f, _fadeTime, true);
            }
        }
    }
}
