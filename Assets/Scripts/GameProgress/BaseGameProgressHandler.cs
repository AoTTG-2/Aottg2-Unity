using UnityEngine;
using Utility;
using System;
using System.Collections.Generic;
using Characters;

namespace GameProgress
{
    abstract class BaseGameProgressHandler
    {
        public virtual void RegisterTitanKill(BasicTitan victim, KillMethod method)
        {
        }
        public virtual void RegisterHumanKill(Human victim, KillMethod method)
        {
        }
        public virtual void RegisterDamage(GameObject victim, KillMethod method, int damage)
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
