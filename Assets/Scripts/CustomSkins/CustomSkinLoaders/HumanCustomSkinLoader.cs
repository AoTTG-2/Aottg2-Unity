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
        public bool Finished;

        public override IEnumerator LoadSkinsFromRPC(object[] data)
        {
            _horseViewId = (int)data[0];
            string[] skinUrls = ((string)data[1]).Split(',');
            for (int partId = 0; partId < 22; partId++)
            {
                if (partId == (int)HumanCustomSkinPartId.Horse && _horseViewId < 0)
                    continue;
                else if (partId == (int)HumanCustomSkinPartId.WeaponTrail)
                {
                    Human humanComponent = _owner.GetComponent<Human>();
                    if (humanComponent != null && humanComponent.enabled && !humanComponent.IsMine())
                        continue;
                }
                else if (partId == (int)HumanCustomSkinPartId.Gas && !SettingsManager.CustomSkinSettings.Human.GasEnabled.Value)
                    continue;
                else if ((partId == (int)HumanCustomSkinPartId.HookL || partId == (int)HumanCustomSkinPartId.HookR) && !SettingsManager.CustomSkinSettings.Human.HookEnabled.Value)
                    continue;
                else if (partId == (int)HumanCustomSkinPartId.HookLTiling || partId == (int)HumanCustomSkinPartId.HookRTiling)
                    continue;
                var part = GetCustomSkinPart(partId);
                if (part != null)
                {
                    if (string.IsNullOrEmpty(skinUrls[partId]))
                    {
                        part.ResetToDefault();
                    }
                    else 
                    {
                        bool cacheHit = part.LoadCache(skinUrls[partId]);
                        if (!cacheHit)
                        {
                            yield return StartCoroutine(part.LoadSkin(skinUrls[partId]));
                        }
                    }
                }
            }
            Finished = true;
        }

        public BaseCustomSkinPart GetCustomSkinPartPublic(int partId)
        {
            return GetCustomSkinPart(partId);
        }

        protected override BaseCustomSkinPart GetCustomSkinPart(int partId)
        {
            HumanSetup setup = null;
            DummyHuman dummyHuman = _owner.GetComponent<DummyHuman>();
            if (dummyHuman != null)
            {
                setup = dummyHuman.Setup;
            }
            else
            {
                Human human = _owner.GetComponent<Human>();
                if (human != null && human.Setup != null)
                {
                    setup = human.Setup;
                }
                else
                {
                    setup = _owner.GetComponent<HumanSetup>();
                }
            }
            
            if (setup == null)
            {
                UnityEngine.Debug.LogError("Could not find HumanSetup component on " + _owner.name);
                return null;
            }
            
            List<Renderer> renderers = new List<Renderer>();
            switch ((HumanCustomSkinPartId)partId)
            {
                case HumanCustomSkinPartId.Horse:
                    if (_horseViewId >= 0)
                    {
                        PhotonView horseView = PhotonView.Find(_horseViewId);
                        if (horseView != null)
                            AddRenderersMatchingName(renderers, horseView.gameObject, "Body");
                    }
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.Hair:
                    AddRendererIfExists(renderers, setup._part_hair);
                    AddRendererIfExists(renderers, setup._part_hair_1);
                    string texture = setup._meshes.GetHairMesh() == string.Empty ? string.Empty : setup._textures.GetHairTexture();
                    return new HumanHairCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium, texture);
                case HumanCustomSkinPartId.Eye:
                    AddRendererIfExists(renderers, setup._part_eye);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, null, true);
                case HumanCustomSkinPartId.Glass:
                    AddRendererIfExists(renderers, setup._part_glass);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, null, true);
                case HumanCustomSkinPartId.Face:
                    AddRendererIfExists(renderers, setup._part_face);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, null, true);
                case HumanCustomSkinPartId.Skin:
                    AddRendererIfExists(renderers, setup._part_hand_l);
                    AddRendererIfExists(renderers, setup._part_hand_r);
                    AddRendererIfExists(renderers, setup._part_head);
                    AddRendererIfExists(renderers, setup._part_chest);
                    return new HumanCostumeCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium);
                case HumanCustomSkinPartId.Costume:
                    AddRendererIfExists(renderers, setup._part_arm_l);
                    AddRendererIfExists(renderers, setup._part_arm_r);
                    AddRendererIfExists(renderers, setup._part_leg);
                    AddRendererIfExists(renderers, setup._part_chest_2);
                    AddRendererIfExists(renderers, setup._part_chest_3);
                    AddRendererIfExists(renderers, setup._part_upper_body);
                    return new HumanCostumeCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeLarge);
                case HumanCustomSkinPartId.Logo:
                    AddRendererIfExists(renderers, setup._part_cape);
                    AddRendererIfExists(renderers, setup._part_brand_1);
                    AddRendererIfExists(renderers, setup._part_brand_2);
                    AddRendererIfExists(renderers, setup._part_brand_3);
                    AddRendererIfExists(renderers, setup._part_brand_4);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, useTransparentMaterial: true);
                case HumanCustomSkinPartId.GearL:
                    AddRendererIfExists(renderers, setup._part_3dmg);
                    AddRendererIfExists(renderers, setup._part_belt);
                    AddRendererIfExists(renderers, setup._part_gas_l);
                    if (setup.Weapon != HumanWeapon.Thunderspear)
                        AddRendererIfExists(renderers, setup._part_blade_l);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium, useTransparentMaterial: true);
                case HumanCustomSkinPartId.GearR:
                    AddRendererIfExists(renderers, setup._part_gas_r);
                    if (setup.Weapon != HumanWeapon.Thunderspear)
                        AddRendererIfExists(renderers, setup._part_blade_r);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium, useTransparentMaterial: true);
                case HumanCustomSkinPartId.Gas:
                    Transform smokeTransform = _owner.transform.Find("3dmg_smoke");
                    if (smokeTransform != null)
                        AddRendererIfExists(renderers, smokeTransform.gameObject);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall);
                case HumanCustomSkinPartId.Hoodie:
                    if (setup._part_chest_1 != null && setup._part_chest_1.name.Contains("char_cap"))
                        AddRendererIfExists(renderers, setup._part_chest_1);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeSmall, useTransparentMaterial: true);
                case HumanCustomSkinPartId.WeaponTrail:
                    List<MeleeWeaponTrail> trails = new List<MeleeWeaponTrail>();
                    if (setup.Weapon == HumanWeapon.Blade)
                    {
                        trails.Add(setup.LeftTrail);
                        trails.Add(setup.RightTrail);
                    }
                    return new WeaponTrailCustomSkinPart(this, trails, GetRendererId(partId), MaxSizeSmall);
                case HumanCustomSkinPartId.ThunderspearL:
                    if (setup.Weapon == HumanWeapon.Thunderspear && setup._part_blade_l != null)
                        AddRendererIfExists(renderers, setup._part_blade_l);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium, useTransparentMaterial: true);
                case HumanCustomSkinPartId.ThunderspearR:
                    if (setup.Weapon == HumanWeapon.Thunderspear && setup._part_blade_r != null)
                        AddRendererIfExists(renderers, setup._part_blade_r);
                    return new BaseCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium, useTransparentMaterial: true);
                case HumanCustomSkinPartId.Back:
                    AddAllRenderersIfExists(renderers, setup._part_back);
                    return new HumanCostumeCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium, null);
                case HumanCustomSkinPartId.Head:
                    AddAllRenderersIfExists(renderers, setup._part_head_decor);
                    return new HumanCostumeCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium, null);
                case HumanCustomSkinPartId.Hat:
                    AddAllRenderersIfExists(renderers, setup._part_hat);
                    return new HumanCostumeCustomSkinPart(this, renderers, GetRendererId(partId), MaxSizeMedium, null);
                case HumanCustomSkinPartId.HookL:
                    Human humanComponent = _owner.GetComponent<Human>();
                    if (humanComponent != null && humanComponent.enabled && humanComponent.HookLeft != null)
                        return new HookCustomSkinPart(this, humanComponent.HookLeft.GetRenderers(), GetRendererId(partId), MaxSizeSmall, _hookLTiling);
                    else
                        return new HookCustomSkinPart(this, new List<Renderer>(), GetRendererId(partId), MaxSizeSmall, _hookLTiling);
                case HumanCustomSkinPartId.HookR:
                    Human humanComponent2 = _owner.GetComponent<Human>();
                    if (humanComponent2 != null && humanComponent2.enabled && humanComponent2.HookRight != null)
                        return new HookCustomSkinPart(this, humanComponent2.HookRight.GetRenderers(), GetRendererId(partId), MaxSizeSmall, _hookRTiling);
                    else
                        return new HookCustomSkinPart(this, new List<Renderer>(), GetRendererId(partId), MaxSizeSmall, _hookRTiling);
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
        HookR,
        Hat,
        Head,
        Back
    }
}
