using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents a collision event.
    /// </summary>
    [CLType(Name = "Collision", Abstract = true, IsComponent = true)]
    partial class CustomLogicCollisionBuiltin : BuiltinClassInstance, ICustomLogicCopyable, ICustomLogicEquals
    {
        public Collision collision;

        public CustomLogicCollisionBuiltin() { }

        public CustomLogicCollisionBuiltin(object[] parameters)
        {
            collision = (Collision)parameters[0];
        }

        /// <summary>
        /// The collider involved in the collision.
        /// </summary>
        [CLProperty]
        public CustomLogicColliderBuiltin Collider
        {
            get
            {
                return new CustomLogicColliderBuiltin(new object[] { collision.collider });
            }
        }

        /// <summary>
        /// The impulse response of the collision.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Impulse => collision.impulse;

        /// <summary>
        /// The relative velocity of the collision. (sum of velocities)
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin RelativeVelocity => collision.relativeVelocity;

        /// <summary>
        /// The number of contacts in the collision, iterate over this in conjunction with the GetContact Point, Norm, Impulse, and Separation.
        /// </summary>
        [CLProperty]
        public int ContactCount => collision.contactCount;

        /// <summary>
        /// The contact point of the collision.
        /// </summary>
        /// <param name="index">The contact index (0 to ContactCount-1).</param>
        [CLMethod]
        public CustomLogicVector3Builtin GetContactPoint(int index)
        {
            return new CustomLogicVector3Builtin(collision.GetContact(index).point);
        }

        /// <summary>
        /// The contact norm of the collision.
        /// </summary>
        /// <param name="index">The contact index (0 to ContactCount-1).</param>
        [CLMethod]
        public CustomLogicVector3Builtin GetContactNorms(int index)
        {
            return new CustomLogicVector3Builtin(collision.GetContact(index).normal);
        }

        /// <summary>
        /// The contact impulse of the collision.
        /// </summary>
        /// <param name="index">The contact index (0 to ContactCount-1).</param>
        [CLMethod]
        public CustomLogicVector3Builtin GetContactImpulses(int index)
        {
            return new CustomLogicVector3Builtin(collision.GetContact(index).impulse);
        }

        /// <summary>
        /// The separation between colliders at the given contact point.
        /// </summary>
        /// <param name="index">The contact index (0 to ContactCount-1).</param>
        [CLMethod]
        public CustomLogicVector3Builtin GetContactSeparations(int index)
        {
            return new CustomLogicVector3Builtin(collision.GetContact(index).separation);
        }

        public object __Copy__()
        {
            return new CustomLogicColliderBuiltin(new object[] { collision });
        }

        public bool __Eq__(object self, object other)
        {
            return ReferenceEquals(self, other);
        }

        public int __Hash__()
        {
            return collision.GetHashCode();
        }
    }
}
