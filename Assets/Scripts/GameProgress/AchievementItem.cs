using Settings;

namespace GameProgress
{
    class AchievementItem : QuestItem
    {
        public StringSetting Tier = new StringSetting(string.Empty);
        public BoolSetting Active = new BoolSetting(false);

        public override string GetQuestName()
        {
            string tier = "A";
            if (Tier.Value == "Bronze")
                tier = "A";
            else if (Tier.Value == "Silver")
                tier = "B";
            else if (Tier.Value == "Gold")
                tier = "C";
            return Category.Value + GetConditionsHash() + tier + Amount.Value.ToString();
        }
    }
}
