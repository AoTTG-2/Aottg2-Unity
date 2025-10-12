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
        public static int BodyCount;
        public static int HeadCount;
        public static int EyeCount;
        public static int HairMCount;
        public static int HairFCount;
        private static Dictionary<string, string> HairPrefabs = new Dictionary<string, string>();
        public static List<string> AllHairs = new List<string>();
        public static List<int> AIEyes = new List<int>();
        public static List<Color255> AIHairColors = new List<Color255>();

        public static void Init()
        {
            Info = JSON.Parse(ResourceManager.TryLoadText(ResourcePaths.CharacterData, "TitanSetupInfo"));
            BodyCount = Info["BodyCount"].AsInt;
            HeadCount = Info["HeadCount"].AsInt;
            EyeCount = Info["EyeCount"].AsInt;
            HairMCount = Info["HairM"].Count;
            HairFCount = Info["HairF"].Count;
            for (int i = 0; i < HairMCount; i++)
            {
                string hair = "HairM" + i;
                AllHairs.Add(hair);
                HairPrefabs[hair] = Info["HairM"][i];
            }
            for (int i = 0; i < HairFCount; i++)
            {
                string hair = "HairF" + i;
                AllHairs.Add(hair);
                HairPrefabs[hair] = Info["HairF"][i];
            }
            foreach (JSONNode node in Info["HairColors"].AsArray)
                AIHairColors.Add(new Color255(node[0].AsInt, node[1].AsInt, node[2].AsInt, node[3].AsInt));
            var excludedEyes = new HashSet<int>();
            foreach (JSONNode node in Info["AIExcludedEyes"].AsArray)
                excludedEyes.Add(node.AsInt);
            for (int i = 0; i < EyeCount; i++)
            {
                if (!excludedEyes.Contains(i))
                    AIEyes.Add(i);
            }
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

        public TitanCustomSet CreateRandomSet(int headPrefab)
        {
            var set = new TitanCustomSet();
            set.Head.Value = headPrefab;
            set.Hair.Value = AllHairs.GetRandomItem();
            set.HairColor.Value = AIHairColors.GetRandomItem();
            set.Eye.Value = AIEyes.GetRandomItem();
            int gray = UnityEngine.Random.Range(160, 256);
            set.SkinColor.Value = new Color255(gray, gray, gray, 255);
            return set;
        }

        public void Load(TitanCustomSet set)
        {
            var head = transform.Find("Amarture_VER2/Core/Controller.Body/hip/spine/chest/neck/head");
            var headIndex = set.Head.Value;
            var bodyColor = set.SkinColor.Value.ToColor();
            transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material.color = bodyColor;

            // hair
            var hairSocket = head.Find("HairSocket");
            for (int i = hairSocket.childCount - 1; i >= 0; i--)
            {
                var child = hairSocket.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
            var hair = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, "Titans/Hairs/Prefabs/" + HairPrefabs[set.Hair.Value], true);
            hair.transform.SetParent(hairSocket);
            hair.transform.localPosition = Vector3.zero;
            hair.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
            hair.transform.localScale = Vector3.one;
            foreach (Renderer renderer in hair.GetComponentsInChildren<Renderer>())
            {
                renderer.material = HumanSetupMaterials.GetHairMaterial(HairPrefabs[set.Hair.Value]);
                renderer.material.color = set.HairColor.Value.ToColor();
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
            int eyeTexture = set.Eye.Value;
            var eyes = head.Find("Eyes");
            var eyesRef = ((GameObject)ResourceManager.LoadAsset(ResourcePaths.Characters, "Titans/Heads/Prefabs/" + eyesAsset, true)).transform;
            eyes.GetComponent<MeshFilter>().sharedMesh = eyesRef.GetComponent<MeshFilter>().sharedMesh;
            eyes.GetComponent<MeshRenderer>().material = HumanSetupMaterials.GetTitanEyeMaterial("Eye" + eyeTexture.ToString());
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
