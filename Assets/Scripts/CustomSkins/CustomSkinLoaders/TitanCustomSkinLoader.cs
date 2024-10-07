using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSkins
{
    class TitanCustomSkinLoader : BaseCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "titan"; } }

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            bool isHair = (bool)data[0];
            if (isHair)
            {
                string hairUrl = (string)data[1];
                BaseCustomSkinPart part = GetCustomSkinPart((int)TitanCustomSkinPartId.Hair);
                if (!part.LoadCache(hairUrl))
                    yield return StartCoroutine(part.LoadSkin(hairUrl));
            }
            else
            {
                string bodyUrl = (string)data[1];
                string eyeUrl = (string)data[2];
                BaseCustomSkinPart bodyPart = GetCustomSkinPart((int)TitanCustomSkinPartId.Body);
                if (!bodyPart.LoadCache(bodyUrl))
                    yield return StartCoroutine(bodyPart.LoadSkin(bodyUrl));
                BaseCustomSkinPart eyePart = GetCustomSkinPart((int)TitanCustomSkinPartId.Eye);
                if (!eyePart.LoadCache(eyeUrl))
                    yield return StartCoroutine(eyePart.LoadSkin(eyeUrl));
            }
        }

        protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
        {
            BasicTitan titan = _owner.GetComponent<BasicTitan>();
            List<Renderer> renderers = new List<Renderer>();
            switch ((TitanCustomSkinPartId)partId)
            {
                case TitanCustomSkinPartId.Hair:
                    //AddRendererIfExists(renderers, titan.GetComponent<TITAN_SETUP>().part_hair);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case TitanCustomSkinPartId.Body:
                    AddRenderersMatchingName(renderers, _owner, "hair");
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case TitanCustomSkinPartId.Eye:
                    AddRenderersContainingName(renderers, _owner, "eye");
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, new Vector2(4f, 8f));
                default:
                    return null;
            }
        }
    }

    public enum TitanCustomSkinPartId
    {
        Hair,
        Body,
        Eye
    }
}
