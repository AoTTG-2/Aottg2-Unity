using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utility
{
    class FolderPaths
    {
        // MyDocuments returns $HOME on Linux, use XDG_DATA_HOME instead
        public static string Documents = Application.platform == RuntimePlatform.LinuxPlayer
        ? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Aottg2"
        : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Aottg2";
        public static string StreamingAssetsPath = Application.streamingAssetsPath;
        public static string LanguagesPath = StreamingAssetsPath + "/Languages";
        public static string TesterData = StreamingAssetsPath + "/TesterData";
        public static string PresetsPath = StreamingAssetsPath + "/Presets";
        public static string UIThemesPath = StreamingAssetsPath + "/UIThemes";
        public static string Settings = Documents + "/Settings";
        public static string Snapshots = Documents + "/Snapshots";
        public static string GameProgress = Documents + "/GameProgress";
        public static string PersistentData = Documents + "/PersistentData";
        public static string CustomLogic = Documents + "/CustomLogic";
        public static string CustomAddon = Documents + "/CustomAddon";
        public static string CustomMap = Documents + "/CustomMap";
        public static string CustomMapAutosave = Documents + "/CustomMap/Autosave";
        public static string CustomAssetsLocal = Documents + "/CustomAssets";
        public static string CustomAssetsWeb = Documents + "/CustomAssets/WebDownload";
        public static string CharacterPreviews = Documents + "/CharacterPreviews";
        public static string CustomLocale = Documents + "/CustomLocale";
    }
}
