using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// A useable that reduces cooldown based on logic.
    /// </summary>
    abstract class RechargeableUseable : ExtendedUseable
    {
        public float ReduceCooldownAmount;
        public RechargeableUseable(BaseCharacter owner, float reduceCooldownAmount = 0f) : base(owner)
        {
            ReduceCooldownAmount = reduceCooldownAmount;
        }
        public void ReduceCooldown()
        {
            _lastUseTime -= ReduceCooldownAmount;
        }
    }
}
