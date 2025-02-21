using System.Collections.Generic;
using UnityEngine;
using Weather;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using System.Diagnostics;
using Photon;
using Map;
using CustomLogic;
using System.Collections;
using Characters;
using Settings;
using System.IO;

namespace GameManagers
{
    class CharacterEditorGameManager : BaseGameManager
    {
        public static bool HumanMode = true;
        public DummyHuman Human;
        public DummyTitan Titan;
        public DummyCharacter Character;
        private static string PreviewFolderPath = FolderPaths.Documents + "/CharacterPreviews";
        private GameObject platform;

        protected override void Awake()
        {
            base.Awake();
            platform = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Map, "Geometry/Prefabs/Cuboid",
                Vector3.down * 0.05f, Quaternion.identity);
            platform.transform.localScale = new Vector3(2f, 0.1f, 2f);
            platform.GetComponent<Renderer>().material = (Material)ResourceManager.LoadAsset(ResourcePaths.Map, "Materials/TransparentMaterial");
            platform.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.2f);
            SettingsManager.HumanCustomSettings.CustomSets.SelectedSetIndex.Value = 0;
            SettingsManager.TitanCustomSettings.TitanCustomSets.SelectedSetIndex.Value = 0;
            ReinstantiateCharacter();
        }

        public void ReinstantiateCharacter()
        {
            if (Character != null)
                Destroy(Character.gameObject);
            if (HumanMode)
            {
                GameObject go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, "Human/Prefabs/HumanPlayer", Vector3.zero, Quaternion.identity);
                Human = go.AddComponent<DummyHuman>();
                go.GetComponent<Human>().enabled = false;
                go.GetComponent<HumanMovementSync>().enabled = false;
                Human.Setup.Load((HumanCustomSet)SettingsManager.HumanCustomSettings.CustomSets.GetSelectedSet(), HumanWeapon.Blade, false);
                Human.Idle();
                Character = Human;
            }
            else
            {
                var set = (TitanCustomSet)SettingsManager.TitanCustomSettings.TitanCustomSets.GetSelectedSet();
                int body = set.Body.Value;
                GameObject go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, CharacterPrefabs.BasicTitanPrefix + body, Vector3.zero, Quaternion.identity);
                Titan = go.AddComponent<DummyTitan>();
                go.GetComponent<BasicTitan>().enabled = false;
                go.GetComponent<BasicTitanMovementSync>().enabled = false;
                Titan.Setup.Load(set);
                Titan.Idle();
                Character = Titan;
            }
        }

        public void GeneratePreviews()
        {
            if (!Directory.Exists(PreviewFolderPath))
                Directory.CreateDirectory(PreviewFolderPath);
            platform.SetActive(false);
            if (HumanMode)
                StartCoroutine(GenerateHumanPreviewsCoroutine());
        }

        private IEnumerator GenerateHumanPreviewsCoroutine()
        {
            var set = new HumanCustomSet();
            set.Hair.Value = "HairM8";
            set.Costume.Value = 1;
            for (int i = 0; i < HumanSetup.EyeCount; i++)
            {
                set.Eye.Value = i;
                Human.Setup.Load(set, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                Screenshot(870f, 500f, 172f, 172f, "Eye" + i.ToString());
            }
            set.Eye.Value = 0;
            for (int i = -1; i < HumanSetup.FaceCount; i++)
            {
                if (i == -1)
                    set.Face.Value = "FaceNone";
                else
                    set.Face.Value = "Face" + i.ToString();
                Human.Setup.Load(set, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                Screenshot(870f, 470f, 172f, 172f, set.Face.Value);
            }
            set.Face.Value = "FaceNone";
            for (int i = -1; i < HumanSetup.GlassCount; i++)
            {
                if (i == -1)
                    set.Glass.Value = "GlassNone";
                else
                    set.Glass.Value = "Glass" + i.ToString();
                Human.Setup.Load(set, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                Screenshot(870f, 500f, 172f, 172f, set.Glass.Value);
            }
            set.Glass.Value = "GlassNone";
            for (int i = 0; i < HumanSetup.HairMCount; i++)
            {
                set.Hair.Value = "HairM" + i.ToString();
                Human.Setup.Load(set, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                Screenshot(816f, 510f, 280f, 280f, set.Hair.Value);
            }
            for (int i = 0; i < HumanSetup.HairFCount; i++)
            {
                set.Hair.Value = "HairF" + i.ToString();
                Human.Setup.Load(set, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                Screenshot(816f, 510f, 280f, 280f, set.Hair.Value);
            }
            set.Hair.Value = "HairM10";
            for (int i = -1; i < HumanSetup.HatCount; i++)
            {
                if (i == -1)
                    set.Hat.Value = "HatNone";
                else
                    set.Hat.Value = "Hat" + i.ToString();
                Human.Setup.Load(set, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                Screenshot(796f, 570f, 320f, 320f, set.Hat.Value);
            }
            set.Hat.Value = "HatNone";
            Human.transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
            for (int i = -1; i < HumanSetup.BackCount; i++)
            {
                if (i == -1)
                    set.Back.Value = "BackNone";
                else
                    set.Back.Value = "Back" + i.ToString();
                Human.Setup.Load(set, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                Screenshot(776f, 300f, 360f, 360f, set.Back.Value);
            }
            set.Back.Value = "BackNone";
            Human.transform.rotation = Quaternion.identity;
            for (int i = -1; i < HumanSetup.HeadCount; i++)
            {
                if (i == -1)
                    set.Head.Value = "HeadNone";
                else
                    set.Head.Value = "Head" + i.ToString();
                Human.Setup.Load(set, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                Screenshot(801f, 470f, 310f, 310f, set.Head.Value);
            }
            set.Head.Value = "HeadNone";
            set.Hair.Value = "HairM8";
            for (int i = 0; i < HumanSetup.CostumeMCount; i++)
            {
                set.Costume.Value = i;
                Human.Setup.Load(set, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                Screenshot(826f, 250f, 260f, 260f, "CostumeM" + i.ToString());
            }
            set.Sex.Value = 1;
            set.Hair.Value = "HairF7";
            for (int i = 0; i < HumanSetup.CostumeFCount; i++)
            {
                set.Costume.Value = i;
                Human.Setup.Load(set, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                Screenshot(826f, 250f, 260f, 260f, "CostumeF" + i.ToString());
            }
            foreach (HumanCustomSet preset in SettingsManager.HumanCustomSettings.Costume1Sets.GetSets().GetItems())
            {
                Human.Setup.Load(preset, HumanWeapon.Blade, false);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                Screenshot(746f, 360f, 420f, 420f, "Preset" + preset.Name.Value);
            }
            Screenshot(450f, 360f, 128f, 128f, "PresetNone");
        }

        private void Screenshot(float x, float y, float w, float h, string file)
        {
            Texture2D texture = new Texture2D((int)w, (int)h, TextureFormat.RGB24, false);
            try
            {
                texture.SetPixel(0, 0, Color.white);
                texture.ReadPixels(new Rect(x, y, w, h), 0, 0);
            }
            catch
            {
                texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.white);
            }
            texture.Apply();
            TextureScaler.ScaleBlocking(texture, 128, 128);
            File.WriteAllBytes(PreviewFolderPath + "/" + file + ".png", texture.EncodeToPNG());
        }
    }
}
