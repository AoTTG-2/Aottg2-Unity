using ApplicationManagers;
using GameManagers;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class BaseEmoteSpecial : ExtendedUseable
    {
        protected Human _human;
        protected virtual bool GroundedOnly => true;

        public BaseEmoteSpecial(BaseCharacter owner): base(owner)
        {
            _human = (Human)owner;
        }

        public override bool CanUse()
        {
            return base.CanUse() && (!GroundedOnly || _human.Grounded) && _human.CanEmote();
        }

        protected bool InSpecial()
        {
            return _human.State == HumanState.EmoteAction;
        }
    }
}
