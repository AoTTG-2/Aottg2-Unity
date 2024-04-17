#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using System.IO;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using static VFolders.Libs.VUtils;
using static VFolders.Libs.VGUI;
using static VFolders.VFoldersData;



namespace VFolders
{
    public static class VFolders
    {
        static void ItemGUI(string guid, Rect rect)
        {
            if (!data) return;

            if (!foldersFirstInited)
                UpdateFoldersFirst();

            var isSingleLine = rect.height == 16;
            var isListView = isSingleLine && rect.x == 14;

            if (!AssetDatabase.IsValidFolder(guid.ToPath()))
            {
                if (eType == EventType.Repaint && isSingleLine)
                    listviewAssetRectX = EditorGUIUtility.GUIToScreenPoint(rect.position).x;

                return;
            }

            FolderData folderData;

            Rect folderIconRect;
            Rect iconRect;

            void folderData_()
            {
                if (!data.folderDatasByGuid.ContainsKey(guid))
                    data.folderDatasByGuid.Add(guid, new FolderData(guid));

                folderData = data.folderDatasByGuid[guid];

                if (folderData.autoIconDirty)
                    UpdateAutoIcon(folderData);

            }
            void rects()
            {
                if (isSingleLine)
                {
                    folderIconRect = rect.SetWidth(16);

                    if (isListView)
                        folderIconRect.x += 3;
                }
                else
                {
                    folderIconRect = rect.SetHeight(rect.width);

                    if (Application.platform == RuntimePlatform.OSXEditor)
                        if (folderIconRect.width > 64)
                            folderIconRect = folderIconRect.SetSizeFromMid(64, 64);
                }


                var iconOffsetMin = new Vector2(4, 4);
                var iconSizeMin = 14f;

                var iconOffsetMax = new Vector2(18, 15);
                var iconSizeMax = 30;

                var t = ((folderIconRect.width - 16) / (64 - 16));
                if (Application.platform == RuntimePlatform.OSXEditor)
                    t = t.Clamp01();

                var iconOffset = Lerp(iconOffsetMin, iconOffsetMax, t);
                var iconSize = Lerp(iconSizeMin, iconSizeMax, t);

                iconRect = folderIconRect.Move(iconOffset).SetSizeFromMid(iconSize, iconSize);

            }
            void color()
            {
                if (eType != EventType.Repaint) return;
                if (folderData.color == Color.white) return;
                if (!VFoldersMenuItems.customIconsEnabled) return;

                var isEmpty = !Directory.EnumerateFileSystemEntries(guid.ToPath()).Any();

                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                SetGUIColor(folderData.color);
                GUI.Label(folderIconRect.Resize(-2), EditorGUIUtility.IconContent(isEmpty ? "d_FolderEmpty Icon" : "d_Folder Icon"));
                ResetGUIColor();
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;


            }
            void icon()
            {
                if (eType != EventType.Repaint) return;
                if (folderData.icon == "") return;

                var texture = EditorGUIUtility.IconContent(folderData.icon).image;

                void material()
                {
                    if (outlineMaterial) return;

                    outlineMaterial = new Material(Shader.Find("Hidden/VFoldersOutline"));
                    outlineMaterial.SetColor("_Color", EditorGUIUtility.isProSkin ? Greyscale(.245f) : Greyscale(.73f));
                }

                void outline()
                {
                    if (isSingleLine) return;

                    var outlineRect = iconRect.Resize(rect.height >= 70 && EditorGUIUtility.pixelsPerPoint >= 2 ? -1f / EditorGUIUtility.pixelsPerPoint : 0).AlignToPixelGrid();

                    EditorGUI.DrawPreviewTexture(outlineRect.Resize(2).Move(-1, -1), texture, outlineMaterial);
                    EditorGUI.DrawPreviewTexture(outlineRect.Resize(2).Move(-1, 1), texture, outlineMaterial);
                    EditorGUI.DrawPreviewTexture(outlineRect.Resize(2).Move(1, 1), texture, outlineMaterial);
                    EditorGUI.DrawPreviewTexture(outlineRect.Resize(2).Move(1, -1), texture, outlineMaterial);

                }
                void background()
                {
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;

                    for (int i = 0; i < iconRect.size.x / 2; i++)
                        EditorGUI.DrawPreviewTexture(iconRect.Resize(i * 1f + 1), texture, outlineMaterial);

                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;

                }
                void icon()
                {
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;

                    GUI.Label(iconRect, EditorGUIUtility.IconContent(folderData.icon));

                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;

                }

                material();
                outline();
                background();
                icon();

            }
            void altDrag() // has to be set up manually before altClick because altClick will use() mouseDown event to prevent selection change
            {
                if (!holdingAlt) return;
                if (!VFoldersMenuItems.customIconsEnabled) return;

                void mouseDown()
                {
                    if (!e.mouseDown()) return;
                    if (!rect.IsHovered()) return;

                    mouseDownPos = e.mousePosition;

                }
                void mouseDrag()
                {
                    if (eType != EventType.MouseDrag) return;
                    if ((e.mousePosition - mouseDownPos).magnitude < 5) return;
                    if (!rect.Contains(mouseDownPos)) return;
                    if (!rect.Contains(e.mousePosition - e.delta)) return;
                    if (DragAndDrop.objectReferences.Any()) return;

                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new[] { AssetDatabase.LoadAssetAtPath<Object>(guid.ToPath()) };
                    DragAndDrop.StartDrag(guid.ToPath().GetName());

                }

                mouseDown();
                mouseDrag();

            }
            void altClick()
            {
                if (!rect.IsHovered()) return;
                if (!holdingAlt) return;
                if (!VFoldersMenuItems.customIconsEnabled) return;

                void mouseDown()
                {
                    if (!e.mouseDown()) return;

                    mouseDownOnGuid = guid;

                    e.Use();

                }
                void mouseUp()
                {
                    if (!e.mouseUp()) return;
                    if (VFoldersIconEditor.currentlyOpenedForGuid == guid) { VFoldersIconEditor.instance?.Close(); return; }
                    if (mouseDownOnGuid != guid) return;

                    var pos = new Vector2(Mathf.Max(e.mousePosition.x + 3, rect.xMax + 3), rect.y);
                    var window = CustomPopupWindow.Create<VFoldersIconEditor>(true, EditorGUIUtility.GUIToScreenPoint(pos));
                    window.Init(folderData);

                }

                mouseDown();
                mouseUp();

            }


            folderData_();
            rects();

            color();
            icon();

            altDrag();
            altClick();

        }
        static float listviewAssetRectX;
        static Material outlineMaterial;
        static Vector2 mouseDownPos;
        static string mouseDownOnGuid;



