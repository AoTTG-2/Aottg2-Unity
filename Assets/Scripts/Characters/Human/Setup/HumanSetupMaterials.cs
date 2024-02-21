using ApplicationManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace Characters
{
    public class HumanSetupMaterials
    {
        public static Dictionary<string, Material> HairMaterials = new Dictionary<string, Material>();
        public static Dictionary<string, Material> PartMaterials = new Dictionary<string, Material>();
        public static Material FaceMaterial;
        public static string TexturePath = "Human/Parts/Costumes/Textures/";
        public static string MaterialPath = "Human/Parts/Costumes/Materials/";

        public static void Init()
        {
            FaceMaterial = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, "Human/Parts/Accessories/Materials/HumanFaceMat");
        }

        public static Material GetCostumeMaterial(string mainTexture, string maskTexture, string colorTexture, string pantsTexture, Color inner, Color outer)
        {
            var material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, MaterialPath + "HumanCostumeMat", true);
            var mainTex = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.Characters, TexturePath + mainTexture);
            var maskTex = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.Characters, TexturePath + maskTexture);
            var colorTex = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.Characters, TexturePath + colorTexture);
            var pantsTex = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.Characters, TexturePath + pantsTexture);
            material.SetTexture("_main_tex", mainTex);
            material.SetTexture("_main_tex_mask", maskTex);
            material.SetTexture("_color_tex", colorTex);
            material.SetTexture("_pants_tex", pantsTex);
            material.SetColor("_inner_color", inner);
            material.SetColor("_outer_color", outer);
            return material;
        }

        public static Material GetPartMaterial(string texture)
        {
            if (!PartMaterials.ContainsKey(texture))
            {
                var material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, MaterialPath + "HumanPartMat", true);
                var mainTexture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.Characters, TexturePath + texture);
                material.mainTexture = mainTexture;
                PartMaterials[texture] = material;
            }
            return PartMaterials[texture];
        }

        public static Material GetCustomSkinMaterial()
        {
            return ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, MaterialPath + "HumanPartMat", true);
        }

        public static Material GetHairMaterial(string texture)
        {
            if (!HairMaterials.ContainsKey(texture))
            {
                var material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, "Human/Parts/Hairs/Materials/HumanHairMat", true);
                var mainTexture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.Characters, "Human/Parts/Hairs/Textures/" + texture + "Tex");
                material.mainTexture = mainTexture;
                HairMaterials[texture] = material;
            }
            return HairMaterials[texture];
        }
    }
}
