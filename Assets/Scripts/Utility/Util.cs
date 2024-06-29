using Settings;
using SimpleJSONFixed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Characters;
using System.Text.RegularExpressions;
using Photon.Pun;
using Photon.Realtime;
using System.Globalization;
using UnityEngine.AI;
using System.Collections.Specialized;

namespace Utility
{
    static class Util
    {
        public static float SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
        {
            float unsignedAngle = Vector3.Angle(from, to);

            float cross_x = from.y * to.z - from.z * to.y;
            float cross_y = from.z * to.x - from.x * to.z;
            float cross_z = from.x * to.y - from.y * to.x;
            float sign = Mathf.Sign(axis.x * cross_x + axis.y * cross_y + axis.z * cross_z);
            return unsignedAngle * sign;
        }

        public static bool IsVectorBetween(Vector3 v, Vector3 start, Vector3 end)
        {
            bool x = (v.x >= start.x && v.x <= end.x) || (v.x >= end.x && v.x <= start.x);
            bool y = (v.y >= start.y && v.y <= end.y) || (v.y >= end.y && v.y <= start.y);
            bool z = (v.z >= start.z && v.z <= end.z) || (v.z >= end.z && v.z <= start.z);
            return x && y && z;
        }

        public static float LinearMap(float x, float inMin, float inMax, float outMin, float outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        public static BaseCharacter FindCharacterByViewId(int viewId)
        {
            if (viewId < 0)
                return null;
            var view = PhotonView.Find(viewId);
            if (view == null)
                return null;
            var character = view.GetComponent<BaseCharacter>();
            return character;
        }

        public static Player FindPlayerById(int id)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.ActorNumber == id)
                    return player;
            }
            return null;
        }

        public static PhotonMessageInfo CreateLocalPhotonInfo()
        {
            var info = new PhotonMessageInfo(null, 0, null);
            return info;
        }

        public static string PascalToSentence(string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
        }

        public static T CreateDontDestroyObj<T>() where T : Component
        {
            GameObject obj = new GameObject();
            obj.name = "Dont Destroy";
            UnityEngine.Object.DontDestroyOnLoad(obj);
            return obj.AddComponent<T>();
        }

        public static T CreateObj<T>() where T : Component
        {
            GameObject obj = new GameObject();
            return obj.AddComponent<T>();
        }

        public static HashSet<T> RemoveNull<T>(HashSet<T> set) where T: UnityEngine.Object
        {
            HashSet<T> newSet = new HashSet<T>();
            foreach (T item in set)
            {
                if (item != null)
                    newSet.Add(item);
            }
            return newSet;
        }

        public static HashSet<T> RemoveNullOrDead<T>(HashSet<T> set) where T : BaseCharacter
        {
            HashSet<T> newSet = new HashSet<T>();
            foreach (T item in set)
            {
                if (item != null && !item.Dead)
                    newSet.Add(item);
            }
            return newSet;
        }

        public static HashSet<BaseShifter> RemoveNullOrDeadShifters(HashSet<BaseShifter> set)
        {
            HashSet<BaseShifter> newSet = new HashSet<BaseShifter>();
            foreach (BaseShifter item in set)
            {
                if (item != null && (!item.Dead || item.TransformingToHuman))
                    newSet.Add(item);
            }
            return newSet;
        }

        public static string CreateMD5(string input)
        {
            if (input == string.Empty)
                return string.Empty;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static IEnumerator WaitForFrames(int frames)
        {
            for (int i = 0; i < frames; i++)
                yield return new WaitForEndOfFrame();
        }

        public static string[] EnumToStringArray<T>()
        {
            return Enum.GetNames(typeof(T));
        }

        public static string[] EnumToStringArrayExceptNone<T>()
        {
            List<string> names = new List<string>();
            foreach (string str in EnumToStringArray<T>())
            {
                if (str != "None")
                    names.Add(str);
            }
            return names.ToArray();
        }

        public static List<T> EnumToList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static Dictionary<string, T> EnumToDict<T>()
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            foreach (T t in EnumToList<T>())
            {
                dict.Add(t.ToString(), t);
            }
            return dict;
        }

        public static string FormatFloat(float num, int decimalPlaces)
        {
            if (decimalPlaces == 0)
                return num.ToString("0");
            return num.ToString("0." + new string('0', decimalPlaces));
        }

        public static Vector3 MultiplyVectors(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3 DivideVectors(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static List<List<T>> GroupItems<T>(List<T> items, int groupSize)
        {
            var list = new List<List<T>>();
            if (items.Count == 0)
                return list;
            var group = new List<T>();
            list.Add(group);
            int current = 0;
            foreach (var obj in items)
            {
                current++;
                if (current >= groupSize + 1)
                {
                    current = 1;
                    group = new List<T>();
                    list.Add(group);
                }
                group.Add(obj);
            }
            return list;
        }

        public static List<List<T>> GroupBuckets<T>(List<T> items, int buckets)
        {
            var list = new List<List<T>>();
            for (int i = 0; i < buckets; i++)
                list.Add(new List<T>());
            if (items.Count == 0 || buckets == 0)
                return list;
            int itemsPerBucket = items.Count / buckets;
            int currentBucket = 0;
            foreach (T item in items)
            {
                var bucket = list[currentBucket];
                bucket.Add(item);
                if (bucket.Count >= itemsPerBucket && currentBucket < buckets - 1)
                    currentBucket++;
            }
            return list;
        }

        public static object GetRandomFromWeightedList(List<object> values, List<float> weights)
        {
            float totalWeight = 0f;
            foreach (float w in weights)
                totalWeight += w;
            float r = UnityEngine.Random.Range(0f, totalWeight);
            float start = 0f;
            for (int i = 0; i < values.Count; i++)
            {
                if (r >= start && r < start + weights[i])
                    return values[i];
                start += weights[i];
            }
            return values[0];
        }

        public static object GetRandomFromWeightedNode(JSONNode node)
        {
            List<object> values = new List<object>();
            List<float> weights = new List<float>();
            foreach (JSONNode key in node.Keys)
            {
                values.Add(key.Value);
                weights.Add(node[key.Value].AsFloat);
            }
            return GetRandomFromWeightedList(values, weights);
        }

        public static float DistanceIgnoreY(Vector3 a, Vector3 b)
        {
            a = new Vector3(a.x, 0f, a.z);
            b = new Vector3(b.x, 0f, b.z);
            return Vector3.Distance(a, b);
        }

        public static List<TValue> PaginateDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict, int pageNumber, int elementsPerPage)
        {
            // Sort the dictionary by key
            var sortedCommands = dict.OrderBy(c => c.Key).ToList();

            var totalPages = (int)Math.Ceiling((double)sortedCommands.Count / elementsPerPage);

            // Calculate the start index
            var startIndex = (pageNumber - 1) * elementsPerPage;

            // Get the paginated commands
            var paginatedCommands = sortedCommands.Skip(startIndex).Take(elementsPerPage).Select(c => c.Value).ToList();

            return paginatedCommands;
        }

        public static string ColorText(string text, string color)
        {
            return $"<color={color}>{text}</color>";
        }

        public static string SizeText(string text, int size)
        {
            return $"<size={size}>{text}</size>";
        }

        public static string RichTextTag(string text, string tag, string value)
        {
            return $"<{tag}={value}>{text}</{tag}>";
        }
        
        public static Quaternion ConstrainedToX(Quaternion rotation) =>
            Quaternion.Euler(rotation.eulerAngles.x, 0f,  0f);
        
        public static Quaternion ConstrainedToY(Quaternion rotation) =>
            Quaternion.Euler(0f, rotation.eulerAngles.y,  0f);
        
        public static Quaternion ConstrainedToZ(Quaternion rotation) =>
            Quaternion.Euler(0f, 0f,  rotation.eulerAngles.z);

        public static List<KeyValuePair<float, string>> _titanSizes = new List<KeyValuePair<float, string>>()
        {
            //new KeyValuePair<float, string>(0.5f, "minTitan"),
            new KeyValuePair<float, string>(1f, "smallTitan"),
            new KeyValuePair<float, string>(2f, "avgTitan"),
            new KeyValuePair<float, string>(3f, "maxTitan")
        };
        
        public static List<int> GetAllTitanAgentIds()
        {
            // for each _titanSize in _titanSizes, return GetNavMeshAgentID(_titanSize.Value), if its null, remove
            return _titanSizes.Select(titanSize => GetNavMeshAgentID(titanSize.Value)).Where(agentId => agentId != null).Select(agentId => agentId.Value).ToList();
        }

        public static int GetNavMeshAgentIDBySize(float size)
        {
            // determine the size to use based on if the size is greater than the current size but less than the next
            string name = "minTitan";
            for (int i = 0; i < _titanSizes.Count; i++)
            {
                if (size >= _titanSizes[i].Key)
                    name = _titanSizes[i].Value;
            }

            return GetNavMeshAgentID(name) ?? 0;
        }

        public static NavMeshBuildSettings GetAgentSettingsCorrected(float size)
        {
            string name = "minTitan";
            float sizeToUse = 0.5f;
            for (int i = 0; i < _titanSizes.Count; i++)
            {
                if (size >= _titanSizes[i].Key)
                {
                    sizeToUse = _titanSizes[i].Key;
                    name = _titanSizes[i].Value;
                }
                    
            }

            int agentId = GetNavMeshAgentID(name).Value;

            // log titan size
            var agentSettings = NavMesh.GetSettingsByID(agentId);
            agentSettings.agentRadius /= sizeToUse;
            agentSettings.agentHeight /= sizeToUse;
            return agentSettings;
        }

        public static int? GetNavMeshAgentID(string name)
        {
            for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
            {
                NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(index: i);
                if (name == NavMesh.GetSettingsNameFromID(agentTypeID: settings.agentTypeID))
                {
                    return settings.agentTypeID;
                }
            }
            return null;
        }

        public static Vector3 Abs(Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static T EnumMax<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Max();
        }

        public static int EnumMaxValue<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<int>().Max();
        }
    }
}