        static void UpdateAutoIcon(FolderData folderData)
        {
            folderData.autoIconDirty = false;
            folderData.autoIcon = "";

            var path = folderData.guid.ToPath();

            if (!Directory.Exists(path)) return;

            var types = Directory.GetFiles(path, "*.*").Select(r => AssetDatabase.GetMainAssetTypeAtPath(r)).Where(r => r != null);
            // types.LogAll();

            if (!types.Any()) return;
            if (!types.All(r => r == types.First())) return;

            var type = types.First();

            if (type == typeof(SceneAsset))
                folderData.autoIcon = "SceneAsset Icon";

            else if (type == typeof(GameObject))
                folderData.autoIcon = "Prefab Icon";

            else if (type == typeof(Material))
                folderData.autoIcon = "Material Icon";

            else if (type == typeof(Texture))
                folderData.autoIcon = "Texture Icon";

            else if (type.BaseType == typeof(ScriptableObject))
                folderData.autoIcon = "ScriptableObject Icon";

            else if (type == typeof(TerrainData))
                folderData.autoIcon = "TerrainData Icon";

            else if (type == typeof(AudioClip))
                folderData.autoIcon = "AudioClip Icon";

            else if (type == typeof(Shader))
                folderData.autoIcon = "Shader Icon";

            else if (type == typeof(ComputeShader))
                folderData.autoIcon = "ComputeShader Icon";

            else if (type == typeof(MonoScript) || type == typeof(AssemblyDefinitionAsset) || type == typeof(AssemblyDefinitionReferenceAsset))
                folderData.autoIcon = "cs Script Icon";

        }




