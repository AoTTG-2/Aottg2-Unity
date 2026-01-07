using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents an Animation component for playing legacy animation clips.
    /// </summary>
    [CLType(Name = "Animation", Abstract = true, IsComponent = true)]
    partial class CustomLogicAnimationBuiltin : BuiltinComponentInstance
    {
        public Animation Value;
        public BuiltinClassInstance OwnerBuiltin;

        /// <summary>
        /// Creates an empty Animation instance.
        /// </summary>
        [CLConstructor]
        public CustomLogicAnimationBuiltin() : base(null) { }

        public CustomLogicAnimationBuiltin(BuiltinClassInstance owner, Animation animation) : base(animation)
        {
            OwnerBuiltin = owner;
            Value = (Animation)Component;
        }

        /// <summary>
        /// Checks if the given animation is playing.
        /// </summary>
        /// <param name="anim">The name of the animation to check.</param>
        [CLMethod]
        public bool IsPlaying([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim)
            => Value.IsPlaying(anim);

        /// <summary>
        /// Plays the specified animation.
        /// </summary>
        /// <param name="anim">The name of the animation to play.</param>
        /// <param name="fade">The fade time in seconds for cross-fading (default: 0.1).</param>
        /// <param name="layer">The animation layer to play on (default: 0).</param>
        [CLMethod]
        public void PlayAnimation([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim, float fade = 0.1f, int layer = 0)
        {
            Value.CrossFade(anim, fade);
            Value[anim].layer = layer;
        }

        /// <summary>
        /// Plays the specified animation starting from a normalized time.
        /// </summary>
        /// <param name="anim">The name of the animation to play.</param>
        /// <param name="normalizedTime">The normalized time (0-1) to start the animation from.</param>
        /// <param name="fade">The fade time in seconds for cross-fading (default: 0.1).</param>
        /// <param name="layer">The animation layer to play on (default: 0).</param>
        [CLMethod]
        public void PlayAnimationAt([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim, float normalizedTime, float fade = 0.1f, int layer = 0)
        {
            Value.CrossFade(anim, fade);
            Value[anim].layer = layer;
            Value[anim].normalizedTime = normalizedTime;
        }

        /// <summary>
        /// Plays the specified animation after the current animation finishes playing.
        /// </summary>
        /// <param name="anim">The name of the animation to queue.</param>
        [CLMethod]
        public void PlayAnimationQueued([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim)
            => Value.PlayQueued(anim);

        /// <summary>
        /// Stops the specified animation. Will stop all animations if no name is given.
        /// </summary>
        /// <param name="anim">The name of the animation to stop. If null, stops all animations.</param>
        [CLMethod]
        public void StopAnimation([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim = null)
        {
            if (anim == null)
                Value.Stop();
            else
                Value.Stop(anim);
        }

        /// <summary>
        /// Sets the playback speed of the specified animation.
        /// </summary>
        /// <param name="name">The name of the animation.</param>
        /// <param name="speed">The playback speed multiplier (1.0 = normal speed).</param>
        [CLMethod]
        public void SetAnimationSpeed([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string name, float speed)
            => Value[name].speed = speed;

        /// <summary>
        /// Gets the playback speed of the specified animation.
        /// </summary>
        /// <param name="name">The name of the animation.</param>
        [CLMethod]
        public float GetAnimationSpeed([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string name)
            => Value[name].speed;

        /// <summary>
        /// Gets the length of the specified animation.
        /// </summary>
        /// <param name="anim">The name of the animation.</param>
        [CLMethod]
        public float GetAnimationLength([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim)
            => Value[anim].length;

        /// <summary>
        /// Gets the normalized time of the specified animation.
        /// </summary>
        /// <param name="anim">The name of the animation.</param>
        [CLMethod]
        public float GetAnimationNormalizedTime([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim)
            => Value[anim].normalizedTime;

        /// <summary>
        /// Sets the normalized playback time of the specified animation.
        /// </summary>
        /// <param name="anim">The name of the animation.</param>
        /// <param name="normalizedTime">The normalized time (0-1) to set.</param>
        [CLMethod]
        public void SetAnimationNormalizedTime([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim, float normalizedTime)
            => Value[anim].normalizedTime = normalizedTime;

        /// <summary>
        /// Sets the weight of the specified animation.
        /// </summary>
        /// <param name="anim">The name of the animation.</param>
        /// <param name="weight">The weight value (0-1) to set.</param>
        [CLMethod]
        public void SetAnimationWeight([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim, float weight)
            => Value[anim].weight = weight;

        /// <summary>
        /// Gets the weight of the specified animation.
        /// </summary>
        /// <param name="anim">The name of the animation.</param>
        [CLMethod]
        public float GetAnimationWeight([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim)
            => Value[anim].weight;
    }
}
