using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using Utility;
using Settings;
using UI;
using System.Collections.Generic;

namespace ApplicationManagers
{
    /// <summary>
    /// Used to handle changes between fullscreen and windowed, or to handle exclusive-fullscreen behaviors.
    /// </summary>
    class FullscreenHandler : MonoBehaviour
    {
        static FullscreenHandler _instance;
        static Resolution _resolution;

        static extern bool ShowWindow(int hWnd, int nCmdShow);

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
        }

        public static void Apply(int resolutionIndex, FullScreenLevel fullscreenLevel)
        {
            var resolutions = GetResolutions();
            _resolution = resolutions[resolutionIndex];
            SetFullscreen(fullscreenLevel);
        }

        public static int SanitizeResolutionSetting(int resolutionIndex)
        {
            var resolutions = GetResolutions();
            if (resolutionIndex >= resolutions.Count)
                resolutionIndex = 0;
            if (resolutionIndex < 0)
                resolutionIndex = 0;
            return resolutionIndex;
        }

        public static string[] GetResolutionOptions()
        {
            List<string> options = new List<string>();
            var resolutions = GetResolutions();
            foreach (var resolution in resolutions)
                options.Add(resolution.width.ToString() + " x " + resolution.height.ToString());
            return options.ToArray();
        }

        static List<Resolution> GetResolutions()
        {
            List<Resolution> resolutions = new List<Resolution>();
            HashSet<string> found = new HashSet<string>();
            foreach (var resolution in Screen.resolutions)
            {
                string str = resolution.width.ToString() + "," + resolution.height.ToString();
                if (!found.Contains(str))
                {
                    resolutions.Add(resolution);
                    found.Add(str);
                }
            }
            resolutions.Reverse();
            if (resolutions.Count == 0)
            {
                var resolution = new Resolution();
                resolution.width = 800;
                resolution.height = 600;
                resolutions.Add(resolution);
            }
            return resolutions;
        }

        static void SetFullscreen(FullScreenLevel fullscreen)
        {
            if (fullscreen == FullScreenLevel.Windowed)
            {
                Screen.SetResolution(_resolution.width, _resolution.height, FullScreenMode.Windowed);
            }
            else
            {
                if (fullscreen == FullScreenLevel.Borderless)
                    Screen.SetResolution(_resolution.width, _resolution.height, FullScreenMode.FullScreenWindow);
                else
                    Screen.SetResolution(_resolution.width, _resolution.height, FullScreenMode.ExclusiveFullScreen);
            }
            CursorManager.RefreshCursorLock();
            if (UIManager.CurrentMenu != null)
                UIManager.CurrentMenu.ApplyScale(SceneLoader.SceneName);
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            if (SettingsManager.SoundSettings.MuteMinimized.Value)
            {
                AudioListener.pause = !hasFocus;
            }
            CursorManager.RefreshCursorLock();
        }

        static bool IsWindowed()
        {
            return SettingsManager.GraphicsSettings.FullScreenMode.Value == (int)FullScreenLevel.Windowed;
        }

        static bool Supported()
        {
            return Application.platform == RuntimePlatform.WindowsPlayer;
        }
    }
}