        class Postprocessor : AssetPostprocessor
        {
#if UNITY_2021_2_OR_NEWER
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
#else
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
#endif
            {
                // if (didDomainReload) return;
                if (!data) return;
                if (!VFoldersMenuItems.autoIconsEnabled) return;

                foreach (var path in deletedAssets)
                    if (data.folderDatasByGuid.ContainsKey(path.ToGuid()))//
                        data.folderDatasByGuid.Remove(path.ToGuid());

                foreach (var path in importedAssets.Concat(deletedAssets).Concat(movedAssets).Concat(movedFromAssetPaths))
                {
                    if (path.HasParentPath())
                        if (data.folderDatasByGuid.TryGetValue(path.GetParentPath().ToGuid(), out FolderData folderData))
                            folderData.autoIconDirty = true;
                }
            }
        }





        static void UpdateFoldersFirst()
        {
            foldersFirstInited = true;
            if (!VFoldersMenuItems.foldersFirstEnabled) return;

            var t_ProjectBrowser = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");

            foreach (var browser in Resources.FindObjectsOfTypeAll(t_ProjectBrowser))
            {
                if (foldersFirstInitedForBrowsers.Contains(browser)) continue;

                var m_ListArea = t_ProjectBrowser.GetField("m_ListArea", maxBindingFlags).GetValue(browser);

                if (m_ListArea == null) continue;

                m_ListArea.GetType().GetProperty("foldersFirst", maxBindingFlags).SetValue(m_ListArea, true);
                browser.GetType().GetMethod("InitListArea", maxBindingFlags).Invoke(browser, null);


                foldersFirstInitedForBrowsers.Add(browser);
            }

            EditorApplication.delayCall += UpdateFoldersFirst;
        }
        static bool foldersFirstInited;
        static List<Object> foldersFirstInitedForBrowsers = new List<Object>();


#if !DISABLED
        [InitializeOnLoadMethod]
#endif
        static void Init()
        {
            void loadData()
            {
                void loadAtLastKnowPath()
                {
                    if (data) return;

                    data = AssetDatabase.LoadAssetAtPath<VFoldersData>(EditorPrefs.GetString("vFolders-lastKnownDataPath"));

                }
                void find()
                {
                    if (data) return;

                    data = AssetDatabase.FindAssets("t:VFoldersData").Select(r => AssetDatabase.LoadAssetAtPath<VFoldersData>(r.ToPath())).FirstOrDefault();

                }
                void create()
                {
                    if (data) return;

                    EditorApplication.delayCall += () =>
                    {
                        data = ScriptableObject.CreateInstance<VFoldersData>();
                        AssetDatabase.CreateAsset(data, GetScriptPath("vFolders").GetParentPath().CombinePath("vFolders Data.asset"));
                    };

                }
                void updateLastKnownPath()
                {
                    if (!data) return;

                    EditorPrefs.SetString("vFolders-lastKnownDataPath", data.GetPath());

                }

                loadAtLastKnowPath();
                find();
                create();
                updateLastKnownPath();

            }
            void subscribe()
            {
                EditorApplication.projectWindowItemOnGUI -= ItemGUI;
                EditorApplication.projectWindowItemOnGUI += ItemGUI;

                // EditorApplication.playModeStateChanged += (PlayModeStateChange obj) => UpdateFoldersFirst();
                // EditorApplication.projectChanged += UpdateFoldersFirst;

            }

            subscribe();
            loadData();

        }

        public static VFoldersData data;


        const string version = "1.0.13";

    }
}
#endif
