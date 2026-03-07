using Assets.Scripts.ApplicationManagers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Utility;

namespace ApplicationManagers
{
    /// <summary>
    /// GUI panel for viewing RPC and serialization profiling data per PhotonView.
    /// Activated by pressing F9.
    /// Shows per-view RPC/serialize breakdown and a summary tab sorted by usage.
    /// Data can be exported to CSV.
    /// </summary>
    class DebugRPCPanel : MonoBehaviour
    {
        static DebugRPCPanel _instance;
        public static bool Enabled;

        // Window
        static float _windowX = 640;
        static float _windowY = 20;
        static float _windowWidth = 900;
        static float _windowHeight = 500;

        // Dragging/resizing
        static bool _isDragging;
        static bool _isResizing;
        static Vector2 _dragOffset;
        static Vector2 _resizeStartSize;
        static Vector2 _resizeStartMousePos;

        // Tabs
        enum RPCTab { PerView, Summary }
        static RPCTab _currentTab = RPCTab.PerView;

        // View
        static bool _showRate;
        static Vector2 _scrollPosition;
        static string _statusMessage = "";
        static float _statusMessageTime;

        // Expanded views in per-view tab
        static HashSet<int> _expandedViews = new HashSet<int>();

        const int RowHeight = 20;
        const int Padding = 10;
        const int TabHeight = 25;
        const int ToolbarHeight = 25;
        const int ResizeHandleSize = 15;
        const int MinWidth = 700;
        const int MinHeight = 350;

