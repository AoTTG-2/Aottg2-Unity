using System;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "RigidbodyBuiltin", Static = true, Abstract = true, Description = "Represents a Rigidbody component that enables physics simulation for game objects.", IsComponent = true)]
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
        [CLProperty(Name = "ForceModeAcceleration", Description = "ForceMode.Acceleration")]
        public static int ForceModeAcceleration => (int)ForceMode.Acceleration;

        [CLProperty(Name = "ForceModeForce", Description = "ForceMode.Force")]
        public static int ForceModeForce => (int)ForceMode.Force;

        [CLProperty(Name = "ForceModeImpulse", Description = "ForceMode.Impulse")]
        public static int ForceModeImpulse => (int)ForceMode.Impulse;

        [CLProperty(Name = "ForceModeVelocityChange", Description = "ForceMode.VelocityChange")]
        public static int ForceModeVelocityChange => (int)ForceMode.VelocityChange;

        [CLProperty("The MapObject this rigidbody is attached to.")]
        public BuiltinClassInstance Owner => OwnerBuiltin;
        // Position
        [CLProperty("The position of the Rigidbody in world space. This is the same as the position of the GameObject it is attached to.")]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Value.position);
            set => Value.position = value.Value;
        }

        // Rotation
        [CLProperty("The rotation of the Rigidbody in world space. This is the same as the rotation of the GameObject it is attached to.")]
        public CustomLogicQuaternionBuiltin Rotation
        {
            get => new CustomLogicQuaternionBuiltin(Value.rotation);
            set => Value.rotation = value.Value;
        }

        // Velocity
        [CLProperty("The velocity of the Rigidbody in world space.")]
        public CustomLogicVector3Builtin Velocity
        {
            get => new CustomLogicVector3Builtin(Value.velocity);
            set => Value.velocity = value.Value;
        }

        // Angular Velocity
        [CLProperty("The angular velocity of the Rigidbody in world space.")]
        public CustomLogicVector3Builtin AngularVelocity
        {
            get => new CustomLogicVector3Builtin(Value.angularVelocity);
            set => Value.angularVelocity = value.Value;
        }

        // Angular damping
        [CLProperty("The angular damping of the Rigidbody. This is a multiplier applied to the angular velocity every frame, reducing it over time.")]
        public float AngularDrag
        {
            get => Value.angularDrag;
            set => Value.angularDrag = value;
        }

        [CLProperty("The Mass of the Rigidbody")]
        public float Mass
        {
            get => Value.mass;
            set => Value.mass = value;
        }

        [CLProperty("Whether or not the Rigidbody use gravity.")]
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

        [CLProperty("The force of gravity applied to the Rigidbody. If null, the Rigidbody will use Unity's default gravity settings and will enable gravity. If Vector3 is provided, it will apply that as a custom gravity force using ConstantForce and disable gravity.")]
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

        [CLProperty("If the x movement axis is frozen")]
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

        [CLProperty("If the y movement axis is frozen")]
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

        [CLProperty("If the z movement axis is frozen")]
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

        [CLProperty("If the x rotation axis is frozen")]
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

        [CLProperty("If the y rotation axis is frozen")]
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

        [CLProperty("If the z rotation axis is frozen")]
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

        [CLProperty("Freeze all rotations")]
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

        [CLProperty("Freeze all positions. This will also freeze all rotations.")]
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


        [CLProperty("If the Rigidbody is kinematic. Kinematic bodies are not affected by forces and can only be moved manually.")]
        public bool IsKinematic
        {
            get => Value.isKinematic;
            set => Value.isKinematic = value;
        }

        // Interpolation
        [CLProperty("Interpolation mode of the Rigidbody. If true, it will interpolate between frames.")]
        public bool Interpolate
        {
            get => Value.interpolation != RigidbodyInterpolation.None;
            set => Value.interpolation = value ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
        }

        // Center of mass
        [CLProperty("The center of mass of the Rigidbody in local space.")]
        public CustomLogicVector3Builtin CenterOfMass
        {
            get => Value.centerOfMass;
            set => Value.centerOfMass = value;
        }

        // Collision detection mode
        [CLProperty("The collision detection mode of the Rigidbody. This determines how collisions are detected and resolved.")]
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
        [CLProperty("If the Rigidbody detects collisions. If false, it will not collide with other colliders.")]
        public bool DetectCollisions
        {
            get => Value.detectCollisions;
            set => Value.detectCollisions = value;
        }

        [CLMethod("Apply a force to the Rigidbody - legacy version, please use optimized if possible.")]
        public void AddForce(
            [CLParam("The force vector to apply.")]
            CustomLogicVector3Builtin force,
            [CLParam("The force mode: \"Force\", \"Acceleration\", \"Impulse\", or \"VelocityChange\" (default: \"Acceleration\").")]
            string forceMode = "Acceleration",
            [CLParam("Optional. If provided, applies force at this world position instead of center of mass.")]
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

        [CLMethod("Apply a force to the Rigidbody.")]
        public void AddForceOptimized(
            [CLParam("The force vector to apply.")]
            CustomLogicVector3Builtin force,
            [CLParam("The force mode as integer (use ForceModeAcceleration, ForceModeForce, ForceModeImpulse, or ForceModeVelocityChange constants, default: 5).")]
            int forceMode = 5,
            [CLParam("Optional. If provided, applies force at this world position instead of center of mass.")]
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

        [CLMethod("Apply a torque to the Rigidbody - legacy version, please use optimized if possible.")]
        public void AddTorque(
            [CLParam("The torque vector to apply.")]
            CustomLogicVector3Builtin torque,
            [CLParam("The force mode: \"Force\", \"Acceleration\", \"Impulse\", or \"VelocityChange\".")]
            string forceMode)
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

        [CLMethod("Apply a torque to the Rigidbody.")]
        public void AddTorqueOptimized(
            [CLParam("The torque vector to apply.")]
            CustomLogicVector3Builtin torque,
            [CLParam("The force mode as integer (use ForceModeAcceleration, ForceModeForce, ForceModeImpulse, or ForceModeVelocityChange constants, default: 5).")]
            int forceMode = 5)
        {
            ForceMode mode = (ForceMode)forceMode;
            Value.AddTorque(torque.Value, mode);
        }

        [CLMethod("Apply an explosion force to the Rigidbody.")]
        public void AddExplosionForce(
            [CLParam("The force of the explosion.")]
            float explosionForce,
            [CLParam("The center position of the explosion.")]
            CustomLogicVector3Builtin explosionPosition,
            [CLParam("The radius of the explosion.")]
            float explosionRadius,
            [CLParam("Adjustment to the apparent position of the explosion to make it seem to lift objects (default: 0.0).")]
            float upwardsModifier = 0.0f,
            [CLParam("The force mode as integer (use ForceModeAcceleration, ForceModeForce, ForceModeImpulse, or ForceModeVelocityChange constants, default: 5).")]
            int forceMode = 5)
        {
            ForceMode mode = (ForceMode)forceMode;
            Value.AddExplosionForce(explosionForce, explosionPosition.Value, explosionRadius, upwardsModifier, mode);
        }

        [CLMethod("Move the Rigidbody to a new position. This will not apply any forces, it will just set the position directly.")]
        public void Move(
            [CLParam("The new position.")]
            CustomLogicVector3Builtin position,
            [CLParam("The new rotation.")]
            CustomLogicQuaternionBuiltin rotation)
        {
            Value.Move(position, rotation);
        }

        [CLMethod("Move the Rigidbody to a new position. This will apply forces to move the Rigidbody to the new position.")]
        public void MovePosition(
            [CLParam("The target position.")]
            CustomLogicVector3Builtin position)
        {
            Value.MovePosition(position);
        }

        [CLMethod("Move the Rigidbody to a new rotation. This will apply forces to rotate the Rigidbody to the new rotation.")]
        public void MoveRotation(
            [CLParam("The target rotation.")]
            CustomLogicQuaternionBuiltin rotation)
        {
            Value.MoveRotation(rotation);
        }

        // ResetCenterOfMass
        [CLMethod("Reset the center of mass of the Rigidbody to the default value (0, 0, 0). This will also reset the inertia tensor.")]
        public void ResetCenterOfMass()
        {
            Value.ResetCenterOfMass();
        }

        // PublishTransform
        [CLMethod("Publish the current position and rotation of the Rigidbody to the MapObject. This will update the MapObject's transform to match the Rigidbody's transform.")]
        public void PublishTransform()
        {
            Value.PublishTransform();
        }

        [CLMethod("Checks if the rigidbody would collide with anything, returns a LineCastHitResult object.")]
        public object SweepTest(
            [CLParam("The direction to sweep in.")]
            CustomLogicVector3Builtin direction,
            [CLParam("The distance to sweep.")]
            float distance)
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
