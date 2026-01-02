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

        [CLProperty("The collider involved in the collision.")]
        public CustomLogicColliderBuiltin Collider
        {
            get
            {
                return new CustomLogicColliderBuiltin(new object[] { collision.collider });
            }
        }

        [CLProperty("The impulse response of the collision.")]
        public CustomLogicVector3Builtin Impulse => collision.impulse;

        [CLProperty("The relative velocity of the collision. (sum of velocities)")]
        public CustomLogicVector3Builtin RelativeVelocity => collision.relativeVelocity;

        [CLProperty("The number of contacts in the collision, iterate over this in conjunction with the GetContact Point, Norm, Impulse, and Separation.")]
        public int ContactCount => collision.contactCount;

        [CLMethod("The contact point of the collision.")]
        public CustomLogicVector3Builtin GetContactPoint(
            [CLParam("The contact index (0 to ContactCount-1).")]
            int index)
        {
            return new CustomLogicVector3Builtin(collision.GetContact(index).point);
        }

        [CLMethod("The contact norm of the collision.")]
        public CustomLogicVector3Builtin GetContactNorms(
            [CLParam("The contact index (0 to ContactCount-1).")]
            int index)
        {
            return new CustomLogicVector3Builtin(collision.GetContact(index).normal);
        }

        [CLMethod("The contact impulse of the collision.")]
        public CustomLogicVector3Builtin GetContactImpulses(
            [CLParam("The contact index (0 to ContactCount-1).")]
            int index)
        {
            return new CustomLogicVector3Builtin(collision.GetContact(index).impulse);
        }

        [CLMethod("The separation between colliders at the given contact point.")]
        public CustomLogicVector3Builtin GetContactSeparations(
            [CLParam("The contact index (0 to ContactCount-1).")]
            int index)
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
