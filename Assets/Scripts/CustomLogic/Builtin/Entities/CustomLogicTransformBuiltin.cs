using System;
using UnityEngine;
using UnityEngine.Video;

namespace CustomLogic
{
    /// <summary>
    /// Represents a transform.
    /// </summary>
    [CLType(Name = "Transform", Abstract = true)]
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

        /// <summary>
        /// Gets or sets the position of the transform.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Position
        {
            get => Value.position;
            set => Value.position = value.Value;
        }

        /// <summary>
        /// Gets or sets the local position of the transform.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin LocalPosition
        {
            get => Value.localPosition;
            set => Value.localPosition = value.Value;
        }

        /// <summary>
        /// Gets or sets the rotation of the transform.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Gets or sets the local rotation of the transform.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Gets or sets the quaternion rotation of the transform.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Gets or sets the quaternion local rotation of the transform.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Gets or sets the scale of the transform.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Scale
        {
            get => Value.localScale;
            set => Value.localScale = value.Value;
        }

        /// <summary>
        /// Gets the forward vector of the transform.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Forward
        {
            get => Value.forward;
            set => Value.forward = value.Value;
        }

        /// <summary>
        /// Gets the up vector of the transform.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Up
        {
            get => Value.up;
            set => Value.up = value.Value;
        }

        /// <summary>
        /// Gets the right vector of the transform.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Right
        {
            get => Value.right;
            set => Value.right = value.Value;
        }

        /// <summary>
        /// Sets the parent of the transform.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Gets the name of the transform.
        /// </summary>
        [CLProperty]
        public string Name
        {
            get => Value.name;
        }

        /// <summary>
        /// Gets the Physics Layer of the transform.
        /// </summary>
        [CLProperty]
        public int Layer
        {
            get => Value.gameObject.layer;
            set => Value.gameObject.layer = value;
        }

        /// <summary>
        /// The Animation attached to this transform, returns null if there is none.
        /// </summary>
        [CLProperty]
        public CustomLogicAnimationBuiltin Animation
        {
            get => _animation;
        }

        /// <summary>
        /// The Animator attached to this transform, returns null if there is none.
        /// </summary>
        [CLProperty]
        public CustomLogicAnimatorBuiltin Animator
        {
            get => _animator;
        }

        /// <summary>
        /// The AudioSource attached to this transform, returns null if there is none.
        /// </summary>
        [CLProperty]
        public CustomLogicAudioSourceBuiltin AudioSource
        {
            get => _audioSource;
        }

        /// <summary>
        /// Gets the transform of the specified child.
        /// </summary>
        /// <param name="name">The name of the child transform to find.</param>
        /// <returns>The child transform if found, null otherwise.</returns>
        [CLMethod]
        public CustomLogicTransformBuiltin GetTransform(string name)
        {
            var transform = Value.Find(name);
            return transform != null ? new CustomLogicTransformBuiltin(transform) : null;
        }

        /// <summary>
        /// Gets all child transforms.
        /// </summary>
        /// <returns>A list of all child transforms.</returns>
        [CLMethod(ReturnTypeArguments = new[] { "Transform" })]
        public CustomLogicListBuiltin GetTransforms()
        {
            var listBuiltin = new CustomLogicListBuiltin();
            foreach (Transform transform in Value)
            {
                listBuiltin.List.Add(new CustomLogicTransformBuiltin(transform));
            }
            return listBuiltin;
        }

