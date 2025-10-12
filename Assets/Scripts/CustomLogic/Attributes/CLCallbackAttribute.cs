using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLogic
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class CLCallbackAttribute : CLBaseAttribute
    {
        public CLCallbackAttribute()
        {
        }
    }
}
