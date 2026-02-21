using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Utility;

namespace Map
{
    public class MapScriptDefaultTiledMaterial: MapScriptBaseMaterial
    {
        [Order(3)] public Vector2 Tiling = Vector2.one;
    }
}
