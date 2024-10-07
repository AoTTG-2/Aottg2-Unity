using UnityEngine;
using System.Collections.Generic;
using Utility;
using Projectiles;
using GameManagers;
using UnityEngine.TextCore.Text;
using SimpleJSONFixed;
using System.Collections;

namespace Characters
{
    class TitanAttackInfo
    {
        public bool HumanOnly;
        public bool FarOnly;
        public Vector3 MinRange;
        public Vector3 MaxRange;
        public bool HasKeyframes = false;
        public List<TitanAttackKeyframe> Keyframes = new List<TitanAttackKeyframe>();

        public TitanAttackInfo(JSONNode attackInfo, JSONNode keyframes)
        {
            HumanOnly = attackInfo["HumanOnly"].AsBool;
            FarOnly = attackInfo["Far"].AsBool;
            if (attackInfo.HasKey("Ranges"))
            {
                var range = attackInfo["Ranges"];
                MinRange = new Vector3(range["X"][0].AsFloat, range["Y"][0].AsFloat, range["Z"][0].AsFloat);
                MaxRange = new Vector3(range["X"][1].AsFloat, range["Y"][1].AsFloat, range["Z"][1].AsFloat);
            }
            if (keyframes != null)
            {
                HasKeyframes = true;
                for (int i = 0; i < keyframes["Keyframes"].Count; i++)
                    Keyframes.Add(new TitanAttackKeyframe(keyframes["Keyframes"][i]));
                var range = keyframes["Ranges"];
                MinRange = new Vector3(range["X"][0].AsFloat, range["Y"][0].AsFloat, range["Z"][0].AsFloat);
                MaxRange = new Vector3(range["X"][1].AsFloat, range["Y"][1].AsFloat, range["Z"][1].AsFloat);
            }
        }

        public bool CheckSimpleAttack(Vector3 relativePosition)
        {
            return (relativePosition.x >= MinRange.x && relativePosition.y >= MinRange.y && relativePosition.z >= MinRange.z
                && relativePosition.x <= MaxRange.x && relativePosition.y <= MaxRange.y && relativePosition.z <= MaxRange.z);
        }

        public bool CheckSmartAttack(Transform titan, Vector3 worldPosition, Vector3 velocity, float attackSpeed, float size)
        {
            foreach (var keyframe in Keyframes)
            {
                if (keyframe.CheckCollision(titan, worldPosition, velocity, attackSpeed, size))
                    return true;
            }
            return false;
        }
    }
}
