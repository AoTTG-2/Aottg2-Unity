using UnityEngine;
using System.Collections.Generic;
using Settings;
using ApplicationManagers;
using System;
using Utility;
using Characters;
using System.Globalization;

namespace GameProgress
{
    class QuestHandler : BaseGameProgressHandler
    {
        protected QuestContainer _quest;
        protected Dictionary<string, List<QuestItem>> _activeQuests = new Dictionary<string, List<QuestItem>>();
        const int DailyQuestCount = 3;
        const int WeeklyQuestCount = 3;
        
        // caching categories for performance
        protected string[] TitanKillCategories = new string[] { "KillTitan" };
        protected string[] HumanKillCategories = new string[] { "KillHuman" };
        protected string[] DamageCategories = new string[] { "DealDamage", "HitDamage" };
        protected string[] SpeedCategories = new string[] { "ReachSpeed" };
        protected string[] InteractionCategories = new string[] { "ShareGas", "CarryPlayer" };

        static Dictionary<string, KillWeapon> NameToKillWeapon = Util.EnumToDict<KillWeapon>();

        public QuestHandler(QuestContainer quest)
        {
            if (quest == null)
                return;
            _quest = quest;
            ReloadQuests();
        }

        public void ReloadQuests()
        {
            LoadQuests();
            CacheActiveQuests();
        }

        public static string GetTimeToQuestReset(bool daily)
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            if (daily)
            {
                int hoursLeft = 24 - t.Hours;
                return string.Format("Resets in: {0} {1}", hoursLeft, hoursLeft == 1 ? "hour" : "hours");
            }
            else
            {
                int day = (t.Days - 1) % 7;
                int daysLeft = 6 - day;
                int hoursLeft = 24 - t.Hours;
                return string.Format("Resets in: {0} {1}, {2} {3}", daysLeft, daysLeft == 1 ? "day": "days", hoursLeft, hoursLeft == 1 ? "hour" : "hours");
            }
        }

