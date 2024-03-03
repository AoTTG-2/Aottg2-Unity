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
using System;
using System.Reflection;
using System.Linq;


namespace GameManagers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class CommandAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Alias { get; set; } = null;
        public MethodInfo Command { get; set; } = null;
        public bool IsAlias { get; set; } = false;

        public CommandAttribute(CommandAttribute commandAttribute)
        {
            Name = commandAttribute.Name;
            Description = commandAttribute.Description;
            Alias = commandAttribute.Alias;
            Command = commandAttribute.Command;
            Alias = commandAttribute.Alias;
        }

        public CommandAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    class ChatManager : MonoBehaviour
    {
        private static ChatManager _instance;
        public static List<string> Lines = new List<string>();
        public static List<string> FeedLines = new List<string>();
        private static readonly int MaxLines = 30;
        public static Dictionary<ChatTextColor, string> ColorTags = new Dictionary<ChatTextColor, string>();
        private static readonly Dictionary<string, CommandAttribute> CommandsCache = new Dictionary<string, CommandAttribute>();

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);

            // Read all methods, filter out methods using CommandAttribute, create mapping from name/alias to CommandAttribute for later reference.
            MethodInfo[] infos = typeof(ChatManager).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            Type cmdAttrType = typeof(CommandAttribute);

            foreach (MethodInfo info in infos)
            {
                object[] attrs = info.GetCustomAttributes(cmdAttrType, false);
                if (attrs == null) continue;

                if (attrs.Length > 0)
                {
                    foreach (object attr in attrs)
                    {
                        if (attr is CommandAttribute cmdAttr)
                        {
                            cmdAttr.Command = info;
                            
                            CommandsCache.Add(cmdAttr.Name, cmdAttr);

                            // Create second mapping from alias to cmd, has to be a separate object flagged as alias.
                            // This lets us ignore alias's later on in the help function.
                            if (cmdAttr.Alias != null)
                            {
                                CommandAttribute alias = new CommandAttribute(cmdAttr);
                                alias.IsAlias = true;
                                CommandsCache.Add(alias.Alias, alias);
                            }                                   
                        }
                    }
                }
            }

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
            if (CommandsCache.TryGetValue(args[0], out CommandAttribute cmdAttr))
            {
                MethodInfo info = cmdAttr.Command;
                if (info.IsStatic)
                {
                    info.Invoke(null, new object[1] { args });
                }

                else
                {
                    info.Invoke(_instance, new object[1] { args });
                }
            }
            else
            {
                AddLine($"Command {args[0]} not found, try /help to see a list of commands.", ChatTextColor.Error);
            }
        }

        [CommandAttribute("restart", "/restart: Restarts the game.", Alias = "r")]
        private static void Restart(string[] args)
        {
            if (CheckMC())
                InGameManager.RestartGame();
        }

        [CommandAttribute("clear", "/clear: Clears the chat window.", Alias = "c")]
        private static void Clear(string[] args)
        {
            Clear();
        }

        [CommandAttribute("reviveall", "/reviveall: Revive all players.", Alias = "rva")]
        private static void ReviveAll(string[] args)
        {
            if (CheckMC())
            {
                RPCManager.PhotonView.RPC("SpawnPlayerRPC", RpcTarget.All, new object[] { false });
                SendChatAll("All players have been revived by master client.", ChatTextColor.System);
            }
        }

        [CommandAttribute("revive", "/revive [ID]: Revives the player with ID", Alias = "rv")]
        private static void Revive(string[] args)
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

        [CommandAttribute("pm", "/pm [ID]: Private message player with ID")]
        private static void PrivateMessage(string[] args)
        {
            var player = GetPlayer(args);
            if (args.Length > 2 && player != null)
            {
                SendChat("From " + PhotonNetwork.LocalPlayer.GetStringProperty(PlayerProperty.Name) + ": " + args[2], player);
                AddLine("To " + player.GetStringProperty(PlayerProperty.Name) + ": " + args[2]);
            }
        }

        [CommandAttribute("kick", "/kick [ID]: Kick the player with ID")]
        private static void Kick(string[] args)
        {
            if (CheckMC())
            {
                var player = GetPlayer(args);
                if (player != null)
                    KickPlayer(player);
            }
        }

        [CommandAttribute("maxplayers", "/maxplayers [num]: Sets room's max player count.")]
        private static void MaxPlayers(string[] args)
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

        [CommandAttribute("mute", "/mute [ID]: Mute player with ID.")]
        private static void Mute(string[] args)
        {
            var player = GetPlayer(args);
            if (player != null)
            {
                MutePlayer(player, "Emote");
                MutePlayer(player, "Text");
                MutePlayer(player, "Voice");
            }
        }

        [CommandAttribute("unmute", "/unmute [ID]: Unmute player with ID.")]
        private static void Unmute(string[] args)
        {
            var player = GetPlayer(args);
            if (player != null)
            {
                UnmutePlayer(player, "Emote");
                UnmutePlayer(player, "Text");
                UnmutePlayer(player, "Voice");
            }
        }

        [CommandAttribute("nextsong", "/nextsong: Play next song in playlist.")]
        private static void NextSong(string[] args)
        {
            MusicManager.NextSong();
        }

        [CommandAttribute("pause", "/pause: Pause the multiplayer game.")]
        private static void Pause(string[] args)
        {
            if (CheckMC())
                ((InGameManager)SceneLoader.CurrentGameManager).PauseGame();
        }

        [CommandAttribute("unpause", "/unpause: Unpause the multiplayer game.")]
        private static void Unpause(string[] args)
        {
            if (CheckMC())
                ((InGameManager)SceneLoader.CurrentGameManager).StartUnpauseGame();
        }

        [CommandAttribute("help", "/help [page(optional)]: Displays command usage.")]
        private static void Help(string[] args)
        {
            int displayPage = 1;
            int elementsPerPage = 7;
            if (args.Length >= 2)
            {
                int.TryParse(args[1], out displayPage);
                if (displayPage == 0)
                {
                    displayPage = 1;
                }
            }

            int totalPages = (int)Math.Ceiling((double)CommandsCache.Count / elementsPerPage);
            if (displayPage < 1 || displayPage > totalPages)
            {
                AddLine($"Page {displayPage} does not exist.", ChatTextColor.Error);
                return;
            }

            List<CommandAttribute> pageElements = Util.PaginateDictionary(CommandsCache, displayPage, elementsPerPage);
            string help = "----Command list----" + "\n";
            foreach (CommandAttribute element in pageElements)
            {
                if (element.IsAlias == false)
                {
                    help += element.Description + "\n";
                }
            }

            help += $"Page {displayPage} / {totalPages}";

            AddLine(help, ChatTextColor.System);
        }

        public static void KickPlayer(Player player)
        {
            if (PhotonNetwork.IsMasterClient && player != PhotonNetwork.LocalPlayer)
            {
                AnticheatManager.KickPlayer(player);
                SendChatAll(player.GetStringProperty(PlayerProperty.Name) + " has been kicked.", ChatTextColor.System);
            }
        }

        public static void MutePlayer(Player player, string muteType)
        {
            if (player == PhotonNetwork.LocalPlayer)
                return;
            if (muteType == "emote")
            {
                InGameManager.MuteEmote.Add(player.ActorNumber);
            }
            else if (muteType == "text")
            {
                InGameManager.MuteText.Add(player.ActorNumber);
            }
            else if (muteType == "voice")
            {
                InGameManager.MuteVoiceChat.Add(player.ActorNumber);
            }

            AddLine($"{player.GetStringProperty(PlayerProperty.Name)} has been muted ({muteType}).", ChatTextColor.System);
        }

        public static void UnmutePlayer(Player player, string muteType)
        {
            if (player == PhotonNetwork.LocalPlayer)
                return;
            if (muteType == "emote" && InGameManager.MuteEmote.Contains(player.ActorNumber))
            {
                InGameManager.MuteEmote.Remove(player.ActorNumber);
            }
            else if (muteType == "text" && InGameManager.MuteText.Contains(player.ActorNumber))
            {
                InGameManager.MuteText.Remove(player.ActorNumber);
            }
            else if (muteType == "voice" && InGameManager.MuteVoiceChat.Contains(player.ActorNumber))
            {
                InGameManager.MuteVoiceChat.Remove(player.ActorNumber);
            }

            AddLine($"{player.GetStringProperty(PlayerProperty.Name)} has been unmuted ({muteType}).", ChatTextColor.System);
        }

        public static void SetPlayerVolume(Player player, float volume)
        {
            if (player == PhotonNetwork.LocalPlayer)
                return;
            if (InGameManager.VoiceChatVolumeMultiplier.ContainsKey(player.ActorNumber))
                if (InGameManager.VoiceChatVolumeMultiplier[player.ActorNumber] == volume)
                    return;
            InGameManager.VoiceChatVolumeMultiplier[player.ActorNumber] = volume;
            AddLine($"{player.GetStringProperty(PlayerProperty.Name)}'s voice volume has been set to {volume}.", ChatTextColor.System);
        }

        private static Player GetPlayer(string stringID)
        {
            int id = -1;
            if (int.TryParse(stringID, out id) && PhotonNetwork.CurrentRoom.GetPlayer(id, true) != null)
            {
                var player = PhotonNetwork.CurrentRoom.GetPlayer(id, true);
                return player;
            }
            AddLine("Invalid player ID.", ChatTextColor.Error);
            return null;
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
