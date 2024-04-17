#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using static VFolders.VFoldersData;
using static VFolders.Libs.VUtils;
using static VFolders.Libs.VGUI;

namespace VFolders
{
    public class VFoldersIconEditor : CustomPopupWindow
    {
        void OnGUI()
        {
            if (iconRows == null)
                if (folderData != null) Init(folderData);
                else { Close(); return; }

            VFolders.data?.RecordUndo();

            var buttonSize = 18;
            var buttonSpacing = 2;
            var rowStartX = 6;

            var bgSelected = new Color(.3f, .5f, .7f, .8f);
            var bgHovered = Greyscale(1, .3f);

            void icons()
            {
                string hoveredIcon = null;

                void iconButton(Rect rect, string icon)
                {
                    if (icon == initIcon)
                        rect.Draw(bgSelected);

                    if (rect.Resize(-1).IsHovered())
                    {
                        rect.Draw(bgHovered);
                        hoveredIcon = icon;
                    }

                    if (e.mouseDown() && rect.Resize(-1).IsHovered())
                        Close();

                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    GUI.Label(rect, EditorGUIUtility.IconContent(icon == "" ? "CrossIcon" : icon));
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;

                }

                for (int i = 0; i < iconRows.Length; i++)
                {
                    GUILayout.Label(i == 0 ? "" : "");

                    var iconRect = lastRect.SetWidth(buttonSize).SetHeightFromMid(buttonSize).MoveX(rowStartX);

                    foreach (var icon in iconRows[i])
                    {
                        iconButton(iconRect, icon);
                        iconRect = iconRect.MoveX(buttonSize + buttonSpacing);
                    }

                    Space(buttonSpacing);

                }

                folderData.icon = hoveredIcon ?? initIcon;

            }
            void colors_()
            {
                int hoveredIColor = -1;

                void colorButton(Rect rect, int iColor)
                {
                    if (iColor == initIColor)
                        rect.Draw(bgSelected);

                    if (rect.Resize(-1).IsHovered())
                    {
                        rect.Draw(bgHovered);
                        hoveredIColor = iColor;
                    }

                    if (e.mouseDown() && rect.Resize(-1).IsHovered())
                        Close();

                    SetGUIColor(GetColor(iColor));
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    GUI.Label(rect, EditorGUIUtility.IconContent(iColor == 0 ? "Folder Icon" : "d_Folder Icon"));
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    ResetGUIColor();

                }

                GUILayout.Label("");

                var iconRect = lastRect.SetWidth(buttonSize).SetHeightFromMid(buttonSize).MoveX(rowStartX);

                for (int i = 0; i < colorsN; i++)
                {
                    colorButton(iconRect, i);
                    iconRect = iconRect.MoveX(buttonSize + buttonSpacing);
                }

                if (hoveredIColor != -1)
                    folderData.iColor = hoveredIColor;
                else
                    folderData.iColor = initIColor;

            }

            // HeaderGUI();

            BeginIndent(8);

            Space(13);
            colors_();

            Space(12);
            icons();

            EndIndent(8);

            if (e.keyDown() && e.keyCode == KeyCode.Escape)
            {
                folderData.icon = initIcon;
                folderData.iColor = initIColor;
                Close();
            }

            Repaint();
            EditorApplication.RepaintProjectWindow();

        }

        public void Init(FolderData folderData)
        {
            this.folderData = folderData;

            currentlyOpenedForGuid = folderData.guid;

            initIcon = folderData.icon;
            initIColor = folderData.iColor;

            if (iconRows == null)
                iconRows = new string[][]
                {
                new[]
                {
                    "",
                    "SceneAsset Icon",
                    "Prefab Icon",
                    "PrefabModel Icon",
                    "Material Icon",
                    "Texture Icon",
                    "Mesh Icon",
                    "cs Script Icon",
                    "Shader Icon",
                    "ComputeShader Icon",
                    "ScriptableObject Icon",

                },
                new[]
                {
                    "Light Icon",
                    "LightProbes Icon",
                    "LightmapParameters Icon",
                    "LightingDataAsset Icon",
                    "Cubemap Icon"

                },
                new[]
                {
                    "PhysicMaterial Icon",
                    "BoxCollider Icon",
                    "TerrainCollider Icon",
                    "MeshCollider Icon",
                    "WheelCollider Icon",
                    "Rigidbody Icon",
                },
                new[]
                {
                    "AudioClip Icon",
                    "AudioMixerController Icon",
                    "AudioMixerGroup Icon",
                    "AudioEchoFilter Icon",
                    "AudioSource Icon",
                },
                new[]
                {
                    "TextAsset Icon",
                    "AssemblyDefinitionAsset Icon",
                    "TerrainData Icon",
                    "Terrain Icon",
                    "AnimatorController Icon",
                    "AnimationClip Icon",
                    "Font Icon",
                    "RawImage Icon",
                    "ParticleSystem Gizmo",
                    // "Settings Icon",
                },

                };

            VFolders.data.Dirty();

            Undo.undoRedoPerformed += RepaintOnUndoRedo;

        }
        FolderData folderData;
        string initIcon;
        int initIColor;

        static string[][] iconRows;

        public static string currentlyOpenedForGuid;
        public static string wasJustDestroyedForGuid;
        void OnDestroy()
        {
            wasJustDestroyedForGuid = currentlyOpenedForGuid;

            EditorApplication.delayCall += () =>
            {
                if (currentlyOpenedForGuid == wasJustDestroyedForGuid)
                    currentlyOpenedForGuid = "";

                wasJustDestroyedForGuid = "";

            };

        }

        public static VFoldersIconEditor instance => Resources.FindObjectsOfTypeAll<VFoldersIconEditor>().FirstOrDefault();



        public static Color GetColor(int iColor)
        {
            if (colors != null) return colors[iColor];

            colors = new Color[colorsN];

            colors[0] = Color.white;

            for (int i = 1; i < colors.Length; i++)
            {
                var color = EditorGUIUtility.isProSkin ?
                    HSLToRGB((i - 0f) / (colors.Length - 1), .9f, .65f)
                  : HSLToRGB((i - 0f) / (colors.Length - 1), .9f, .6f);
                color *= 1.1f;
                color.a = 1;

                colors[i] = color;
            }

            return colors[iColor];

        }
        static Color[] colors;
        public static int colorsN = 11;



        static void RepaintOnUndoRedo()
        {
            EditorApplication.RepaintProjectWindow();
            Undo.undoRedoPerformed -= RepaintOnUndoRedo;
        }


        public override float initWidth => 246;
        public override float initHeight => 162;

    }
}
#endif