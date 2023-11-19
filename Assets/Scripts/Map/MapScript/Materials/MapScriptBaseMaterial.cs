using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Map
{
    class MapScriptBaseMaterial: BaseCSVRowItem
    {
        [Order(1)] public string Shader = "Default";
        [Order(2)] public Color255 Color = new Color255();
    }
}
