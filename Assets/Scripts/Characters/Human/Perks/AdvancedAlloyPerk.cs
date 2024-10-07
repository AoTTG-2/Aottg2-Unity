using System;

namespace Characters
{
    class AdvancedAlloyPerk: BasePerk
    {
        public override string Name => "AdvancedAlloy";
        public override int MaxPoints => 1;

        protected override void SetupRequirements()
        {
            Requirements.Add("DurableBlades", 1);
        }
    }
}