        private void LoadQuests()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int currentDay = t.Days;
            int currentWeek = (currentDay - 1) / 7;
            QuestContainer defaultQuest = new QuestContainer();
            defaultQuest.DeserializeFromJsonString(((TextAsset)ResourceManager.LoadAsset(ResourcePaths.Info, "QuestList")).text);
            ListSetting<QuestItem> newDailyQuestItems = new ListSetting<QuestItem>();
            foreach (QuestItem item in _quest.DailyQuestItems.Value)
            {
                if (item.DayCreated.Value == currentDay)
                    newDailyQuestItems.Value.Add(item);
            }
            newDailyQuestItems.Value.AddRange(CreateQuests(defaultQuest, currentDay, true, DailyQuestCount - newDailyQuestItems.Value.Count));
            ListSetting<QuestItem> newWeeklyQuestItems = new ListSetting<QuestItem>();
            foreach (QuestItem item in _quest.WeeklyQuestItems.Value)
            {
                int weekCreated = (item.DayCreated.Value - 1) / 7;
                if (weekCreated == currentWeek)
                    newWeeklyQuestItems.Value.Add(item);
            }
            newWeeklyQuestItems.Value.AddRange(CreateQuests(defaultQuest, currentDay, false, WeeklyQuestCount - newWeeklyQuestItems.Value.Count));
            _quest.DailyQuestItems.Copy(newDailyQuestItems);
            _quest.WeeklyQuestItems.Copy(newWeeklyQuestItems);
        }

        private List<QuestItem> CreateQuests(QuestContainer defaultQuest, int currentDay, bool daily, int count)
        {
            List<QuestItem> defaultQuests = daily ? defaultQuest.DailyQuestItems.Value : defaultQuest.WeeklyQuestItems.Value;
            List<QuestItem> newQuests = new List<QuestItem>();
            HashSet<string> encounteredQuests = new HashSet<string>();
            for (int i = 0; i < count; i++)
            {
                // hacky, just going to pick a random quest and retry up to 10 times if it's a duplicate
                for (int j = 0; j < 10; j++)
                {
                    int index = UnityEngine.Random.Range(0, defaultQuests.Count);
                    if (!encounteredQuests.Contains(defaultQuests[index].GetQuestName()))
                    {
                        QuestItem newQuest = new QuestItem();
                        newQuest.Copy(defaultQuests[index]);
                        newQuest.DayCreated.Value = currentDay;
                        newQuests.Add(newQuest);
                        encounteredQuests.Add(newQuest.GetQuestName());
                        break;
                    }
                }
            }
            return newQuests;
        }

        private void CacheActiveQuests()
        {
            _activeQuests.Clear();
            foreach (QuestItem item in _quest.DailyQuestItems.Value)
            {
                if (item.Progress.Value < item.Amount.Value)
                    AddActiveQuest(item);
            }
            foreach (QuestItem item in _quest.WeeklyQuestItems.Value)
            {
                if (item.Progress.Value < item.Amount.Value)
                    AddActiveQuest(item);
            }
        }

        protected void AddActiveQuest(QuestItem item)
        {
            string category = item.Category.Value;
            if (!_activeQuests.ContainsKey(category))
                _activeQuests.Add(category, new List<QuestItem>());
            _activeQuests[category].Add(item);
        }

        protected virtual bool CheckKillConditions(List<StringSetting> conditions, KillWeapon weapon)
        {
            foreach (StringSetting condition in conditions)
            {
                string[] conditionArray = condition.Value.Split(':');
                string conditionType = conditionArray[0];
                string conditionValue = conditionArray[1];
                if (conditionType == "Weapon" && NameToKillWeapon[conditionValue] != weapon)
                    return false;
            }
            return true;
        }

        protected virtual bool CheckDamageConditions(List<StringSetting> conditions, KillWeapon weapon, int damage)
        {
            foreach (StringSetting condition in conditions)
            {
                string[] conditionArray = condition.Value.Split(':');
                string conditionType = conditionArray[0];
                string conditionValue = conditionArray[1];
                if (conditionType == "Weapon" && NameToKillWeapon[conditionValue] != weapon)
                    return false;
                if (conditionType == "Damage" && damage < int.Parse(conditionValue))
                    return false;
            }
            return true;
        }

        protected virtual bool CheckSpeedConditions(List<StringSetting> conditions, float speed)
        {
            foreach (StringSetting condition in conditions)
            {
                string[] conditionArray = condition.Value.Split(':');
                string conditionType = conditionArray[0];
                string conditionValue = conditionArray[1];
                if (conditionType == "Speed" && speed < int.Parse(conditionValue))
                    return false;
            }
            return true;
        }

        public override void RegisterTitanKill(BasicTitan victim, KillWeapon weapon)
        {
            foreach (string category in TitanKillCategories)
            {
                if (!_activeQuests.ContainsKey(category))
                    continue;
                foreach (QuestItem item in _activeQuests[category])
                {
                    if (!CheckKillConditions(item.Conditions.Value, weapon))
                        continue;
                    if (category == "KillTitan")
                        item.AddProgress();
                }
            }
        }

        public override void RegisterHumanKill(Human victim, KillWeapon weapon)
        {
            foreach (string category in HumanKillCategories)
            {
                if (!_activeQuests.ContainsKey(category))
                    continue;
                foreach (QuestItem item in _activeQuests[category])
                {
                    if (!CheckKillConditions(item.Conditions.Value, weapon))
                        continue;
                    if (category == "KillHuman")
                        item.AddProgress();
                }
            }
        }

        public override void RegisterDamage(GameObject victim, KillWeapon weapon, int damage)
        {
            foreach (string category in DamageCategories)
            {
                if (!_activeQuests.ContainsKey(category))
                    continue;
                foreach (QuestItem item in _activeQuests[category])
                {
                    if (!CheckDamageConditions(item.Conditions.Value, weapon, damage))
                        continue;
                    if (category == "HitDamage")
                        item.AddProgress();
                    else if (category == "DealDamage")
                        item.AddProgress(damage);
                }
            }
        }

        public override void RegisterSpeed(float speed)
        {
            foreach (string category in SpeedCategories)
            {
                if (!_activeQuests.ContainsKey(category))
                    continue;
                foreach (QuestItem item in _activeQuests[category])
                {
                    if (!CheckSpeedConditions(item.Conditions.Value, speed))
                        continue;
                    if (category == "ReachSpeed")
                        item.AddProgress();
                }
            }
        }

        public override void RegisterInteraction(GameObject interact, InteractionType interactionType)
        {
            foreach (string category in InteractionCategories)
            {
                if (!_activeQuests.ContainsKey(category))
                    continue;
                foreach (QuestItem item in _activeQuests[category])
                {
                    if (category == "ShareGas")
                        item.AddProgress();
                    else if (category == "CarryHuman")
                        item.AddProgress();
                }
            }
        }
    }
}
