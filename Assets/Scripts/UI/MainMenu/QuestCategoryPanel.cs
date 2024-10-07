using ApplicationManagers;
using GameProgress;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class QuestCategoryPanel: CategoryPanel
    {
        protected override string ThemePanel => "QuestPopup";
        protected float QuestItemWidth = 940f;
        protected float QuestItemHeight = 100f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 20;
        protected override int VerticalPadding => 20;
        protected override TextAnchor PanelAlignment => TextAnchor.UpperCenter;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
        }

        protected void CreateQuestItems(List<QuestItem> items)
        {
            foreach (QuestItem item in items)
            {
                Transform panel = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/MainMenu/QuestItemPanel").transform;
                panel.GetComponent<LayoutElement>().preferredWidth = QuestItemWidth;
                panel.GetComponent<LayoutElement>().preferredHeight = QuestItemHeight;
                panel.Find("Panel/Icon").GetComponent<RawImage>().texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, 
                    "Icons/Quests/" + item.Icon.Value + "Icon", cached: true);
                SetTitle(item, panel);
                SetRewardLabel(item, panel);
                SetProgress(item, panel);
                panel.Find("Background").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "QuestItem", "BackgroundColor");
                panel.Find("Panel/CheckIcon").gameObject.SetActive(item.Finished());
                panel.Find("Border").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "QuestItem", "BorderColor");
                panel.Find("Panel/Icon").GetComponent<RawImage>().color = UIManager.GetThemeColor(ThemePanel, "QuestItem", "IconColor");
                panel.Find("Panel/CheckIcon").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "QuestItem", "CheckColor");
                panel.Find("Panel/Title").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "QuestItem", "TextColor");
                panel.Find("Panel/ProgressLabel").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "QuestItem", "TextColor");
                panel.Find("Panel/RewardLabel").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "QuestItem", "TextColor");
                panel.Find("Panel/CheckIcon").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "QuestItem", "IconColor");
                panel.Find("Panel/ProgressBar/Background").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "QuestItem", "ProgressBarBackgroundColor");
                panel.Find("Panel/ProgressBar/Fill Area/Fill").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "QuestItem", "ProgressBarFillColor");
            }
        }

        protected void SetRewardLabel(QuestItem item, Transform panel)
        {
            if (item is AchievementItem)
            {
                panel.Find("Panel/RewardLabel").gameObject.SetActive(false);
                panel.Find("Panel/AchievementIcon").gameObject.SetActive(true);
                panel.Find("Panel/AchievementIcon").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "Trophy", ((AchievementItem)item).Tier.Value + "Color");
            }
            else
            {
                panel.Find("Panel/RewardLabel").gameObject.SetActive(true);
                panel.Find("Panel/AchievementIcon").gameObject.SetActive(false);
                if (item.RewardType.Value == "Exp")
                {
                    panel.Find("Panel/RewardLabel").GetComponent<Text>().text = "+" + item.RewardValue.Value + " exp";
                }
            }
        }

        protected void SetTitle(QuestItem item, Transform panel)
        {
            string name = item.Category.Value;
            string locale = UIManager.GetLocale("QuestItems", name, "");
            Dictionary<string, string> conditionToValue = new Dictionary<string, string>();
            foreach (StringSetting setting in item.Conditions.Value)
            {
                string[] strArray = setting.Value.Split(':');
                conditionToValue.Add(strArray[0], strArray[1]);
            }
            string finalTitle = "";
            try
            {
                for (int i = 0; i < locale.Length; i++)
                {
                    if (locale[i] == '{')
                    {
                        finalTitle += HandleConditionVariable(locale, i, conditionToValue);
                        i = locale.IndexOf('}', i);
                    }
                    else if (locale[i] == '[')
                    {
                        int closingPar = locale.IndexOf(']', i);
                        int openingCurly = locale.IndexOf('{', i);
                        int closingCurly = locale.IndexOf('}', i);
                        string condition = HandleConditionVariable(locale, openingCurly, conditionToValue);
                        if (condition != string.Empty)
                            finalTitle += locale.Substring(i + 1, openingCurly - i - 1) + condition + locale.Substring(closingCurly + 1, closingPar - closingCurly - 1);
                        i = closingPar;
                    }
                    else
                        finalTitle += locale[i].ToString();
                }
            }
            catch
            {
                finalTitle = "Locale error.";
            }
            panel.Find("Panel/Title").GetComponent<Text>().text = finalTitle;
        }

        private string HandleConditionVariable(string locale, int index, Dictionary<string, string> conditionToValue)
        {
            int closingCurly = locale.IndexOf('}', index);
            string condition = locale.Substring(index + 1, closingCurly - index - 1);
            if (conditionToValue.ContainsKey(condition))
            {
                string conditionLocale = UIManager.GetLocale("QuestItems", condition + "." + conditionToValue[condition], defaultValue: "Error");
                if (conditionLocale == "Error")
                    return conditionToValue[condition];
                return conditionLocale;
            }
            return string.Empty;
        }

        protected void SetProgress(QuestItem item, Transform panel)
        {
            panel.Find("Panel/ProgressBar").GetComponent<Slider>().value = (item.Progress.Value / (float)item.Amount.Value);
            panel.Find("Panel/ProgressLabel").GetComponent<Text>().text = item.Progress.Value.ToString() + " / " + item.Amount.Value.ToString();
        }
    }
}
