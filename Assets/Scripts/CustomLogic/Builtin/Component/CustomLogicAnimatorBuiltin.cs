using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents an Animator component for controlling animations using Animator Controller.
    /// </summary>
    [CLType(Name = "Animator", Abstract = true, IsComponent = true)]
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

        /// <summary>
        /// Checks if the given animation is playing.
        /// </summary>
        /// <param name="anim">The name of the animation to check.</param>
        /// <param name="layer">The animation layer to check (default: 0).</param>
        [CLMethod]
        public bool IsPlaying([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim, int layer = 0)
        {
            AnimatorStateInfo stateInfo = Value.IsInTransition(layer) ? Value.GetNextAnimatorStateInfo(layer) : Value.GetCurrentAnimatorStateInfo(layer);
            return stateInfo.IsName(anim);
        }

        /// <summary>
        /// Plays the specified animation.
        /// </summary>
        /// <param name="anim">The name of the animation to play.</param>
        /// <param name="fade">The fade time in seconds for cross-fading (default: 0.1).</param>
        /// <param name="layer">The animation layer to play on (default: 0).</param>
        [CLMethod]
        public void PlayAnimation([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim, float fade = 0.1f, int layer = 0) => Value.CrossFade(anim, fade, layer);

        /// <summary>
        /// Plays the specified animation starting from a normalized time.
        /// </summary>
        /// <param name="anim">The name of the animation to play.</param>
        /// <param name="normalizedTime">The normalized time (0-1) to start the animation from.</param>
        /// <param name="fade">The fade time in seconds for cross-fading (default: 0.1).</param>
        /// <param name="layer">The animation layer to play on (default: 0).</param>
        [CLMethod]
        public void PlayAnimationAt([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim, float normalizedTime, float fade = 0.1f, int layer = 0)
            => Value.CrossFade(anim, fade, layer, normalizedTime);

        /// <summary>
        /// Sets the animation playback speed.
        /// </summary>
        /// <param name="speed">The playback speed multiplier (1.0 = normal speed).</param>
        [CLMethod]
        public void SetAnimationSpeed(float speed) => Value.speed = speed;

        /// <summary>
        /// Gets the length of the specified animation.
        /// </summary>
        /// <param name="anim">The name of the animation.</param>
        [CLMethod]
        public float GetAnimationLength([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim) => _animatorClips[anim].length;

        /// <summary>
        /// Gets an animation float parameter.
        /// </summary>
        /// <param name="name">The name of the float parameter.</param>
        [CLMethod]
        public float GetAnimatorFloat(string name) => Value.GetFloat(name);

        /// <summary>
        /// Gets an animation int parameter.
        /// </summary>
        /// <param name="name">The name of the int parameter.</param>
        [CLMethod]
        public int GetAnimatorInt(string name) => Value.GetInteger(name);

        /// <summary>
        /// Gets an animation bool parameter.
        /// </summary>
        /// <param name="name">The name of the bool parameter.</param>
        [CLMethod]
        public bool GetAnimatorBool(string name) => Value.GetBool(name);

        /// <summary>
        /// Sets an animation float parameter.
        /// </summary>
        /// <param name="name">The name of the float parameter.</param>
        /// <param name="value">The value to set.</param>
        [CLMethod]
        public void SetAnimatorFloat(string name, float value) => Value.SetFloat(name, value);

        /// <summary>
        /// Sets an animation int parameter.
        /// </summary>
        /// <param name="name">The name of the int parameter.</param>
        /// <param name="value">The value to set.</param>
        [CLMethod]
        public void SetAnimatorInt(string name, int value) => Value.SetInteger(name, value);

        /// <summary>
        /// Sets an animation bool parameter.
        /// </summary>
        /// <param name="name">The name of the bool parameter.</param>
        /// <param name="value">The value to set.</param>
        [CLMethod]
        public void SetAnimatorBool(string name, bool value) => Value.SetBool(name, value);

        /// <summary>
        /// Sets an animation trigger.
        /// </summary>
        /// <param name="name">The name of the trigger parameter.</param>
        [CLMethod]
        public void SetAnimatorTrigger(string name) => Value.SetTrigger(name);

        /// <summary>
        /// Resets an animation trigger.
        /// </summary>
        /// <param name="name">The name of the trigger parameter to reset.</param>
        [CLMethod]
        public void ResetAnimatorTrigger(string name) => Value.ResetTrigger(name);

        /// <summary>
        /// Sets the weight of the specified layer.
        /// </summary>
        /// <param name="layer">The layer index.</param>
        /// <param name="weight">The weight value (0-1) to set.</param>
        [CLMethod]
        public void SetLayerWeight(int layer, float weight) => Value.SetLayerWeight(layer, weight);

        /// <summary>
        /// Gets the weight of the specified layer.
        /// </summary>
        /// <param name="layer">The layer index.</param>
        [CLMethod]
        public float GetLayerWeight(int layer) => Value.GetLayerWeight(layer);

        /// <summary>
        /// Gets the normalized time of the current animation.
        /// </summary>
        /// <param name="layer">The animation layer to check (default: 0).</param>
        [CLMethod]
        public float GetAnimationNormalizedTime(int layer = 0)
        {
            AnimatorStateInfo stateInfo = Value.IsInTransition(layer) ? Value.GetNextAnimatorStateInfo(layer) : Value.GetCurrentAnimatorStateInfo(layer);
            return stateInfo.normalizedTime;
        }
    }
}
