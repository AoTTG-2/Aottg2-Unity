using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Utility;

namespace Map
{
    class MapScriptBasicMaterial: MapScriptBaseMaterial
    {
        [Order(3)] public string Texture = "Misc/None";
        [Order(4)] public Vector2 Tiling = Vector2.one;
        [Order(5)] public Vector2 Offset = Vector3.zero;
    }
}
