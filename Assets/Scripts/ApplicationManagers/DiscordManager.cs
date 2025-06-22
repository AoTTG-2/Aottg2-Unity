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

        private static long appID = 480451799908876289;
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

        public static bool IsAuthenticated { get; private set; } = false;
        public static User CurrentUser { get; private set; }
        public static OAuth2Token CurrentToken { get; private set; }
        
        public static event Action<User> OnUserAuthenticated;
        public static event Action<string> OnAuthenticationFailed;
        public static event Action OnUserLoggedOut;

        public static void Init()
        {
            if (_instance != null)
                return;
            
            _instance = SingletonFactory.CreateSingleton(_instance);
            
            try
            {
                discord = new Discord.Discord(appID, (ulong)CreateFlags.NoRequireDiscord);
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                _instance.roomActivity = new Activity();
                _instance.mainMenuActivity = new Activity();
                
                SetupOAuth2Callbacks();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Discord SDK failed to initialize: {ex.Message}");
                Debug.LogWarning("Discord features will be disabled");
                
                discord = null;
                
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                _instance.roomActivity = new Activity();
                _instance.mainMenuActivity = new Activity();
            }
        }

        private static void SetupOAuth2Callbacks()
        {
            if (discord == null) return;
            
            try
            {
                var userManager = discord.GetUserManager();
                userManager.OnCurrentUserUpdate += OnCurrentUserUpdate;
                
                try
                {
                    var currentUser = userManager.GetCurrentUser();
                    if (currentUser.Id != 0)
                    {
                        CurrentUser = currentUser;
                        IsAuthenticated = true;
                        OnUserAuthenticated?.Invoke(currentUser);
                    }
                }
                catch
                {
                    // TODO: This a funny way to handle this, will figure something else later
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to setup OAuth2 callbacks: {ex.Message}");
            }
        }

        private static void OnCurrentUserUpdate()
        {
            try
            {
                var userManager = discord.GetUserManager();
                var user = userManager.GetCurrentUser();
                CurrentUser = user;
                IsAuthenticated = true;
                OnUserAuthenticated?.Invoke(user);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get current user: {ex.Message}");
                OnAuthenticationFailed?.Invoke(ex.Message);
            }
        }



        public static void StartOAuth2Authentication()
        {
            if (_instance == null)
            {
                Init();
            }
            
            if (_instance == null || discord == null)
            {
                Debug.LogError("DiscordManager not initialized. Call Init() first.");
                OnAuthenticationFailed?.Invoke("Discord SDK not initialized. Please wait and try again.");
                return;
            }

            if (IsAuthenticated && CurrentUser.Id != 0)
            {
                OnUserAuthenticated?.Invoke(CurrentUser);
                return;
            }

            try
            {
                var userManager = discord.GetUserManager();
                var currentUser = userManager.GetCurrentUser();
                
                if (currentUser.Id != 0)
                {
                    CurrentUser = currentUser;
                    IsAuthenticated = true;
                    OnUserAuthenticated?.Invoke(currentUser);
                    return;
                }
                else
                {

                    OnAuthenticationFailed?.Invoke("Waiting for Discord authentication. Please make sure Discord is running and you're logged in.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get Discord user: {ex.Message}");
                
                string errorMessage = "Waiting for Discord connection. ";
                if (ex.Message.Contains("NotFound"))
                {
                    errorMessage += "Please make sure Discord is running and you're logged in.";
                }
                else
                {
                    errorMessage += "Please make sure Discord is running and you're logged in.";
                }
                
                OnAuthenticationFailed?.Invoke(errorMessage);
                return;
            }
        }



        public static void Logout()
        {
            IsAuthenticated = false;
            CurrentUser = new User();
            CurrentToken = new OAuth2Token();
            OnUserLoggedOut?.Invoke();
        }

        public static string GetUserDisplayName()
        {
            if (discord == null)
                return "Discord not initialized";
            
            if (IsAuthenticated && CurrentUser.Id != 0)
            {
                return $"{CurrentUser.Username}#{CurrentUser.Discriminator}";
            }
            return "Not logged in";
        }

        public static string GetUserAvatarUrl()
        {
            if (discord == null || !IsAuthenticated || CurrentUser.Id == 0)
                return null;
                
            return $"https://cdn.discordapp.com/avatars/{CurrentUser.Id}/{CurrentUser.Avatar}.png";
        }

        public static string GetAccessToken()
        {
            if (discord == null || !IsAuthenticated || string.IsNullOrEmpty(CurrentToken.AccessToken))
                return null;
                
            return CurrentToken.AccessToken;
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

                if (IsAuthenticated)
                {
                    name = $"{GetUserDisplayName()} | {name}";
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