using UnityEngine;

namespace Characters
{
    class AmmoWeapon : SimpleUseable
    {
        public int MaxAmmo;
        public int AmmoLeft;
        public int MaxRound;
        public int RoundLeft;

        public AmmoWeapon(BaseCharacter owner, int ammo, int round, float cooldown) : base(owner)
        {
            MaxAmmo = AmmoLeft = ammo;
            MaxRound = RoundLeft = round;
            Cooldown = cooldown;
            if (MaxRound == -1)
            {
                RoundLeft = MaxAmmo;
                AmmoLeft = 0;
            }
            else
                AmmoLeft -= MaxRound;
        }

        public bool NeedRefill()
        {
            if (MaxRound == -1)
                return RoundLeft < MaxAmmo;
            else
                return RoundLeft < MaxRound || (RoundLeft + AmmoLeft) < MaxAmmo;
        }

        public override void Reload()
        {
            if (MaxRound != -1)
            {
                int max = MaxRound - RoundLeft;
                int amount = Mathf.Min(max, AmmoLeft);
                amount = Mathf.Max(amount, 0);
                AmmoLeft -= amount;
                RoundLeft += amount;
            }
        }

        public override void Reset()
        {
            if (MaxRound == -1)
            {
                RoundLeft = MaxAmmo;
                AmmoLeft = 0;
            }
            else
            {
                AmmoLeft = MaxAmmo - MaxRound;
                RoundLeft = MaxRound;
            }
        }

        protected override void OnUse()
        {
            base.OnUse();
            if (RoundLeft >= 0)
                RoundLeft--;
        }

        public override bool CanUse()
        {
            return base.CanUse() && (RoundLeft > 0 || RoundLeft == -1);
        }
    }
}
