using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Photon.Pun;

namespace Assets.Scripts.ApplicationManagers
{
    /// <summary>
    /// Tracks RPC calls and serialization per PhotonView, recording counts, bandwidth, and rates.
    /// Subscribes to PhotonNetwork callbacks.
    /// </summary>
    static class RPCProfiler
    {
        public static bool IsTracking;
        public static float SessionStartTime;
        public static float SessionDuration => IsTracking ? Time.unscaledTime - SessionStartTime : _pausedDuration;
        static float _pausedDuration;

        // viewId -> (rpcName -> RpcStats)
        static readonly Dictionary<int, Dictionary<string, RpcStats>> _sentStats = new Dictionary<int, Dictionary<string, RpcStats>>();
        static readonly Dictionary<int, Dictionary<string, RpcStats>> _recvStats = new Dictionary<int, Dictionary<string, RpcStats>>();

        // Serialize tracking: viewId -> SerializeStats
        static readonly Dictionary<int, SerializeStats> _serializeWriteStats = new Dictionary<int, SerializeStats>();
        static readonly Dictionary<int, SerializeStats> _serializeReadStats = new Dictionary<int, SerializeStats>();

        // Cached view info
        static readonly Dictionary<int, string> _viewNames = new Dictionary<int, string>();

        public static void Init()
        {
            PhotonNetwork.OnRpcSent += RecordSent;
            PhotonNetwork.OnRpcReceived += RecordReceived;
            PhotonNetwork.OnSerializeWriteCallback += RecordSerializeWrite;
            PhotonNetwork.OnSerializeReadCallback += RecordSerializeRead;
        }

        public static void StartTracking()
        {
            IsTracking = true;
            SessionStartTime = Time.unscaledTime;
        }

        public static void PauseTracking()
        {
            _pausedDuration = Time.unscaledTime - SessionStartTime;
            IsTracking = false;
        }

        public static void ResumeTracking()
        {
            SessionStartTime = Time.unscaledTime - _pausedDuration;
            IsTracking = true;
        }

        static void RecordSent(int viewId, string rpcName, object[] parameters)
        {
            if (!IsTracking) return;
            int bytes = EstimateRpcBytes(rpcName, parameters);
            GetOrCreateStats(_sentStats, viewId, rpcName).Record(bytes);
            CacheViewName(viewId);
        }

        static void RecordReceived(int viewId, string rpcName, object[] parameters)
        {
            if (!IsTracking) return;
            int bytes = EstimateRpcBytes(rpcName, parameters);
            GetOrCreateStats(_recvStats, viewId, rpcName).Record(bytes);
            CacheViewName(viewId);
        }

        static void RecordSerializeWrite(int viewId, int dataCount)
        {
            if (!IsTracking) return;
            int bytes = EstimateSerializeBytes(dataCount);
            if (!_serializeWriteStats.TryGetValue(viewId, out var stats))
            {
                stats = new SerializeStats();
                _serializeWriteStats[viewId] = stats;
            }
            stats.Record(bytes);
            CacheViewName(viewId);
        }

        static void RecordSerializeRead(int viewId, int dataCount)
        {
            if (!IsTracking) return;
            int bytes = EstimateSerializeBytes(dataCount);
            if (!_serializeReadStats.TryGetValue(viewId, out var stats))
            {
                stats = new SerializeStats();
                _serializeReadStats[viewId] = stats;
            }
            stats.Record(bytes);
            CacheViewName(viewId);
        }

        static RpcStats GetOrCreateStats(Dictionary<int, Dictionary<string, RpcStats>> dict, int viewId, string rpcName)
        {
            if (!dict.TryGetValue(viewId, out var rpcMap))
            {
                rpcMap = new Dictionary<string, RpcStats>();
                dict[viewId] = rpcMap;
            }
            if (!rpcMap.TryGetValue(rpcName, out var stats))
            {
                stats = new RpcStats();
                rpcMap[rpcName] = stats;
            }
            return stats;
        }

        static int EstimateRpcBytes(string methodName, object[] parameters)
        {
            // PUN RPC wire format (Hashtable inside Photon event):
            // Event envelope: ~8 bytes (event code, sender, etc.)
            // Hashtable framing: ~4 bytes
            // ViewID entry: key(1) + type(1) + int(4) = 6
            // Timestamp entry: key(1) + type(1) + int(4) = 6
            // Method entry: key(1) + type(1) + shortcut byte(1) = 3 (if in RpcList, else 2 + string length)
            int bytes = 24; // envelope + framing + viewId + timestamp

            // Check if method uses a byte shortcut (registered in PhotonServerSettings.RpcList)
            var rpcList = PhotonNetwork.PhotonServerSettings.RpcList;
            bool hasShortcut = methodName != null && rpcList != null && rpcList.Contains(methodName);
            if (hasShortcut)
                bytes += 3; // key + type + 1 byte index
            else
                bytes += methodName != null ? 2 + methodName.Length * 2 : 0; // key + type + UTF-16 string

            if (parameters != null && parameters.Length > 0)
            {
                bytes += 4; // key + type + array header
                for (int i = 0; i < parameters.Length; i++)
                    bytes += 1 + EstimateObjectSize(parameters[i]); // type tag per element + value
            }
            return bytes;
        }

