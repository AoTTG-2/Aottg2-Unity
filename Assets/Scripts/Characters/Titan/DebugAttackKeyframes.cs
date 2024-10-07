using Settings;
using Characters;
using UnityEngine;
using GameManagers;
using UI;
using CustomLogic;
using SimpleJSONFixed;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Utility;

namespace Controllers
{
    class DebugAttackKeyframes: MonoBehaviour
    {
        protected BaseTitan _titan;
        protected string _name;

        protected void Awake()
        {
            _titan = GetComponent<BaseTitan>();
            _titan.AttackSpeedMultiplier = 1f;
            _titan.AttackSpeeds.Clear();
            if (_titan is BasicTitan)
                _name = "Titan";
            else if (_titan is BaseShifter)
            {
                _name = _titan.GetType().ToString();
                _name = _name.Replace("Shifter", "");
                _name = _name.Replace("Characters.", "");
            }
            StartCoroutine(GenerateAttackFrames());
        }

        public IEnumerator GenerateAttackFrames()
        {
            var data = CharacterData.TitanAIInfos[_name]["AttackInfo"];
            JSONNode newData = new JSONObject();
            _titan.Cache.Transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(1f);
            int total = 0;
            foreach (string attackName in data.Keys)
            {
                var info = data[attackName];
                var newInfo = new JSONObject();
                if (info["Far"].AsBool || attackName == "AttackBellyFlop")
                    continue;
                var frames = new JSONArray();
                float startTime = Time.fixedTime;
                Vector3 startPosition = _titan.Cache.Transform.position;
                float[] mins = new float[] { 1000f, 1000f, 1000f };
                float[] maxes = new float[] { -1000f, -1000f, -1000f };
                _titan.Attack(attackName);
                int currFrame = -100;
                while (_titan.State == TitanState.Attack)
                {
                    float currTime = Time.fixedTime;
                    int frame = (int)((currTime - startTime) * 50f);
                    if (frame > currFrame)
                    {
                        currFrame = frame;
                        if (_titan.BaseTitanCache.HandLHitbox.IsActive())
                            frames.Add(DebugCreateFrameJSON(frame, (_titan.BaseTitanCache.HandLHitbox), mins, maxes, _titan.BaseTitanCache.HandLHitbox.transform.position - startPosition));
                        if (_titan.BaseTitanCache.HandRHitbox.IsActive())
                            frames.Add(DebugCreateFrameJSON(frame, (_titan.BaseTitanCache.HandRHitbox), mins, maxes, _titan.BaseTitanCache.HandRHitbox.transform.position - startPosition));
                        if (_titan.BaseTitanCache.FootLHitbox.IsActive())
                            frames.Add(DebugCreateFrameJSON(frame, (_titan.BaseTitanCache.FootLHitbox), mins, maxes, _titan.BaseTitanCache.FootLHitbox.transform.position - startPosition));
                        if (_titan.BaseTitanCache.FootRHitbox.IsActive())
                            frames.Add(DebugCreateFrameJSON(frame, (_titan.BaseTitanCache.FootRHitbox), mins, maxes, _titan.BaseTitanCache.FootRHitbox.transform.position - startPosition));
                        if (_titan.BaseTitanCache.MouthHitbox != null && _titan.BaseTitanCache.MouthHitbox.IsActive())
                            frames.Add(DebugCreateFrameJSON(frame, (_titan.BaseTitanCache.MouthHitbox), mins, maxes, _titan.BaseTitanCache.MouthHitbox.transform.position - startPosition));
                    }
                    yield return null;
                }
                total += frames.Count;
                var ranges = new JSONObject();
                var x = new JSONArray();
                x.Add(mins[0]);
                x.Add(maxes[0]);
                var y = new JSONArray();
                y.Add(mins[1]);
                y.Add(maxes[1]);
                var z = new JSONArray();
                z.Add(mins[2]);
                z.Add(maxes[2]);
                ranges["X"] = x;
                ranges["Y"] = y;
                ranges["Z"] = z;
                newInfo["Ranges"] = ranges;
                newInfo["Keyframes"] = frames;
                newData[attackName] = newInfo;
                yield return new WaitForSeconds(0.5f);
                _titan.Cache.Transform.position = startPosition;
                _titan.Cache.Transform.rotation = Quaternion.identity;
                yield return new WaitForSeconds(0.5f);
            }
            string result = newData.ToString(4);
            File.WriteAllText(FolderPaths.Documents + "/Keyframes.txt", result);
            Debug.Log(total);
        }

        private JSONObject DebugCreateFrameJSON(int frame, BaseHitbox hitbox, float[] mins, float[] maxes, Vector3 position)
        {
            var collider = (SphereCollider)hitbox._collider;
            float radius = collider.radius * collider.transform.lossyScale.x;
            var node = new JSONObject();
            node["f"] = frame;
            node["r"] = radius;
            node["x"] = position.x;
            node["y"] = position.y;
            node["z"] = position.z;
            mins[0] = Mathf.Min(mins[0], position.x - radius);
            mins[1] = Mathf.Min(mins[1], position.y - radius);
            mins[2] = Mathf.Min(mins[2], position.z - radius);
            maxes[0] = Mathf.Max(maxes[0], position.x + radius);
            maxes[1] = Mathf.Max(maxes[1], position.y + radius);
            maxes[2] = Mathf.Max(maxes[2], position.z + radius);
            return node;
        }
    }
}
