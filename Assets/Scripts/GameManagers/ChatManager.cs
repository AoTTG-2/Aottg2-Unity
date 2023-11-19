using System.Collections.Generic;
using UnityEngine;
using Weather;
using UI;
using Utility;
using CustomSkins;
using CustomLogic;
using ApplicationManagers;
using System.Diagnostics;
using Settings;
using Anticheat;
using Photon.Realtime;
using Photon.Pun;

namespace GameManagers
{
    class ChatManager : MonoBehaviour
    {
        private static ChatManager _instance;
        public static List<string> Lines = new List<string>();
        public static List<string> FeedLines = new List<string>();
        private static readonly int MaxLines = 30;
        public static Dictionary<ChatTextColor, string> ColorTags = new Dictionary<ChatTextColor, string>();

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
        }
        
        public static void Reset()
        {
            Lines.Clear();
            FeedLines.Clear();
            LoadTheme();
        }

        public static void Clear()
        {
            Lines.Clear();
            FeedLines.Clear();
            GetChatPanel().Sync();
            var feedPanel = GetFeedPanel();
            if (feedPanel != null)
                feedPanel.Sync();
        }

        public static bool IsChatActive()
        {
            return GetChatPanel().IsInputActive();
        }

        public static bool IsChatAvailable()
        {
            return SceneLoader.SceneName == SceneName.InGame && UIManager.CurrentMenu != null && UIManager.CurrentMenu is InGameMenu;
        }

        public static void SendChatAll(string message, ChatTextColor color = ChatTextColor.Default)
        {
            message = GetColorString(message, color);
            RPCManager.PhotonView.RPC("ChatRPC", RpcTarget.All, new object[] { message });
        }

        public static void SendChat(string message, Player player, ChatTextColor color = ChatTextColor.Default)
        {
            message = GetColorString(message, color);
            RPCManager.PhotonView.RPC("ChatRPC", player, new object[] { message });
        }

        public static void OnChatRPC(string message, PhotonMessageInfo info)
        {
            if (InGameManager.MuteText.Contains(info.Sender.ActorNumber))
                return;
            AddLine(message, info.Sender.ActorNumber);
        }

        public static void AddLine(string line, ChatTextColor color = ChatTextColor.Default)
        {
            AddLine(GetColorString(line, color));
        }

        public static void AddLine(string line, int senderID)
        {
            line = GetIDString(senderID) + line;
            AddLine(line);
        }

        public static void AddLine(string line)
        {
            line = line.FilterSizeTag();
            Lines.Add(line);
            if (Lines.Count > MaxLines)
                Lines.RemoveAt(0);
            if (IsChatAvailable())
            {
                var panel = GetChatPanel();
                if (panel != null)
                    panel.AddLine(line);
            }
        }

        public static void AddFeed(string line)
        {
            if (!IsChatAvailable())
                return;
            var feed = GetFeedPanel();
            if (feed == null)
            {
                AddLine(line);
                return;
            }
            line = line.FilterSizeTag();
            FeedLines.Add(line);
            if (FeedLines.Count > MaxLines)
                FeedLines.RemoveAt(0);
            feed.AddLine(line);
        }

        protected static void LoadTheme()
        {
            ColorTags.Clear();
            foreach (ChatTextColor color in Util.EnumToList<ChatTextColor>())
            {
                Color c = UIManager.GetThemeColor("ChatPanel", "TextColor", color.ToString());
                ColorTags.Add(color, string.Format("{0:X2}{1:X2}{2:X2}", (int)(c.r * 255), (int)(c.g * 255), (int)(c.b * 255)));
            }
        }

        public static void HandleInput(string input)
        {
            if (input == string.Empty)
                return;
            if (input.StartsWith("/"))
            {
                if (input.Length == 1)
                    return;
                string[] args = input.Substring(1).Split(' ');
                if (args.Length > 0)
                    HandleCommand(args);
            }
            else
            {
                string name = PhotonNetwork.LocalPlayer.GetStringProperty(PlayerProperty.Name);
                SendChatAll(name + ": " + input);
            }
            CustomLogicManager.Evaluator.OnChatInput(input);
        }

