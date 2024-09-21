using Settings;
using System;
using System.Globalization;

namespace GameProgress
{
    class QuestItem : BaseSettingsContainer
    {
        public StringSetting Category = new StringSetting(string.Empty);
        public ListSetting<StringSetting> Conditions = new ListSetting<StringSetting>();
        public IntSetting Amount = new IntSetting(0);
        public StringSetting RewardType = new StringSetting(string.Empty);
        public StringSetting RewardValue = new StringSetting(string.Empty);
        public StringSetting Icon = new StringSetting(string.Empty);

        public IntSetting Progress = new IntSetting(0);
        public BoolSetting Daily = new BoolSetting(true);
        public IntSetting DayCreated = new IntSetting(0);
        public BoolSetting Collected = new BoolSetting(false);

        public virtual string GetQuestName()
        {
            return Category.Value + GetConditionsHash() + Amount.Value.ToString();
        }

        public string GetConditionsHash()
        {
            string conditionStr = "";
            foreach (StringSetting condition in Conditions.Value)
                conditionStr += condition.Value;
            return conditionStr;
        }

        public bool Finished()
        {
            return Progress.Value >= Amount.Value;
        }

        public void AddProgress(int count = 1)
        {
            Progress.Value += count;
            Progress.Value = Math.Min(Progress.Value, Amount.Value);
        }

        public void CollectReward()
        {
            if (Collected.Value)
                return;
            if (Progress.Value >= Amount.Value)
            {
                Collected.Value = true;
                if (RewardType.Value == "Exp")
                    GameProgressManager.AddExp(int.Parse(RewardValue.Value));
            }
        }
    }
}
