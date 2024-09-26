using UnityEngine;
using Utility;
using System;
using System.Collections.Generic;
using Characters;

namespace GameProgress
{
    abstract class BaseGameProgressHandler
    {
        public virtual void RegisterKill(BaseCharacter player, BaseCharacter enemy)
        {
        }
        public virtual void RegisterDamage(BaseCharacter player, BaseCharacter enemy, int damage)
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