        private static void HandleCommand(string[] args)
        {
            if (args[0] == "restart")
            {
                if (CheckMC())
                    InGameManager.RestartGame();
            }
            else if (args[0] == "clear")
                Clear();
            else if (args[0] == "reviveall")
            {
                if (CheckMC())
                {
                    RPCManager.PhotonView.RPC("SpawnPlayerRPC", RpcTarget.All, new object[] { false });
                    SendChatAll("All players have been revived by master client.", ChatTextColor.System);
                }
            }
            else if (args[0] == "revive")
            {
                if (CheckMC())
                {
                    var player = GetPlayer(args);
                    if (player != null)
                    {
                        RPCManager.PhotonView.RPC("SpawnPlayerRPC", player, new object[] { false });
                        SendChat("You have been revived by master client.", player, ChatTextColor.System);
                        AddLine(player.GetStringProperty(PlayerProperty.Name) + " has been revived.", ChatTextColor.System);
                    }
                }
            }
            else if (args[0] == "pm")
            {
                var player = GetPlayer(args);
                if (args.Length > 2 && player != null)
                {
                    SendChat("From " + PhotonNetwork.LocalPlayer.GetStringProperty(PlayerProperty.Name) + ": " + args[2], player);
                    AddLine("To " + player.GetStringProperty(PlayerProperty.Name) + ": " + args[2]);
                }
            }
            else if (args[0] == "kick")
            {
                if (CheckMC())
                {
                    var player = GetPlayer(args);
                    if (player != null)
                        KickPlayer(player);
                }
            }
            else if (args[0] == "maxplayers")
            {
                if (CheckMC())
                {
                    int players;
                    if (args.Length > 1 && int.TryParse(args[1], out players) && players >= 0)
                    {
                        PhotonNetwork.CurrentRoom.MaxPlayers = players;
                        AddLine("Max players set to " + players.ToString() + ".", ChatTextColor.System);
                    }
                    else
                        AddLine("Max players must be >= 0.", ChatTextColor.Error);
                }
            }
            else if (args[0] == "mute")
            {
                var player = GetPlayer(args);
                if (player != null)
                {
                    MutePlayer(player, true);
                    MutePlayer(player, false);
                }
            }
            else if (args[0] == "unmute")
            {
                var player = GetPlayer(args);
                if (player != null)
                {
                    UnmutePlayer(player, true);
                    UnmutePlayer(player, false);
                }
            }
            else if (args[0] == "nextsong")
                MusicManager.NextSong();
            else if (args[0] == "pause")
            {
                if (CheckMC())
                    ((InGameManager)SceneLoader.CurrentGameManager).PauseGame();
            }
            else if (args[0] == "unpause")
            {
                if (CheckMC())
                    ((InGameManager)SceneLoader.CurrentGameManager).StartUnpauseGame();
            }
            else if (args[0] == "help")
            {
                string help = "----Command list----" + "\n";
                help += "/restart: Restart the game\n";
                help += "/clear: Clear the chat\n";
                help += "/revive [ID]: Revive the player with ID\n";
                help += "/reviveall: Revive all players\n";
                help += "/pm [ID]: Private message player with ID\n";
                help += "/kick [ID]: Kick the player with ID\n";
                help += "/mute [ID]: Mute player with ID\n";
                help += "/unmute[ID]: Unmute player with ID\n";
                help += "/maxplayers [num]: Sets max players\n";
                help += "/nextsong: Play next song in playlist\n";
                help += "/pause: Pause the multiplayer game\n";
                help += "/unpause: Unpause the multiplayer game";
                AddLine(help, ChatTextColor.System);
            }
        }

        public static void KickPlayer(Player player)
        {
            if (PhotonNetwork.IsMasterClient && player != PhotonNetwork.LocalPlayer)
            {
                AnticheatManager.KickPlayer(player);
                SendChatAll(player.GetStringProperty(PlayerProperty.Name) + " has been kicked.", ChatTextColor.System);
            }
        }

        public static void MutePlayer(Player player, bool emote)
        {
            if (player == PhotonNetwork.LocalPlayer)
                return;
            if (emote)
            {
                InGameManager.MuteEmote.Add(player.ActorNumber);
                AddLine(player.GetStringProperty(PlayerProperty.Name) + " has been muted (emote).", ChatTextColor.System);
            }
            else
            {
                InGameManager.MuteText.Add(player.ActorNumber);
                AddLine(player.GetStringProperty(PlayerProperty.Name) + " has been muted (chat).", ChatTextColor.System);
            }
        }

        public static void UnmutePlayer(Player player, bool emote)
        {
            if (player == PhotonNetwork.LocalPlayer)
                return;
            if (emote && InGameManager.MuteEmote.Contains(player.ActorNumber))
            {
                InGameManager.MuteEmote.Remove(player.ActorNumber);
                AddLine(player.GetStringProperty(PlayerProperty.Name) + " has been unmuted (emote).", ChatTextColor.System);
            }
            else if (!emote && InGameManager.MuteText.Contains(player.ActorNumber))
            {
                InGameManager.MuteText.Remove(player.ActorNumber);
                AddLine(player.GetStringProperty(PlayerProperty.Name) + " has been unmuted (chat).", ChatTextColor.System);
            }
        }

        private static Player GetPlayer(string[] args)
        {
            int id = -1;
            if (args.Length > 1 && int.TryParse(args[1], out id) && PhotonNetwork.CurrentRoom.GetPlayer(id, true) != null)
            {
                var player = PhotonNetwork.CurrentRoom.GetPlayer(id, true);
                return player;
            }
            AddLine("Invalid player ID.", ChatTextColor.Error);
            return null;
        }

        private static bool CheckMC()
        {
            if (PhotonNetwork.IsMasterClient)
                return true;
            AddLine("Must be master client to use that command.", ChatTextColor.Error);
            return false;
        }

        private static ChatPanel GetChatPanel()
        {
            return ((InGameMenu)UIManager.CurrentMenu).ChatPanel;
        }

        private static FeedPanel GetFeedPanel()
        {
            return ((InGameMenu)UIManager.CurrentMenu).FeedPanel;
        }

        public static string GetIDString(int id, bool includeMC = false)
        {
            string str = "[" + id.ToString() + "] ";
            if (includeMC)
                str = "[M]" + str;
            return GetColorString(str, ChatTextColor.ID);
        }

        public static string GetColorString(string str, ChatTextColor color)
        {
            if (color == ChatTextColor.Default)
                return str;
            return "<color=#" + ColorTags[color] + ">" + str + "</color>";
        }

        private void Update()
        {
            if (IsChatAvailable() && !InGameMenu.InMenu() && !DebugConsole.Enabled)
            {
                var chatPanel = GetChatPanel();
                var key = SettingsManager.InputSettings.General.Chat;
                if (key.GetKeyDown())
                {
                    if (chatPanel.IgnoreNextActivation)
                        chatPanel.IgnoreNextActivation = false;
                    else
                        chatPanel.Activate();
                }
            }
        }
    }

    enum ChatTextColor
    {
        Default,
        ID,
        System,
        Error,
        TeamRed,
        TeamBlue
    }
}
