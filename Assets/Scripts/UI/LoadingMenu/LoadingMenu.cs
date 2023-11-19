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

namespace UI
{
    class LoadingMenu : BaseMenu
    {
        private LoadingBackgroundPanel _backgroundPanel;
        private LoadingProgressPanel _progessPanel;
        private TipPanel _tipPanel;

        public override void Setup()
        {
            _backgroundPanel = ElementFactory.CreateDefaultPopup<LoadingBackgroundPanel>(transform);
            _progessPanel = ElementFactory.CreateDefaultPopup<LoadingProgressPanel>(transform);
            _tipPanel = ElementFactory.CreateTipPanel(transform, enabled: false);
            ElementFactory.SetAnchor(_tipPanel.gameObject, TextAnchor.LowerRight, TextAnchor.LowerRight, new Vector2(10f, -10f));
        }

        public void Show(bool immediate = false)
        {
            if (!_tipPanel.gameObject.activeSelf)
            {
                _tipPanel.SetRandomTip();
                _tipPanel.gameObject.SetActive(true);
                _backgroundPanel.SetRandomBackground(true);
                if (immediate)
                    _backgroundPanel.ShowImmediate();
                else
                    _backgroundPanel.Show();
                UpdateLoading(0f, false, immediate);
            }
        }

        public void Hide()
        {
            _tipPanel.gameObject.SetActive(false);
            _progessPanel.Hide();
            _backgroundPanel.Hide();
        }

        public void UpdateLoading(float percentage, bool finished = false, bool immediate = false)
        {
            percentage = Mathf.Clamp(percentage, 0f, 1f);
            if (immediate)
                _progessPanel.ShowImmediate();
            else
                _progessPanel.Show(percentage);
            if (finished)
            {
                ((InGameMenu)UIManager.CurrentMenu).OnFinishLoading();
                StartCoroutine(WaitAndHide());
            }
        }

        private IEnumerator WaitAndHide()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            Hide();
        }
    }
}
