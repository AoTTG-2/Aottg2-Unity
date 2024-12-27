using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Abstract = true, InheritBaseMembers = true)]
    class CustomLogicCollisionBuiltin : CustomLogicClassInstanceBuiltin, ICustomLogicCopyable, ICustomLogicEquals
    {
        public Collision collision;

        public CustomLogicCollisionBuiltin() : base("Collision") { }

        public CustomLogicCollisionBuiltin(object[] parameters) : base("Collision")
        {
            collision = (Collision)parameters[0];
        }

        [CLProperty(Description = "")]
        public CustomLogicColliderBuiltin Collider
        {
            get
            {
                return new CustomLogicColliderBuiltin(new object[] { collision.collider });
            }
        }

        [CLProperty(Description = "")]
        public CustomLogicVector3Builtin Impulse => collision.impulse;

        [CLProperty(Description = "")]
        public CustomLogicVector3Builtin RelativeVelocity => collision.relativeVelocity;

        [CLMethod(Description = "")]
        public void GetContact(int index)
        {
            throw new NotImplementedException();
        }

        [CLMethod(Description = "")]
        public void GetContacts()
        {
            throw new NotImplementedException();
        }

        public object __Copy__()
        {
            throw new NotImplementedException();
        }

        public bool __Eq__(object other)
        {
            throw new NotImplementedException();
        }

        public int __Hash__()
        {
            throw new NotImplementedException();
        }
    }
}
