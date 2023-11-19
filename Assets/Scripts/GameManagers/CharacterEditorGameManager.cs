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
        public HumanDummy Character;
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
            GameObject go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, "Human/Prefabs/HumanPlayer", Vector3.zero, Quaternion.identity);
            Character = go.AddComponent<HumanDummy>();
            go.GetComponent<Human>().enabled = false;
            go.GetComponent<HumanMovementSync>().enabled = false;
            SettingsManager.HumanCustomSettings.CustomSets.SelectedSetIndex.Value = 0;
            Character.Setup.Load((HumanCustomSet)SettingsManager.HumanCustomSettings.CustomSets.GetSelectedSet(), HumanWeapon.Blade, false);
            Character.Idle();
        }

        public void GeneratePreviews()
        {
            if (!Directory.Exists(PreviewFolderPath))
                Directory.CreateDirectory(PreviewFolderPath);
            platform.SetActive(false);
            var set = new HumanCustomSet();
            // File.WriteAllBytes(SnapshotPath + "/" + fileName, texture.EncodeToPNG());
        }
    }
}
