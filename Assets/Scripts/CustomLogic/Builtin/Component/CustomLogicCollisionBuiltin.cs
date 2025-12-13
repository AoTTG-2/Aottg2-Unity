using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Collision", Abstract = true, IsComponent = true)]
    partial class CustomLogicCollisionBuiltin : BuiltinClassInstance, ICustomLogicCopyable, ICustomLogicEquals
    {
        public Collision collision;

        public CustomLogicCollisionBuiltin() { }

        public CustomLogicCollisionBuiltin(object[] parameters)
        {
            collision = (Collision)parameters[0];
        }

        [CLProperty(Description = "The collider involved in the collision.")]
        public CustomLogicColliderBuiltin Collider
        {
            get
            {
                return new CustomLogicColliderBuiltin(new object[] { collision.collider });
            }
        }

        [CLProperty(Description = "The impulse response of the collision.")]
        public CustomLogicVector3Builtin Impulse => collision.impulse;

        [CLProperty(Description = "The relative velocity of the collision. (sum of velocities)")]
        public CustomLogicVector3Builtin RelativeVelocity => collision.relativeVelocity;

        [CLProperty(Description = "The number of contacts in the collision, iterate over this in conjunction with the GetContact Point, Norm, Impulse, and Separation.")]
        public int ContactCount => collision.contactCount;

        [CLMethod(Description = "The contact point of the collision.")]
        public CustomLogicVector3Builtin GetContactPoint(int index)
        {
            return new CustomLogicVector3Builtin(collision.GetContact(index).point);
        }

        [CLMethod(Description = "The contact norm of the collision.")]
        public CustomLogicVector3Builtin GetContactNorms(int index)
        {
            return new CustomLogicVector3Builtin(collision.GetContact(index).normal);
        }

        [CLMethod(Description = "The contact impulse of the collision.")]
        public CustomLogicVector3Builtin GetContactImpulses(int index)
        {
            return new CustomLogicVector3Builtin(collision.GetContact(index).impulse);
        }

        [CLMethod(Description = "The separation between colliders at the given contact point.")]
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
