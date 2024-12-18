using ApplicationManagers;
using Discord;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using UnityEngine;
using Utility;

namespace Settings
{
    class MultiplayerSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "Multiplayer.json"; } }
        public static string VoiceRoomSuffix = "vc";
        public static string PublicAppId = "28521206-90d0-41b1-93b0-f35460fef0b6";
        public IntSetting LobbyMode = new IntSetting(0, minValue: 0, maxValue: 1);
        public IntSetting AppIdMode = new IntSetting(0);
        public StringSetting CustomLobby = new StringSetting(string.Empty);
        public StringSetting CustomAppId = new StringSetting(string.Empty);
        public StringSetting LanIP = new StringSetting(string.Empty);
        public IntSetting LanPort = new IntSetting(5055);
        public StringSetting LanPassword = new StringSetting(string.Empty);
        public MultiplayerServerType CurrentMultiplayerServerType;
        public readonly Dictionary<MultiplayerRegion, string> CloudAddresses = new Dictionary<MultiplayerRegion, string>()
        {
            { MultiplayerRegion.EU, "eu" },
            { MultiplayerRegion.US, "us" },
            { MultiplayerRegion.SA, "sa" },
            { MultiplayerRegion.ASIA, "asia" },
            { MultiplayerRegion.CN, "asia" }
        };
        public readonly Dictionary<MultiplayerRegion, string> PublicAddresses = new Dictionary<MultiplayerRegion, string>()
        {
            { MultiplayerRegion.EU, "135.125.239.180" },
            { MultiplayerRegion.US, "142.44.242.29" },
            { MultiplayerRegion.SA, "108.181.69.221" },
            { MultiplayerRegion.ASIA, "51.79.164.137" },
            { MultiplayerRegion.CN, "47.116.117.128" }
        };
        public readonly int DefaultPort = 5055;

        public bool IsConnectedToPublic()
        {
            return CurrentMultiplayerServerType == MultiplayerServerType.Public && LobbyMode.Value == (int)LobbyModeType.Public;
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
            if (VoiceChatManager.Client.Client.IsConnected)
                VoiceChatManager.Client.Disconnect();
        }

        public void ConnectServer(MultiplayerRegion region)
        {
            Disconnect();
            string address;
            if (AppIdMode.Value == (int)AppIdModeType.Public)
            {
                address = PublicAddresses[region];
                CurrentMultiplayerServerType = MultiplayerServerType.Public;
                PhotonNetwork.ConnectToMaster(address, DefaultPort, string.Empty);
                PhotonNetwork.NetworkingClient.AppVersion = GetCurrentLobby(true);
                var settings = new AppSettings();
                settings.Server = address;
                settings.Port = DefaultPort;
                settings.UseNameServer = false;
                VoiceChatManager.Client.ConnectUsingSettings(settings);
                VoiceChatManager.Client.Client.AppVersion = GetCurrentLobby(true);
            }
            else
            {
                address = CloudAddresses[region];
                CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
                PhotonNetwork.NetworkingClient.AppId = CustomAppId.Value;
                PhotonNetwork.NetworkingClient.AppVersion = GetCurrentLobby(false);
                PhotonNetwork.ConnectToRegion(address);
                var settings = new AppSettings();
                settings.AppIdVoice = CustomAppId.Value;
                settings.FixedRegion = address;
                VoiceChatManager.Client.ConnectUsingSettings(settings);
                VoiceChatManager.Client.Client.AppVersion = GetCurrentLobby(false);
            }
        }

        public void ConnectLAN()
        {
            Disconnect();
            if (PhotonNetwork.ConnectToMaster(LanIP.Value, LanPort.Value, string.Empty))
            {
                PhotonNetwork.NetworkingClient.AppVersion = GetCurrentLobby(false);
                CurrentMultiplayerServerType = MultiplayerServerType.LAN;
            }
        }

        public void ConnectOffline()
        {
            Disconnect();
            PhotonNetwork.OfflineMode = true;
            CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
        }

        public NetworkCredential GetCurrentLobby(bool isPublic)
        {
            if (LobbyMode.Value == (int)LobbyModeType.Public)
            {
                if (isPublic)
                    return ApplicationVersion.GetVersion();
                return new NetworkCredential("Public", "Public");
            }
            var credential = new NetworkCredential(CustomLobby.Value, CustomLobby.Value);
            return credential;
        }

        public void StartRoom()
        {
            InGameSet settings = SettingsManager.InGameCurrent;
            string roomName = settings.General.RoomName.Value;
            string mapName = settings.General.MapName.Value;
            string gameMode = settings.General.GameMode.Value;
            int maxPlayers = settings.General.MaxPlayers.Value;
            string password = settings.General.Password.Value;
            string passwordHash = string.Empty;
            if (password.Length > 0)
                passwordHash = Util.CreateMD5(password);
            string roomId = UnityEngine.Random.Range(0, 100000).ToString();
            var properties = new ExitGames.Client.Photon.Hashtable
            {
                { RoomProperty.Name, roomName },
                { RoomProperty.Map, mapName },
                { RoomProperty.GameMode, gameMode },
                { RoomProperty.Password, password },
                { RoomProperty.PasswordHash, passwordHash },
                { "Hash", GetHashCode(roomId + roomName)}
            };
            string[] lobbyProperties = new string[] { RoomProperty.Name, RoomProperty.Map, RoomProperty.GameMode, RoomProperty.PasswordHash };
            var roomOptions = new RoomOptions();
            roomOptions.CustomRoomProperties = properties;
            roomOptions.CustomRoomPropertiesForLobby = lobbyProperties;
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
            roomOptions.MaxPlayers = maxPlayers;
            roomOptions.BroadcastPropsChangeToAll = false;
            PhotonNetwork.CreateRoom(roomId, roomOptions);
            if (!PhotonNetwork.OfflineMode)
            {
                var vcRoomOptions = new RoomOptions();
                vcRoomOptions.CustomRoomProperties = properties;
                vcRoomOptions.CustomRoomPropertiesForLobby = lobbyProperties;
                vcRoomOptions.IsVisible = false;
                vcRoomOptions.IsOpen = true;
                vcRoomOptions.MaxPlayers = 255;
                vcRoomOptions.BroadcastPropsChangeToAll = false;
                vcRoomOptions.EmptyRoomTtl = 10;
                VoiceChatManager.Client.CreateRoom(roomId + VoiceRoomSuffix, vcRoomOptions);
            }
        }

        public void JoinRoom(string roomId, string roomName, string password)
        {
            PhotonNetwork.JoinRoom(roomId, password: password, hash: GetHashCode(roomId + roomName));
            if (!PhotonNetwork.OfflineMode)
                VoiceChatManager.Client.JoinRoom(roomId + VoiceRoomSuffix, password: password, hash: GetHashCode(roomId + roomName));
        }

        public string GetHashCode(string str)
        {
            if (!IsConnectedToPublic())
                return string.Empty;
            return ApplicationVersion.GetHashCode(str);
        }
    }

    public enum MultiplayerServerType
    {
        LAN,
        Cloud,
        Public
    }

    public enum MultiplayerRegion
    {
        EU,
        US,
        SA,
        ASIA,
        CN
    }

    public enum LobbyModeType
    {
        Public,
        Custom
    }

    public enum AppIdModeType
    {
        Public,
        Custom
    }
}
