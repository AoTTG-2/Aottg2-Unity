using System;
using System.Collections.Generic;

namespace Characters
{
    class BasePerk
    {
        public bool Enabled;
        public virtual string Name => "Default";
        public virtual int MaxPoints => 1;
        public int CurrPoints = 0;

        protected Dictionary<string, int> Requirements = new Dictionary<string, int>();

        public BasePerk()
        {
            SetupRequirements();
        }

        protected virtual void SetupRequirements()
        {
        }

        public virtual bool HasRequirements(Dictionary<string, BasePerk> perks)
        {
            foreach (string key in Requirements.Keys)
            {
                if (!perks.ContainsKey(key) || perks[key].CurrPoints < Requirements[key])
                    return false;
            }
            return true;
        }

        public virtual bool Validate(Dictionary<string, BasePerk> perks)
        {
            return (HasRequirements(perks) || CurrPoints == 0) && CurrPoints >= 0 && CurrPoints <= MaxPoints;
        }
    }
}
