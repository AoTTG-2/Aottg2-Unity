using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
    class ErenCustomSkinLoader : BaseCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "eren"; } }

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            string skinUrl = (string)data[0];
            BaseCustomSkinPart part = GetCustomSkinPart((int)ErenCustomSkinPartId.Body);
            if (!part.LoadCache(skinUrl))
                yield return StartCoroutine(part.LoadSkin(skinUrl));
        }

        protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
        {
            List<Renderer> renderers = new List<Renderer>();
            switch ((ErenCustomSkinPartId)partId)
            {
                case ErenCustomSkinPartId.Body:
                    AddAllRenderers(renderers, _owner);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge);
                default:
                    return null;
            }
        }
    }

    public enum ErenCustomSkinPartId
    {
        Body
    }
}
