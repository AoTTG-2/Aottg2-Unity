#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static VFolders.Libs.VUtils;
using static VFolders.Libs.VGUI;


namespace VFolders
{
    class VFoldersMenuItems
    {
        public static bool autoIconsEnabled { get => EditorPrefs.GetBool("vFolders-autoIconsEnabled", true); set => EditorPrefs.SetBool("vFolders-autoIconsEnabled", value); }
        public static bool customIconsEnabled { get => EditorPrefs.GetBool("vFolders-customIconsEnabled", true); set => EditorPrefs.SetBool("vFolders-customIconsEnabled", value); }

        public static bool foldersFirstEnabled { get => EditorPrefs.GetBool("vFolders-foldersFirstEnabled", false); set => EditorPrefs.SetBool("vFolders-foldersFirstEnabled", value); }


        const string menuDir = "Tools/vFolders/";

        const string autoIcons = menuDir + "Automatic icons";
        const string customIcons = menuDir + "Custom icons and colors via Alt-Click";

        const string foldersFirst = menuDir + "Sort folders first";



        [MenuItem(autoIcons, false, 1)] static void dadsadadsas() => autoIconsEnabled = !autoIconsEnabled;
        [MenuItem(autoIcons, true, 1)] static bool dadsaddasadsas() { UnityEditor.Menu.SetChecked(autoIcons, autoIconsEnabled); return true; }

        [MenuItem(customIcons, false, 2)] static void dadsaadsdadsas() => customIconsEnabled = !customIconsEnabled;
        [MenuItem(customIcons, true, 2)] static bool dadsadadsdasadsas() { UnityEditor.Menu.SetChecked(customIcons, customIconsEnabled); return true; }

        // #if UNITY_EDITOR_OSX
        //         [MenuItem(foldersFirst, false, 12)] static void dadsdsfaadsdadsas() => foldersFirstEnabled = !foldersFirstEnabled;
        //         [MenuItem(foldersFirst, true, 12)] static bool dadsasdfdadsdasadsas() { UnityEditor.Menu.SetChecked(foldersFirst, foldersFirstEnabled); return true; }
        // #endif


        [MenuItem(menuDir + "Upgrade to vHierarchy 2", false, 101)]
        static void dadadssadsas() => Application.OpenURL("https://assetstore.unity.com/packages/slug/263644?aid=1100lGLBn&pubref=menuupgrade");

        [MenuItem(menuDir + "Join our Discord", false, 102)]
        static void dadasdsas() => Application.OpenURL("https://discord.gg/4dG9KsbspG");




        [MenuItem(menuDir + "Disable vFolders", false, 1001)]
        static void das() => ToggleDefineDisabledInScript(typeof(VFolders));
        [MenuItem(menuDir + "Disable vFolders", true, 1001)]
        static bool dassadc() { UnityEditor.Menu.SetChecked(menuDir + "Disable vFolders", ScriptHasDefineDisabled(typeof(VFolders))); return true; }



    }
}
#endif