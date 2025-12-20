using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Animation", Abstract = true, Description = "", IsComponent = true)]
    partial class CustomLogicAnimationBuiltin : BuiltinComponentInstance
    {
        public Animation Value;
        public BuiltinClassInstance OwnerBuiltin;

        [CLConstructor]
        public CustomLogicAnimationBuiltin() : base(null) { }

        public CustomLogicAnimationBuiltin(BuiltinClassInstance owner, Animation animation) : base(animation)
        {
            OwnerBuiltin = owner;
            Value = (Animation)Component;
        }

        [CLMethod("Checks if the given animation is playing.")]
        public bool IsPlaying(string anim) => Value.IsPlaying(anim);

        [CLMethod("Plays the specified animation.")]
        public void PlayAnimation(string anim, float fade = 0.1f, int layer = 0)
        {
            Value.CrossFade(anim, fade);
            Value[anim].layer = layer;
        }

        [CLMethod("Plays the specified animation starting from a normalized time.")]
        public void PlayAnimationAt(string anim, float normalizedTime, float fade = 0.1f, int layer = 0)
        {
            Value.CrossFade(anim, fade);
            Value[anim].layer = layer;
            Value[anim].normalizedTime = normalizedTime;
        }

        [CLMethod("Plays the specified animation after the current animation finishes playing.")]
        public void PlayAnimationQueued(string anim) => Value.PlayQueued(anim);

        [CLMethod("Stops the specified animation. Will stop all animations if no name is given.")]
        public void StopAnimation(string anim = null)
        {
            if (anim == null)
                Value.Stop();
            else
                Value.Stop(anim);
        }

        [CLMethod("Sets the playback speed of the specified animation.")]
        public void SetAnimationSpeed(string name, float speed) => Value[name].speed = speed;

        [CLMethod("Gets the playback speed of the specified animation.")]
        public float GetAnimationSpeed(string name) => Value[name].speed;

        [CLMethod("Gets the length of the specified animation.")]
        public float GetAnimationLength(string anim) => Value[anim].length;

        [CLMethod("Gets the normalized time of the specified animation.")]
        public float GetAnimationNormalizedTime(string anim) => Value[anim].normalizedTime;

        [CLMethod("Sets the normalized playback time of the specified animation.")]
        public float SetAnimationNormalizedTime(string anim, float normalizedTime) => Value[anim].normalizedTime = normalizedTime;

        [CLMethod("Sets the weight of the specified animation.")]
        public float SetAnimationWeight(string anim, float weight) => Value[anim].weight = weight;

        [CLMethod("Gets the weight of the specified animation.")]
        public float GetAnimationWeight(string anim) => Value[anim].weight;
    }
}
