using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLogic
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Method)]
    internal class CLCallbackAttribute : CLBaseAttribute
    {
        public CLCallbackAttribute()
        {
        }
    }
}
