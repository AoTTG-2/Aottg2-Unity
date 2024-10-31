using ApplicationManagers;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using Utility;

namespace Map
{
    class MapConverter
    {
        private static int _currentId;
        public static bool IsLegacy(string map)
        {
            foreach (string str in map.Split(';'))
            {
                if (str.Trim() == string.Empty)
                    continue;
                if (str.StartsWith("///"))
                    return false;
                else
                    return true;
            }
            return false;
        }

        private static MapScriptSceneObject CreateForestFloor()
        {
            MapScriptSceneObject sceneObject = new MapScriptSceneObject();
            sceneObject.Asset = "Geometry/Cuboid";
            sceneObject.SetScale(new Vector3(134.286f, 6.407f, 134.286f));
            sceneObject.SetPosition(new Vector3(-7.76f, -32.03f, 5.333f));
            sceneObject.Id = GetNextId();
            var material = new MapScriptBasicMaterial();
            material.Shader = "Basic";
            material.Texture = BuiltinMapTextures.AllTextures["Grass1"].Texture;
            material.Tiling = new Vector2(50f, 50f);
            material.Color = new Color255(new Color(0.678f, 0.684f, 0.654f, 1f));
            sceneObject.Material = material;
            return sceneObject;
        }

        private static MapScriptSceneObject CreateLight()
        {
            MapScriptSceneObject sceneObject = new MapScriptSceneObject();
            sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Daylight"]);
            sceneObject.SetRotation(Quaternion.Euler(50.3f, 121.9f, 1.4f));
            sceneObject.Id = GetNextId();
            return sceneObject;
        }

        private static List<MapScriptBaseObject> CreateFengBounds()
        {
            var objects = new List<MapScriptBaseObject>();
            Vector3 rotation = new Vector3(0f, 0f, 0f);
            objects.Add(CreateBound(new Vector3(-700f, 745.8f, 0.01f), new Vector3(10f, 160f, 160f), rotation));
            objects.Add(CreateBound(new Vector3(0f, 745.8f, -700f), new Vector3(160f, 160f, 10f), rotation));
            objects.Add(CreateBound(new Vector3(0f, 745.8f, 700f), new Vector3(160, 160f, 10f), rotation));
            objects.Add(CreateBound(new Vector3(700f, 745.8f, 0.01f), new Vector3(10f, 160f, 160f), rotation));
            objects.Add(CreateBound(new Vector3(-2.23f, 1253.07f, 17.889f), new Vector3(160f, 10f, 160f), rotation));
            return objects;
        }

        private static MapScriptSceneObject CreateBound(Vector3 center, Vector3 size, Vector3 rotation)
        {
            var sceneObject = new MapScriptSceneObject();
            sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["LegacyBarrier"]);
            sceneObject.SetPosition(center);
            sceneObject.SetScale(size);
            sceneObject.SetRotation(rotation);
            sceneObject.Id = GetNextId();
            return sceneObject;
        }

