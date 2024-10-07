using GameProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class QuestDailyPanel : QuestCategoryPanel
    {
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            GameObject timeLabel = ElementFactory.CreateDefaultLabel(SinglePanel, new ElementStyle(themePanel: ThemePanel),
               QuestHandler.GetTimeToQuestReset(true), alignment: TextAnchor.MiddleLeft);
            timeLabel.GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "QuestHeader", "ResetTextColor");
            CreateQuestItems(GameProgressManager.GameProgress.Quest.DailyQuestItems.Value);
        }
    }
}
