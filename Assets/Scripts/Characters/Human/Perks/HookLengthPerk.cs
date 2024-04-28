using System;

namespace Characters
{
    class HookLengthPerk: BasePerk
    {
        public override string Name => "HookLength";
        public override int MaxPoints => 3;

        protected override void SetupRequirements()
        {
            Requirements.Add("HookSpeed", 3);
        }
    }
}
