using System;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents a Rigidbody component that enables physics simulation for game objects.
    /// </summary>
    [CLType(Name = "RigidbodyBuiltin", Static = true, Abstract = true, IsComponent = true)]
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

        public CustomLogicRigidbodyBuiltin(BuiltinClassInstance owner, Rigidbody rb) : base(rb)
        {
            OwnerBuiltin = owner;
            AttachedGameObject = rb.gameObject;
            Value = (Rigidbody)Component;
        }

        // Add Static Getters for ForceMode
        /// <summary>
        /// ForceMode.Acceleration.
        /// </summary>
        [CLProperty(Name = "ForceModeAcceleration")]
        public static int ForceModeAcceleration => (int)ForceMode.Acceleration;

        /// <summary>
        /// ForceMode.Force.
        /// </summary>
        [CLProperty(Name = "ForceModeForce")]
        public static int ForceModeForce => (int)ForceMode.Force;

        /// <summary>
        /// ForceMode.Impulse.
        /// </summary>
        [CLProperty(Name = "ForceModeImpulse")]
        public static int ForceModeImpulse => (int)ForceMode.Impulse;

        /// <summary>
        /// ForceMode.VelocityChange.
        /// </summary>
        [CLProperty(Name = "ForceModeVelocityChange")]
        public static int ForceModeVelocityChange => (int)ForceMode.VelocityChange;

        /// <summary>
        /// The MapObject this rigidbody is attached to.
        /// </summary>
        [CLProperty]
        public BuiltinClassInstance Owner => OwnerBuiltin;
        // Position
        /// <summary>
        /// The position of the Rigidbody in world space. This is the same as the position of the GameObject it is attached to.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Value.position);
            set => Value.position = value.Value;
        }

        // Rotation
        /// <summary>
        /// The rotation of the Rigidbody in world space. This is the same as the rotation of the GameObject it is attached to.
        /// </summary>
        [CLProperty]
        public CustomLogicQuaternionBuiltin Rotation
        {
            get => new CustomLogicQuaternionBuiltin(Value.rotation);
            set => Value.rotation = value.Value;
        }

        // Velocity
        /// <summary>
        /// The velocity of the Rigidbody in world space.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Velocity
        {
            get => new CustomLogicVector3Builtin(Value.velocity);
            set => Value.velocity = value.Value;
        }

        // Angular Velocity
        /// <summary>
        /// The angular velocity of the Rigidbody in world space.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin AngularVelocity
        {
            get => new CustomLogicVector3Builtin(Value.angularVelocity);
            set => Value.angularVelocity = value.Value;
        }

        // Angular damping
        /// <summary>
        /// The angular damping of the Rigidbody. This is a multiplier applied to the angular velocity every frame,
        /// reducing it over time.
        /// </summary>
        [CLProperty]
        public float AngularDrag
        {
            get => Value.angularDrag;
            set => Value.angularDrag = value;
        }

        /// <summary>
        /// The Mass of the Rigidbody.
        /// </summary>
        [CLProperty]
        public float Mass
        {
            get => Value.mass;
            set => Value.mass = value;
        }

        /// <summary>
        /// Whether or not the Rigidbody use gravity.
        /// </summary>
        [CLProperty]
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
        /// The force of gravity applied to the Rigidbody. If null, the Rigidbody will use Unity's default gravity settings
        /// and will enable gravity. If Vector3 is provided, it will apply that as a custom gravity force using ConstantForce
        /// and disable gravity.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// If the x movement axis is frozen.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// If the y movement axis is frozen.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// If the z movement axis is frozen.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// If the x rotation axis is frozen.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// If the y rotation axis is frozen.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// If the z rotation axis is frozen.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Freeze all rotations.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Freeze all positions. This will also freeze all rotations.
        /// </summary>
        [CLProperty]
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


        /// <summary>
        /// If the Rigidbody is kinematic. Kinematic bodies are not affected by forces and can only be moved manually.
        /// </summary>
        [CLProperty]
        public bool IsKinematic
        {
            get => Value.isKinematic;
            set => Value.isKinematic = value;
        }

        // Interpolation
        /// <summary>
        /// Interpolation mode of the Rigidbody. If true, it will interpolate between frames.
        /// </summary>
        [CLProperty]
        public bool Interpolate
        {
            get => Value.interpolation != RigidbodyInterpolation.None;
            set => Value.interpolation = value ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
        }

        // Center of mass
        /// <summary>
        /// The center of mass of the Rigidbody in local space.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin CenterOfMass
        {
            get => Value.centerOfMass;
            set => Value.centerOfMass = value;
        }

        // Collision detection mode
        /// <summary>
        /// The collision detection mode of the Rigidbody. This determines how collisions are detected and resolved.
        /// </summary>
        [CLProperty(Enum = typeof(CustomLogicCollisionDetectionModeEnum))]
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
        /// <summary>
        /// If the Rigidbody detects collisions. If false, it will not collide with other colliders.
        /// </summary>
        [CLProperty]
        public bool DetectCollisions
        {
            get => Value.detectCollisions;
            set => Value.detectCollisions = value;
        }

        /// <summary>
        /// Apply a force to the Rigidbody - legacy version, please use optimized if possible.
        /// </summary>
        /// <param name="force">The force vector to apply.</param>
        /// <param name="forceMode">The force mode.</param>
        /// <param name="atPoint">Optional. If provided, applies force at this world position instead of center of mass.</param>
        [CLMethod]
        public void AddForce(
            CustomLogicVector3Builtin force,
            [CLParam(Enum = typeof(CustomLogicForceModeEnum))] string forceMode = "Acceleration",
            CustomLogicVector3Builtin? atPoint = null)
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

        /// <summary>
        /// Apply a force to the Rigidbody.
        /// </summary>
        /// <param name="force">The force vector to apply.</param>
        /// <param name="forceMode">The force mode as integer (use ForceModeAcceleration, ForceModeForce, ForceModeImpulse, or ForceModeVelocityChange constants, default: 5).</param>
        /// <param name="atPoint">Optional. If provided, applies force at this world position instead of center of mass.</param>
        [CLMethod]
        public void AddForceOptimized(
            CustomLogicVector3Builtin force,
            int forceMode = 5,
            CustomLogicVector3Builtin? atPoint = null)
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

        /// <summary>
        /// Apply a torque to the Rigidbody - legacy version, please use optimized if possible.
        /// </summary>
        /// <param name="torque">The torque vector to apply.</param>
        /// <param name="forceMode">The force mode.</param>
        [CLMethod]
        public void AddTorque(
            CustomLogicVector3Builtin torque,
            [CLParam(Enum = typeof(CustomLogicForceModeEnum))] string forceMode)
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

        /// <summary>
        /// Apply a torque to the Rigidbody.
        /// </summary>
        /// <param name="torque">The torque vector to apply.</param>
        /// <param name="forceMode">The force mode as integer (use ForceModeAcceleration, ForceModeForce, ForceModeImpulse, or ForceModeVelocityChange constants, default: 5).</param>
        [CLMethod]
        public void AddTorqueOptimized(CustomLogicVector3Builtin torque, int forceMode = 5)
        {
            ForceMode mode = (ForceMode)forceMode;
            Value.AddTorque(torque.Value, mode);
        }

        /// <summary>
        /// Apply an explosion force to the Rigidbody.
        /// </summary>
        /// <param name="explosionForce">The force of the explosion.</param>
        /// <param name="explosionPosition">The center position of the explosion.</param>
        /// <param name="explosionRadius">The radius of the explosion.</param>
        /// <param name="upwardsModifier">Adjustment to the apparent position of the explosion to make it seem to lift objects (default: 0.0).</param>
        /// <param name="forceMode">The force mode as integer (use ForceModeAcceleration, ForceModeForce, ForceModeImpulse, or ForceModeVelocityChange constants, default: 5).</param>
        [CLMethod]
        public void AddExplosionForce(
            float explosionForce,
            CustomLogicVector3Builtin explosionPosition,
            float explosionRadius,
            float upwardsModifier = 0.0f,
            int forceMode = 5)
        {
            ForceMode mode = (ForceMode)forceMode;
            Value.AddExplosionForce(explosionForce, explosionPosition.Value, explosionRadius, upwardsModifier, mode);
        }

        /// <summary>
        /// Move the Rigidbody to a new position. This will not apply any forces, it will just set the position directly.
        /// </summary>
        /// <param name="position">The new position.</param>
        /// <param name="rotation">The new rotation.</param>
        [CLMethod]
        public void Move(CustomLogicVector3Builtin position, CustomLogicQuaternionBuiltin rotation)
        {
            Value.Move(position, rotation);
        }

        /// <summary>
        /// Move the Rigidbody to a new position. This will apply forces to move the Rigidbody to the new position.
        /// </summary>
        /// <param name="position">The target position.</param>
        [CLMethod]
        public void MovePosition(CustomLogicVector3Builtin position)
        {
            Value.MovePosition(position);
        }

        /// <summary>
        /// Move the Rigidbody to a new rotation. This will apply forces to rotate the Rigidbody to the new rotation.
        /// </summary>
        /// <param name="rotation">The target rotation.</param>
        [CLMethod]
        public void MoveRotation(CustomLogicQuaternionBuiltin rotation)
        {
            Value.MoveRotation(rotation);
        }

        // ResetCenterOfMass
        /// <summary>
        /// Reset the center of mass of the Rigidbody to the default value (0, 0, 0).
        /// This will also reset the inertia tensor.
        /// </summary>
        [CLMethod]
        public void ResetCenterOfMass()
        {
            Value.ResetCenterOfMass();
        }

        // PublishTransform
        /// <summary>
        /// Publish the current position and rotation of the Rigidbody to the MapObject.
        /// This will update the MapObject's transform to match the Rigidbody's transform.
        /// </summary>
        [CLMethod]
        public void PublishTransform()
        {
            Value.PublishTransform();
        }

        /// <summary>
        /// Checks if the rigidbody would collide with anything, returns a LineCastHitResult object.
        /// </summary>
        /// <param name="direction">The direction to sweep in.</param>
        /// <param name="distance">The distance to sweep.</param>
        /// <returns>A LineCastHitResult if collision detected, null otherwise.</returns>
        [CLMethod]
        public object SweepTest(CustomLogicVector3Builtin direction, float distance)
        {
            if (Value.SweepTest(direction, out RaycastHit hit, distance))
            {
                var collider = CustomLogicCollisionHandler.GetBuiltin(hit.collider);
                if (collider != null)
                {
                    return new CustomLogicLineCastHitResultBuiltin
                    {
                        IsCharacter = collider != null && collider is CustomLogicCharacterBuiltin,
                        IsMapObject = collider != null && collider is CustomLogicMapObjectBuiltin,
                        Point = new CustomLogicVector3Builtin(hit.point),
                        Normal = new CustomLogicVector3Builtin(hit.normal),
                        Distance = hit.distance,
                        Collider = collider,
                        ColliderInfo = new CustomLogicColliderBuiltin(new object[] { hit.collider })
                    };
                }
            }
            return null;
        }
    }
}
