using ApplicationManagers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace CustomSkins
{
    class SkyboxCustomSkinLoader : BaseCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "skybox"; } }
        public static Material SkyboxMaterial;

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            SkyboxMaterial = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Weather, "Skyboxes/EmptySkybox", true);
            foreach (int partId in GetCustomSkinPartIds(typeof(SkyboxCustomSkinPartId)))
            {
                BaseCustomSkinPart part = GetCustomSkinPart(partId);
                string url = (string)data[partId];
                if (!part.LoadCache(url))
                    yield return StartCoroutine(part.LoadSkin(url));
            }
        }

        protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
        {
            return new SkyboxCustomSkinPart(this, SkyboxMaterial, PartIdToTextureName((SkyboxCustomSkinPartId)partId), GetRendererId(partId), MaxSizeLarge);
        }

        public string PartIdToTextureName(SkyboxCustomSkinPartId partId)
        {
            return "_" + partId.ToString() + "Tex";
        }
    }

    public enum SkyboxCustomSkinPartId
    {
        Front,
        Back,
        Left,
        Right,
        Up,
        Down
    }
}
