#if PUN_2_OR_NEWER
using System;

namespace Photon.Voice.PUN.Editor
{
    using Unity.Editor;
    using UnityEditor;
    using UnityEngine;
    using Pun;

    [CustomEditor(typeof(PunVoiceClient), true)]
    public class PunVoiceClientEditor : VoiceConnectionEditor
    {
        private SerializedProperty autoConnectAndJoinSp;
        private SerializedProperty usePunAppSettingsSp;
        private SerializedProperty usePunAuthValuesSp;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.autoConnectAndJoinSp = this.serializedObject.FindProperty("AutoConnectAndJoin");
            this.usePunAppSettingsSp = this.serializedObject.FindProperty("usePunAppSettings");
            this.usePunAuthValuesSp = this.serializedObject.FindProperty("usePunAuthValues");
        }

        protected override void DisplayAppSettings()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(this.usePunAppSettingsSp, new GUIContent("Use PUN App Settings", "Use App Settings From PUN's PhotonServerSettings"));
            if (GUILayout.Button("PhotonServerSettings", EditorStyles.miniButton, GUILayout.Width(150)))
            {
                Selection.objects = new Object[] { PhotonNetwork.PhotonServerSettings };
                EditorGUIUtility.PingObject(PhotonNetwork.PhotonServerSettings);
            }
            EditorGUILayout.EndHorizontal();
            if (!this.usePunAppSettingsSp.boolValue)
            {
                EditorGUI.indentLevel++;
                base.DisplayAppSettings();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.PropertyField(this.usePunAuthValuesSp, new GUIContent("Use PUN Auth Values", "Use the same Authentication Values From PUN client"));
        }

        protected override void ShowHeader()
        {
            base.ShowHeader();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(this.autoConnectAndJoinSp, new GUIContent("Auto Connect And Join", "Auto connect voice client and join a voice room when PUN client is joined to a PUN room"));
            if (EditorGUI.EndChangeCheck())
            {
                this.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif