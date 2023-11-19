using GameProgress;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class QuestAchievementsPanel : QuestCategoryPanel
    {
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SinglePanel.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(10, 25, VerticalPadding, VerticalPadding);
            Transform header = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/MainMenu/AchievementHeader").transform;
            header.GetComponent<LayoutElement>().preferredWidth = QuestItemWidth;
            header.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(10, 10, 0, 0);
            QuestPopup popup = (QuestPopup)parent;
            popup.CreateAchievementDropdowns(header.Find("LeftPanel"));
            AchievementCount count = GameProgressManager.GameProgress.Achievement.GetAchievementCount();
            header.Find("RightPanel/TrophyCountBronze/Label").GetComponent<Text>().text = count.FinishedBronze.ToString() + "/" + count.TotalBronze.ToString();
            header.Find("RightPanel/TrophyCountSilver/Label").GetComponent<Text>().text = count.FinishedSilver.ToString() + "/" + count.TotalSilver.ToString();
            header.Find("RightPanel/TrophyCountGold/Label").GetComponent<Text>().text = count.FinishedGold.ToString() + "/" + count.TotalGold.ToString();
            header.Find("RightPanel/TrophyCountBronze/Image").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "Trophy", "BronzeColor");
            header.Find("RightPanel/TrophyCountSilver/Image").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "Trophy", "SilverColor");
            header.Find("RightPanel/TrophyCountGold/Image").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "Trophy", "GoldColor");
            header.Find("RightPanel/TrophyCountBronze/Label").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "Trophy", "TextColor");
            header.Find("RightPanel/TrophyCountSilver/Label").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "Trophy", "TextColor");
            header.Find("RightPanel/TrophyCountGold/Label").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "Trophy", "TextColor");
            List<QuestItem> filtered = new List<QuestItem>();
            foreach (AchievementItem item in GameProgressManager.GameProgress.Achievement.AchievementItems.Value)
            {
                if (popup.TierSelection.Value != item.Tier.Value)
                    continue;
                if (popup.CompletedSelection.Value == "Completed" && !item.Finished())
                    continue;
                if (popup.CompletedSelection.Value == "In Progress" && item.Finished())
                    continue;
                filtered.Add(item);
            }
            CreateQuestItems(filtered);
        }
    }
}
