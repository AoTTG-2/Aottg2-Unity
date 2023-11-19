using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
    class ColossalCustomSkinLoader : BaseCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "colossal"; } }

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            string skinUrl = (string)data[0];
            BaseCustomSkinPart part = GetCustomSkinPart((int)ColossalCustomSkinPartId.Body);
            if (!part.LoadCache(skinUrl))
                yield return StartCoroutine(part.LoadSkin(skinUrl));
        }

        protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
        {
            List<Renderer> renderers = new List<Renderer>();
            switch ((ColossalCustomSkinPartId)partId)
            {
                case ColossalCustomSkinPartId.Body:
                    AddRenderersContainingName(renderers, _owner, "hair");
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge);
                default:
                    return null;
            }
        }
    }

    public enum ColossalCustomSkinPartId
    {
        Body
    }
}
