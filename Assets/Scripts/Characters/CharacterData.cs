using SimpleJSONFixed;
using System.IO;
using Utility;
using Settings;
using System.Collections.Generic;
using ApplicationManagers;
using UnityEngine;

namespace Characters
{
    class CharacterData
    {
        public static JSONNode HumanWeaponInfo;
        public static Dictionary<string, JSONNode> TitanAIInfos = new Dictionary<string, JSONNode>();
        public static Dictionary<string, Dictionary<string, TitanAttackInfo>> TitanAttackInfos = new Dictionary<string, Dictionary<string, TitanAttackInfo>>();

        public static void Init()
        {
            foreach (string name in new string[] {"Titan", "Annie"})
            {
                TitanAIInfos.Add(name, JSON.Parse(ResourceManager.TryLoadText(ResourcePaths.CharacterData, name + "AIInfo")));
                TitanAttackInfos.Add(name, LoadTitanAttackInfos(TitanAIInfos[name], name + "Keyframes"));
            }
            HumanWeaponInfo = JSON.Parse(ResourceManager.TryLoadText(ResourcePaths.CharacterData, "HumanWeaponInfo"));
        }

        private static Dictionary<string, TitanAttackInfo> LoadTitanAttackInfos(JSONNode info, string keyframeFile)
        {
            var keyframes = JSON.Parse(ResourceManager.TryLoadText(ResourcePaths.CharacterData, keyframeFile));
            var attackInfo = info["AttackInfo"];
            var attacks = new Dictionary<string, TitanAttackInfo>();
            foreach (string attackName in attackInfo.Keys)
            {
                if (keyframes.HasKey(attackName))
                    attacks[attackName] = new TitanAttackInfo(attackInfo[attackName], keyframes[attackName]);
                else
                    attacks[attackName] = new TitanAttackInfo(attackInfo[attackName], null);
            }
            return attacks;
        }

        public static JSONNode GetTitanAI(GameDifficulty difficulty, string titanType)
        {
            JSONNode spawnRates = TitanAIInfos["Titan"]["SpawnRates"][difficulty.ToString()];
            if (titanType == "Default")
                titanType = (string)Util.GetRandomFromWeightedNode(spawnRates);
            var titanNode = TitanAIInfos["Titan"]["Default"].Clone();
            var defaultNode = TitanAIInfos["Titan"][titanType]["Default"];
            CopyNode(titanNode, defaultNode);
            if (TitanAIInfos["Titan"][titanType].HasKey(difficulty.ToString()))
            {
                var difficultyNode = TitanAIInfos["Titan"][titanType][difficulty.ToString()];
                CopyNode(titanNode, difficultyNode);
            }
            titanNode["Type"] = "Titan";
            return titanNode;
        }

        public static JSONNode GetShifterAI(GameDifficulty difficulty, string name)
        {
            var shifterNode = TitanAIInfos[name]["Default"].Clone();
            if (TitanAIInfos[name].HasKey(difficulty.ToString()))
            {
                var difficultyNode = TitanAIInfos[name][difficulty.ToString()];
                CopyNode(shifterNode, difficultyNode);
            }
            shifterNode["Type"] = TitanAIInfos[name]["Type"].Value;
            return shifterNode;
        }

        private static void CopyNode(JSONNode current, JSONNode copy)
        {
            foreach (JSONNode key in copy.Keys)
            {
                current[key.Value] = copy[key.Value];
            }
        }
    }
}
