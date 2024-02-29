using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    class HumanSetupTextures
    {
        private HumanSetup _setup;

        public HumanSetupTextures(HumanSetup setup)
        {
            _setup = setup;
        }

        public string Get3dmgTexture()
        {
            if (_setup.Weapon == HumanWeapon.AHSS || _setup.Weapon == HumanWeapon.APG)
                return "Misc/aottg_hero_AHSS_3dmg";
            return "Misc/AOTTG_HERO_3DMG";
        }

        public string GetBrandTexture()
        {
            switch (_setup.CustomSet.Logo.Value)
            {
                case 0:
                    return "Brand/aottg_hero_brand_ts";
                case 1:
                    return "Brand/aottg_hero_brand_sc";
                case 2:
                    return "Brand/aottg_hero_brand_g";
                case 3:
                    return "Brand/aottg_hero_brand_mp";
            }
            return string.Empty;
        }

        public string GetSkinTexture()
        {
            string end = (_setup.CustomSet.Skin.Value + 1).ToString();
            if (_setup.Weapon == HumanWeapon.Thunderspear)
                return "Skin/skin_TS" + end;
            else if (_setup.Weapon == HumanWeapon.AHSS || _setup.Weapon == HumanWeapon.APG)
                return "Skin/skin_AHSS" + end;
            return "Skin/skin_blades" + end;
        }

        public string GetHairTexture()
        {
            return _setup.CurrentHair["Texture"].Value;
        }

        public string GetBodyMainTexture()
        {
            if (_setup.CurrentCostume["Type"].Value.StartsWith("Uniform"))
                return "Uniform/" + _setup.CurrentCostume["_main_tex"].Value;
            return "Casual/" + _setup.CurrentCostume["_main_tex"].Value;
        }

        public string GetBodyMaskTexture()
        {
            if (_setup.CurrentCostume["Type"].Value.StartsWith("Uniform"))
                return "Uniform/" + _setup.CurrentCostume["_main_tex_mask"].Value;
            return "Casual/" + _setup.CurrentCostume["_main_tex_mask"].Value;
        }

        public string GetBodyColorTexture()
        {
            if (_setup.CurrentCostume["Type"].Value.StartsWith("Uniform"))
                return "Uniform/" + _setup.CurrentCostume["_color_tex"].Value;
            return "Casual/" + _setup.CurrentCostume["_color_tex"].Value;
        }

        public string GetBodyPantsTexture()
        {
            return "Pants/" + _setup.CurrentCostume["_pants_tex"].Value;
        }

        public string GetChestTexture(int chest)
        {
            if (chest == 1)
            {
                if (_setup.CurrentCostume["Type"].Value.StartsWith("Uniform"))
                    return "Misc/aottg_hero_annie_cap_uniform";
                return "Misc/aottg_hero_annie_cap_causal";
            }
            return string.Empty;
        }
    }
}
