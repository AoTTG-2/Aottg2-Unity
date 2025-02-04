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
using System.IO;

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
            set.RemoveWhere(e => !e);
            return set;
        }

        public static HashSet<T> RemoveNullOrDead<T>(HashSet<T> set) where T : BaseCharacter
        {
            set.RemoveWhere(e => !e || e.Dead);
            return set;
        }

        public static HashSet<T> RemoveNullOrDeadDetections<T>(HashSet<T> set) where T : BaseDetection
        {
            set.RemoveWhere(e => e.IsNullOrDead());
            return set;
        }

        public static HashSet<BaseShifter> RemoveNullOrDeadShifters(HashSet<BaseShifter> set)
        {
            set.RemoveWhere(e => !e || (e.Dead && !e.TransformingToHuman));
            return set;
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
            float x = a.x - b.x;
            float z = a.z - b.z;
            return Mathf.Sqrt(x * x + z * z);
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
            // determine the size to use based on if the size is greater than the current size but less than the next
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

        public static bool IsValidFileName(string fileName)
        {
            // filter out invalid file characters based on windows AND ., /, and \\ characters don't use regex
            char[] invalidCharacters = new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*', '.', '\b', '\0', '\t', '\r', '\n', '^', '!', '@', '#', '$', '%', '&', '(', ')', '=', '+' };
            // CON, PRN, AUX, NUL, COM1, COM2, COM3, COM4, COM5, COM6, COM7, COM8, COM9, LPT1, LPT2, LPT3, LPT4, LPT5, LPT6, LPT7, LPT8, and LPT9
            string[] invalidFileNames = new string[] { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
            return fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) == -1
                && fileName.IndexOfAny(invalidCharacters) == -1
                && !invalidFileNames.Contains(fileName)
                && fileName.Trim() == fileName
                && fileName.Length < 50;
        }

        public static double GetPhotonTimestampDifference(double sentTime, double serverTime)
        {
            if (serverTime >= sentTime)
            {
                return serverTime - sentTime;
            }

            // Handle wrap-around
            return (4294967.295 - sentTime) + serverTime;
        }

        public static (bool success, string message) SaveChatHistory(IEnumerable<string> messages)
        {
            try
            {
                DateTime timestamp = DateTime.UtcNow;
                string baseFilename = $"chat_history_{timestamp:yyyy-MM-dd_HH-mm-ss}";
                
                if (!IsValidFileName(baseFilename))
                    return (false, "Invalid filename generated.");
                    
                string filename = baseFilename + ".txt";
                string downloadsPath = GetDownloadsPath();
                if (downloadsPath == null)
                    return (false, "Could not locate Downloads folder.");
                    
                string filePath = Path.Combine(downloadsPath, filename);
                // Ensure the final path is still within Downloads directory
                if (!IsPathInDirectory(filePath, downloadsPath))
                    return (false, "Invalid file path.");
                
                // Collect chat content
                string chatContent = string.Join("\n", messages);
                
                // Calculate hash of content
                string hash;
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(chatContent);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
                
                // Create file content with hash header
                string fileContent = $"[HASH:{hash}]\n[TIME:{timestamp:yyyy-MM-dd HH:mm:ss UTC}]\n\n{chatContent}";
                
                // Write file
                File.WriteAllText(filePath, fileContent);
                
                // Set file as read-only
                File.SetAttributes(filePath, FileAttributes.ReadOnly);
                
                return (true, $"Chat history saved to Downloads/{filename}");
            }
            catch (Exception ex)
            {
                return (false, $"Failed to save chat history: {ex.Message}");
            }
        }

        public static (bool success, string message) VerifyChatHistory(string filename)
        {
            if (!IsValidFileName(Path.GetFileNameWithoutExtension(filename)))
                return (false, "Invalid filename.");
                
            try
            {
                string downloadsPath = GetDownloadsPath();
                if (downloadsPath == null)
                    return (false, "Could not locate Downloads folder.");
                    
                string filePath = Path.Combine(downloadsPath, filename);
                // Ensure the final path is still within Downloads directory
                if (!IsPathInDirectory(filePath, downloadsPath))
                    return (false, "Invalid file path.");
                    
                if (!File.Exists(filePath))
                    return (false, $"File not found: {filename}");

                // Read file content
                string[] lines = File.ReadAllLines(filePath);
                
                // File must have at least 4 lines (hash, time, blank line, and content)
                if (lines.Length < 4)
                    return (false, "Invalid file format.");

                // Extract stored hash
                if (!lines[0].StartsWith("[HASH:") || !lines[0].EndsWith("]"))
                    return (false, "Invalid file format: missing hash header.");

                string storedHash = lines[0].Substring(6, lines[0].Length - 7);

                // Get content (everything after the blank line)
                string content = string.Join("\n", lines.Skip(3));

                // Calculate hash of current content
                string currentHash;
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(content);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    currentHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }

                return currentHash == storedHash
                    ? (true, "File verification successful - content has not been modified.")
                    : (false, "Warning: File has been modified!");
            }
            catch (Exception ex)
            {
                return (false, $"Failed to verify file: {ex.Message}");
            }
        }

        private static string GetDownloadsPath()
        {
            try
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads"
                );
            }
            catch
            {
                return null;
            }
        }

        private static bool IsPathInDirectory(string path, string directory)
        {
            // Convert both to full paths
            string fullPath = Path.GetFullPath(path);
            string fullDir = Path.GetFullPath(directory);
            
            // Check if the full path starts with the full directory path
            return fullPath.StartsWith(fullDir, StringComparison.OrdinalIgnoreCase) &&
                   // Ensure there's either a directory separator or end of string after the directory path
                   (fullPath.Length == fullDir.Length || 
                    fullPath[fullDir.Length] == Path.DirectorySeparatorChar ||
                    fullPath[fullDir.Length] == Path.AltDirectorySeparatorChar);
        }
    }
}