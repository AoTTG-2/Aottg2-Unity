//using CustomLogic;
//using Map;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace CustomLogic
//{
//    class CustomLogicRigidbodyComponent : CustomLogicComponentInstance
//    {
//        private Rigidbody _rigidbody;

//        public CustomLogicRigidbodyComponent(string name, MapObject obj, MapScriptComponent script, CustomLogicNetworkViewBuiltin networkView) : base(name, obj, script, networkView)
//        {
//        }

//        /// <inheritdoc cref="Rigidbody.position"/>
//        [CLProperty]
//        public CustomLogicVector3Builtin position
//        {
//            get => _rigidbody.position; set => _rigidbody.position = value;
//        }

//        /// <inheritdoc cref="Rigidbody.rotation"/>
//        [CLProperty]
//        public CustomLogicQuaternionBuiltin rotation
//        {
//            get => _rigidbody.rotation; set => _rigidbody.rotation = value;
//        }

//        /// <inheritdoc cref="Rigidbody.velocity"/>
//        [CLProperty]
//        public CustomLogicVector3Builtin velocity
//        {
//            get => _rigidbody.velocity; set => _rigidbody.velocity = value;
//        }

//        /// <inheritdoc cref="Rigidbody.angularVelocity"/>
//        [CLProperty]
//        public CustomLogicVector3Builtin angularVelocity
//        {
//            get => _rigidbody.angularVelocity; set => _rigidbody.angularVelocity = value;
//        }

//        /// <inheritdoc cref="Rigidbody.drag"/>
//        [CLProperty]
//        public float drag
//        {
//            get => _rigidbody.drag; set => _rigidbody.drag = value;
//        }

//        /// <inheritdoc cref="Rigidbody.angularDrag"/>
//        [CLProperty]
//        public float angularDrag
//        {
//            get => _rigidbody.angularDrag; set => _rigidbody.angularDrag = value;
//        }

//        /// <inheritdoc cref="Rigidbody.mass"/>
//        [CLProperty]
//        public float mass
//        {
//            get => _rigidbody.mass; set => _rigidbody.mass = value;
//        }

//        /// <inheritdoc cref="Rigidbody.useGravity"/>
//        [CLProperty]
//        public bool useGravity
//        {
//            get => _rigidbody.useGravity; set => _rigidbody.useGravity = value;
//        }

//        /// <inheritdoc cref="Rigidbody.isKinematic"/>
//        [CLProperty]
//        public bool isKinematic
//        {
//            get => _rigidbody.isKinematic; set => _rigidbody.isKinematic = value;
//        }

//        /// <inheritdoc cref="Rigidbody.interpolation"/>
//        [CLProperty]
//        public RigidbodyInterpolation interpolation
//        {
//            get => _rigidbody.interpolation; set => _rigidbody.interpolation = value;
//        }

//        /// <inheritdoc cref="Rigidbody.collisionDetectionMode"/>
//        [CLProperty]
//        public CollisionDetectionMode collisionDetectionMode
//        {
//            get => _rigidbody.collisionDetectionMode; set => _rigidbody.collisionDetectionMode = value;
//        }

//        /// <inheritdoc cref="Rigidbody.constraints"/>
//        [CLProperty]
//        public RigidbodyConstraints constraints
//        {
//            get => _rigidbody.constraints; set => _rigidbody.constraints = value;
//        }

//        /// <inheritdoc cref="Rigidbody.AddForce(Vector3, ForceMode)"/>
//        [CLMethod]
//        public void AddForce(CustomLogicVector3Builtin force, ForceMode mode)
//        {
//            _rigidbody.AddForce(force, mode);
//        }

//        /// <inheritdoc cref="Rigidbody.AddRelativeForce(Vector3, ForceMode)"/>
//        [CLMethod]
//        public void AddRelativeForce(CustomLogicVector3Builtin force, ForceMode mode)
//        {
//            _rigidbody.AddRelativeForce(force, mode);
//        }

//        /// <inheritdoc cref="Rigidbody.AddTorque(Vector3, ForceMode)"/>
//        [CLMethod]
//        public void AddTorque(CustomLogicVector3Builtin torque, ForceMode mode)
//        {
//            _rigidbody.AddTorque(torque, mode);
//        }

//        /// <inheritdoc cref="Rigidbody.AddRelativeTorque(Vector3, ForceMode)"/>
//        [CLMethod]
//        public void AddRelativeTorque(CustomLogicVector3Builtin torque, ForceMode mode)
//        {
//            _rigidbody.AddRelativeTorque(torque, mode);
//        }

//        /// <inheritdoc cref="Rigidbody.AddForceAtPosition(Vector3, Vector3, ForceMode)"/>
//        [CLMethod]
//        public void AddForceAtPosition(CustomLogicVector3Builtin force, CustomLogicVector3Builtin position, ForceMode mode)
//        {
//            _rigidbody.AddForceAtPosition(force, position, mode);
//        }

//        /// <inheritdoc cref="Rigidbody.AddExplosionForce(float, Vector3, float, float, ForceMode)"/>
//        [CLMethod]
//        public void AddExplosionForce(float explosionForce, CustomLogicVector3Builtin explosionPosition, float explosionRadius, float upwardsModifier, ForceMode mode)
//        {
//            _rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, mode);
//        }

//        /// <inheritdoc cref="Rigidbody.ClosestPointOnBounds(Vector3)"/>
//        [CLMethod]
//        public CustomLogicVector3Builtin ClosestPointOnBounds(CustomLogicVector3Builtin position)
//        {
//            return _rigidbody.ClosestPointOnBounds(position);
//        }

//        /// <inheritdoc cref="Rigidbody.GetRelativePointVelocity(Vector3)"/>
//        [CLMethod]
//        public CustomLogicVector3Builtin GetRelativePointVelocity(CustomLogicVector3Builtin relativePoint)
//        {
//            return _rigidbody.GetRelativePointVelocity(relativePoint);
//        }

//        /// <inheritdoc cref="Rigidbody.GetPointVelocity(Vector3)"/>
//        [CLMethod]
//        public void MovePosition(CustomLogicVector3Builtin position)
//        {
//            _rigidbody.MovePosition(position);
//        }

//        /// <inheritdoc cref="Rigidbody.MoveRotation(Quaternion)"/>
//        [CLMethod]
//        public void MoveRotation(CustomLogicQuaternionBuiltin rotation)
//        {
//            _rigidbody.MoveRotation(rotation);
//        }

//        /// <inheritdoc cref="Rigidbody.Sleep()"/>
//        [CLMethod]
//        public void Sleep()
//        {
//            _rigidbody.Sleep();
//        }

//        /// <inheritdoc cref="Rigidbody.WakeUp()"/>
//        [CLMethod]
//        public void WakeUp()
//        {
//            _rigidbody.WakeUp();
//        }

//        /// <inheritdoc cref="Rigidbody.ResetCenterOfMass()"/>
//        [CLMethod]
//        public void ResetCenterOfMass()
//        {
//            _rigidbody.ResetCenterOfMass();
//        }



//    }
//}
