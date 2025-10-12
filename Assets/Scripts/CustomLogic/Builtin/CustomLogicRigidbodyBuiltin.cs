using System;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "RigidbodyBuiltin", Static = true, Abstract = true, Description = "")]
    partial class CustomLogicRigidbodyBuiltin : BuiltinComponentInstance
    {
        public Rigidbody Value;
        public ConstantForce CustomGravity;
        public BuiltinClassInstance OwnerBuiltin;
        public GameObject AttachedGameObject;
        private bool _isGravityEnabled = true;
        private Vector3? _gravity;

        [CLConstructor]
        public CustomLogicRigidbodyBuiltin() : base(null) { }

        /// <summary>
        /// MapObject rigidbody constructor.
        /// </summary>
        public CustomLogicRigidbodyBuiltin(CustomLogicMapObjectBuiltin owner, float mass = 1, Vector3? gravity = null, bool freezeRotation = false, bool interpolate = false) : base(GetOrAddComponent<Rigidbody>(owner.Value.GameObject))
        {
            OwnerBuiltin = owner;
            AttachedGameObject = owner.Value.GameObject;
            Value = (Rigidbody)Component;

            Gravity = gravity; // Set the custom gravity force
            UseGravity = true;
            FreezeXRotation = FreezeYRotation = FreezeZRotation = freezeRotation; // Freeze all rotation axes
            Interpolate = interpolate;
        }

        /// <summary>
        /// Simpler case for any of our objects that have a rigidbody by default, could be cached but this should really be good enough.
        /// GC Alloc here will be in the new CustomLogicRigidbodyBuiltin call itself.
        /// </summary>
        public CustomLogicRigidbodyBuiltin(BuiltinClassInstance owner, Rigidbody rb) : base(rb)
        {
            OwnerBuiltin = owner;
            AttachedGameObject = rb.gameObject;
            Value = (Rigidbody)Component;
        }

        // Add Static Getters for ForceMode
        [CLProperty(Name = "ForceModeAcceleration", Static = true, Description = "ForceMode.Acceleration")]
        public static int ForceModeAcceleration => (int)ForceMode.Acceleration;

        [CLProperty(Name = "ForceModeForce", Static = true, Description = "ForceMode.Force")]
        public static int ForceModeForce => (int)ForceMode.Force;

        [CLProperty(Name = "ForceModeImpulse", Static = true, Description = "ForceMode.Impulse")]
        public static int ForceModeImpulse => (int)ForceMode.Impulse;

        [CLProperty(Name = "ForceModeVelocityChange", Static = true, Description = "ForceMode.VelocityChange")]
        public static int ForceModeVelocityChange => (int)ForceMode.VelocityChange;

        [CLProperty(Description = "The MapObject this rigidbody is attached to.")]
        public BuiltinClassInstance Owner => OwnerBuiltin;
        // Position
        [CLProperty(Description = "The position of the Rigidbody in world space. This is the same as the position of the GameObject it is attached to.")]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Value.position);
            set => Value.position = value.Value;
        }

        // Rotation
        [CLProperty(Description = "The rotation of the Rigidbody in world space. This is the same as the rotation of the GameObject it is attached to.")]
        public CustomLogicQuaternionBuiltin Rotation
        {
            get => new CustomLogicQuaternionBuiltin(Value.rotation);
            set => Value.rotation = value.Value;
        }

        // Velocity
        [CLProperty(Description = "The velocity of the Rigidbody in world space.")]
        public CustomLogicVector3Builtin Velocity
        {
            get => new CustomLogicVector3Builtin(Value.velocity);
            set => Value.velocity = value.Value;
        }

        // Angular Velocity
        [CLProperty(Description = "The angular velocity of the Rigidbody in world space.")]
        public CustomLogicVector3Builtin AngularVelocity
        {
            get => new CustomLogicVector3Builtin(Value.angularVelocity);
            set => Value.angularVelocity = value.Value;
        }

        // Angular damping
        [CLProperty(Description = "The angular damping of the Rigidbody. This is a multiplier applied to the angular velocity every frame, reducing it over time.")]
        public float AngularDrag
        {
            get => Value.angularDrag;
            set => Value.angularDrag = value;
        }

        [CLProperty(Description = "The Mass of the Rigidbody")]
        public float Mass
        {
            get => Value.mass;
            set => Value.mass = value;
        }

        [CLProperty(Description = "Whether or not the Rigidbody use gravity.")]
        public bool UseGravity
        {
            get => _isGravityEnabled;
            set
            {
                _isGravityEnabled = value;
                if (CustomGravity != null)
                {
                    Value.useGravity = false;
                    CustomGravity.enabled = value;
                }
                else
                {
                    Value.useGravity = value;
                }
            }
        }

        /// <summary>
        /// The force of gravity applied to the Rigidbody.
        /// If null, the Rigidbody will use Unity's default gravity settings and will enable gravity.
        /// If Vector3 is provided, it will apply that as a custom gravity force using ConstantForce and disable gravity.
        /// </summary>
        [CLProperty(Description = "The force of gravity.")]
        public CustomLogicVector3Builtin? Gravity
        {
            get => _gravity;
            set
            {
                _gravity = value == null ? null : value.Value; // Always update stored gravity
                if (value == null)
                {
                    if (CustomGravity != null)
                    {
                        CustomGravity.enabled = false;
                        GameObject.Destroy(CustomGravity);
                        CustomGravity = null;
                    }
                    Value.useGravity = UseGravity;
                }
                else
                {
                    if (CustomGravity == null)
                    {
                        CustomGravity = Value.gameObject.AddComponent<ConstantForce>();
                        Value.useGravity = false;
                    }

                    CustomGravity.force = value.Value;
                    CustomGravity.enabled = UseGravity;
                }
            }
        }

        [CLProperty(Description = "If the x movement axis is frozen")]
        public bool FreezeXPosition
        {
            get => Value.constraints.HasFlag(RigidbodyConstraints.FreezePositionX);
            set
            {
                if (value)
                    Value.constraints |= RigidbodyConstraints.FreezePositionX;
                else
                    Value.constraints &= ~RigidbodyConstraints.FreezePositionX;
            }
        }

        [CLProperty(Description = "If the y movement axis is frozen")]
        public bool FreezeYPosition
        {
            get => Value.constraints.HasFlag(RigidbodyConstraints.FreezePositionY);
            set
            {
                if (value)
                    Value.constraints |= RigidbodyConstraints.FreezePositionY;
                else
                    Value.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
        }

        [CLProperty(Description = "If the z movement axis is frozen")]
        public bool FreezeZPosition
        {
            get => Value.constraints.HasFlag(RigidbodyConstraints.FreezePositionZ);
            set
            {
                if (value)
                    Value.constraints |= RigidbodyConstraints.FreezePositionZ;
                else
                    Value.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            }
        }

        [CLProperty(Description = "If the x rotation axis is frozen")]
        public bool FreezeXRotation
        {
            get => Value.constraints.HasFlag(RigidbodyConstraints.FreezeRotationX);
            set
            {
                if (value)
                    Value.constraints |= RigidbodyConstraints.FreezeRotationX;
                else
                    Value.constraints &= ~RigidbodyConstraints.FreezeRotationX;
            }
        }

        [CLProperty(Description = "If the y rotation axis is frozen")]
        public bool FreezeYRotation
        {
            get => Value.constraints.HasFlag(RigidbodyConstraints.FreezeRotationY);
            set
            {
                if (value)
                    Value.constraints |= RigidbodyConstraints.FreezeRotationY;
                else
                    Value.constraints &= ~RigidbodyConstraints.FreezeRotationY;
            }
        }

        [CLProperty(Description = "If the z rotation axis is frozen")]
        public bool FreezeZRotation
        {
            get => Value.constraints.HasFlag(RigidbodyConstraints.FreezeRotationZ);
            set
            {
                if (value)
                    Value.constraints |= RigidbodyConstraints.FreezeRotationZ;
                else
                    Value.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
            }
        }

        [CLProperty(Description = "Freeze all rotations")]
        public bool FreezeAllRotations
        {
            get => Value.constraints.HasFlag(RigidbodyConstraints.FreezeRotation);
            set
            {
                if (value)
                    Value.constraints |= RigidbodyConstraints.FreezeRotation;
                else
                    Value.constraints &= ~RigidbodyConstraints.FreezeRotation;
            }
        }

        [CLProperty(Description = "Freeze all positions. This will also freeze all rotations.")]
        public bool FreezeAllPositions
        {
            get => Value.constraints.HasFlag(RigidbodyConstraints.FreezePosition);
            set
            {
                if (value)
                    Value.constraints |= RigidbodyConstraints.FreezePosition;
                else
                    Value.constraints &= ~RigidbodyConstraints.FreezePosition;
            }
        }


        [CLProperty(Description = "If the Rigidbody is kinematic. Kinematic bodies are not affected by forces and can only be moved manually.")]
        public bool IsKinematic
        {
            get => Value.isKinematic;
            set => Value.isKinematic = value;
        }

        // Interpolation
        [CLProperty(Description = "Interpolation mode of the Rigidbody. If true, it will interpolate between frames.")]
        public bool Interpolate
        {
            get => Value.interpolation != RigidbodyInterpolation.None;
            set => Value.interpolation = value ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
        }

        // Center of mass
        [CLProperty(Description = "The center of mass of the Rigidbody in local space.")]
        public CustomLogicVector3Builtin CenterOfMass
        {
            get => Value.centerOfMass;
            set => Value.centerOfMass = value;
        }

        // Collision detection mode
        [CLProperty(Description = "The collision detection mode of the Rigidbody. This determines how collisions are detected and resolved.")]
        public string CollisionDetectionMode
        {
            get => Value.collisionDetectionMode.ToString();
            set
            {
                switch (value)
                {
                    case "Discrete":
                        Value.collisionDetectionMode = UnityEngine.CollisionDetectionMode.Discrete;
                        break;
                    case "Continuous":
                        Value.collisionDetectionMode = UnityEngine.CollisionDetectionMode.Continuous;
                        break;
                    case "ContinuousDynamic":
                        Value.collisionDetectionMode = UnityEngine.CollisionDetectionMode.ContinuousDynamic;
                        break;
                    case "ContinuousSpeculative":
                        Value.collisionDetectionMode = UnityEngine.CollisionDetectionMode.ContinuousSpeculative;
                        break;
                    default:
                        throw new ArgumentException($"Invalid collision detection mode: {value}, valid options are Discrete, Continuous, ContinuousDynamic, and ContinuousSpeculative");
                }
            }
        }

        // Detect Collisions
        [CLProperty(Description = "If the Rigidbody detects collisions. If false, it will not collide with other colliders.")]
        public bool DetectCollisions
        {
            get => Value.detectCollisions;
            set => Value.detectCollisions = value;
        }

        [CLMethod(Description = "Apply a force to the Rigidbody - legacy version, please use optimized if possible.")]
        public void AddForce(CustomLogicVector3Builtin force, string forceMode = "Acceleration", CustomLogicVector3Builtin? atPoint = null)
        {
            ForceMode mode = ForceMode.Acceleration;
            switch (forceMode)
            {
                case "Force":
                    mode = ForceMode.Force;
                    break;
                case "Acceleration":
                    mode = ForceMode.Acceleration;
                    break;
                case "Impulse":
                    mode = ForceMode.Impulse;
                    break;
                case "VelocityChange":
                    mode = ForceMode.VelocityChange;
                    break;
                default:
                    throw new ArgumentException($"Invalid force mode: {forceMode}, valid options are Force, Acceleration, Impulse, and VelocityChange");
            }
            AddForceOptimized(force, (int)mode, atPoint);
        }

        [CLMethod(Description = "Apply a force to the Rigidbody.")]
        public void AddForceOptimized(CustomLogicVector3Builtin force, int forceMode = 5, CustomLogicVector3Builtin? atPoint = null)
        {
            ForceMode mode = (ForceMode)forceMode;
            if (atPoint != null)
            {
                Value.AddForceAtPosition(force.Value, atPoint.Value, mode);
            }
            else
            {
                Value.AddForce(force.Value, mode);
            }
        }

        [CLMethod(Description = "Apply a torque to the Rigidbody - legacy version, please use optimized if possible.")]
        public void AddTorque(CustomLogicVector3Builtin torque, string forceMode)
        {
            ForceMode mode = ForceMode.Acceleration;
            switch (forceMode)
            {
                case "Force":
                    mode = ForceMode.Force;
                    break;
                case "Acceleration":
                    mode = ForceMode.Acceleration;
                    break;
                case "Impulse":
                    mode = ForceMode.Impulse;
                    break;
                case "VelocityChange":
                    mode = ForceMode.VelocityChange;
                    break;
                default:
                    throw new ArgumentException($"Invalid force mode: {forceMode}, valid options are Force, Acceleration, Impulse, and VelocityChange");
            }
            AddTorqueOptimized(torque, (int)mode);
        }

        [CLMethod(Description = "Apply a torque to the Rigidbody.")]
        public void AddTorqueOptimized(CustomLogicVector3Builtin torque, int forceMode = 5)
        {
            ForceMode mode = (ForceMode)forceMode;
            Value.AddTorque(torque.Value, mode);
        }

        // AddExplosionForce
        [CLMethod(Description = "Apply an explosion force to the Rigidbody.")]
        public void AddExplosionForce(float explosionForce, CustomLogicVector3Builtin explosionPosition, float explosionRadius, float upwardsModifier = 0.0f, int forceMode = 5)
        {
            ForceMode mode = (ForceMode)forceMode;
            Value.AddExplosionForce(explosionForce, explosionPosition.Value, explosionRadius, upwardsModifier, mode);
        }

        // Move
        [CLMethod(Description = "Move the Rigidbody to a new position. This will not apply any forces, it will just set the position directly.")]
        public void Move(CustomLogicVector3Builtin position, CustomLogicQuaternionBuiltin rotation)
        {
            Value.Move(position, rotation);
        }

        // MovePosition
        [CLMethod(Description = "Move the Rigidbody to a new position. This will apply forces to move the Rigidbody to the new position.")]
        public void MovePosition(CustomLogicVector3Builtin position)
        {
            Value.MovePosition(position);
        }

        // MoveRotation
        [CLMethod(Description = "Move the Rigidbody to a new rotation. This will apply forces to rotate the Rigidbody to the new rotation.")]
        public void MoveRotation(CustomLogicQuaternionBuiltin rotation)
        {
            Value.MoveRotation(rotation);
        }

        // ResetCenterOfMass
        [CLMethod(Description = "Reset the center of mass of the Rigidbody to the default value (0, 0, 0). This will also reset the inertia tensor.")]
        public void ResetCenterOfMass()
        {
            Value.ResetCenterOfMass();
        }

        // PublishTransform
        [CLMethod(Description = "Publish the current position and rotation of the Rigidbody to the MapObject. This will update the MapObject's transform to match the Rigidbody's transform.")]
        public void PublishTransform()
        {
            Value.PublishTransform();
        }
    }
}
