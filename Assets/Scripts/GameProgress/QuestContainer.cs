using System;
using UnityEngine;
using Settings;

namespace GameProgress
{
    class QuestContainer: BaseSettingsContainer
    {
        public ListSetting<QuestItem> DailyQuestItems = new ListSetting<QuestItem>();
        public ListSetting<QuestItem> WeeklyQuestItems = new ListSetting<QuestItem>();

        public void CollectRewards()
        {
            foreach (QuestItem item in DailyQuestItems.Value)
                item.CollectReward();
            foreach (QuestItem item in WeeklyQuestItems.Value)
                item.CollectReward();
        }
    }
}
