using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Animator", Abstract = true, Description = "Represents an Animator component for controlling animations using Animator Controller.", IsComponent = true)]
    partial class CustomLogicAnimatorBuiltin : BuiltinComponentInstance
    {
        public Animator Value;
        public BuiltinClassInstance OwnerBuiltin;
        private readonly Dictionary<string, AnimationClip> _animatorClips;

        [CLConstructor]
        public CustomLogicAnimatorBuiltin() : base(null) { }

        public CustomLogicAnimatorBuiltin(BuiltinClassInstance owner, Animator animator) : base(animator)
        {
            OwnerBuiltin = owner;
            Value = (Animator)Component;

            _animatorClips = new Dictionary<string, AnimationClip>();
            foreach (AnimationClip clip in Value.runtimeAnimatorController.animationClips)
                _animatorClips[clip.name] = clip;
        }

        [CLMethod("Checks if the given animation is playing.")]
        public bool IsPlaying(
            [CLParam("The name of the animation to check.")]
            string anim,
            [CLParam("The animation layer to check (default: 0).")]
            int layer = 0)
        {
            AnimatorStateInfo stateInfo = Value.IsInTransition(layer) ? Value.GetNextAnimatorStateInfo(layer) : Value.GetCurrentAnimatorStateInfo(layer);
            return stateInfo.IsName(anim);
        }

        [CLMethod("Plays the specified animation.")]
        public void PlayAnimation(
            [CLParam("The name of the animation to play.")]
            string anim,
            [CLParam("The fade time in seconds for cross-fading (default: 0.1).")]
            float fade = 0.1f,
            [CLParam("The animation layer to play on (default: 0).")]
            int layer = 0)
            => Value.CrossFade(anim, fade, layer);

        [CLMethod("Plays the specified animation starting from a normalized time.")]
        public void PlayAnimationAt(
            [CLParam("The name of the animation to play.")]
            string anim,
            [CLParam("The normalized time (0-1) to start the animation from.")]
            float normalizedTime,
            [CLParam("The fade time in seconds for cross-fading (default: 0.1).")]
            float fade = 0.1f,
            [CLParam("The animation layer to play on (default: 0).")]
            int layer = 0)
            => Value.CrossFade(anim, fade, layer, normalizedTime);

        [CLMethod("Sets the animation playback speed.")]
        public void SetAnimationSpeed(
            [CLParam("The playback speed multiplier (1.0 = normal speed).")]
            float speed)
            => Value.speed = speed;

        [CLMethod("Gets the length of the specified animation.")]
        public float GetAnimationLength(
            [CLParam("The name of the animation.")]
            string anim)
            => _animatorClips[anim].length;

        [CLMethod("Gets an animation float parameter.")]
        public float GetAnimatorFloat(
            [CLParam("The name of the float parameter.")]
            string name)
            => Value.GetFloat(name);

        [CLMethod("Gets an animation int parameter.")]
        public int GetAnimatorInt(
            [CLParam("The name of the int parameter.")]
            string name)
            => Value.GetInteger(name);

        [CLMethod("Gets an animation bool parameter.")]
        public bool GetAnimatorBool(
            [CLParam("The name of the bool parameter.")]
            string name)
            => Value.GetBool(name);

        [CLMethod("Sets an animation float parameter.")]
        public void SetAnimatorFloat(
            [CLParam("The name of the float parameter.")]
            string name,
            [CLParam("The value to set.")]
            float value)
            => Value.SetFloat(name, value);

        [CLMethod("Sets an animation int parameter.")]
        public void SetAnimatorInt(
            [CLParam("The name of the int parameter.")]
            string name,
            [CLParam("The value to set.")]
            int value)
            => Value.SetInteger(name, value);

        [CLMethod("Sets an animation bool parameter.")]
        public void SetAnimatorBool(
            [CLParam("The name of the bool parameter.")]
            string name,
            [CLParam("The value to set.")]
            bool value)
            => Value.SetBool(name, value);

        [CLMethod("Sets an animation trigger.")]
        public void SetAnimatorTrigger(
            [CLParam("The name of the trigger parameter.")]
            string name)
            => Value.SetTrigger(name);

        [CLMethod("Resets an animation trigger.")]
        public void ResetAnimatorTrigger(
            [CLParam("The name of the trigger parameter to reset.")]
            string name)
            => Value.ResetTrigger(name);

        [CLMethod("Sets the weight of the specified layer.")]
        public void SetLayerWeight(
            [CLParam("The layer index.")]
            int layer,
            [CLParam("The weight value (0-1) to set.")]
            float weight)
            => Value.SetLayerWeight(layer, weight);

        [CLMethod("Gets the weight of the specified layer.")]
        public float GetLayerWeight(
            [CLParam("The layer index.")]
            int layer)
            => Value.GetLayerWeight(layer);

        [CLMethod("Gets the normalized time of the current animation.")]
        public float GetAnimationNormalizedTime(
            [CLParam("The animation layer to check (default: 0).")]
            int layer = 0)
        {
            AnimatorStateInfo stateInfo = Value.IsInTransition(layer) ? Value.GetNextAnimatorStateInfo(layer) : Value.GetCurrentAnimatorStateInfo(layer);
            return stateInfo.normalizedTime;
        }
    }
}
