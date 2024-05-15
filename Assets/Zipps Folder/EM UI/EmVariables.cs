using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EmVariables
{
    public static Player SelectedPlayer;
    public static bool LogisticianOpen = false;

    public static int LogisticianBladeSupply = 4;
    public static int LogisticianGasSupply = 4;

    public static List<string> sceneNames = new List<string>();
    public static bool IsSceneInBuild(string sceneName) { return sceneNames.Contains(sceneName); }
}