        // Column positions (relative to content area)
        const int ColViewId = 0;
        const int ColViewIdW = 55;
        const int ColName = 55;
        const int ColNameW = 155;
        const int ColRpc = 210;
        const int ColRpcW = 160;
        const int ColSentCnt = 370;
        const int ColSentCntW = 70;
        const int ColRecvCnt = 440;
        const int ColRecvCntW = 70;
        const int ColSentBw = 510;
        const int ColSentBwW = 90;
        const int ColRecvBw = 600;
        const int ColRecvBwW = 90;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            RPCProfiler.Init();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                Enabled = !Enabled;
                if (Enabled)
                    RPCProfiler.StartTracking();
                else
                    RPCProfiler.PauseTracking();
            }
        }

        void OnGUI()
        {
            if (!Enabled) return;

            GUI.depth = 1;
            HandleWindowInteraction();

            Color oldColor = GUI.color;
            GUI.color = new Color(0.15f, 0.15f, 0.15f, 1f);
            GUI.DrawTexture(new Rect(_windowX, _windowY, _windowWidth, _windowHeight), Texture2D.whiteTexture);
            GUI.color = oldColor;

            GUI.Box(new Rect(_windowX, _windowY, _windowWidth, _windowHeight), "");

            DrawToolbar();
            DrawTabs();
            DrawContent();
            DrawResizeHandle();

            GUI.depth = 0;
        }

        static void HandleWindowInteraction()
        {
            Event e = Event.current;
            Vector2 mousePos = e.mousePosition;

            Rect resizeRect = new Rect(_windowX + _windowWidth - ResizeHandleSize,
                                       _windowY + _windowHeight - ResizeHandleSize,
                                       ResizeHandleSize, ResizeHandleSize);
            Rect titleRect = new Rect(_windowX, _windowY, _windowWidth, ToolbarHeight + Padding * 3 + TabHeight);
            Rect buttonRect = new Rect(_windowX + _windowWidth - 470, _windowY + Padding, 460, ToolbarHeight);
            Rect tabRect = new Rect(_windowX + Padding, _windowY + Padding * 2 + ToolbarHeight, _windowWidth - Padding * 2, TabHeight);

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (resizeRect.Contains(mousePos))
                {
                    _isResizing = true;
                    _resizeStartSize = new Vector2(_windowWidth, _windowHeight);
                    _resizeStartMousePos = mousePos;
                    e.Use();
                }
                else if (titleRect.Contains(mousePos) && !buttonRect.Contains(mousePos) && !tabRect.Contains(mousePos))
                {
                    _isDragging = true;
                    _dragOffset = new Vector2(mousePos.x - _windowX, mousePos.y - _windowY);
                    e.Use();
                }
            }
            else if (e.type == EventType.MouseUp && e.button == 0)
            {
                _isDragging = false;
                _isResizing = false;
            }
            else if (e.type == EventType.MouseDrag)
            {
                if (_isDragging)
                {
                    _windowX = mousePos.x - _dragOffset.x;
                    _windowY = mousePos.y - _dragOffset.y;
                    e.Use();
                }
                else if (_isResizing)
                {
                    Vector2 delta = mousePos - _resizeStartMousePos;
                    _windowWidth = Mathf.Max(MinWidth, _resizeStartSize.x + delta.x);
                    _windowHeight = Mathf.Max(MinHeight, _resizeStartSize.y + delta.y);
                    e.Use();
                }
            }
        }

        static void DrawToolbar()
        {
            int x = (int)_windowX + Padding;
            int y = (int)_windowY + Padding;

            float dur = RPCProfiler.SessionDuration;
            string durStr = dur >= 60f ? ((int)(dur / 60f)) + "m " + ((int)(dur % 60f)) + "s" : dur.ToString("F1") + "s";
            GUI.Label(new Rect(x, y, 280, ToolbarHeight), "RPC Profiler (F9)  Session: " + durStr);

            int bw = 80;
            int bs = 5;
            int bx = (int)(_windowX + _windowWidth) - Padding - (bw * 4 + bs * 3);

            string rateLabel = _showRate ? "Count" : "Rate/s";
            if (GUI.Button(new Rect(bx, y, bw, ToolbarHeight), rateLabel))
                _showRate = !_showRate;
            bx += bw + bs;

            if (GUI.Button(new Rect(bx, y, bw, ToolbarHeight), "Clear"))
            {
                RPCProfiler.Clear();
                _expandedViews.Clear();
            }
            bx += bw + bs;

            if (GUI.Button(new Rect(bx, y, bw, ToolbarHeight), "Export CSV"))
                ExportCsv();
            bx += bw + bs;

            string trackLabel = RPCProfiler.IsTracking ? "Pause" : "Resume";
            if (GUI.Button(new Rect(bx, y, bw, ToolbarHeight), trackLabel))
            {
                if (RPCProfiler.IsTracking)
                    RPCProfiler.PauseTracking();
                else
                    RPCProfiler.ResumeTracking();
            }

            if (!string.IsNullOrEmpty(_statusMessage) && Time.unscaledTime - _statusMessageTime < 3f)
                GUI.Label(new Rect(x + 290, y, 300, ToolbarHeight), _statusMessage);
        }

        static void DrawTabs()
        {
            int x = (int)_windowX + Padding;
            int y = (int)_windowY + Padding * 2 + ToolbarHeight;
            int width = (int)_windowWidth - Padding * 2;
            int tabWidth = (width - Padding) / 2;

            if (GUI.Button(new Rect(x, y, tabWidth, TabHeight), "Per View"))
                _currentTab = RPCTab.PerView;
            if (GUI.Button(new Rect(x + tabWidth + Padding, y, tabWidth, TabHeight), "Summary"))
                _currentTab = RPCTab.Summary;

            int highlightX = _currentTab == RPCTab.PerView ? x : x + tabWidth + Padding;
            GUI.Box(new Rect(highlightX, y, tabWidth, TabHeight), "");
        }

        static void DrawContent()
        {
            int x = (int)_windowX + Padding;
            int y = (int)_windowY + Padding * 3 + ToolbarHeight + TabHeight;
            int width = (int)_windowWidth - Padding * 2;
            int height = (int)_windowHeight - Padding * 5 - ToolbarHeight - TabHeight;

            if (_currentTab == RPCTab.PerView)
                DrawPerViewContent(x, y, width, height);
            else
                DrawSummaryContent(x, y, width, height);
        }

        static GUIStyle GetHeaderStyle()
        {
            var s = new GUIStyle(GUI.skin.label);
            s.fontStyle = FontStyle.Bold;
            s.richText = true;
            return s;
        }

        static GUIStyle GetRowStyle()
        {
            var s = new GUIStyle(GUI.skin.label);
            s.richText = true;
            return s;
        }

        static void DrawPerViewContent(int x, int y, int width, int height)
        {
            var sentStats = RPCProfiler.GetSentStats();
            var recvStats = RPCProfiler.GetRecvStats();
            var serWrites = RPCProfiler.GetSerializeWriteStats();
            var serReads = RPCProfiler.GetSerializeReadStats();
            var viewIds = RPCProfiler.GetAllViewIds().ToList();
            viewIds.Sort();

            float duration = RPCProfiler.SessionDuration;
            if (duration <= 0f) duration = 1f;

            // Calculate content height
            float contentHeight = RowHeight; // header row
            foreach (int viewId in viewIds)
            {
                contentHeight += RowHeight;
                if (_expandedViews.Contains(viewId))
                {
                    int rpcCount = CollectRpcNames(viewId, sentStats, recvStats).Count;
                    contentHeight += rpcCount * RowHeight;
                    bool hasSer = serWrites.ContainsKey(viewId) || serReads.ContainsKey(viewId);
                    if (hasSer) contentHeight += RowHeight;
                    contentHeight += RowHeight; // total row
                }
            }
            if (viewIds.Count == 0)
                contentHeight += RowHeight;

            contentHeight = Mathf.Max(contentHeight, height);
            int contentWidth = width - 20;

            _scrollPosition = GUI.BeginScrollView(new Rect(x, y, width, height), _scrollPosition,
                new Rect(0, 0, contentWidth, contentHeight));

            var headerStyle = GetHeaderStyle();
            var rowStyle = GetRowStyle();
            float cy = 0;

            // Header row
            GUI.Label(new Rect(ColViewId, cy, ColViewIdW, RowHeight), "<b>ViewID</b>", headerStyle);
            GUI.Label(new Rect(ColName, cy, ColNameW, RowHeight), "<b>Name</b>", headerStyle);
            GUI.Label(new Rect(ColRpc, cy, ColRpcW, RowHeight), "<b>RPC</b>", headerStyle);
            if (_showRate)
            {
                GUI.Label(new Rect(ColSentCnt, cy, ColSentCntW, RowHeight), "<b>Sent/s</b>", headerStyle);
                GUI.Label(new Rect(ColRecvCnt, cy, ColRecvCntW, RowHeight), "<b>Recv/s</b>", headerStyle);
                GUI.Label(new Rect(ColSentBw, cy, ColSentBwW, RowHeight), "<b>Sent BW/s</b>", headerStyle);
                GUI.Label(new Rect(ColRecvBw, cy, ColRecvBwW, RowHeight), "<b>Recv BW/s</b>", headerStyle);
            }
            else
            {
                GUI.Label(new Rect(ColSentCnt, cy, ColSentCntW, RowHeight), "<b>Sent</b>", headerStyle);
                GUI.Label(new Rect(ColRecvCnt, cy, ColRecvCntW, RowHeight), "<b>Recv</b>", headerStyle);
                GUI.Label(new Rect(ColSentBw, cy, ColSentBwW, RowHeight), "<b>Sent BW</b>", headerStyle);
                GUI.Label(new Rect(ColRecvBw, cy, ColRecvBwW, RowHeight), "<b>Recv BW</b>", headerStyle);
            }
            cy += RowHeight;

            if (viewIds.Count == 0)
            {
                GUI.Label(new Rect(ColViewId, cy, contentWidth, RowHeight), "No data yet. RPCs and serialization will appear as they occur.", rowStyle);
                cy += RowHeight;
            }

            foreach (int viewId in viewIds)
            {
                string viewName = RPCProfiler.GetViewName(viewId);
                RPCProfiler.GetViewRpcTotals(viewId, out int totalSentCnt, out long totalSentBytes, out int totalRecvCnt, out long totalRecvBytes);

                // Add serialize totals
                long serWriteBytes = 0; int serWriteCnt = 0;
                long serReadBytes = 0; int serReadCnt = 0;
                if (serWrites.TryGetValue(viewId, out var sw)) { serWriteCnt = sw.Count; serWriteBytes = sw.TotalBytes; }
                if (serReads.TryGetValue(viewId, out var sr)) { serReadCnt = sr.Count; serReadBytes = sr.TotalBytes; }

                bool expanded = _expandedViews.Contains(viewId);
                string arrow = expanded ? "?" : "?";

                // View header row as a clickable button
                if (GUI.Button(new Rect(ColViewId, cy, contentWidth, RowHeight), "", GUIStyle.none))
                {
                    if (expanded) _expandedViews.Remove(viewId);
                    else _expandedViews.Add(viewId);
                }

                GUI.Label(new Rect(ColViewId, cy, ColViewIdW, RowHeight), "<b>" + arrow + " " + viewId + "</b>", headerStyle);
                GUI.Label(new Rect(ColName, cy, ColNameW, RowHeight), "<b>" + viewName + "</b>", headerStyle);
                GUI.Label(new Rect(ColRpc, cy, ColRpcW, RowHeight), "", headerStyle);
                if (_showRate)
                {
                    GUI.Label(new Rect(ColSentCnt, cy, ColSentCntW, RowHeight), ((totalSentCnt + serWriteCnt) / duration).ToString("F1"), rowStyle);
                    GUI.Label(new Rect(ColRecvCnt, cy, ColRecvCntW, RowHeight), ((totalRecvCnt + serReadCnt) / duration).ToString("F1"), rowStyle);
                    GUI.Label(new Rect(ColSentBw, cy, ColSentBwW, RowHeight), RPCProfiler.FormatBytesRate((totalSentBytes + serWriteBytes) / duration), rowStyle);
                    GUI.Label(new Rect(ColRecvBw, cy, ColRecvBwW, RowHeight), RPCProfiler.FormatBytesRate((totalRecvBytes + serReadBytes) / duration), rowStyle);
                }
                else
                {
                    GUI.Label(new Rect(ColSentCnt, cy, ColSentCntW, RowHeight), (totalSentCnt + serWriteCnt).ToString(), rowStyle);
                    GUI.Label(new Rect(ColRecvCnt, cy, ColRecvCntW, RowHeight), (totalRecvCnt + serReadCnt).ToString(), rowStyle);
                    GUI.Label(new Rect(ColSentBw, cy, ColSentBwW, RowHeight), RPCProfiler.FormatBytes(totalSentBytes + serWriteBytes), rowStyle);
                    GUI.Label(new Rect(ColRecvBw, cy, ColRecvBwW, RowHeight), RPCProfiler.FormatBytes(totalRecvBytes + serReadBytes), rowStyle);
                }
                cy += RowHeight;

                if (!expanded) continue;

                // Individual RPC rows
                var rpcNames = CollectRpcNames(viewId, sentStats, recvStats);
                sentStats.TryGetValue(viewId, out var sentMap);
                recvStats.TryGetValue(viewId, out var recvMap);

                foreach (string rpc in rpcNames)
                {
                    int sCnt = 0; long sBytes = 0;
                    int rCnt = 0; long rBytes = 0;
                    if (sentMap != null && sentMap.TryGetValue(rpc, out var ss)) { sCnt = ss.Count; sBytes = ss.TotalBytes; }
                    if (recvMap != null && recvMap.TryGetValue(rpc, out var rs)) { rCnt = rs.Count; rBytes = rs.TotalBytes; }

                    GUI.Label(new Rect(ColViewId, cy, ColViewIdW, RowHeight), "", rowStyle);
                    GUI.Label(new Rect(ColName, cy, ColNameW, RowHeight), "", rowStyle);
                    GUI.Label(new Rect(ColRpc, cy, ColRpcW, RowHeight), "  " + rpc, rowStyle);
                    if (_showRate)
                    {
                        GUI.Label(new Rect(ColSentCnt, cy, ColSentCntW, RowHeight), (sCnt / duration).ToString("F1"), rowStyle);
                        GUI.Label(new Rect(ColRecvCnt, cy, ColRecvCntW, RowHeight), (rCnt / duration).ToString("F1"), rowStyle);
                        GUI.Label(new Rect(ColSentBw, cy, ColSentBwW, RowHeight), RPCProfiler.FormatBytesRate(sBytes / duration), rowStyle);
                        GUI.Label(new Rect(ColRecvBw, cy, ColRecvBwW, RowHeight), RPCProfiler.FormatBytesRate(rBytes / duration), rowStyle);
                    }
                    else
                    {
                        GUI.Label(new Rect(ColSentCnt, cy, ColSentCntW, RowHeight), sCnt.ToString(), rowStyle);
                        GUI.Label(new Rect(ColRecvCnt, cy, ColRecvCntW, RowHeight), rCnt.ToString(), rowStyle);
                        GUI.Label(new Rect(ColSentBw, cy, ColSentBwW, RowHeight), RPCProfiler.FormatBytes(sBytes), rowStyle);
                        GUI.Label(new Rect(ColRecvBw, cy, ColRecvBwW, RowHeight), RPCProfiler.FormatBytes(rBytes), rowStyle);
                    }
                    cy += RowHeight;
                }

                // Serialize row
                bool hasSer = serWrites.ContainsKey(viewId) || serReads.ContainsKey(viewId);
                if (hasSer)
                {
                    GUI.Label(new Rect(ColViewId, cy, ColViewIdW, RowHeight), "", rowStyle);
                    GUI.Label(new Rect(ColName, cy, ColNameW, RowHeight), "", rowStyle);
                    GUI.Label(new Rect(ColRpc, cy, ColRpcW, RowHeight), "  <i>Serialize</i>", rowStyle);
                    if (_showRate)
                    {
                        GUI.Label(new Rect(ColSentCnt, cy, ColSentCntW, RowHeight), (serWriteCnt / duration).ToString("F1"), rowStyle);
                        GUI.Label(new Rect(ColRecvCnt, cy, ColRecvCntW, RowHeight), (serReadCnt / duration).ToString("F1"), rowStyle);
                        GUI.Label(new Rect(ColSentBw, cy, ColSentBwW, RowHeight), RPCProfiler.FormatBytesRate(serWriteBytes / duration), rowStyle);
                        GUI.Label(new Rect(ColRecvBw, cy, ColRecvBwW, RowHeight), RPCProfiler.FormatBytesRate(serReadBytes / duration), rowStyle);
                    }
                    else
                    {
                        GUI.Label(new Rect(ColSentCnt, cy, ColSentCntW, RowHeight), serWriteCnt.ToString(), rowStyle);
                        GUI.Label(new Rect(ColRecvCnt, cy, ColRecvCntW, RowHeight), serReadCnt.ToString(), rowStyle);
                        GUI.Label(new Rect(ColSentBw, cy, ColSentBwW, RowHeight), RPCProfiler.FormatBytes(serWriteBytes), rowStyle);
                        GUI.Label(new Rect(ColRecvBw, cy, ColRecvBwW, RowHeight), RPCProfiler.FormatBytes(serReadBytes), rowStyle);
                    }
                    cy += RowHeight;
                }

                // Total row
                int allSentCnt = totalSentCnt + serWriteCnt;
                int allRecvCnt = totalRecvCnt + serReadCnt;
                long allSentBytes = totalSentBytes + serWriteBytes;
                long allRecvBytes = totalRecvBytes + serReadBytes;
                GUI.Label(new Rect(ColViewId, cy, ColViewIdW, RowHeight), "", rowStyle);
                GUI.Label(new Rect(ColName, cy, ColNameW, RowHeight), "", rowStyle);
                GUI.Label(new Rect(ColRpc, cy, ColRpcW, RowHeight), "  <b>TOTAL</b>", headerStyle);
                if (_showRate)
                {
                    GUI.Label(new Rect(ColSentCnt, cy, ColSentCntW, RowHeight), "<b>" + (allSentCnt / duration).ToString("F1") + "</b>", headerStyle);
                    GUI.Label(new Rect(ColRecvCnt, cy, ColRecvCntW, RowHeight), "<b>" + (allRecvCnt / duration).ToString("F1") + "</b>", headerStyle);
                    GUI.Label(new Rect(ColSentBw, cy, ColSentBwW, RowHeight), "<b>" + RPCProfiler.FormatBytesRate(allSentBytes / duration) + "</b>", headerStyle);
                    GUI.Label(new Rect(ColRecvBw, cy, ColRecvBwW, RowHeight), "<b>" + RPCProfiler.FormatBytesRate(allRecvBytes / duration) + "</b>", headerStyle);
                }
                else
                {
                    GUI.Label(new Rect(ColSentCnt, cy, ColSentCntW, RowHeight), "<b>" + allSentCnt + "</b>", headerStyle);
                    GUI.Label(new Rect(ColRecvCnt, cy, ColRecvCntW, RowHeight), "<b>" + allRecvCnt + "</b>", headerStyle);
                    GUI.Label(new Rect(ColSentBw, cy, ColSentBwW, RowHeight), "<b>" + RPCProfiler.FormatBytes(allSentBytes) + "</b>", headerStyle);
                    GUI.Label(new Rect(ColRecvBw, cy, ColRecvBwW, RowHeight), "<b>" + RPCProfiler.FormatBytes(allRecvBytes) + "</b>", headerStyle);
                }
                cy += RowHeight;
            }

            GUI.EndScrollView();
        }

        static void DrawSummaryContent(int x, int y, int width, int height)
        {
            var summary = RPCProfiler.GetSummary(_showRate);
            float duration = RPCProfiler.SessionDuration;
            if (duration <= 0f) duration = 1f;

            float contentHeight = Mathf.Max((summary.Count + 1) * RowHeight, height);
            int contentWidth = width - 20;

            _scrollPosition = GUI.BeginScrollView(new Rect(x, y, width, height), _scrollPosition,
                new Rect(0, 0, contentWidth, contentHeight));

            var headerStyle = GetHeaderStyle();
            var rowStyle = GetRowStyle();
            float cy = 0;

            // Summary columns
            int c1 = 10, c1w = 200;
            int c2 = 210, c2w = 80;
            int c3 = 290, c3w = 80;
            int c4 = 370, c4w = 90;
            int c5 = 460, c5w = 90;
            int c6 = 550, c6w = 90;

            if (_showRate)
            {
                GUI.Label(new Rect(c1, cy, c1w, RowHeight), "<b>RPC Name</b>", headerStyle);
                GUI.Label(new Rect(c2, cy, c2w, RowHeight), "<b>Sent/s</b>", headerStyle);
                GUI.Label(new Rect(c3, cy, c3w, RowHeight), "<b>Recv/s</b>", headerStyle);
                GUI.Label(new Rect(c4, cy, c4w, RowHeight), "<b>Sent BW/s</b>", headerStyle);
                GUI.Label(new Rect(c5, cy, c5w, RowHeight), "<b>Recv BW/s</b>", headerStyle);
                GUI.Label(new Rect(c6, cy, c6w, RowHeight), "<b>Total BW/s</b>", headerStyle);
            }
            else
            {
                GUI.Label(new Rect(c1, cy, c1w, RowHeight), "<b>RPC Name</b>", headerStyle);
                GUI.Label(new Rect(c2, cy, c2w, RowHeight), "<b>Sent</b>", headerStyle);
                GUI.Label(new Rect(c3, cy, c3w, RowHeight), "<b>Recv</b>", headerStyle);
                GUI.Label(new Rect(c4, cy, c4w, RowHeight), "<b>Sent BW</b>", headerStyle);
                GUI.Label(new Rect(c5, cy, c5w, RowHeight), "<b>Recv BW</b>", headerStyle);
                GUI.Label(new Rect(c6, cy, c6w, RowHeight), "<b>Total BW</b>", headerStyle);
            }
            cy += RowHeight;

            foreach (var entry in summary)
            {
                GUI.Label(new Rect(c1, cy, c1w, RowHeight), entry.RpcName, rowStyle);
                if (_showRate)
                {
                    GUI.Label(new Rect(c2, cy, c2w, RowHeight), entry.SentRate.ToString("F1"), rowStyle);
                    GUI.Label(new Rect(c3, cy, c3w, RowHeight), entry.RecvRate.ToString("F1"), rowStyle);
                    GUI.Label(new Rect(c4, cy, c4w, RowHeight), RPCProfiler.FormatBytesRate(entry.SentBytesRate), rowStyle);
                    GUI.Label(new Rect(c5, cy, c5w, RowHeight), RPCProfiler.FormatBytesRate(entry.RecvBytesRate), rowStyle);
                    GUI.Label(new Rect(c6, cy, c6w, RowHeight), RPCProfiler.FormatBytesRate(entry.SentBytesRate + entry.RecvBytesRate), rowStyle);
                }
                else
                {
                    GUI.Label(new Rect(c2, cy, c2w, RowHeight), entry.TotalSentCount.ToString(), rowStyle);
                    GUI.Label(new Rect(c3, cy, c3w, RowHeight), entry.TotalRecvCount.ToString(), rowStyle);
                    GUI.Label(new Rect(c4, cy, c4w, RowHeight), RPCProfiler.FormatBytes(entry.TotalSentBytes), rowStyle);
                    GUI.Label(new Rect(c5, cy, c5w, RowHeight), RPCProfiler.FormatBytes(entry.TotalRecvBytes), rowStyle);
                    GUI.Label(new Rect(c6, cy, c6w, RowHeight), RPCProfiler.FormatBytes(entry.TotalSentBytes + entry.TotalRecvBytes), rowStyle);
                }
                cy += RowHeight;
            }

            GUI.EndScrollView();
        }

        static List<string> CollectRpcNames(int viewId, Dictionary<int, Dictionary<string, RPCProfiler.RpcStats>> sentStats,
            Dictionary<int, Dictionary<string, RPCProfiler.RpcStats>> recvStats)
        {
            var names = new HashSet<string>();
            if (sentStats.TryGetValue(viewId, out var sc))
                foreach (var k in sc.Keys) names.Add(k);
            if (recvStats.TryGetValue(viewId, out var rc))
                foreach (var k in rc.Keys) names.Add(k);
            var list = names.ToList();
            list.Sort();
            return list;
        }

        static void ExportCsv()
        {
            string csv = RPCProfiler.ExportCsv();
            string dir = FolderPaths.Documents;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string path = dir + "/RPCProfiler_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            File.WriteAllText(path, csv);
            _statusMessage = "Exported: " + path;
            _statusMessageTime = Time.unscaledTime;
            Debug.Log("RPC Profiler CSV exported to: " + path);
        }

        static void DrawResizeHandle()
        {
            Rect rect = new Rect(_windowX + _windowWidth - ResizeHandleSize,
                                 _windowY + _windowHeight - ResizeHandleSize,
                                 ResizeHandleSize, ResizeHandleSize);
            GUI.Box(rect, "?");
        }
    }
}
