using Assets.Scripts.Characters.Human.Perks;
using System;

namespace Characters
{
    class OmniDashPerk : DivisivePowerPerk
    {
        public override string Name => "OmniDash";
        public override int MaxPoints => 1;

        protected override float MaxPower => 100f;
        protected override float MinPower => 0f;
        protected override float PowerUsageDivisor => 2f;
        protected override float LinearRecoveryRate => 25f; // 4 seconds to fully recover

        protected override void SetupRequirements()
        {
            Requirements.Add("VerticalDash", 1);
        }
    }
}
