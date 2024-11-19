using Builtin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLogic
{
    [CLType]
    class CustomLogicVector2Child : CustomLogicVector2Builtin
    {
        public CustomLogicVector2Child(List<object> param) : base(param, "Vector2Child")
        {
        }

        private float _xy;
        
        [CLProperty]
        public float XY
        {
            get => _xy;
            set => _xy = value;
        }

        // public override object __Copy__()
        // {
        //     return new CustomLogicVector2Child(new List<object> { _xy, _xy });
        // }
    }
    
    [CLType]
    class CustomLogicVector2ChildChild : CustomLogicVector2Builtin
    {
        public CustomLogicVector2ChildChild(List<object> param) : base(param, "Vector2ChildChild")
        {
        }

        private float _xz;
        
        [CLProperty]
        public float XZ
        {
            get => _xz;
            set => _xz = value;
        }
    }
}
