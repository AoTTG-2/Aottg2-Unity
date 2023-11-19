using UnityEngine;
using Utility;
using System;
using System.Collections.Generic;
using Characters;

namespace GameProgress
{
    abstract class BaseGameProgressHandler
    {
        public virtual void RegisterTitanKill(GameObject character, BasicTitan victim, KillWeapon weapon)
        {
        }
        public virtual void RegisterHumanKill(GameObject character, Human victim, KillWeapon weapon)
        {
        }
        public virtual void RegisterDamage(GameObject character, GameObject victim, KillWeapon weapon, int damage)
        {
        }
        public virtual void RegisterSpeed(GameObject character, float speed)
        {
        }
        public virtual void RegisterInteraction(GameObject character, GameObject interact, InteractionType interactionType)
        {
        }
    }
}
