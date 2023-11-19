using System.Collections;
using UnityEngine;

namespace Characters
{
    class BladeWeapon : HoldUseable
    {
        public float MaxDurability;
        public float CurrentDurability;
        public int MaxBlades;
        public int BladesLeft;

        public BladeWeapon(BaseCharacter owner, float durability, int blades): base(owner)
        {
            BladesLeft = MaxBlades = blades;
            CurrentDurability = MaxDurability = durability;
        }

        public void UseDurability(float amount)
        {
            CurrentDurability -= amount;
            CurrentDurability = Mathf.Max(CurrentDurability, 0f);
        }

        public override void Reload()
        {
            if (BladesLeft > 0)
            {
                BladesLeft--;
                CurrentDurability = MaxDurability;
            }
        }

        public override void Reset()
        {
            BladesLeft = MaxBlades;
            CurrentDurability = MaxDurability;
        }

        public override bool CanUse()
        {
            return base.CanUse() && CurrentDurability > 0f && ((Human)_owner).State == HumanState.Idle;
        }

        protected override void Activate()
        {
            ((Human)_owner).StartBladeSwing();
        }

        protected override void Deactivate()
        {
        }

        protected override void ActiveFixedUpdate()
        {
        }
    }
}
