using ApplicationManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Utility;

namespace Characters
{
    public class HumanSetupMaterials
    {
        public static Dictionary<string, Material> HairMaterials = new Dictionary<string, Material>();
        public static Dictionary<string, Material> PartMaterials = new Dictionary<string, Material>();
        public static Dictionary<string, Material> FaceMaterials = new Dictionary<string, Material>();
        public static Dictionary<string, Material> TitanEyeMaterials = new Dictionary<string, Material>();
        public static string TexturePath = "Human/Parts/Costumes/Textures/";
        public static string MaterialPath = "Human/Parts/Costumes/Materials/";

        public static void Init()
        {
        }

        public static Material GetCostumeMaterial(string mainTexture, string maskTexture, string colorTexture, string pantsTexture, Color shirt, Color straps, Color pants, Color jacket, Color boots)
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
            material.SetColor("_shirt_color", shirt);
            material.SetColor("_straps_color", straps);
			material.SetColor("_pants_color", pants);
			material.SetColor("_jacket_color", jacket);
			material.SetColor("_boots_color", boots);
            return material;
        }

        public static Material GetPartMaterial(string texture, bool isOneSided = false)
        {
            if (!PartMaterials.ContainsKey(texture))
            {
                Material material;
                if (isOneSided)
                    material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, MaterialPath + "HumanPart1SMat", true);
                else
                    material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, MaterialPath + "HumanPartMat", true);
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

        public static Material GetFaceMaterial(string texture, bool untiled)
        {
            if (!FaceMaterials.ContainsKey(texture))
            {
                string name = "HumanFaceMat";
                if (untiled)
                    name = "HumanFaceUntiledMat";
                var material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, "Human/Parts/Face/Materials/" + name, true);
                var mainTexture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.Characters, "Human/Parts/Face/Textures/" + texture);
                material.mainTexture = mainTexture;
                FaceMaterials[texture] = material;
            }
            return FaceMaterials[texture];
        }

        public static Material GetTitanEyeMaterial(string texture)
        {
            if (!TitanEyeMaterials.ContainsKey(texture))
            {
                var material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, "Titans/Heads/Materials/TitanEyesMat", true);
                var mainTexture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.Characters, "Titans/Heads/Textures/" + texture);
                material.mainTexture = mainTexture;
                TitanEyeMaterials[texture] = material;
            }
            return TitanEyeMaterials[texture];
        }

        public static Material GetSkinMaterial(string texture, Color color)
        {
            var material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Characters, "Human/Parts/Costumes/Materials/HumanSkinMat", true);
            var mainTexture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.Characters, "Human/Parts/Costumes/Textures/" + texture);
            material.SetTexture("_weapon_tex", mainTexture);
            material.SetColor("_skin_color", color);
            return material;
        }
    }
}
