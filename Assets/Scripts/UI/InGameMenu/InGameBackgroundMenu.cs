using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ApplicationManagers;
using Settings;
using SimpleJSONFixed;
using System.Collections;

namespace UI
{
    class InGameBackgroundMenu : BaseMenu
    {
        private BloodBackgroundPanel _bloodBackgroundPanel;

        public override void Setup()
        {
            _bloodBackgroundPanel = ElementFactory.CreateDefaultPopup<BloodBackgroundPanel>(transform);
        }

        public void ShowBlood()
        {
            _bloodBackgroundPanel.Show();
        }

        public void HideBlood()
        {
            _bloodBackgroundPanel.Hide();
        }
    }
}
