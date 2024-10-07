using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using Settings;
using Utility;
using Characters;
using Photon.Pun;

namespace CustomSkins
{
    class HumanCustomSkinLoader : BaseCustomSkinLoader
    {
        protected override string RendererIdPrefix { get { return "human"; } }
        private int _horseViewId;
        private float _hookLTiling;
        private float _hookRTiling;

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            _horseViewId = (int)data[0];
            string[] skinUrls = ((string)data[1]).Split(',');
            foreach (int partId in GetCustomSkinPartIds(typeof(HumanCustomSkinPartId)))
            {
                if (partId == (int)HumanCustomSkinPartId.Horse && _horseViewId < 0)
                    continue;
                else if (partId == (int)HumanCustomSkinPartId.WeaponTrail && !_owner.GetComponent<Human>().IsMine())
                    continue;
                else if (partId == (int)HumanCustomSkinPartId.Gas && !SettingsManager.CustomSkinSettings.Human.GasEnabled.Value)
                    continue;
                else if (partId == (int)HumanCustomSkinPartId.HookLTiling)
                {
                    float.TryParse(skinUrls[partId], out _hookLTiling);
                    continue;
                }
                else if (partId == (int)HumanCustomSkinPartId.HookRTiling)
                {
                    float.TryParse(skinUrls[partId], out _hookRTiling);
                    continue;
                }
                else if (partId == (int)HumanCustomSkinPartId.HookL && !SettingsManager.CustomSkinSettings.Human.HookEnabled.Value)
                    continue;
                else if (partId == (int)HumanCustomSkinPartId.HookR && !SettingsManager.CustomSkinSettings.Human.HookEnabled.Value)
                    continue;
                BaseCustomSkinPart part = GetCustomSkinPart(partId);
                if (skinUrls.Length > partId && !part.LoadCache(skinUrls[partId]))
                    yield return StartCoroutine(part.LoadSkin(skinUrls[partId]));
            }
        }

        protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
        {
            Human human = _owner.GetComponent<Human>();
            List<Renderer> renderers = new List<Renderer>();
            switch ((HumanCustomSkinPartId)partId)
            {
                case HumanCustomSkinPartId.Horse:
                    AddRenderersMatchingName(renderers, PhotonView.Find(_horseViewId).gameObject, "HORSE");
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.Hair:
                    AddRendererIfExists(renderers, human.Setup._part_hair);
                    AddRendererIfExists(renderers, human.Setup._part_hair_1);
                    string texture = human.Setup._meshes.GetHairMesh() == string.Empty ? string.Empty : human.Setup._textures.GetHairTexture();
                    return new HumanHairCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium, texture);
                case HumanCustomSkinPartId.Eye:
                    AddRendererIfExists(renderers, human.Setup._part_eye);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, new Vector2(8f, 8f));
                case HumanCustomSkinPartId.Glass:
                    AddRendererIfExists(renderers, human.Setup._part_glass);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, new Vector2(8f, 8f));
                case HumanCustomSkinPartId.Face:
                    AddRendererIfExists(renderers, human.Setup._part_face);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, new Vector2(8f, 8f));
                case HumanCustomSkinPartId.Skin:
                    AddRendererIfExists(renderers, human.Setup._part_hand_l);
                    AddRendererIfExists(renderers, human.Setup._part_hand_r);
                    AddRendererIfExists(renderers, human.Setup._part_head);
                    AddRendererIfExists(renderers, human.Setup._part_chest);
                    return new HumanCostumeCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.Costume:
                    AddRendererIfExists(renderers, human.Setup._part_arm_l);
                    AddRendererIfExists(renderers, human.Setup._part_arm_r);
                    AddRendererIfExists(renderers, human.Setup._part_leg);
                    AddRendererIfExists(renderers, human.Setup._part_chest_2);
                    AddRendererIfExists(renderers, human.Setup._part_chest_3);
                    AddRendererIfExists(renderers, human.Setup._part_upper_body);
                    // disabling costume renderers causes animation glitches, so we use transparent material instead
                    return new HumanCostumeCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge, useTransparentMaterial: true);
                case HumanCustomSkinPartId.Logo:
                    AddRendererIfExists(renderers, human.Setup._part_cape);
                    AddRendererIfExists(renderers, human.Setup._part_brand_1);
                    AddRendererIfExists(renderers, human.Setup._part_brand_2);
                    AddRendererIfExists(renderers, human.Setup._part_brand_3);
                    AddRendererIfExists(renderers, human.Setup._part_brand_4);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case HumanCustomSkinPartId.GearL:
                    AddRendererIfExists(renderers, human.Setup._part_3dmg);
                    AddRendererIfExists(renderers, human.Setup._part_belt);
                    AddRendererIfExists(renderers, human.Setup._part_gas_l);
                    AddRendererIfExists(renderers, human.Setup._part_blade_l);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.GearR:
                    AddRendererIfExists(renderers, human.Setup._part_gas_r);
                    AddRendererIfExists(renderers, human.Setup._part_blade_r);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.Gas:
                    AddRendererIfExists(renderers, human.transform.Find("3dmg_smoke").gameObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case HumanCustomSkinPartId.Hoodie:
                    if (human.Setup._part_chest_1 != null && human.Setup._part_chest_1.name.Contains("character_cap"))
                        AddRendererIfExists(renderers, human.Setup._part_chest_1);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case HumanCustomSkinPartId.WeaponTrail:
                    List<MeleeWeaponTrail> trails = new List<MeleeWeaponTrail>();
                    trails.Add(human.Setup.LeftTrail);
                    trails.Add(human.Setup.RightTrail);
                    return new WeaponTrailCustomSkinPart(this, trails, GetRendererId(partId), MaxSizeSmall);
                case HumanCustomSkinPartId.ThunderspearL:
                    if (human.Setup._part_blade_l != null)
                        AddRendererIfExists(renderers, human.Setup._part_blade_l);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.ThunderspearR:
                    if (human.Setup._part_blade_r != null)
                        AddRendererIfExists(renderers, human.Setup._part_blade_r);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.HookL:
                    return new HookCustomSkinPart(this, human.HookLeft.GetRenderers(), GetRendererId(partId), MaxSizeSmall, _hookLTiling);
                case HumanCustomSkinPartId.HookR:
                    return new HookCustomSkinPart(this, human.HookRight.GetRenderers(), GetRendererId(partId), MaxSizeSmall, _hookRTiling);
                default:
                    return null;
            }
        }
    }

    public enum HumanCustomSkinPartId
    {
        Horse,
        Hair,
        Eye,
        Glass,
        Face,
        Skin,
        Costume,
        Logo,
        GearL,
        GearR,
        Gas,
        Hoodie,
        WeaponTrail,
        ThunderspearL,
        ThunderspearR,
        HookLTiling,
        HookL,
        HookRTiling,
        HookR
    }
}
