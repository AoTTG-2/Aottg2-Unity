using System.Collections.Generic;
using UnityEngine;
using Map;

namespace CustomLogic
{
    [CLType(Static = false)]
    class CustomLogicTransformBuiltin: CustomLogicClassInstanceBuiltin
    {
        public Transform Value;
        private Vector3 _internalRotation;
        private Vector3 _internalLocalRotation;
        private bool _needSetRotation = true;
        private bool _needSetLocalRotation = true;

        public CustomLogicTransformBuiltin(Transform transform) : base("Transform")
        {
            Value = transform;
        }

        [CLProperty("Gets or sets the position of the transform.")]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Value.position);
            set => Value.position = value.Value;
        }

        [CLProperty("Gets or sets the local position of the transform.")]
        public CustomLogicVector3Builtin LocalPosition
        {
            get => new CustomLogicVector3Builtin(Value.localPosition);
            set => Value.localPosition = value.Value;
        }

        [CLProperty("Gets or sets the rotation of the transform.")]
        public CustomLogicVector3Builtin Rotation
        {
            get
            {
                if (_needSetRotation)
                {
                    _internalRotation = Value.rotation.eulerAngles;
                    _needSetRotation = false;
                }
                return new CustomLogicVector3Builtin(_internalRotation);
            }
            set
            {
                _internalRotation = value.Value;
                _needSetRotation = false;
                Value.rotation = Quaternion.Euler(_internalRotation);
            }
        }

        [CLProperty("Gets or sets the local rotation of the transform.")]
        public CustomLogicVector3Builtin LocalRotation
        {
            get
            {
                if (_needSetLocalRotation)
                {
                    _internalLocalRotation = Value.localRotation.eulerAngles;
                    _needSetLocalRotation = false;
                }
                return new CustomLogicVector3Builtin(_internalLocalRotation);
            }
            set
            {
                _internalLocalRotation = value.Value;
                _needSetLocalRotation = false;
                Value.localRotation = Quaternion.Euler(_internalLocalRotation);
            }
        }

        [CLProperty("Gets or sets the quaternion rotation of the transform.")]
        public CustomLogicQuaternionBuiltin QuaternionRotation
        {
            get => new CustomLogicQuaternionBuiltin(Value.rotation);
            set => Value.rotation = value.Value;
        }

        [CLProperty("Gets or sets the quaternion local rotation of the transform.")]
        public CustomLogicQuaternionBuiltin QuaternionLocalRotation
        {
            get => new CustomLogicQuaternionBuiltin(Value.localRotation);
            set => Value.localRotation = value.Value;
        }

        [CLProperty("Gets or sets the scale of the transform.")]
        public CustomLogicVector3Builtin Scale
        {
            get => new CustomLogicVector3Builtin(Value.localScale);
            set => Value.localScale = value.Value;
        }

        [CLProperty("Gets the forward vector of the transform.")]
        public CustomLogicVector3Builtin Forward => new CustomLogicVector3Builtin(Value.forward.normalized);

        [CLProperty("Gets the up vector of the transform.")]
        public CustomLogicVector3Builtin Up => new CustomLogicVector3Builtin(Value.up.normalized);

        [CLProperty("Gets the right vector of the transform.")]
        public CustomLogicVector3Builtin Right => new CustomLogicVector3Builtin(Value.right.normalized);

        [CLMethod("Gets the transform of the specified child.")]
        public CustomLogicTransformBuiltin GetTransform(string name)
        {
            Transform transform = Value.Find(name);
            if (transform != null)
            {
                return new CustomLogicTransformBuiltin(transform);
            }
            return null;
        }

        [CLMethod("Gets all child transforms.")]
        public CustomLogicListBuiltin GetTransforms()
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            foreach (Transform transform in Value)
            {
                listBuiltin.List.Add(new CustomLogicTransformBuiltin(transform));
            }
            return listBuiltin;
        }

        [CLMethod("Plays the specified animation.")]
        public void PlayAnimation(string anim, float fade = 0.1f)
        {
            var animation = Value.GetComponent<Animation>();
            if (!animation.IsPlaying(anim))
                animation.CrossFade(anim, fade);
        }

        [CLMethod("Gets the length of the specified animation.")]
        public float GetAnimationLength(string anim)
        {
            var animation = Value.GetComponent<Animation>();
            return animation[anim].length;
        }

        [CLMethod("Plays the sound.")]
        public void PlaySound()
        {
            var sound = Value.GetComponent<AudioSource>();
            if (!sound.isPlaying)
                sound.Play();
        }

        [CLMethod("Stops the sound.")]
        public void StopSound()
        {
            var sound = Value.GetComponent<AudioSource>();
            if (sound.isPlaying)
                sound.Stop();
        }

        [CLMethod("Toggles the particle system.")]
        public void ToggleParticle(bool enabled)
        {
            var particle = Value.GetComponent<ParticleSystem>();
            if (!particle.isPlaying)
                particle.Play();
            var emission = particle.emission;
            emission.enabled = enabled;
        }

        [CLMethod("Inverse transforms the direction.")]
        public CustomLogicVector3Builtin InverseTransformDirection(CustomLogicVector3Builtin direction)
        {
            return new CustomLogicVector3Builtin(Value.InverseTransformDirection(direction.Value));
        }

        [CLMethod("Inverse transforms the point.")]
        public CustomLogicVector3Builtin InverseTransformPoint(CustomLogicVector3Builtin point)
        {
            return new CustomLogicVector3Builtin(Value.InverseTransformPoint(point.Value));
        }

        [CLMethod("Transforms the direction.")]
        public CustomLogicVector3Builtin TransformDirection(CustomLogicVector3Builtin direction)
        {
            return new CustomLogicVector3Builtin(Value.TransformDirection(direction.Value));
        }

        [CLMethod("Transforms the point.")]
        public CustomLogicVector3Builtin TransformPoint(CustomLogicVector3Builtin point)
        {
            return new CustomLogicVector3Builtin(Value.TransformPoint(point.Value));
        }

        [CLMethod("Rotates the transform.")]
        public void Rotate(CustomLogicVector3Builtin rotation)
        {
            Value.Rotate(rotation.Value);
        }

        [CLMethod("Rotates the transform around a point and axis.")]
        public void RotateAround(CustomLogicVector3Builtin point, CustomLogicVector3Builtin axis, float angle)
        {
            Value.RotateAround(point.Value, axis.Value, angle);
        }

        [CLMethod("Looks at the target position.")]
        public void LookAt(CustomLogicVector3Builtin target)
        {
            Value.LookAt(target.Value);
        }

        [CLMethod("Sets the enabled state of all child renderers.")]
        public void SetRenderersEnabled(bool enabled)
        {
            foreach (var renderer in Value.GetComponentsInChildren<Renderer>())
                renderer.enabled = enabled;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return Value == null;
            if (!(obj is CustomLogicTransformBuiltin))
                return false;
            return Value == ((CustomLogicTransformBuiltin)obj).Value;
        }
    }
}
