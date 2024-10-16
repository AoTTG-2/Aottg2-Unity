using Discord;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
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
        private Activity roomActivity;
        private Activity mainMenuActivity;
        private string[] trackedProperties = new string[] { "Kills", "Deaths", "HighestDamage", "TotalDamage" };

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            try
            {
                discord = new Discord.Discord(appID, (ulong)CreateFlags.NoRequireDiscord);
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                _instance.roomActivity = new Activity();
                _instance.mainMenuActivity = new Activity();
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

        private void SetRoomActivity(string state, string details, string profileIcon, long startTime, int playerCount, int maxPlayers)
        {
            roomActivity.State = state;
            roomActivity.Details = details;
            roomActivity.Assets.LargeImage = largeImage;
            roomActivity.Assets.SmallImage = profileIcon;
            roomActivity.Timestamps.Start = startTime;
            roomActivity.Party.Size.CurrentSize = playerCount;
            roomActivity.Party.Size.MaxSize = maxPlayers;
        }

        private void SetMenuActivity(string state, string details, string profileIcon, long startTime)
        {
            mainMenuActivity.State = state;
            mainMenuActivity.Details = details;
            mainMenuActivity.Assets.LargeImage = largeImage;
            mainMenuActivity.Assets.SmallImage = profileIcon;
            mainMenuActivity.Timestamps.Start = startTime;
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
                string state = "Creating a room";
                string details = guild + name;
                string icon = SettingsManager.ProfileSettings.ProfileIcon.Value.ToLower();
                if (PhotonNetwork.CurrentRoom != null)
                {
                    playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
                    maxPlayerCount = PhotonNetwork.OfflineMode ? 1 : PhotonNetwork.CurrentRoom.MaxPlayers;
                    InGameSet settings = SettingsManager.InGameCurrent;
                    roomName = settings.General.RoomName.Value;
                    Player player = PhotonNetwork.LocalPlayer;
                    string score = string.Empty;
                    for (int i = 0; i < trackedProperties.Length; i++)
                    {
                        object value = player.GetCustomProperty(trackedProperties[i]);
                        score += value != null ? value.ToString() : "0";
                        if (i < trackedProperties.Length - 1)
                        {
                            score += " / ";
                        }
                    }

                    details = details + " " + score;
                    state = PhotonNetwork.OfflineMode ? "SinglePlayer" : roomName;
                    
                    activityManager.UpdateActivity(
                        new Activity()
                        {
                            State = state,
                            Details = details,
                            Assets = new ActivityAssets()
                            {
                                LargeImage = largeImage,
                                SmallImage = icon
                            },
                            Timestamps =
                            {
                                Start = time
                            },
                            Party =
                            {
                                Size =
                                {
                                    CurrentSize = playerCount,
                                    MaxSize = maxPlayerCount
                                }
                            }
                        },
                        null);
                }
                else
                {
                    activityManager.UpdateActivity(
                        new Activity()
                        {
                            State = state,
                            Details = details,
                            Assets = new ActivityAssets()
                            {
                                LargeImage = largeImage,
                                SmallImage = icon
                            },
                            Timestamps =
                            {
                                Start = time
                            }
                        },
                        null);
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