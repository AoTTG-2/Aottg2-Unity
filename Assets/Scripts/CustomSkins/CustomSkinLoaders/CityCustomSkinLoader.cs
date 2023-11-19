using Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace CustomSkins
{
    class CityCustomSkinLoader : LevelCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "city"; } }
        private List<GameObject> _houseObjects = new List<GameObject>();
        private List<GameObject> _groundObjects = new List<GameObject>();
        private List<GameObject> _wallObjects = new List<GameObject>();
        private List<GameObject> _gateObjects = new List<GameObject>();

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            FindAndIndexLevelObjects();
            char[] randomIndices = ((string)data[0]).ToCharArray();
            string[] houseUrls = ((string)data[1]).Split(',');
            string[] miscUrls = ((string)data[2]).Split(',');
            for (int i = 0; i < _houseObjects.Count; i++)
            {
                int randomIndex = int.Parse(randomIndices[i].ToString());
                BaseCustomSkinPart part = GetCustomSkinPart((int)CityCustomSkinPartId.House, _houseObjects[i]);
                if (!part.LoadCache(houseUrls[randomIndex]))
                    yield return StartCoroutine(part.LoadSkin(houseUrls[randomIndex]));
            }
            foreach (GameObject groundObject in _groundObjects)
            {
                BaseCustomSkinPart part = GetCustomSkinPart((int)CityCustomSkinPartId.Ground, groundObject);
                if (!part.LoadCache(miscUrls[0]))
                    yield return StartCoroutine(part.LoadSkin(miscUrls[0]));
            }
            foreach (GameObject wallObject in _wallObjects)
            {
                BaseCustomSkinPart part = GetCustomSkinPart((int)CityCustomSkinPartId.Wall, wallObject);
                if (!part.LoadCache(miscUrls[1]))
                    yield return StartCoroutine(part.LoadSkin(miscUrls[1]));

            }
            foreach (GameObject gateObject in _gateObjects)
            {
                BaseCustomSkinPart part = GetCustomSkinPart((int)CityCustomSkinPartId.Gate, gateObject);
                if (!part.LoadCache(miscUrls[2]))
                    yield return StartCoroutine(part.LoadSkin(miscUrls[2]));
            }
        }

        protected BaseCustomSkinPart GetCustomSkinPart(int partId, GameObject levelObject)
        {
            List<Renderer> renderers = new List<Renderer>();
            switch ((CityCustomSkinPartId)partId)
            {
                case CityCustomSkinPartId.House:
                    AddAllRenderers(renderers, levelObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge);
                case CityCustomSkinPartId.Ground:
                    AddAllRenderers(renderers, levelObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case CityCustomSkinPartId.Wall:
                    AddAllRenderers(renderers, levelObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case CityCustomSkinPartId.Gate:
                    AddAllRenderers(renderers, levelObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge);
                default:
                    return null;
            }
        }

        protected override void FindAndIndexLevelObjects()
        {
            _houseObjects.Clear();
            _groundObjects.Clear();
            _wallObjects.Clear();
            _gateObjects.Clear();
            foreach (var mapObject in MapLoader.IdToMapObject.Values)
            {
                var name = mapObject.ScriptObject.Name;
                if (name == "House1" || name == "Arch1" || name == "Tower1")
                    _houseObjects.Add(mapObject.GameObject);
                else if (name == "Ground")
                    _groundObjects.Add(mapObject.GameObject);
                else if (name == "Cuboid")
                    _wallObjects.Add(mapObject.GameObject);
                else if (name == "Gate")
                    _gateObjects.Add(mapObject.GameObject);
            }
        }
    }

    public enum CityCustomSkinPartId
    {
        House,
        Ground,
        Wall,
        Gate
    }
}
