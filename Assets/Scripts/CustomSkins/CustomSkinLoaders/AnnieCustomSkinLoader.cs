using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
    class AnnieCustomSkinLoader : BaseCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "annie"; } }

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            string skinUrl = (string)data[0];
            BaseCustomSkinPart part = GetCustomSkinPart((int)AnnieCustomSkinPartId.Body);
            if (!part.LoadCache(skinUrl))
                yield return StartCoroutine(part.LoadSkin(skinUrl));
        }

        protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
        {
            List<Renderer> renderers = new List<Renderer>();
            switch ((AnnieCustomSkinPartId)partId)
            {
                case AnnieCustomSkinPartId.Body:
                    AddAllRenderers(renderers, _owner);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge);
                default:
                    return null;
            }
        }
    }

    public enum AnnieCustomSkinPartId
    {
        Body
    }
}
