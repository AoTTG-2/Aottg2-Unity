﻿using Photon;
using System;
using UnityEngine;
using System.IO;

namespace ApplicationManagers
{
    /// <summary>
    /// Config file to store project-level settings.
    /// </summary>
    class ApplicationConfig
    {
        private static readonly string DevelopmentConfigPath = Application.dataPath + "/DevelopmentConfig";
        public static bool DevelopmentMode = false;
        public const string GameVersion = "12.23.2024";

        public static void Init()
        {
            if (File.Exists(DevelopmentConfigPath) || Application.platform == RuntimePlatform.WindowsEditor)
            {
                DevelopmentMode = true;
            }
        }
    }
}
