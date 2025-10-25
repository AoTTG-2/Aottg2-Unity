using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Animator", Abstract = true, Description = "")]
    partial class CustomLogicAnimatorBuiltin : BuiltinComponentInstance
    {
        public Animator Value;
        public BuiltinClassInstance OwnerBuiltin;
        private string _currentAnimation;
        private readonly Dictionary<string, AnimationClip> _animatorClips;

        [CLConstructor]
        public CustomLogicAnimatorBuiltin() : base(null) { }

        public CustomLogicAnimatorBuiltin(BuiltinClassInstance owner, Animator animator) : base(animator)
        {
            OwnerBuiltin = owner;
            Value = (Animator)Component;

            _animatorClips = new Dictionary<string, AnimationClip>();
            foreach (AnimationClip clip in Value.runtimeAnimatorController.animationClips)
                _animatorClips[clip.name.Replace('.', '_')] = clip;
        }


        [CLMethod("Checks if the given animation is playing.")]
        public bool IsPlaying(string anim)
        {
            anim = anim.Replace('.', '_');
            return _currentAnimation == anim;
        }

        [CLMethod("Plays the specified animation.")]
        public void PlayAnimation(string anim, float fade = 0.1f, int layer = 0)
        {
            anim = anim.Replace('.', '_');
            if (_currentAnimation != anim)
            {
                Value.CrossFade(anim, fade, layer);
                _currentAnimation = anim;
            }
        }

        [CLMethod("Plays the specified animation starting from a normalized time.")]
        public void PlayAnimationAt(string anim, float normalizedTime, float fade = 0.1f, int layer = 0)
        {
            anim = anim.Replace('.', '_');
            if (_currentAnimation != anim)
            {
                Value.CrossFade(anim, fade, layer, normalizedTime);
                _currentAnimation = anim;
            }
        }

        [CLMethod("Sets the animation playback speed.")]
        public void SetAnimationSpeed(float speed) => Value.speed = speed;

        [CLMethod("Gets the length of the specified animation.")]
        public float GetAnimationLength(string anim) => _animatorClips[anim.Replace('.', '_')].length;

        [CLMethod("Gets an animation float parameter.")]
        public float GetAnimatorFloat(string name) => Value.GetFloat(name);

        [CLMethod("Gets an animation int parameter.")]
        public int GetAnimatorInt(string name) => Value.GetInteger(name);

        [CLMethod("Gets an animation bool parameter.")]
        public bool GetAnimatorBool(string name) => Value.GetBool(name);

        [CLMethod("Sets an animation float parameter.")]
        public void SetAnimatorFloat(string name, float value) => Value.SetFloat(name, value);

        [CLMethod("Sets an animation int parameter.")]
        public void SetAnimatorInt(string name, int value) => Value.SetInteger(name, value);

        [CLMethod("Sets an animation bool parameter.")]
        public void SetAnimatorBool(string name, bool value) => Value.SetBool(name, value);

        [CLMethod("Sets an animation trigger.")]
        public void SetAnimatorTrigger(string name) => Value.SetTrigger(name);

        [CLMethod("Resets an animation trigger.")]
        public void ResetAnimatorTrigger(string name) => Value.ResetTrigger(name);

        [CLMethod("Sets the weight of the specified layer.")]
        public void SetLayerWeight(int layer, float weight) => Value.SetLayerWeight(layer, weight);

        [CLMethod("Gets the weight of the specified layer.")]
        public void GetLayerWeight(int layer) => Value.GetLayerWeight(layer);

        [CLMethod("Gets the normalized time of the current animation.")]
        public float GetAnimationNormalizedTime(int layer = 0)
        {
            AnimatorStateInfo stateInfo = Value.GetCurrentAnimatorStateInfo(layer);
            return stateInfo.normalizedTime;
        }
    }
}
