using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utility;
using UnityEngine;

namespace Map
{
    public class MapScriptObjects: BaseCSVContainer
    {
        public List<MapScriptBaseObject> Objects = new List<MapScriptBaseObject>();

        protected override object DeserializeValue(Type t, string value)
        {
            BaseCSVObject item = new BaseCSVObject();
            string type = value.Split(item.Delimiter)[0];
            if (type == "Scene")
                item = new MapScriptSceneObject();
            item.Deserialize(value);
            return item;
        }
    }
}
