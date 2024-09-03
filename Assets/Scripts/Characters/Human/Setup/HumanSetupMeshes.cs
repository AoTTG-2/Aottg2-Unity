using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters
{
    class HumanSetupMeshes
    {
        private HumanSetup _setup;
        private string CostumesPath = "Human/Parts/Costumes/Prefabs/";
        private string AccessoriesPath = "Human/Parts/Accessories/Prefabs/";
        private string HairsPath = "Human/Parts/Hairs/Prefabs/";
        private string WeaponsPath = "Human/Parts/Weapons/Prefabs/";

        public HumanSetupMeshes(HumanSetup setup)
        {
            _setup = setup;
        }

        public string GetBootsMesh(int boots)
        {
            return CostumesPath + "character_leg_" + boots.ToString();
        }

        public string GetHandMesh(bool left)
        {
            string mesh = left ? "character_hand_l" : "character_hand_r";
            if (_setup.Weapon == HumanWeapon.Blade)
                mesh += "_0";
            else if (_setup.Weapon == HumanWeapon.AHSS || _setup.Weapon == HumanWeapon.APG)
                mesh += "_ah_0";
            else if (_setup.Weapon == HumanWeapon.Thunderspear)
                mesh += "_ts";
            return CostumesPath + mesh;
        }

        public string GetArmMesh(bool left)
        {
            string mesh = "player";
            if (_setup.Weapon == HumanWeapon.AHSS || _setup.Weapon == HumanWeapon.APG)
            {
                return CostumesPath + mesh + (left ? "_casual_arm_AH_L" : "_casual_arm_AH_R");
            }
            else if (_setup.CurrentCostume["Type"].Value.StartsWith("Uniform"))
                mesh += "_uniform";
            else
                mesh += "_casual";
            return CostumesPath + mesh + (left ? "_arm_L" : "_arm_R");
        }

        public string Get3dmgMesh()
        {
            return AccessoriesPath + ((_setup.Weapon == HumanWeapon.AHSS || _setup.Weapon == HumanWeapon.APG) ? "3dmg_2" : "3dmg");
        }

        public string GetBeltMesh()
        {
            return  (_setup.Weapon == HumanWeapon.AHSS || _setup.Weapon == HumanWeapon.APG) ? string.Empty : AccessoriesPath + "3dmg_belt";
        }

        public string GetGasMesh(bool left)
        {
            if (_setup.Weapon == HumanWeapon.AHSS || _setup.Weapon == HumanWeapon.APG)
                return AccessoriesPath + (left ? "char_gun_mag_l" : "char_gun_mag_r");
            return AccessoriesPath + (left ? "scabbard_L" : "scabbard_R");
        }

        public string GetWeaponMesh(bool left)
        {
            if (_setup.Weapon == HumanWeapon.AHSS || _setup.Weapon == HumanWeapon.APG)
                return WeaponsPath + (left ? "character_gun_l_0" : "character_gun_r_0");
            else if (_setup.Weapon == HumanWeapon.Thunderspear)
                return WeaponsPath + (left ? "thunderspear_l" : "thunderspear_r");
            return WeaponsPath + (left ? "blade_L" : "blade_R");
        }

        public string GetBodyMesh()
        {
            string mesh = "player";
            string type = _setup.CurrentCostume["Type"].Value;
            mesh += type.StartsWith("Uniform") ? "_uniform" : "_casual";
            mesh += _setup.CustomSet.Sex.Value == (int)HumanSex.Male ? "_M" : "_F";
            mesh += type.Last().ToString();
            return CostumesPath + mesh;
        }

        public string GetBrandMesh(int brand)
        {
            string type = _setup.CurrentCostume["Type"].Value;
            if (type.StartsWith("Uniform"))
            {
                if (brand == 1)
                    return CostumesPath  + "character_brand_arm_l_0";
                else if (brand == 2)
                    return CostumesPath + "character_brand_arm_r_0";
                else if (brand == 3)
                    return CostumesPath  + (_setup.CustomSet.Sex.Value == (int)HumanSex.Male ? "character_brand_chest_m_0" : "character_brand_chest_f_0");
                else if (brand == 4)
                    return CostumesPath + (_setup.CustomSet.Sex.Value == (int)HumanSex.Male ? "character_brand_back_m_0" : "character_brand_back_f_0");
            }
            return string.Empty;
        }

        public string GetEyeMesh()
        {
            return AccessoriesPath + "char_eyes";
        }

        public string GetFaceMesh()
        {
            return AccessoriesPath + "char_face";
        }

        public string GetGlassMesh()
        {
            return AccessoriesPath + "char_glasses";
        }

        public string GetHairMesh()
        {
            string hair = _setup.CurrentHair["Texture"].Value;
            if (hair == string.Empty)
                return string.Empty;
            return HairsPath + hair;
        }

        public string GetHairClothMesh()
        {
            if (_setup.CurrentHair.HasKey("Cloth"))
                return HairsPath + _setup.CurrentHair["Cloth"].Value;
            return string.Empty;
        }

        public string GetCapeMesh()
        {
            if (_setup.CustomSet.Cape.Value == 0)
                return string.Empty;
            return CostumesPath + "character_cape_0";
        }

        public string GetChestMesh(int chest)
        {
            if (chest == 1)
            {
                if (_setup.CurrentCostume.HasKey("Hoodie"))
                {
                    if (_setup.CurrentCostume["Type"].Value.StartsWith("Uniform"))
                        return AccessoriesPath + "char_cap_uni";
                    return AccessoriesPath + "char_cap_cas";
                }
            }
            else if (chest == 2)
            {
                if (_setup.CurrentCostume.HasKey("Holster"))
                {
                    if (_setup.CustomSet.Sex.Value == (int)HumanSex.Male)
                        return AccessoriesPath + "body_blade_keeper_m_0";
                    return AccessoriesPath + "body_blade_keeper_f_0";
                }
            }
            else if (chest == 3)
            {
                if (_setup.CurrentCostume.HasKey("Scarf"))
                {
                    if (_setup.CurrentCostume["Type"].Value.StartsWith("Uniform"))
                        return CostumesPath + "mikasa_asset_uni_0";
                    return CostumesPath + "mikasa_asset_cas_0";
                }
            }
            return string.Empty;
        }
    }
}
