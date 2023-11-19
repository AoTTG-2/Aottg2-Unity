using ApplicationManagers;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Characters
{
    public class HumanSetupMaterials
    {
        public static Dictionary<string, Material> Materials = new Dictionary<string, Material>();

        public static void Init()
        {
            AddCostume("AOTTG_HERO_3DMG");
            AddCostume("aottg_hero_AHSS_3dmg");
            AddCostume("aottg_hero_annie_cap_causal");
            AddCostume("aottg_hero_annie_cap_uniform");
            AddCostume("aottg_hero_brand_sc");
            AddCostume("aottg_hero_brand_mp");
            AddCostume("aottg_hero_brand_g");
            AddCostume("aottg_hero_brand_ts");
            AddCostume("skin_blades1");
            AddCostume("skin_blades2");
            AddCostume("skin_AHSS1");
            AddCostume("skin_AHSS2");
            AddCostume("skin_TS1");
            AddCostume("skin_TS2");
            AddCostume("aottg_hero_skin_2");
            AddCostume("aottg_hero_skin_3");
            AddCostume("aottg_hero_casual_fa_1");
            AddCostume("aottg_hero_casual_fa_2");
            AddCostume("aottg_hero_casual_fa_3");
            AddCostume("aottg_hero_casual_fb_1");
            AddCostume("aottg_hero_casual_fb_2");
            AddCostume("aottg_hero_casual_ma_1");
            AddCostume("aottg_hero_casual_ma_1_ahss");
            AddCostume("aottg_hero_casual_fa_1_ahss");
            AddCostume("aottg_hero_casual_ma_2");
            AddCostume("aottg_hero_casual_ma_3");
            AddCostume("aottg_hero_casual_mb_1");
            AddCostume("aottg_hero_casual_mb_2");
            AddCostume("aottg_hero_casual_mb_3");
            AddCostume("aottg_hero_casual_mb_4");
            AddCostume("aottg_hero_uniform_fa_1");
            AddCostume("aottg_hero_uniform_fa_2");
            AddCostume("aottg_hero_uniform_fa_3");
            AddCostume("aottg_hero_uniform_fb_1");
            AddCostume("aottg_hero_uniform_fb_2");
            AddCostume("aottg_hero_uniform_ma_1");
            AddCostume("aottg_hero_uniform_ma_2");
            AddCostume("aottg_hero_uniform_ma_3");
            AddCostume("aottg_hero_uniform_mb_1");
            AddCostume("aottg_hero_uniform_mb_2");
            AddCostume("aottg_hero_uniform_mb_3");
            AddCostume("aottg_hero_uniform_mb_4");
            AddHair("hair_annie");
            AddHair("hair_armin");
            AddHair("hair_boy1");
            AddHair("hair_boy2");
            AddHair("hair_boy3");
            AddHair("hair_boy4");
            AddHair("hair_eren");
            AddHair("hair_girl1");
            AddHair("hair_girl2");
            AddHair("hair_girl3");
            AddHair("hair_girl4");
            AddHair("hair_girl5");
            AddHair("hair_hanji");
            AddHair("hair_jean");
            AddHair("hair_levi");
            AddHair("hair_marco");
            AddHair("hair_mike");
            AddHair("hair_petra");
            AddHair("hair_rico");
            AddHair("hair_sasha");
            AddHair("hair_mikasa");
            Materials.Add("HumanFace", ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters,
                "Human/Parts/Accessories/Materials/HumanFaceMat"));
        }

        private static void AddCostume(string tex)
        {
            AddMaterial(tex, "Human/Parts/Costumes/Textures/" + tex + "Tex", "Human/Parts/Costumes/Materials/HumanCostumeMat");
        }

        private static void AddHair(string tex)
        {
            AddMaterial(tex, "Human/Parts/Hairs/Textures/" + tex + "Tex", "Human/Parts/Hairs/Materials/HumanHairMat");
        }

        private static void AddMaterial(string name, string tex, string mat)
        {
            Texture texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.Characters, tex);
            Material material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, mat);
            material.mainTexture = texture;
            Materials.Add(name, material);
        }
    }
}
