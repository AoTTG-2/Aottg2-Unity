using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace AutoLOD
{
    // Create a new type of Settings Asset.
    class AutoLODSettings : ScriptableObject
    {
        public const string autolodSettingsPath = "Assets/AutoLOD/Editor/AutoLODSettings.asset";

#pragma warning disable 0414 
        [SerializeField]
        private string autolodDefaultExportFolder;
#pragma warning restore 0414 

        internal static AutoLODSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<AutoLODSettings>(autolodSettingsPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<AutoLODSettings>();
                settings.autolodDefaultExportFolder = "Assets/AutoLOD/Generated";
                AssetDatabase.CreateAsset(settings, autolodSettingsPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    // Register a SettingsProvider using IMGUI for the drawing framework:
    static class AutoLODSettingsIMGUIRegister
    {
        [SettingsProvider]
        public static SettingsProvider CreateAutoLODSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("Preferences/AutoLOD", SettingsScope.User)
            {
                // By default the last token of the path is used as display name if no label is provided.
                label = "AutoLOD Settings",
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    var settings = AutoLODSettings.GetSerializedSettings();
                    string currentValue = EditorPrefs.GetString("autolodDefaultExportFolder", "Assets/AutoLOD/Generated");
                    SerializedProperty pathProperty = settings.FindProperty("autolodDefaultExportFolder");
                    EditorGUILayout.PropertyField(pathProperty, new GUIContent("Default export folder"));
                    if (pathProperty.stringValue != currentValue)
                    {
                        EditorPrefs.SetString("autolodDefaultExportFolder", pathProperty.stringValue);
                    }
                    settings.ApplyModifiedPropertiesWithoutUndo();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "Default export folder" })
            };

            return provider;
        }
    }

}