        static int EstimateSerializeBytes(int dataCount)
        {
            // Header: viewId(4) + compressed(1) + nullValues(4) = 9 bytes
            // Average ~8 bytes per data value (mix of floats, ints, vectors, quaternions)
            return 9 + dataCount * 8;
        }

        static int EstimateObjectSize(object obj)
        {
            if (obj == null) return 1;
            if (obj is bool) return 1;
            if (obj is byte) return 1;
            if (obj is short) return 2;
            if (obj is int) return 4;
            if (obj is long) return 8;
            if (obj is float) return 4;
            if (obj is double) return 8;
            if (obj is string s) return 2 + s.Length * 2;
            if (obj is byte[] ba) return 4 + ba.Length;
            if (obj is int[] ia) return 4 + ia.Length * 4;
            if (obj is float[] fa) return 4 + fa.Length * 4;
            if (obj is Vector3) return 12;
            if (obj is Vector2) return 8;
            if (obj is Quaternion) return 16;
            if (obj is object[] oa)
            {
                int total = 4;
                for (int i = 0; i < oa.Length; i++)
                    total += EstimateObjectSize(oa[i]);
                return total;
            }
            return 8; // default estimate
        }

        static void CacheViewName(int viewId)
        {
            if (_viewNames.ContainsKey(viewId)) return;
            var pv = PhotonView.Find(viewId);
            if (pv != null)
                _viewNames[viewId] = pv.gameObject.name;
            else
                _viewNames[viewId] = "Unknown";
        }

        public static string GetViewName(int viewId)
        {
            if (_viewNames.TryGetValue(viewId, out var name)) return name;
            CacheViewName(viewId);
            return _viewNames.TryGetValue(viewId, out name) ? name : "Unknown";
        }

        public static HashSet<int> GetAllViewIds()
        {
            var ids = new HashSet<int>();
            foreach (var k in _sentStats.Keys) ids.Add(k);
            foreach (var k in _recvStats.Keys) ids.Add(k);
            foreach (var k in _serializeWriteStats.Keys) ids.Add(k);
            foreach (var k in _serializeReadStats.Keys) ids.Add(k);
            return ids;
        }

        public static Dictionary<int, Dictionary<string, RpcStats>> GetSentStats() => _sentStats;
        public static Dictionary<int, Dictionary<string, RpcStats>> GetRecvStats() => _recvStats;
        public static Dictionary<int, SerializeStats> GetSerializeWriteStats() => _serializeWriteStats;
        public static Dictionary<int, SerializeStats> GetSerializeReadStats() => _serializeReadStats;

        public static void GetViewRpcTotals(int viewId, out int sentCount, out long sentBytes, out int recvCount, out long recvBytes)
        {
            sentCount = 0; sentBytes = 0; recvCount = 0; recvBytes = 0;
            if (_sentStats.TryGetValue(viewId, out var sm))
                foreach (var s in sm.Values) { sentCount += s.Count; sentBytes += s.TotalBytes; }
            if (_recvStats.TryGetValue(viewId, out var rm))
                foreach (var s in rm.Values) { recvCount += s.Count; recvBytes += s.TotalBytes; }
        }

        public static void Clear()
        {
            _sentStats.Clear();
            _recvStats.Clear();
            _serializeWriteStats.Clear();
            _serializeReadStats.Clear();
            _viewNames.Clear();
            SessionStartTime = Time.unscaledTime;
            _pausedDuration = 0f;
        }

        public static List<RPCSummaryEntry> GetSummary(bool sortByRate)
        {
            float duration = SessionDuration;
            if (duration <= 0f) duration = 1f;
            var summary = new Dictionary<string, RPCSummaryEntry>();

            foreach (var viewKvp in _sentStats)
            {
                foreach (var rpcKvp in viewKvp.Value)
                {
                    if (!summary.TryGetValue(rpcKvp.Key, out var entry))
                    {
                        entry = new RPCSummaryEntry { RpcName = rpcKvp.Key };
                        summary[rpcKvp.Key] = entry;
                    }
                    entry.TotalSentCount += rpcKvp.Value.Count;
                    entry.TotalSentBytes += rpcKvp.Value.TotalBytes;
                }
            }

            foreach (var viewKvp in _recvStats)
            {
                foreach (var rpcKvp in viewKvp.Value)
                {
                    if (!summary.TryGetValue(rpcKvp.Key, out var entry))
                    {
                        entry = new RPCSummaryEntry { RpcName = rpcKvp.Key };
                        summary[rpcKvp.Key] = entry;
                    }
                    entry.TotalRecvCount += rpcKvp.Value.Count;
                    entry.TotalRecvBytes += rpcKvp.Value.TotalBytes;
                }
            }

            foreach (var entry in summary.Values)
            {
                entry.SentRate = entry.TotalSentCount / duration;
                entry.RecvRate = entry.TotalRecvCount / duration;
                entry.SentBytesRate = entry.TotalSentBytes / duration;
                entry.RecvBytesRate = entry.TotalRecvBytes / duration;
            }

            var result = summary.Values.ToList();
            if (sortByRate)
                result.Sort((a, b) => (b.SentRate + b.RecvRate).CompareTo(a.SentRate + a.RecvRate));
            else
                result.Sort((a, b) => (b.TotalSentCount + b.TotalRecvCount).CompareTo(a.TotalSentCount + a.TotalRecvCount));
            return result;
        }

