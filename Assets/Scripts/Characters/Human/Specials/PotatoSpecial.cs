using System.Collections;
using UnityEngine;

namespace Characters
{
    class PotatoSpecial : BaseEmoteSpecial
    {
        protected override float ActiveTime => 10f;
        private float _oldSpeed;

        public PotatoSpecial(BaseCharacter owner): base(owner)
        {
            Cooldown = 20f;
        }

        protected override void Activate()
        {
            _oldSpeed = _human.RunSpeed;
            _human.RunSpeed = _oldSpeed * 4f;
            _human.RunAnimation = HumanAnimations.RunBuffed;
            _human.EmoteAnimation(HumanAnimations.SpecialSasha);
        }

        protected override void Deactivate()
        {
            _human.RunSpeed = _oldSpeed;
            _human.RunAnimation = _human.Weapon is ThunderspearWeapon? HumanAnimations.RunTS : HumanAnimations.Run;
            if (_human.Cache.Animation.IsPlaying(HumanAnimations.RunBuffed))
                _human.CrossFade(_human.RunAnimation, 0.1f);
        }
    }
}
