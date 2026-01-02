using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Animation", Abstract = true, Description = "Represents an Animation component for playing legacy animation clips.", IsComponent = true)]
    partial class CustomLogicAnimationBuiltin : BuiltinComponentInstance
    {
        public Animation Value;
        public BuiltinClassInstance OwnerBuiltin;

        [CLConstructor("Creates an empty Animation instance.")]
        public CustomLogicAnimationBuiltin() : base(null) { }

        public CustomLogicAnimationBuiltin(BuiltinClassInstance owner, Animation animation) : base(animation)
        {
            OwnerBuiltin = owner;
            Value = (Animation)Component;
        }

        [CLMethod("Checks if the given animation is playing.")]
        public bool IsPlaying(
            [CLParam("The name of the animation to check.")]
            string anim)
            => Value.IsPlaying(anim);

        [CLMethod("Plays the specified animation.")]
        public void PlayAnimation(
            [CLParam("The name of the animation to play.")]
            string anim,
            [CLParam("The fade time in seconds for cross-fading (default: 0.1).")]
            float fade = 0.1f,
            [CLParam("The animation layer to play on (default: 0).")]
            int layer = 0)
        {
            Value.CrossFade(anim, fade);
            Value[anim].layer = layer;
        }

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
        {
            Value.CrossFade(anim, fade);
            Value[anim].layer = layer;
            Value[anim].normalizedTime = normalizedTime;
        }

        [CLMethod("Plays the specified animation after the current animation finishes playing.")]
        public void PlayAnimationQueued(
            [CLParam("The name of the animation to queue.")]
            string anim)
            => Value.PlayQueued(anim);

        [CLMethod("Stops the specified animation. Will stop all animations if no name is given.")]
        public void StopAnimation(
            [CLParam("The name of the animation to stop. If null, stops all animations.")]
            string anim = null)
        {
            if (anim == null)
                Value.Stop();
            else
                Value.Stop(anim);
        }

        [CLMethod("Sets the playback speed of the specified animation.")]
        public void SetAnimationSpeed(
            [CLParam("The name of the animation.")]
            string name,
            [CLParam("The playback speed multiplier (1.0 = normal speed).")]
            float speed)
            => Value[name].speed = speed;

        [CLMethod("Gets the playback speed of the specified animation.")]
        public float GetAnimationSpeed(
            [CLParam("The name of the animation.")]
            string name)
            => Value[name].speed;

        [CLMethod("Gets the length of the specified animation.")]
        public float GetAnimationLength(
            [CLParam("The name of the animation.")]
            string anim)
            => Value[anim].length;

        [CLMethod("Gets the normalized time of the specified animation.")]
        public float GetAnimationNormalizedTime(
            [CLParam("The name of the animation.")]
            string anim)
            => Value[anim].normalizedTime;

        [CLMethod("Sets the normalized playback time of the specified animation.")]
        public float SetAnimationNormalizedTime(
            [CLParam("The name of the animation.")]
            string anim,
            [CLParam("The normalized time (0-1) to set.")]
            float normalizedTime)
            => Value[anim].normalizedTime = normalizedTime;

        [CLMethod("Sets the weight of the specified animation.")]
        public float SetAnimationWeight(
            [CLParam("The name of the animation.")]
            string anim,
            [CLParam("The weight value (0-1) to set.")]
            float weight)
            => Value[anim].weight = weight;

        [CLMethod("Gets the weight of the specified animation.")]
        public float GetAnimationWeight(
            [CLParam("The name of the animation.")]
            string anim)
            => Value[anim].weight;
    }
}
