using ApplicationManagers;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    class MultiplayerSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "Multiplayer.json"; } }
        public static string PrivateLobbyPrefix = "private343";
        public static string PublicAppId = "28521206-90d0-41b1-93b0-f35460fef0b6";
        public IntSetting LobbyMode = new IntSetting(0);
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

        public void ConnectServer(MultiplayerRegion region)
        {
            PhotonNetwork.Disconnect();
            string address;
            if (AppIdMode.Value == (int)AppIdModeType.Public)
            {
                address = PublicAddresses[region];
                CurrentMultiplayerServerType = MultiplayerServerType.Public;
                PhotonNetwork.ConnectToMaster(address, DefaultPort, string.Empty);
                PhotonNetwork.GameVersion = GetCurrentLobby();

                /*
                address = CloudAddresses[region];
                CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
                PhotonNetwork.NetworkingClient.AppId = PublicAppId;
                PhotonNetwork.NetworkingClient.AppVersion = GetCurrentLobby();
                PhotonNetwork.ConnectToRegion(address);
                */
            }
            else
            {
                address = CloudAddresses[region];
                CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
                PhotonNetwork.NetworkingClient.AppId = CustomAppId.Value;
                PhotonNetwork.NetworkingClient.AppVersion = GetCurrentLobby();
                PhotonNetwork.ConnectToRegion(address);
            }
        }

        public string GetCurrentLobby()
        {
            if (LobbyMode.Value == (int)LobbyModeType.Public)
                return ApplicationConfig.LobbyVersion;
            if (LobbyMode.Value == (int)LobbyModeType.Private)
                return PrivateLobbyPrefix + ApplicationConfig.LobbyVersion;
            return CustomLobby.Value;
        }

        public void ConnectLAN()
        {
            PhotonNetwork.Disconnect();
            if (PhotonNetwork.ConnectToMaster(LanIP.Value, LanPort.Value, string.Empty))
            {
                PhotonNetwork.GameVersion = GetCurrentLobby();
                CurrentMultiplayerServerType = MultiplayerServerType.LAN;
            }
        }

        public void ConnectOffline()
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.OfflineMode = true;
            CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
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
        Private,
        Custom
    }

    public enum AppIdModeType
    {
        Public,
        Custom
    }
}