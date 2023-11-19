using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomSkins
{
    abstract class LevelCustomSkinLoader : BaseCustomSkinLoader
    {
        protected virtual BaseCustomSkinPart GetCustomSkinPart(int partId, int randomIndex)
        {
            throw new NotImplementedException();
        }

        protected virtual void FindAndIndexLevelObjects()
        {
            throw new NotImplementedException();
        }
    }
}
