using SimpleJSONFixed;
using System.IO;
using Utility;
using Settings;
using System.Collections.Generic;

namespace Characters
{
    class CharacterData
    {
        public static JSONNode TitanAIInfo;
        public static JSONNode ShifterAIInfo;
        public static JSONNode HumanWeaponInfo;

        public static void Init()
        {
            TitanAIInfo = JSON.Parse(File.ReadAllText(FolderPaths.TesterData + "/TitanAIInfo.json"));
            ShifterAIInfo = JSON.Parse(File.ReadAllText(FolderPaths.TesterData + "/ShifterAIInfo.json"));
            HumanWeaponInfo = JSON.Parse(File.ReadAllText(FolderPaths.TesterData + "/HumanWeaponInfo.json"));
        }

        public static JSONNode GetTitanAI(GameDifficulty difficulty, string titanType)
        {
            JSONNode spawnRates = TitanAIInfo["SpawnRates"][difficulty.ToString()];
            if (titanType == "Default")
                titanType = (string)Util.GetRandomFromWeightedNode(spawnRates);
            var titanNode = TitanAIInfo["Default"].Clone();
            var defaultNode = TitanAIInfo[titanType]["Default"];
            CopyNode(titanNode, defaultNode);
            if (TitanAIInfo[titanType].HasKey(difficulty.ToString()))
            {
                var difficultyNode = TitanAIInfo[titanType][difficulty.ToString()];
                CopyNode(titanNode, difficultyNode);
            }
            titanNode["AttackRanges"] = TitanAIInfo["AttackRanges"];
            return titanNode;
        }

        public static JSONNode GetShifterAI(GameDifficulty difficulty, string shifterType)
        {
            var shifterNode = ShifterAIInfo[shifterType]["Default"].Clone();
            if (ShifterAIInfo[shifterType].HasKey(difficulty.ToString()))
            {
                var difficultyNode = ShifterAIInfo[shifterType][difficulty.ToString()];
                CopyNode(shifterNode, difficultyNode);
            }
            shifterNode["AttackRanges"] = ShifterAIInfo[shifterType]["AttackRanges"];
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
