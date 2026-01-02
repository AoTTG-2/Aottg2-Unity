using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Transform", Abstract = true, Description = "Represents a transform.")]
    partial class CustomLogicTransformBuiltin : BuiltinClassInstance, ICustomLogicEquals
    {
        public readonly Transform Value;

        private Vector3 _internalRotation;
        private Vector3 _internalLocalRotation;
        private bool _needSetRotation = true;
        private bool _needSetLocalRotation = true;
        private string _currentAnimation;

        private readonly CustomLogicAnimationBuiltin _animation;
        private readonly CustomLogicAnimatorBuiltin _animator;
        private readonly CustomLogicAudioSourceBuiltin _audioSource;
        private readonly ParticleSystem _particleSystem;
        // private readonly Renderer _renderer;

        public CustomLogicTransformBuiltin(Transform transform)
        {
            Value = transform;

            _particleSystem = Value.GetComponent<ParticleSystem>();

            if (Value.TryGetComponent<AudioSource>(out var audioSource))
                _audioSource = new CustomLogicAudioSourceBuiltin(this, audioSource);
            if (Value.TryGetComponent<Animation>(out var animation))
                _animation = new CustomLogicAnimationBuiltin(this, animation);
            else if (Value.TryGetComponent<Animator>(out var animator))
                _animator = new CustomLogicAnimatorBuiltin(this, animator);
        }

        [CLProperty("Gets or sets the position of the transform.")]
        public CustomLogicVector3Builtin Position
        {
            get => Value.position;
            set => Value.position = value.Value;
        }

        [CLProperty("Gets or sets the local position of the transform.")]
        public CustomLogicVector3Builtin LocalPosition
        {
            get => Value.localPosition;
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
                return _internalRotation;
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
                return _internalLocalRotation;
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
            get => Value.rotation;
            set
            {
                _needSetLocalRotation = true;
                _needSetRotation = true;
                Value.rotation = value.Value;
            }
        }

        [CLProperty("Gets or sets the quaternion local rotation of the transform.")]
        public CustomLogicQuaternionBuiltin QuaternionLocalRotation
        {
            get => Value.localRotation;
            set
            {
                _needSetLocalRotation = true;
                _needSetRotation = true;
                Value.localRotation = value.Value;
            }
        }

        [CLProperty("Gets or sets the scale of the transform.")]
        public CustomLogicVector3Builtin Scale
        {
            get => Value.localScale;
            set => Value.localScale = value.Value;
        }

        [CLProperty("Gets the forward vector of the transform.")]
        public CustomLogicVector3Builtin Forward
        {
            get => Value.forward;
            set => Value.forward = value.Value;
        }

        [CLProperty("Gets the up vector of the transform.")]
        public CustomLogicVector3Builtin Up
        {
            get => Value.up;
            set => Value.up = value.Value;
        }

        [CLProperty("Gets the right vector of the transform.")]
        public CustomLogicVector3Builtin Right
        {
            get => Value.right;
            set => Value.right = value.Value;
        }

        [CLProperty("Sets the parent of the transform")]
        public CustomLogicTransformBuiltin Parent
        {
            get => Value.parent != null ? new CustomLogicTransformBuiltin(Value.parent) : null;
            set
            {
                if (value == null)
                    Value.SetParent(null);
                else
                    Value.SetParent(value.Value);

                _needSetLocalRotation = true;
                _needSetRotation = true;
            }
        }

        [CLProperty("Gets the name of the transform.")]
        public string Name
        {
            get => Value.name;
        }

        [CLProperty("Gets the Physics Layer of the transform.")]
        public int Layer
        {
            get => Value.gameObject.layer;
            set => Value.gameObject.layer = value;
        }

        [CLProperty("The Animation attached to this transform, returns null if there is none.")]
        public CustomLogicAnimationBuiltin Animation
        {
            get => _animation;
        }

        [CLProperty("The Animator attached to this transform, returns null if there is none.")]
        public CustomLogicAnimatorBuiltin Animator
        {
            get => _animator;
        }

        [CLProperty("The AudioSource attached to this transform, returns null if there is none.")]
        public CustomLogicAudioSourceBuiltin AudioSource
        {
            get => _audioSource;
        }

        [CLMethod("Gets the transform of the specified child.")]
        public CustomLogicTransformBuiltin GetTransform(
            [CLParam("The name of the child transform to find.")]
            string name)
        {
            var transform = Value.Find(name);
            return transform != null ? new CustomLogicTransformBuiltin(transform) : null;
        }

        [CLMethod("Gets all child transforms.", ReturnTypeArguments = new[] { "Transform" })]
        public CustomLogicListBuiltin GetTransforms()
        {
            var listBuiltin = new CustomLogicListBuiltin();
            foreach (Transform transform in Value)
            {
                listBuiltin.List.Add(new CustomLogicTransformBuiltin(transform));
            }
            return listBuiltin;
        }

        [CLMethod("Checks if the given animation is playing.")]
        public bool IsPlayingAnimation(
            [CLParam("The name of the animation to check.")]
            string anim)
        {
            if (_animation != null)
                return _animation.IsPlaying(anim);
            if (_animator != null)
            {
                anim = anim.Replace('.', '_');
                return _currentAnimation == anim;
            }

            return false;
        }

        [CLMethod("Plays the specified animation.")]
        public void PlayAnimation(
            [CLParam("The name of the animation to play.")]
            string anim,
            [CLParam("The fade time in seconds for cross-fading (default: 0.1).")]
            float fade = 0.1f)
        {
            if (_animation != null)
            {
                if (!_animation.IsPlaying(anim))
                    _animation.PlayAnimation(anim, fade);
            }
            else if (_animator != null)
            {
                anim = anim.Replace('.', '_');
                if (_currentAnimation != anim)
                {
                    _animator.Value.CrossFade(anim, fade);
                    _currentAnimation = anim;
                }
            }
        }

        [CLMethod("Gets the length of the specified animation.")]
        public float GetAnimationLength(
            [CLParam("The name of the animation.")]
            string anim)
        {
            if (_animation != null)
                return _animation.GetAnimationLength(anim);
            else if (_animator != null)
                return _animator.GetAnimationLength(anim.Replace('.', '_'));
            return -1;
        }

        [CLMethod("Plays the sound.")]
        public void PlaySound()
        {
            if (!_audioSource.IsPlaying)
                _audioSource.Play();
        }

        [CLMethod("Stops the sound.")]
        public void StopSound()
        {
            if (_audioSource.IsPlaying)
                _audioSource.Stop();
        }

        [CLMethod("Toggles the particle system.")]
        public void ToggleParticle(
            [CLParam("Whether to enable or disable the particle emission.")]
            bool enabled)
        {
            if (!_particleSystem.isPlaying)
                _particleSystem.Play();
            var emission = _particleSystem.emission;
            emission.enabled = enabled;
        }

        [CLMethod("Transforms a direction from world space to local space. Returns: The direction in local space.")]
        public CustomLogicVector3Builtin InverseTransformDirection(
            [CLParam("The direction vector in world space.")]
            CustomLogicVector3Builtin direction)
            => Value.InverseTransformDirection(direction);

        [CLMethod("Transforms a position from world space to local space. Returns: The position in local space.")]
        public CustomLogicVector3Builtin InverseTransformPoint(
            [CLParam("The point in world space.")]
            CustomLogicVector3Builtin point)
            => Value.InverseTransformPoint(point);

        [CLMethod("Transforms a direction from local space to world space. Returns: The direction in world space.")]
        public CustomLogicVector3Builtin TransformDirection(
            [CLParam("The direction vector in local space.")]
            CustomLogicVector3Builtin direction)
            => Value.TransformDirection(direction);

        [CLMethod("Transforms a position from local space to world space. Returns: The position in world space.")]
        public CustomLogicVector3Builtin TransformPoint(
            [CLParam("The point in local space.")]
            CustomLogicVector3Builtin point)
            => Value.TransformPoint(point);

        [CLMethod("Rotates the transform by the given rotation in euler angles.")]
        public void Rotate(
            [CLParam("The rotation in euler angles to apply.")]
            CustomLogicVector3Builtin rotation)
        {
            Value.Rotate(rotation);
            _needSetLocalRotation = true;
            _needSetRotation = true;
        }

        [CLMethod("Rotates the transform around a point by the given angle.")]
        public void RotateAround(
            [CLParam("The point to rotate around.")]
            CustomLogicVector3Builtin point,
            [CLParam("The axis to rotate around.")]
            CustomLogicVector3Builtin axis,
            [CLParam("The angle in degrees to rotate.")]
            float angle)
        {
            _needSetLocalRotation = true;
            _needSetRotation = true;
            Value.RotateAround(point.Value, axis.Value, angle);
        }

        [CLMethod("Rotates the transform to look at the target position.")]
        public void LookAt(
            [CLParam("The world position to look at.")]
            CustomLogicVector3Builtin target)
        {
            _needSetLocalRotation = true;
            _needSetRotation = true;
            Value.LookAt(target);
        }

        [CLMethod("Sets the enabled state of all child renderers.")]
        public void SetRenderersEnabled(
            [CLParam("Whether to enable or disable the renderers.")]
            bool enabled)
        {
            foreach (var renderer in Value.GetComponentsInChildren<Renderer>())
                renderer.enabled = enabled;
        }

        [CLMethod("Gets colliders of the transform.", ReturnTypeArguments = new[] { "Collider" })]
        public CustomLogicListBuiltin GetColliders(
            [CLParam("If true, includes colliders from all children recursively (default: false).")]
            bool recursive = false)
        {
            var listBuiltin = new CustomLogicListBuiltin();
            if (recursive)
            {
                foreach (var collider in Value.gameObject.GetComponentsInChildren<Collider>())
                {
                    listBuiltin.List.Add(new CustomLogicColliderBuiltin(new object[] { collider }));
                }
            }
            else
            {
                foreach (var collider in Value.gameObject.GetComponents<Collider>())
                {
                    listBuiltin.List.Add(new CustomLogicColliderBuiltin(new object[] { collider }));
                }
            }
            return listBuiltin;
        }

        public static implicit operator CustomLogicTransformBuiltin(Transform value) => new CustomLogicTransformBuiltin(value);
        public static implicit operator Transform(CustomLogicTransformBuiltin value) => value.Value;

        [CLMethod("Checks if two transforms are equal. Returns: True if the transforms are equal, false otherwise.")]
        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (null, null) => true,
                (CustomLogicTransformBuiltin selfTransform, CustomLogicTransformBuiltin otherTransform) => selfTransform.Value == otherTransform.Value,
                _ => false
            };
        }

        [CLMethod("Gets the hash code of the transform. Returns: The hash code.")]
        public int __Hash__() => Value == null ? 0 : Value.GetHashCode();
    }
}
