using UnityEngine;

namespace Controllers
{
    interface IHumanController
    {
        public bool MovingLeft();
        public bool MovingRight();

        public bool UsingGas();

        public bool HookingLeft();

        public bool HookingRight();

        public bool HookingBoth();

        public Vector3 GetAimPoint()
        {
            return Vector3.zero;
        }
    }
}