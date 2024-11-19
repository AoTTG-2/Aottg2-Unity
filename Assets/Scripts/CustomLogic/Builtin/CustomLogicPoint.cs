using Builtin;
using UnityEngine;

namespace CustomLogic
{
    [CLType]
    class CustomLogicPoint : CustomLogicClassInstanceBuiltin
    {
        private float _x;
        private float _y;
        
        [CLProperty(ReadOnly = true)]
        public float Radius = 3.14f;
        
        public CustomLogicPoint() : base("Point")
        {
        }
        
        [CLProperty]
        public float X
        {
            get => _x;
            set => _x = value;
        }
        
        [CLProperty]
        public float Y
        {
            get => _y;
            set => _y = value;
        }
        
        [CLProperty]
        public CustomLogicVector2Builtin XY => new(new Vector2(_x, _y));
    }
}