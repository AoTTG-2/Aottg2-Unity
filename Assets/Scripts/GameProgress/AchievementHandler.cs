using UnityEngine;
using System;
using System.Collections.Generic;
using ApplicationManagers;
using Settings;
using System.Linq;
using Utility;

namespace GameProgress
{
    class AchievementHandler : QuestHandler
    {
        private AchievementContainer _achievement;

        public AchievementHandler(AchievementContainer achievement): base(null)
        {
            _achievement = achievement;
            ReloadAchievements();
        }

        public void ReloadAchievements()
        {
            LoadAchievements();
            CacheActiveAchievements();
        }

        private void LoadAchievements()
        {
            ListSetting<AchievementItem> finalAchievementItems = new ListSetting<AchievementItem>();
            Dictionary<string, AchievementItem> currentAchievementDict = new Dictionary<string, AchievementItem>();
            foreach (AchievementItem item in _achievement.AchievementItems.Value)
            {
                currentAchievementDict.Add(item.GetQuestName(), item);
            }
            AchievementContainer defaultAchievement = new AchievementContainer();
            defaultAchievement.DeserializeFromJsonString(((TextAsset)ResourceManager.LoadAsset(ResourcePaths.Info, "AchievementList")).text);
            foreach (AchievementItem item in defaultAchievement.AchievementItems.Value)
            {
                if (currentAchievementDict.ContainsKey(item.GetQuestName()))
                {
                    AchievementItem current = currentAchievementDict[item.GetQuestName()];
                    item.Progress.Value = current.Progress.Value;
                }
                item.Active.Value = false;
                finalAchievementItems.Value.Add(item);
            }
            _achievement.AchievementItems.Copy(finalAchievementItems);
        }

        private void CacheActiveAchievements()
        {
            _activeQuests.Clear();
            Dictionary<string, List<AchievementItem>> achievementCategories = new Dictionary<string, List<AchievementItem>>();
            foreach (AchievementItem item in _achievement.AchievementItems.Value)
            {
                string id = item.Category.Value + item.GetConditionsHash();
                if (!achievementCategories.ContainsKey(id))
                    achievementCategories.Add(id, new List<AchievementItem>());
                achievementCategories[id].Add(item);
            }
            foreach (string category in achievementCategories.Keys)
            {
                List<AchievementItem> items = achievementCategories[category].OrderBy(x => x.GetQuestName()).ToList();
                AchievementItem activeItem = null;
                foreach (AchievementItem item in items)
                {
                    if (item.Progress.Value < item.Amount.Value)
                    {
                        activeItem = item;
                        break;
                    }
                }
                if (activeItem == null)
                    continue;
                activeItem.Active.Value = true;
                AddActiveQuest(activeItem);
            }
        }
    }

    public enum AchievementTier
    {
        Bronze,
        Silver,
        Gold
    }
}