        public static string ExportCsv()
        {
            float duration = SessionDuration;
            if (duration <= 0f) duration = 1f;
            var sb = new StringBuilder();
            sb.AppendLine("ViewID,ViewName,Type,RPC,Count,Bytes,Count/s,Bytes/s");

            var viewIds = GetAllViewIds().ToList();
            viewIds.Sort();

            foreach (int viewId in viewIds)
            {
                string viewName = GetViewName(viewId).Replace(",", ";");

                if (_sentStats.TryGetValue(viewId, out var sm))
                {
                    foreach (var rpcKvp in sm.OrderBy(k => k.Key))
                    {
                        var s = rpcKvp.Value;
                        sb.AppendLine(viewId + "," + viewName + ",RPC Sent," + rpcKvp.Key + "," +
                            s.Count + "," + s.TotalBytes + "," +
                            (s.Count / duration).ToString("F2") + "," + (s.TotalBytes / duration).ToString("F2"));
                    }
                }

                if (_recvStats.TryGetValue(viewId, out var rm))
                {
                    foreach (var rpcKvp in rm.OrderBy(k => k.Key))
                    {
                        var s = rpcKvp.Value;
                        sb.AppendLine(viewId + "," + viewName + ",RPC Recv," + rpcKvp.Key + "," +
                            s.Count + "," + s.TotalBytes + "," +
                            (s.Count / duration).ToString("F2") + "," + (s.TotalBytes / duration).ToString("F2"));
                    }
                }

                if (_serializeWriteStats.TryGetValue(viewId, out var sw))
                {
                    sb.AppendLine(viewId + "," + viewName + ",Serialize Write,—," +
                        sw.Count + "," + sw.TotalBytes + "," +
                        (sw.Count / duration).ToString("F2") + "," + (sw.TotalBytes / duration).ToString("F2"));
                }

                if (_serializeReadStats.TryGetValue(viewId, out var sr))
                {
                    sb.AppendLine(viewId + "," + viewName + ",Serialize Read,—," +
                        sr.Count + "," + sr.TotalBytes + "," +
                        (sr.Count / duration).ToString("F2") + "," + (sr.TotalBytes / duration).ToString("F2"));
                }
            }

            // Summary section
            sb.AppendLine();
            sb.AppendLine("--- Summary (Session: " + duration.ToString("F1") + "s) ---");
            sb.AppendLine("RPC,SentCount,RecvCount,SentBytes,RecvBytes,Sent/s,Recv/s,SentB/s,RecvB/s");
            var summary = GetSummary(false);
            foreach (var entry in summary)
            {
                sb.AppendLine(entry.RpcName + "," + entry.TotalSentCount + "," + entry.TotalRecvCount + "," +
                    entry.TotalSentBytes + "," + entry.TotalRecvBytes + "," +
                    entry.SentRate.ToString("F2") + "," + entry.RecvRate.ToString("F2") + "," +
                    entry.SentBytesRate.ToString("F2") + "," + entry.RecvBytesRate.ToString("F2"));
            }

            return sb.ToString();
        }

        public static string FormatBytes(long bytes)
        {
            if (bytes < 1024) return bytes + " B";
            if (bytes < 1024 * 1024) return (bytes / 1024f).ToString("F1") + " KB";
            return (bytes / (1024f * 1024f)).ToString("F2") + " MB";
        }

        public static string FormatBytesRate(float bytesPerSec)
        {
            if (bytesPerSec < 1024f) return bytesPerSec.ToString("F0") + " B/s";
            if (bytesPerSec < 1024f * 1024f) return (bytesPerSec / 1024f).ToString("F1") + " KB/s";
            return (bytesPerSec / (1024f * 1024f)).ToString("F2") + " MB/s";
        }

        public class RpcStats
        {
            public int Count;
            public long TotalBytes;

            public void Record(int bytes)
            {
                Count++;
                TotalBytes += bytes;
            }
        }

        public class SerializeStats
        {
            public int Count;
            public long TotalBytes;

            public void Record(int bytes)
            {
                Count++;
                TotalBytes += bytes;
            }
        }

        public class RPCSummaryEntry
        {
            public string RpcName;
            public int TotalSentCount;
            public int TotalRecvCount;
            public long TotalSentBytes;
            public long TotalRecvBytes;
            public float SentRate;
            public float RecvRate;
            public float SentBytesRate;
            public float RecvBytesRate;
        }
    }
}
