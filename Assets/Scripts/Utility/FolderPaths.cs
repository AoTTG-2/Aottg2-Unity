using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utility
{
    class FolderPaths
    {
        public static string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Aottg2";
        public static string StreamingAssetsPath = Application.streamingAssetsPath;
        public static string LanguagesPath = StreamingAssetsPath + "/Languages";
        public static string PresetsPath = StreamingAssetsPath + "/Presets";
        public static string UIThemesPath = StreamingAssetsPath + "/UIThemes";
        public static string Settings = Documents + "/Settings";
        public static string Snapshots = Documents + "/Snapshots";
        public static string GameProgress = Documents + "/GameProgress";
        public static string PersistentData = Documents + "/PersistentData";
        public static string CustomLogic = Documents + "/CustomLogic";
        public static string CustomMap = Documents + "/CustomMap";
        public static string CustomAssetsLocal = Documents + "/CustomAssets";
        public static string CustomAssetsWeb = Documents + "/CustomAssets/WebDownload";
    }
}
