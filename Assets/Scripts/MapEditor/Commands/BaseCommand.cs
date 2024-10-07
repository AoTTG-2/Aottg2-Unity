using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using ApplicationManagers;
using Map;
using GameManagers;

namespace MapEditor
{
    class BaseCommand
    {
        public virtual void Execute()
        {
        }

        public virtual void Unexecute()
        {
        }
    }
}
