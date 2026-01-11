using ApplicationManagers;
using Photon.Pun;
using Settings;
using UnityEngine;
using Utility;

public class HeadlessManager : MonoBehaviour
{
    private static HeadlessManager _instance;
    static string presetName;

    public static void Init()
    {
        _instance = SingletonFactory.CreateSingleton(_instance);

        Debug.Log("Running in headless mode!");
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-region":
                    if (i + 1 < args.Length)
                    {
                        string region = args[i + 1];
                        switch (region)
                        {
                            case "eu":
                                SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.EU);
                                break;
                            case "us":
                                SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.US);
                                break;
                            case "sa":
                                SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.SA);
                                break;
                            case "asia":
                                SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.ASIA);
                                break;
                            case "cn":
                                SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.CN);
                                break;
                        }
                    }
                    break;
                case "-preset":
                    if (i + 1 < args.Length)
                    {
                        presetName = args[i + 1];
                    }
                    break;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (SceneLoader.SceneName != SceneName.MainMenu) return;
        if (PhotonNetwork.IsConnected)
        {
            var preset = presetName;
            foreach (InGameSet set in SettingsManager.InGameSettings.InGameSets.Sets.GetItems())
            {
                if (set.Name.Value == preset)
                {
                    SettingsManager.InGameCurrent.Copy(set);
                }
            }
            SettingsManager.MultiplayerSettings.StartRoom();
        }
    }
}
