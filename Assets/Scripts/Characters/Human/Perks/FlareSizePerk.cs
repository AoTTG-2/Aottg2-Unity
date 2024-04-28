using System;

namespace Characters
{
    class FlareSizePerk: BasePerk
    {
        public override string Name => "FlareSize";
        public override int MaxPoints => 2;

        protected override void SetupRequirements()
        {
            Requirements.Add("FlareCD", 2);
        }
    }
}
