using System;
using UnityEngine;
using Settings;

namespace GameProgress
{
    class AchievementContainer: BaseSettingsContainer
    {
        public ListSetting<AchievementItem> AchievementItems = new ListSetting<AchievementItem>();

        public AchievementCount GetAchievementCount()
        {
            AchievementCount count = new AchievementCount();
            foreach (AchievementItem item in AchievementItems.Value)
            {
                if (item.Tier.Value == "Bronze")
                {
                    count.TotalBronze++;
                    if (item.Finished())
                        count.FinishedBronze++;
                }
                else if (item.Tier.Value == "Silver")
                {
                    count.TotalSilver++;
                    if (item.Finished())
                        count.FinishedSilver++;
                }
                else if (item.Tier.Value == "Gold")
                {
                    count.TotalGold++;
                    if (item.Finished())
                        count.FinishedGold++;
                }
            }
            count.TotalAll = count.TotalBronze + count.TotalSilver + count.TotalGold;
            count.FinishedAll = count.FinishedBronze + count.FinishedSilver + count.FinishedGold;
            return count;
        }
    }
}
