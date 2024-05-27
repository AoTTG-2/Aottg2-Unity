using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EmVariables
{
    public static readonly int LogisticianMaxSupply = 6; // added by ata for a single source of truth on max supplies. separate it to blade and gas if you need to but i don't think it's needed.

    public static Player SelectedPlayer;
    public static bool LogisticianOpen = false;
    public static bool AbilityWheelOpen = false;
    public static int LogisticianBladeSupply = LogisticianMaxSupply;
    public static int LogisticianGasSupply = LogisticianMaxSupply;
    public static bool isVeteranSet = false;
    public static bool HorseAutorun = false;
    public static bool EmHUD = false ; //added by Snake for EM HUD 26 May 24
    public static int DetailDistance;  // Added by Snake for Terrain Detail Slider 26 may 24
    public static int DetailDensity;  // Added by Snake for Terrain Detail Slider 27 may 24


    public static List<string> sceneNames = new List<string>();
    public static bool IsSceneInBuild(string sceneName) { return sceneNames.Contains(sceneName); }
}
