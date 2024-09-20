using UnityEngine;
using Utility;
using System;
using System.Collections.Generic;
using Characters;

namespace GameProgress
{
    abstract class BaseGameProgressHandler
    {
        public virtual void RegisterTitanKill(BasicTitan victim, KillWeapon weapon)
        {
        }
        public virtual void RegisterHumanKill(Human victim, KillWeapon weapon)
        {
        }
        public virtual void RegisterDamage(GameObject victim, KillWeapon weapon, int damage)
        {
        }
        public virtual void RegisterSpeed(float speed)
        {
        }
        public virtual void RegisterInteraction(GameObject interact, InteractionType interactionType)
        {
        }
    }
}
