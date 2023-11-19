using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Map
{
    class MapScriptSceneObject: MapScriptBaseObject
    {
        // physics
        [Order(19)] public string CollideMode = MapObjectCollideMode.Physical;
        [Order(20)] public string CollideWith = MapObjectCollideWith.Entities;
        [Order(21)] public string PhysicsMaterial = "Default";

        // material
        [Order(22)] public MapScriptBaseMaterial Material = new MapScriptBaseMaterial();
        [Order(23)] public List<MapScriptComponent> Components = new List<MapScriptComponent>();

        public MapScriptSceneObject()
        {
            Type = "Scene";
        }

        protected override object DeserializeValue(Type t, string value)
        {
            if (typeof(MapScriptBaseMaterial).IsAssignableFrom(t))
            {
                return DeserializeMaterial(value);
            }
            return base.DeserializeValue(t, value);
        }

        public static MapScriptBaseMaterial DeserializeMaterial(string value)
        {
            MapScriptBaseMaterial material = new MapScriptBaseMaterial();
            string shader = value.Split(material.Delimiter)[0];
            if (shader == MapObjectShader.Basic || shader == MapObjectShader.Transparent)
                material = new MapScriptBasicMaterial();
            else if (shader == MapObjectShader.Reflective)
                material = new MapScriptReflectiveMaterial();
            else if (MapObjectShader.IsLegacyShader(shader))
                material = new MapScriptLegacyMaterial();
            material.Deserialize(value);
            return material;
        }
    }
}
