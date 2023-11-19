using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Utility;

namespace Map
{
    class MapScriptReflectiveMaterial: MapScriptBasicMaterial
    {
        [Order(6)] public Color255 ReflectColor = new Color255();
    }
}