        public static MapScript Convert(string map)
        {
            _currentId = 0;
            MapScript mapScript = new MapScript();
            mapScript.Objects.Objects.Add(CreateForestFloor());
            mapScript.Objects.Objects.Add(CreateLight());
            bool disableBounds = false;
            string[] mapArray = Regex.Replace(map, @"\s+", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Split(new char[] { ';' });
            foreach (string str in mapArray)
            {
                string[] strArray = str.Trim().Split(',');
                if (strArray.Length == 0 || strArray[0].StartsWith("//") || str.Trim() == "")
                    continue;
                MapScriptSceneObject sceneObject = new MapScriptSceneObject();
                if (strArray[0].StartsWith("custom"))
                {
                    if (BuiltinMapPrefabs.AllPrefabsLower.ContainsKey(strArray[1].ToLower()))
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabsLower[strArray[1].ToLower()]);
                    sceneObject.SetPosition(new Vector3(float.Parse(strArray[12]), float.Parse(strArray[13]), 
                        float.Parse(strArray[14])));
                    sceneObject.SetRotation(new Quaternion(float.Parse(strArray[15]), float.Parse(strArray[16]), 
                        float.Parse(strArray[17]), float.Parse(strArray[18])));
                    sceneObject.SetScale(new Vector3(float.Parse(strArray[3]), float.Parse(strArray[4]), float.Parse(strArray[5])));
                    string texture = strArray[2].ToLower();
                    MapScriptBaseMaterial material = sceneObject.Material;
                    float alpha = (float)material.Color.A / 255f;
                    if (texture != "default")
                    {
                        if (texture.StartsWith("transparent"))
                        {
                            material = new MapScriptBasicMaterial
                            {
                                Shader = "Transparent"
                            };
                            float num;
                            if (float.TryParse(texture.Substring(11), out num))
                                alpha = num;
                        }
                        else if (texture == "empty")
                        {
                            material = new MapScriptBasicMaterial
                            {
                                Shader = "Basic"
                            };
                        }
                        else if (texture == "ice1")
                        {
                            material = new MapScriptReflectiveMaterial
                            {
                                Shader = "Reflective",
                                Color = new Color255(178, 227, 255)
                            };
                            var newMaterial = BuiltinMapTextures.AllTexturesLower[texture];
                            ((MapScriptBasicMaterial)material).Texture = newMaterial.Texture;
                            ((MapScriptBasicMaterial)material).Tiling = new Vector2(float.Parse(strArray[10]), float.Parse(strArray[11]));
                        }
                        else if (texture == "barriereditormat")
                        {
                            material = new MapScriptBasicMaterial
                            {
                                Shader = "Transparent",
                                Color = new Color255(new Color(0f, 0.917f, 1f, 0.325f))
                            };
                            alpha = 0.32f;
                        }
                        else if (texture == "regioneditormat")
                        {
                            material = new MapScriptBasicMaterial
                            {
                                Shader = "Transparent",
                                Color = new Color255(new Color(1f, 0f, 0f, 0.325f))
                            };
                            alpha = 0.32f;
                        }
                        else if (texture == "bombexplosiontex")
                        {
                            material = new MapScriptLegacyMaterial
                            {
                                Shader = MapObjectShader.OldBombExplode,
                                Color = new Color255(new Color(0.5f, 0.5f, 0.5f, 0.5f)),
                                Tiling = new Vector2(float.Parse(strArray[10]), float.Parse(strArray[11]))
                            };
                            alpha = 0.5f;
                        }
                        else if (texture == "cannonregionmat")
                        {
                            material = new MapScriptLegacyMaterial
                            {
                                Shader = MapObjectShader.CannonRegionMat,
                                Color = new Color255(new Color(0f, 1f, 0f, 1f))
                            };
                            alpha = 1f;
                        }
                        else if (texture == "bombtex")
                        {
                            material = new MapScriptLegacyMaterial
                            {
                                Shader = MapObjectShader.BombTexMat,
                                Color = new Color255(new Color(0.5f, 0.5f, 0.5f, 0.5f)),
                                Tiling = new Vector2(float.Parse(strArray[10]), float.Parse(strArray[11]))
                            };
                            alpha = 0.5f;
                        }
                        else if (texture == "cannonballtrail")
                        {
                            material = new MapScriptLegacyMaterial
                            {
                                Shader = MapObjectShader.Smoke1Mat,
                                Color = new Color255(new Color(0.5f, 0.5f, 0.5f, 0.5f)),
                                Tiling = new Vector2(float.Parse(strArray[10]), float.Parse(strArray[11]))
                            };
                            alpha = 0.5f;
                        }
                        else
                        {
                            material = new MapScriptBasicMaterial();
                            material.Shader = "Basic";
                            if (texture == "bark" || texture == "grass")
                                texture += "1";
                            if (BuiltinMapTextures.AllTexturesLower.ContainsKey(texture))
                            {
                                var newMaterial = BuiltinMapTextures.AllTexturesLower[texture];
                                ((MapScriptBasicMaterial)material).Texture = newMaterial.Texture;
                                ((MapScriptBasicMaterial)material).Tiling = new Vector2(float.Parse(strArray[10]), float.Parse(strArray[11]));
                            }
                            else
                                Debug.Log("Unhandled legacy texture: " + str);
                        }
                    }
                    string assetStr = strArray[1].ToLower();
                    if (strArray[6] != "0" && (texture != "default" || (!assetStr.StartsWith("start") && !assetStr.StartsWith("kill") && 
                        !assetStr.StartsWith("end") && !assetStr.StartsWith("checkpoint"))))
                    {
                        if (texture == "cannonregionmat")
                            material.Color = new Color255(new Color(0f, float.Parse(strArray[8]), 0f, alpha));
                        else
                            material.Color = new Color255(new Color(float.Parse(strArray[7]), 
                                float.Parse(strArray[8]), float.Parse(strArray[9]), alpha));
                    }
                    sceneObject.Material = material;
                }
                else if (strArray[0].StartsWith("spawnpoint"))
                {
                    if (strArray[1].ToLower() == "titan")
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Titan SpawnPoint"]);
                    else if (strArray[1].ToLower() == "player")
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Human SpawnPoint"]);
                    else if (strArray[1].ToLower() == "playerc")
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Human SpawnPoint (blue)"]);
                    else if (strArray[1].ToLower() == "playerm")
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Human SpawnPoint (red)"]);
                    sceneObject.SetPosition(new Vector3(float.Parse(strArray[2]), 
                        float.Parse(strArray[3]), float.Parse(strArray[4])));
                }
                else if (strArray[0].StartsWith("misc"))
                {
                    bool skip = false;
                    if (strArray[1] == "barrier")
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["LegacyBarrier"]);
                    else if (strArray[1] == "barrierEditor")
                    {
                        sceneObject.Asset = "Geometry/Cuboid";
                        sceneObject.Material = new MapScriptBasicMaterial
                        {
                            Shader = "Transparent",
                            Color = new Color255(new Color(0f, 0.917f, 1f, 0.32f))
                        };
                    }
                    else if (strArray[1] == "racingStart")
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Racing Start Barrier Cuboid"]);
                    else if (strArray[1] == "racingEnd")
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Racing Finish Region Cuboid"]);
                    else if (strArray[1] == "region")
                        continue;
                    else
                        skip = true;
                    if (!skip)
                    {
                        sceneObject.SetPosition(new Vector3(float.Parse(strArray[5]), float.Parse(strArray[6]), float.Parse(strArray[7])));
                        sceneObject.SetRotation(new Quaternion(float.Parse(strArray[8]), float.Parse(strArray[9]), float.Parse(strArray[10]), float.Parse(strArray[11])));
                        sceneObject.SetScale(new Vector3(float.Parse(strArray[2]), float.Parse(strArray[3]), float.Parse(strArray[4])));
                    }
                }
                else if (strArray[0].StartsWith("base"))
                {
                    if (strArray[1].ToLower() == "aot_supply")
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Supply1"]);
                    else if (strArray[1].ToLower() == "levelbottom")
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["LevelBottom"]);
                    if (strArray.Length < 15)
                    {
                        sceneObject.SetPosition(new Vector3(float.Parse(strArray[2]), float.Parse(strArray[3]), float.Parse(strArray[4])));
                        sceneObject.SetRotation(new Quaternion(float.Parse(strArray[5]), float.Parse(strArray[6]), float.Parse(strArray[7]), float.Parse(strArray[8])));
                    }
                    else
                    {
                        sceneObject.SetPosition(new Vector3(float.Parse(strArray[12]), float.Parse(strArray[13]), float.Parse(strArray[14])));
                        sceneObject.SetRotation(new Quaternion(float.Parse(strArray[15]), float.Parse(strArray[16]), float.Parse(strArray[17]), float.Parse(strArray[18])));
                        sceneObject.SetScale(new Vector3(float.Parse(strArray[3]), float.Parse(strArray[4]), float.Parse(strArray[5])));
                    }
                    if (strArray[1].ToLower() == "aot_supply")
                        sceneObject.SetPosition(sceneObject.GetPosition() + Quaternion.Euler(sceneObject.GetRotation()) * Vector3.back * 0.37f);
                }
                else if (strArray[0].StartsWith("photon"))
                {
                    if (strArray[1].ToLower() == "cannonground" || strArray[1].ToLower() == "cannonwall")
                    {
                        if (strArray[1].ToLower() == "cannonground")
                            sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Cannon2"]);
                        else
                            sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Cannon3"]);
                        if (strArray.Length < 15)
                        {
                            sceneObject.SetPosition(new Vector3(float.Parse(strArray[2]), float.Parse(strArray[3]), float.Parse(strArray[4])));
                            sceneObject.SetRotation(new Quaternion(float.Parse(strArray[5]), float.Parse(strArray[6]), float.Parse(strArray[7]), float.Parse(strArray[8])));
                        }
                        else
                        {
                            sceneObject.SetPosition(new Vector3(float.Parse(strArray[12]), float.Parse(strArray[13]), float.Parse(strArray[14])));
                            sceneObject.SetRotation(new Quaternion(float.Parse(strArray[15]), float.Parse(strArray[16]), float.Parse(strArray[17]), float.Parse(strArray[18])));
                            sceneObject.SetScale(new Vector3(float.Parse(strArray[3]), float.Parse(strArray[4]), float.Parse(strArray[5])));
                        }
                    }
                    else if (strArray[1].ToLower().StartsWith("spawn"))
                    {
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Titan SpawnPoint"]);
                        sceneObject.SetPosition(new Vector3(float.Parse(strArray[4]), float.Parse(strArray[5]), float.Parse(strArray[6])));
                        sceneObject.SetRotation(new Quaternion(float.Parse(strArray[7]), float.Parse(strArray[8]), float.Parse(strArray[9]), float.Parse(strArray[10])));
                    }
                }
                else if (strArray[0].StartsWith("racing"))
                {
                    string prefab = "";
                    if (strArray[1].StartsWith("start"))
                        prefab = "Racing Start Barrier " + strArray[1].Substring("start".Length);
                    else if (strArray[1].StartsWith("end"))
                        prefab = "Racing Finish Region " + strArray[1].Substring("end".Length);
                    else if (strArray[1].StartsWith("kill"))
                        prefab = "Kill Region " + strArray[1].Substring("kill".Length);
                    else if (strArray[1].StartsWith("checkpoint"))
                        prefab = "Racing Checkpoint Region " + strArray[1].Substring("checkpoint".Length);
                    if (prefab.Length > 0 && BuiltinMapPrefabs.AllPrefabsLower.ContainsKey(prefab.ToLower()))
                    {
                        sceneObject.Copy(BuiltinMapPrefabs.AllPrefabsLower[prefab.ToLower()]);
                        sceneObject.SetPosition(new Vector3(float.Parse(strArray[5]), float.Parse(strArray[6]), float.Parse(strArray[7])));
                        sceneObject.SetRotation(new Quaternion(float.Parse(strArray[8]), float.Parse(strArray[9]), float.Parse(strArray[10]), float.Parse(strArray[11])));
                        sceneObject.SetScale(new Vector3(float.Parse(strArray[2]), float.Parse(strArray[3]), float.Parse(strArray[4])));
                    }
                }
                else if (strArray[0].StartsWith("map"))
                {
                    if (strArray[1].StartsWith("disablebounds"))
                    {
                        disableBounds = true;
                        continue;
                    }
                }
                if (sceneObject.Asset == "None" && sceneObject.Name == "Unnamed")
                    Debug.Log("Unhandled legacy object: " + str);
                else
                {
                    sceneObject.Id = GetNextId();
                    mapScript.Objects.Objects.Add(sceneObject);
                }
            }
            if (!disableBounds)
                mapScript.Objects.Objects.AddRange(CreateFengBounds());
            return mapScript;
        }

        private static int GetNextId()
        {
            _currentId++;
            return _currentId;
        }
    }
}
