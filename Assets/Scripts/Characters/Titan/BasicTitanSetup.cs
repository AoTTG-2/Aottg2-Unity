using System;
using UnityEngine;
using ApplicationManagers;
using GameManagers;
using UnityEngine.UI;
using Utility;
using Controllers;
using CustomSkins;
using System.Collections.Generic;
using SimpleJSONFixed;
using System.Collections;
using System.IO;
using Settings;

namespace Characters
{
    class BasicTitanSetup: MonoBehaviour
    {
        public static JSONNode Info;

        public static void Init()
        {
            Info = JSON.Parse(ResourceManager.TryLoadText(ResourcePaths.CharacterData, "TitanSetupInfo"));
        }

        public static int[] GetRandomBodyHeadCombo(JSONNode node = null)
        {
            int[] result = new int[2];
            if (SettingsManager.InGameCurrent.Titan.TitanStandardModels.Value)
            {
                result[0] = 0;
                result[1] = 0;
                return result;
            }
            if (node == null)
                node = CharacterData.TitanAIInfos["Titan"]["Default"];
            var combos = node["BodyHeadCombos"];
            List<object> nodes = new List<object>();
            List<float> weights = new List<float>();
            foreach (JSONNode n in combos)
            {
                nodes.Add(n);
                weights.Add(n["Chance"].AsFloat);
            }
            var combo = (JSONNode)Util.GetRandomFromWeightedList(nodes, weights);
            result[0] = combo["Body"].AsInt;
            result[1] = combo["Head"].AsInt;
            return result;
        }

        public string CreateRandomSetupJson(int headPrefab)
        {
            var json = new JSONObject();
            json.Add("HeadPrefab", headPrefab);
            json.Add("HairPrefab", Info["HairPrefabs"].GetRandomItem());
            json.Add("HairColor", Info["HairColors"].GetRandomItem());
            json.Add("EyeTexture", UnityEngine.Random.Range(0, Info["EyeTextureCount"].AsInt));
            return json.ToString();
        }

        public void Load(string jsonString)
        {
            var json = JSON.Parse(jsonString);
            var head = transform.Find("Amarture_VER2/Core/Controller.Body/hip/spine/chest/neck/head");
            var headIndex = json["HeadPrefab"].AsInt;
            float gray = UnityEngine.Random.Range(0.7f, 1f);
            var bodyColor = new Color(gray, gray, gray);
            transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material.color = bodyColor;

            // hair
            var hairSocket = head.Find("HairSocket");
            var hair = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, "Titans/Hairs/Prefabs/" + json["HairPrefab"].Value, true);
            hair.transform.SetParent(hairSocket);
            hair.transform.localPosition = Vector3.zero;
            hair.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
            hair.transform.localScale = Vector3.one;
            foreach (Renderer renderer in hair.GetComponentsInChildren<Renderer>())
            {
                renderer.material = HumanSetupMaterials.GetHairMaterial(json["HairPrefab"].Value);
                renderer.material.color = json["HairColor"].ToColor();
            }

            // head
            string headAsset = "TitanHead" + headIndex.ToString();
            var headMesh = transform.Find("Head");
            var headRef = ((GameObject)ResourceManager.LoadAsset(ResourcePaths.Characters, "Titans/Heads/Prefabs/" + headAsset, true)).transform;
            headMesh.GetComponent<SkinnedMeshRenderer>().material = transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material;
            headMesh.GetComponent<SkinnedMeshRenderer>().sharedMesh = headRef.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            string headColliderAsset = "TitanHeadCollider" + headIndex.ToString();
            headRef = ((GameObject)ResourceManager.LoadAsset(ResourcePaths.Characters, "Titans/Heads/Prefabs/" + headColliderAsset, true)).transform;
            CopyColliders(headRef, head, true, false);
            CopyColliders(headRef.Find("Bone"), head.Find("Bone"), false, false);
            CopyColliders(headRef.Find("EyesHurtbox"), head.Find("EyesHurtbox"), false, true);
            var hairRef = headRef.Find("HairSocket");
            var hairTo = head.Find("HairSocket");
            hairTo.localPosition = hairRef.localPosition;
            hairTo.localRotation = hairRef.localRotation;
            hairTo.localScale = hairRef.localScale;
            if (headRef.Find("Nose") != null)
            {
                var noseRef = headRef.Find("Nose");
                var nose = new GameObject();
                nose.layer = PhysicsLayer.TitanPushbox;
                nose.transform.SetParent(head);
                nose.transform.localPosition = noseRef.localPosition;
                nose.transform.localRotation = noseRef.localRotation;
                nose.transform.localScale = noseRef.localScale;
                var collider = nose.AddComponent<BoxCollider>();
                collider.center = Vector3.zero;
                collider.size = Vector3.one;
            }

            // eyes
            string eyesAsset = "TitanEyes" + headIndex.ToString();
            int eyeTexture = json["EyeTexture"].AsInt;
            if (eyeTexture == 7)
                eyeTexture = 0;
            var eyes = head.Find("Eyes");
            var eyesRef = ((GameObject)ResourceManager.LoadAsset(ResourcePaths.Characters, "Titans/Heads/Prefabs/" + eyesAsset, true)).transform;
            eyes.GetComponent<MeshFilter>().sharedMesh = eyesRef.GetComponent<MeshFilter>().sharedMesh;
            int col = eyeTexture / 8;
            int row = eyeTexture % 8;
            eyes.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(0.25f * col, -0.125f * row);
        }

        protected void CopyColliders(Transform from, Transform to, bool capsule, bool moveTransform)
        {
            if (capsule)
            {
                var fromCollider = from.GetComponent<CapsuleCollider>();
                var toCollider = to.GetComponent<CapsuleCollider>();
                toCollider.center = fromCollider.center;
                toCollider.radius = fromCollider.radius;
                toCollider.height = fromCollider.height;
                if (moveTransform)
                {
                    toCollider.transform.localPosition = fromCollider.transform.localPosition;
                    toCollider.transform.localRotation = fromCollider.transform.localRotation;
                    toCollider.transform.localScale = fromCollider.transform.localScale;
                }
            }
            else
            {
                var fromCollider = from.GetComponent<BoxCollider>();
                var toCollider = to.GetComponent<BoxCollider>();
                toCollider.center = fromCollider.center;
                toCollider.size = fromCollider.size;
                if (moveTransform)
                {
                    toCollider.transform.localPosition = fromCollider.transform.localPosition;
                    toCollider.transform.localRotation = fromCollider.transform.localRotation;
                    toCollider.transform.localScale = fromCollider.transform.localScale;
                }
            }
        }
    }
}
