using Discord;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ApplicationManagers
{
    class DiscordManager : MonoBehaviour
    {
        private static DiscordManager _instance;
        public static Discord.Discord discord;

        private static long appID = 1247921316913483888;
        private static bool instanceExists;
        private string largeImage = "aottg2-logo2";
        private static long time;
        private string roomName;
        private int playerCount;
        private int maxPlayerCount;
        private RoomInfo roomInfo;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            try
            {
                discord = new Discord.Discord(appID, (ulong)CreateFlags.NoRequireDiscord);
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
            catch
            {
                Debug.Log("Unable to initialize discord manager.");
                Destroy(_instance);
            }
        }

        private void Update()
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

        private void UpdateStatus()
        {
            try
            {
                var activityManager = discord.GetActivityManager();

                string name = SettingsManager.ProfileSettings.Name.Value.StripHex();
                string guild = "";
                if(SettingsManager.ProfileSettings.Guild.Value != "")
                {
                    guild += "[" + SettingsManager.ProfileSettings.Guild.Value.StripHex() + "] ";
                }

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
                        var activity = new Activity
                        {
                            State = "SinglePlayer",
                            Details = guild + name + " " + score,
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
                            if (res != Result.Ok)
                            {
                                Debug.Log("Discord is not connected!");
                            }
                        });
                    }
                    else
                    {
                        var activity = new Activity
                        {
                            State = roomName,
                            Details = guild + name + " " + score,
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
                            if (res != Result.Ok)
                            {
                                Debug.Log("Discord is not connected!");
                            }
                        });
                    }
                }
                //main menu activity
                else
                {
                    var activity = new Activity
                    {
                        State = "Creating a room!",
                        Details = guild + name,
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
                        if (res != Result.Ok)
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
    }
}