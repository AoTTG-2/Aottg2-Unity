using UnityEngine;

namespace CustomLogic
{
    [CLType(Static = false)]
    class CustomLogicTransformBuiltin : CustomLogicClassInstanceBuiltin, ICustomLogicEquals
    {
        public readonly Transform Value;
        
        private Vector3 _internalRotation;
        private Vector3 _internalLocalRotation;
        private bool _needSetRotation = true;
        private bool _needSetLocalRotation = true;

        private readonly Animation _animation;
        private readonly AudioSource _audioSource;
        private readonly ParticleSystem _particleSystem;

        public CustomLogicTransformBuiltin(Transform transform) : base("Transform")
        {
            Value = transform;
            
            _animation = Value.GetComponent<Animation>();
            _audioSource = Value.GetComponent<AudioSource>();
            _particleSystem = Value.GetComponent<ParticleSystem>();
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
            set => Value.rotation = value.Value;
        }

        [CLProperty("Gets or sets the quaternion local rotation of the transform.")]
        public CustomLogicQuaternionBuiltin QuaternionLocalRotation
        {
            get => Value.localRotation;
            set => Value.localRotation = value.Value;
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

        [CLMethod("Gets the transform of the specified child.")]
        public CustomLogicTransformBuiltin GetTransform(string name)
        {
            var transform = Value.Find(name);
            return transform != null ? new CustomLogicTransformBuiltin(transform) : null;
        }

        [CLMethod("Gets all child transforms.")]
        public CustomLogicListBuiltin GetTransforms()
        {
            var listBuiltin = new CustomLogicListBuiltin();
            foreach (Transform transform in Value)
            {
                listBuiltin.List.Add(new CustomLogicTransformBuiltin(transform));
            }
            return listBuiltin;
        }

        [CLMethod("Plays the specified animation.")]
        public void PlayAnimation(string anim, float fade = 0.1f)
        {
            if (!_animation.IsPlaying(anim))
                _animation.CrossFade(anim, fade);
        }

        [CLMethod("Gets the length of the specified animation.")]
        public float GetAnimationLength(string anim) => _animation[anim].length;

        [CLMethod("Plays the sound.")]
        public void PlaySound()
        {
            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }

        [CLMethod("Stops the sound.")]
        public void StopSound()
        {
            if (_audioSource.isPlaying)
                _audioSource.Stop();
        }

        [CLMethod("Toggles the particle system.")]
        public void ToggleParticle(bool enabled)
        {
            if (!_particleSystem.isPlaying)
                _particleSystem.Play();
            var emission = _particleSystem.emission;
            emission.enabled = enabled;
        }

        /// <inheritdoc cref="Transform.InverseTransformDirection(Vector3)"/>
        [CLMethod] public CustomLogicVector3Builtin InverseTransformDirection(CustomLogicVector3Builtin direction) => Value.InverseTransformDirection(direction);

        /// <inheritdoc cref="Transform.InverseTransformPoint(Vector3)"/>
        [CLMethod] public CustomLogicVector3Builtin InverseTransformPoint(CustomLogicVector3Builtin point) => Value.InverseTransformPoint(point);

        /// <inheritdoc cref="Transform.TransformDirection(Vector3)"/>
        [CLMethod] public CustomLogicVector3Builtin TransformDirection(CustomLogicVector3Builtin direction) => Value.TransformDirection(direction);

        /// <inheritdoc cref="Transform.TransformPoint(Vector3)"/>
        [CLMethod] public CustomLogicVector3Builtin TransformPoint(CustomLogicVector3Builtin point) => Value.TransformPoint(point);

        /// <inheritdoc cref="Transform.Rotate(Vector3)"/>
        [CLMethod] public void Rotate(CustomLogicVector3Builtin rotation) => Value.Rotate(rotation);

        /// <inheritdoc cref="Transform.RotateAround(Vector3, Vector3, float)"/>
        [CLMethod] public void RotateAround(CustomLogicVector3Builtin point, CustomLogicVector3Builtin axis, float angle) => Value.RotateAround(point.Value, axis.Value, angle);
        
        /// <inheritdoc cref="Transform.LookAt(Vector3)"/>
        [CLMethod] public void LookAt(CustomLogicVector3Builtin target) => Value.LookAt(target);

        [CLMethod("Sets the enabled state of all child renderers.")]
        public void SetRenderersEnabled(bool enabled)
        {
            foreach (var renderer in Value.GetComponentsInChildren<Renderer>())
                renderer.enabled = enabled;
        }
        
        public static implicit operator CustomLogicTransformBuiltin(Transform value) => new CustomLogicTransformBuiltin(value);
        public static implicit operator Transform(CustomLogicTransformBuiltin value) => value.Value;
        
        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (null, null) => true,
                (CustomLogicTransformBuiltin selfTransform, CustomLogicTransformBuiltin otherTransform) => selfTransform.Value == otherTransform.Value,
                _ => false
            };
        }

        public int __Hash__() => Value == null ? 0 : Value.GetHashCode();
    }
}
