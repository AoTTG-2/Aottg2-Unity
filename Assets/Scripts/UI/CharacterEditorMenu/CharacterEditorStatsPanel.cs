using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using GameManagers;
using Characters;
using static UnityEngine.Rendering.DebugUI;

namespace UI
{
    class CharacterEditorStatsPanel: HeadedPanel
    {
        protected override string Title => UIManager.GetLocale("CharacterEditor", "Stats", "Title");
        protected override float Width => 330f;
        protected override float Height => 360f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 25;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 130f, themePanel: ThemePanel);
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            HumanCustomSet set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            var stats = HumanStats.Deserialize(new HumanStats(null), set.Stats.Value);
            string cat = "CharacterEditor";
            string sub = "Stats";
            CreateStatBar(UIManager.GetLocale(cat, sub, "Acceleration"), stats.Acceleration);
            CreateStatBar(UIManager.GetLocale(cat, sub, "Speed"), stats.Speed);
            CreateStatBar(UIManager.GetLocale(cat, sub, "Gas"), stats.Gas);
            CreateStatBar(UIManager.GetLocale(cat, sub, "Ammunition"), stats.Ammunition);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocale(cat, sub, "EditStats"), onClick: () => OnButtonClick("EditStats"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocale(cat, sub, "EditPerks"), onClick: () => OnButtonClick("EditPerks"));
        }

        protected void CreateStatBar(string title, int value)
        {
            var statbar = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/Misc/StatBar").transform;
            float percentage = Mathf.Clamp((value - 50f) / 50f, 0f, 1f);
            statbar.Find("Label").GetComponent<Text>().text = title;
            statbar.Find("Label").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "DefaultLabel", "TextColor");
            statbar.Find("ProgressBar").GetComponent<Slider>().value = percentage;
            statbar.Find("ProgressBar/Background").GetComponent<Image>().color = UIManager.GetThemeColor("QuestPopup", "QuestItem", "ProgressBarBackgroundColor");
            statbar.Find("ProgressBar/Fill Area/Fill").GetComponent<Image>().color = UIManager.GetThemeColor("QuestPopup", "QuestItem", "ProgressBarFillColor");
        }

        protected void OnButtonClick(string button)
        {
            if (button == "EditStats")
            {
                ((CharacterEditorMenu)UIManager.CurrentMenu)._editStatsPopup.Show();
            }
            else if (button == "EditPerks")
            {
                ((CharacterEditorMenu)UIManager.CurrentMenu)._editPerksPopup.Show();
            }
        }
    }
}
