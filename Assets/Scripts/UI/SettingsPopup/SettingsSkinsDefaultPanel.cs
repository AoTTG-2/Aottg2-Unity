using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsSkinsDefaultPanel: SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        protected override float VerticalSpacing => 20f;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsSkinsPanel skinsPanel = (SettingsSkinsPanel)parent;
            skinsPanel.CreateCommonSettings(DoublePanelLeft, DoublePanelRight);
            CreateHorizontalDivider(DoublePanelRight);
            skinsPanel.CreateSkinStringSettings(DoublePanelLeft, DoublePanelRight, leftCount: 0);
        }
    }
}
