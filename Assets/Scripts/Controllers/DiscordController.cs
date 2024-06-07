using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Characters;
using Settings;
using Photon.Pun;
using GameManagers;
using Unity.VisualScripting;
using ApplicationManagers;

public class DiscordController : MonoBehaviour
{

    public Discord.Discord discord;

    private static bool instanceExists;
    private string largeImage = "aottg2-logo1";
    private long time;
    private string roomName;
    private string activityInfo;
    private int playerCount;
    private int maxPlayerCount;


    private void Awake()
    {
        if (!instanceExists)
        {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        discord = new Discord.Discord(1247921316913483888, (UInt64)Discord.CreateFlags.NoRequireDiscord);
        time = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateStatus();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        UpdateStatus();
    }

    void UpdateStatus()
    {
        try
        {
            var activityManager = discord.GetActivityManager();
            if (PhotonNetwork.CurrentRoom != null)
            {
                playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
                maxPlayerCount = PhotonNetwork.CurrentRoom.MaxPlayers;
                var activity = new Discord.Activity
                {
                    State = "Playing Aottg2!",
                    Details = "Name: " + SettingsManager.ProfileSettings.Name.Value + " Guild: " + SettingsManager.ProfileSettings.Guild.Value,
                    Assets =
                    {
                    LargeImage = largeImage,
                    SmallImage = SettingsManager.ProfileSettings.ProfileIcon.Value,
                    },
                    Timestamps =
                    {
                    Start = time,
                    },
                    Party =
                    {
                        Size =
                        {
                            CurrentSize = playerCount,
                            MaxSize = maxPlayerCount,
                        },
                    }
                };
                activityManager.UpdateActivity(activity, (res) =>
                {
                    if (res != Discord.Result.Ok)
                    {
                        Debug.Log("Discord is not connected!");
                    }
                });
            }
            else
            {
                var activity = new Discord.Activity
                {
                    State = "Playing Aottg2!",
                    Details = "Name: " + SettingsManager.ProfileSettings.Name.Value + " Guild: " + SettingsManager.ProfileSettings.Guild.Value,
                    Assets =
                    {
                    LargeImage = largeImage,
                    SmallImage = SettingsManager.ProfileSettings.ProfileIcon.Value,
                    },
                    Timestamps =
                    {
                    Start = time,
                    }
                };
                activityManager.UpdateActivity(activity, (res) =>
                {
                    if (res != Discord.Result.Ok)
                    {
                        Debug.Log("Discord is not connected!");
                    }
                });
            }





        }
        catch
        {
            Debug.Log("Discord activity update Failed");
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }
}
