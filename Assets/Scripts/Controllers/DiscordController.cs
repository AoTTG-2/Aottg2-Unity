using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Characters;
using Settings;
using Photon.Pun;
using GameManagers;
using Unity.VisualScripting;
using ApplicationManagers;
using Photon.Realtime;
using UI;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class DiscordController : MonoBehaviour
{

    public Discord.Discord discord;

    private long appID = 1247921316913483888;
    private static bool instanceExists;
    private string largeImage = "aottg2-logo2";
    private long time;
    private string roomName;
    private int playerCount;
    private int maxPlayerCount;
    private RoomInfo roomInfo;

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
        discord = new Discord.Discord(appID, (UInt64)Discord.CreateFlags.NoRequireDiscord);
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

            //in game activity
            if (PhotonNetwork.CurrentRoom != null)
            {
                playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
                maxPlayerCount = PhotonNetwork.CurrentRoom.MaxPlayers;
                InGameSet settings = SettingsManager.InGameCurrent;
                roomName = settings.General.RoomName.Value;
                Player player = PhotonNetwork.LocalPlayer;
                List<string> scoreList = new List<string>();
                foreach (string property in new string[] { "Kills", "Deaths", "HighestDamage", "TotalDamage" })
                {
                    object value = player.GetCustomProperty(property);
                    string str = value != null ? value.ToString() : string.Empty;
                    scoreList.Add(str);
                }
                string score = string.Join(" / ", scoreList.ToArray());

                if (PhotonNetwork.OfflineMode) 
                {
                    var activity = new Discord.Activity
                    {
                        State = "SinglePlayer",
                        Details = "[" + SettingsManager.ProfileSettings.Guild.Value + "] " + SettingsManager.ProfileSettings.Name.Value + score,
                        Assets =
                    {
                    LargeImage = largeImage,
                    SmallImage = SettingsManager.ProfileSettings.ProfileIcon.Value.ToLower(),
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
                            MaxSize = 1,
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
                        State = roomName,
                        Details = "[" + SettingsManager.ProfileSettings.Guild.Value + "] " + SettingsManager.ProfileSettings.Name.Value + score,
                        Assets =
                    {
                    LargeImage = largeImage,
                    SmallImage = SettingsManager.ProfileSettings.ProfileIcon.Value.ToLower(),
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
            }
            //main menu activity
            else
            {
                var activity = new Discord.Activity
                {
                    State = "Creating a room!",
                    Details = "[" + SettingsManager.ProfileSettings.Guild.Value + "] " + SettingsManager.ProfileSettings.Name.Value,
                    Assets =
                    {
                    LargeImage = largeImage,
                    SmallImage = SettingsManager.ProfileSettings.ProfileIcon.Value.ToLower(),
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