        /// <summary>
        /// Checks if the given animation is playing.
        /// </summary>
        /// <param name="anim">The name of the animation to check.</param>
        /// <returns>True if the animation is playing, false otherwise.</returns>
        [CLMethod]
        public bool IsPlayingAnimation([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim)
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

        /// <summary>
        /// Plays the specified animation.
        /// </summary>
        /// <param name="anim">The name of the animation to play.</param>
        /// <param name="fade">The fade time in seconds for cross-fading (default: 0.1).</param>
        [CLMethod]
        public void PlayAnimation([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim, float fade = 0.1f)
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

        /// <summary>
        /// Gets the length of the specified animation.
        /// </summary>
        /// <param name="anim">The name of the animation.</param>
        /// <returns>The length of the animation in seconds, or -1 if not found.</returns>
        [CLMethod]
        public float GetAnimationLength([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string anim)
        {
            if (_animation != null)
                return _animation.GetAnimationLength(anim);
            else if (_animator != null)
                return _animator.GetAnimationLength(anim.Replace('.', '_'));
            return -1;
        }

        /// <summary>
        /// Plays the sound.
        /// </summary>
        [CLMethod]
        public void PlaySound()
        {
            if (!_audioSource.IsPlaying)
                _audioSource.Play();
        }

        /// <summary>
        /// Stops the sound.
        /// </summary>
        [CLMethod]
        public void StopSound()
        {
            if (_audioSource.IsPlaying)
                _audioSource.Stop();
        }

        /// <summary>
        /// Toggles the particle system.
        /// </summary>
        /// <param name="enabled">Whether to enable or disable the particle emission.</param>
        [CLMethod]
        public void ToggleParticle(bool enabled)
        {
            if (!_particleSystem.isPlaying)
                _particleSystem.Play();
            var emission = _particleSystem.emission;
            emission.enabled = enabled;
        }

        /// <summary>
        /// Transforms a direction from world space to local space.
        /// </summary>
        /// <param name="direction">The direction vector in world space.</param>
        /// <returns>The direction in local space.</returns>
        [CLMethod]
        public CustomLogicVector3Builtin InverseTransformDirection(CustomLogicVector3Builtin direction)
            => Value.InverseTransformDirection(direction);

        /// <summary>
        /// Transforms a position from world space to local space.
        /// </summary>
        /// <param name="point">The point in world space.</param>
        /// <returns>The position in local space.</returns>
        [CLMethod]
        public CustomLogicVector3Builtin InverseTransformPoint(CustomLogicVector3Builtin point)
            => Value.InverseTransformPoint(point);

        /// <summary>
        /// Transforms a direction from local space to world space.
        /// </summary>
        /// <param name="direction">The direction vector in local space.</param>
        /// <returns>The direction in world space.</returns>
        [CLMethod]
        public CustomLogicVector3Builtin TransformDirection(CustomLogicVector3Builtin direction)
            => Value.TransformDirection(direction);

        /// <summary>
        /// Transforms a position from local space to world space.
        /// </summary>
        /// <param name="point">The point in local space.</param>
        /// <returns>The position in world space.</returns>
        [CLMethod]
        public CustomLogicVector3Builtin TransformPoint(CustomLogicVector3Builtin point)
            => Value.TransformPoint(point);

        /// <summary>
        /// Rotates the transform by the given rotation in euler angles.
        /// </summary>
        /// <param name="rotation">The rotation in euler angles to apply.</param>
        [CLMethod]
        public void Rotate(CustomLogicVector3Builtin rotation)
        {
            Value.Rotate(rotation);
            _needSetLocalRotation = true;
            _needSetRotation = true;
        }

        /// <summary>
        /// Rotates the transform around a point by the given angle.
        /// </summary>
        /// <param name="point">The point to rotate around.</param>
        /// <param name="axis">The axis to rotate around.</param>
        /// <param name="angle">The angle in degrees to rotate.</param>
        [CLMethod]
        public void RotateAround(CustomLogicVector3Builtin point, CustomLogicVector3Builtin axis, float angle)
        {
            _needSetLocalRotation = true;
            _needSetRotation = true;
            Value.RotateAround(point.Value, axis.Value, angle);
        }

        /// <summary>
        /// Rotates the transform to look at the target position.
        /// </summary>
        /// <param name="target">The world position to look at.</param>
        [CLMethod]
        public void LookAt(CustomLogicVector3Builtin target)
        {
            _needSetLocalRotation = true;
            _needSetRotation = true;
            Value.LookAt(target);
        }

        /// <summary>
        /// Sets the enabled state of all child renderers.
        /// </summary>
        /// <param name="enabled">Whether to enable or disable the renderers.</param>
        [CLMethod]
        public void SetRenderersEnabled(bool enabled)
        {
            foreach (var renderer in Value.GetComponentsInChildren<Renderer>())
                renderer.enabled = enabled;
        }

        /// <summary>
        /// Gets colliders of the transform.
        /// </summary>
        /// <param name="recursive">If true, includes colliders from all children recursively (default: false).</param>
        /// <returns>A list of colliders.</returns>
        [CLMethod(ReturnTypeArguments = new[] { "Collider" })]
        public CustomLogicListBuiltin GetColliders(bool recursive = false)
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

        [CLMethod]
        public void SetActive(bool active)
        {
            Value.gameObject.SetActive(active);
        }

        public static implicit operator CustomLogicTransformBuiltin(Transform value) => new CustomLogicTransformBuiltin(value);
        public static implicit operator Transform(CustomLogicTransformBuiltin value) => value.Value;

        /// <summary>
        /// Checks if two transforms are equal.
        /// </summary>
        /// <param name="self">The first transform.</param>
        /// <param name="other">The second transform.</param>
        /// <returns>True if the transforms are equal, false otherwise.</returns>
        [CLMethod]
        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (null, null) => true,
                (CustomLogicTransformBuiltin selfTransform, CustomLogicTransformBuiltin otherTransform) => selfTransform.Value == otherTransform.Value,
                _ => false
            };
        }

        /// <summary>
        /// Gets the hash code of the transform.
        /// </summary>
        /// <returns>The hash code.</returns>
        [CLMethod]
        public int __Hash__() => Value == null ? 0 : Value.GetHashCode();
    }
}
