using System;

namespace Characters
{
    class OmniDashPerk: BasePerk
    {
        public override string Name => "OmniDash";
        public override int MaxPoints => 1;

        protected override void SetupRequirements()
        {
            Requirements.Add("VerticalDash", 1);
        }
    }
}
