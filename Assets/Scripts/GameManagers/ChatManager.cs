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
using Map;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace GameManagers
{
    class ChatManager : MonoBehaviour
    {
        private static readonly Regex CommandRegex = new Regex(@"^/(\w+)(?:\s+(.*))?$", RegexOptions.Compiled);
        private static readonly Regex MentionRegex = new Regex(@"@(\w*)$", RegexOptions.Compiled);
        private static readonly Regex ParamRegex = new Regex(@"\[([^\]]+)\]", RegexOptions.Compiled);
        private static readonly Regex RichTextPattern = new Regex(@"<[^>]+>|</[^>]+>", RegexOptions.Compiled);

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        class CommandAttribute : Attribute
        {
            public string Name { get; private set; }
            public string Description { get; private set; }
            public string Alias { get; set; } = null;
            public MethodInfo Command { get; set; } = null;
            public bool IsAlias { get; set; } = false;
            public string[] Parameters { get; private set; }

            public CommandAttribute(CommandAttribute commandAttribute)
            {
                Name = commandAttribute.Name;
                Description = commandAttribute.Description;
                Alias = commandAttribute.Alias;
                Command = commandAttribute.Command;
            }

            public CommandAttribute(string name, string description)
            {
                Name = name;
                Description = description;
                Parameters = ParamRegex.Matches(description)
                                     .Cast<Match>()
                                     .Select(m => m.Groups[1].Value)
                                     .ToArray();
            }
        }

        private static ChatManager _instance;
        public static List<string> RawMessages = new List<string>();
        public static List<int> SenderIDs = new List<int>();
        public static List<ChatTextColor> Colors = new List<ChatTextColor>();
        public static List<bool> SystemFlags = new List<bool>();
        public static List<DateTime> Timestamps = new List<DateTime>();
        public static List<bool> SuggestionFlags = new List<bool>();
        public static List<string> FeedLines = new List<string>();
        private static readonly int MaxLines = 30;
        public static Dictionary<ChatTextColor, string> ColorTags = new Dictionary<ChatTextColor, string>();
        private static readonly Dictionary<string, CommandAttribute> CommandsCache = new Dictionary<string, CommandAttribute>();
        private static string LastException;
        private static int LastExceptionCount;

        private static readonly StringBuilder MessageBuilder = new StringBuilder(256);
        private static readonly StringBuilder TimeBuilder = new StringBuilder(8);
        private static readonly StringBuilder MentionBuilder = new StringBuilder(256);

        private static class SuggestionState
        {
            public static string PartialText = "";
            public static List<string> Suggestions = new List<string>();
            public static int CurrentIndex = -1;
            public static SuggestionType Type = SuggestionType.None;
            
            public static void Clear()
            {
                PartialText = "";
                Suggestions.Clear();
                CurrentIndex = -1;
                Type = SuggestionType.None;
            }
        }

        private enum SuggestionType
        {
            None,
            Command,
            Mention,
            PlayerID
        }

        private static readonly string[] PlayerIdCommands = new string[] {
            "kick", "ban", "mute", "unmute", "revive", "pm"
        };

        // Add new lists for PM tracking
        public static List<bool> PrivateFlags = new List<bool>();
        public static List<int> PMPartnerIDs = new List<int>();

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
            Clear();
            LoadTheme();
        }

        public static void Clear()
        {
            RawMessages.Clear();
            Colors.Clear();
            SystemFlags.Clear();
            Timestamps.Clear();
            SenderIDs.Clear();
            SuggestionFlags.Clear();
            LastException = string.Empty;
            LastExceptionCount = 0;
            FeedLines.Clear();

            if (IsChatAvailable())
            {
                var chatPanel = GetChatPanel();
                if (chatPanel != null)
                    chatPanel.Sync();
                    
                var feedPanel = GetFeedPanel();
                if (feedPanel != null)
                    feedPanel.Sync();
            }
        }

        public static bool IsChatActive()
        {
            var chatPanel = GetChatPanel();
            return chatPanel != null && chatPanel.IsInputActive();
        }

        public static bool IsChatAvailable()
        {
            return SceneLoader.SceneName == SceneName.InGame && UIManager.CurrentMenu != null && UIManager.CurrentMenu is InGameMenu;
        }

        public static void SendChatAll(string message, ChatTextColor color = ChatTextColor.Default)
        {
            string formattedMessage = GetColorString(message, color);
            RPCManager.PhotonView.RPC("ChatRPC", RpcTarget.All, new object[] { formattedMessage });
        }

        public static void SendChat(string message, Player player, ChatTextColor color = ChatTextColor.Default)
        {
            string formattedMessage = GetColorString(message, color);
            RPCManager.PhotonView.RPC("ChatRPC", player, new object[] { formattedMessage });
        }

        public static void OnChatRPC(string message, PhotonMessageInfo info)
        {
            if (InGameManager.MuteText.Contains(info.Sender.ActorNumber))
                return;
            string clickableId = $"<link=\"{info.Sender.ActorNumber}\">{GetColorString($"[{info.Sender.ActorNumber}]", ChatTextColor.ID)}</link>";
            string formattedMessage = $"{clickableId} {message}";
            DateTime timestamp = DateTime.UtcNow.AddSeconds(-Util.GetPhotonTimestampDifference(info.SentServerTime, PhotonNetwork.Time));
            AddLine(formattedMessage, ChatTextColor.Default, false, timestamp, info.Sender.ActorNumber);
        }

        public static void OnAnnounceRPC(string message)
        {
            AddLine(message, ChatTextColor.System, true);
        }

        public static void AddLine(string message, ChatTextColor color = ChatTextColor.Default, bool isSystem = false, DateTime? timestamp = null, int senderID = -1, bool isSuggestion = false, bool isPM = false, int pmPartnerID = -1)
        {
            message = message.FilterSizeTag();
            string formattedMessage = GetColorString(message, color);
            DateTime messageTime = timestamp ?? DateTime.UtcNow;
            RawMessages.Add(formattedMessage);
            Colors.Add(color);
            SystemFlags.Add(isSystem);
            Timestamps.Add(messageTime);
            SenderIDs.Add(senderID);
            SuggestionFlags.Add(isSuggestion);
            PrivateFlags.Add(isPM);
            PMPartnerIDs.Add(pmPartnerID);
            if (RawMessages.Count > MaxLines)
            {
                RawMessages.RemoveAt(0);
                Colors.RemoveAt(0);
                SystemFlags.RemoveAt(0);
                Timestamps.RemoveAt(0);
                SenderIDs.RemoveAt(0);
                SuggestionFlags.RemoveAt(0);
                PrivateFlags.RemoveAt(0);
                PMPartnerIDs.RemoveAt(0);
            }
            if (IsChatAvailable())
            {
                var panel = GetChatPanel();
                if (panel != null)
                    panel.AddLine(GetFormattedMessage(formattedMessage, messageTime, isSuggestion));
            }
        }

        public static void AddException(string line)
        {
            if (line == LastException)
            {
                LastExceptionCount++;
                MessageBuilder.Clear();
                MessageBuilder.Append(line).Append('(').Append(LastExceptionCount).Append(')');
                string formattedMessage = GetColorString(MessageBuilder.ToString(), ChatTextColor.Error);
                ReplaceLastLine(formattedMessage, ChatTextColor.Error, true);
            }
            else
            {
                LastException = line;
                LastExceptionCount = 0;
                AddLine(line, ChatTextColor.Error, true);
            }
        }

        private static void ReplaceLastLine(string message, ChatTextColor color, bool isSystem)
        {
            if (RawMessages.Count > 0)
            {
                DateTime timestamp = DateTime.UtcNow;
                int lastIndex = RawMessages.Count - 1;
                
                RawMessages[lastIndex] = message;
                Colors[lastIndex] = color;
                SystemFlags[lastIndex] = isSystem;
                Timestamps[lastIndex] = timestamp;
                SenderIDs[lastIndex] = -1;
                SuggestionFlags[lastIndex] = false;
                PrivateFlags[lastIndex] = false;
                PMPartnerIDs[lastIndex] = -1;

                if (IsChatAvailable())
                {
                    var panel = GetChatPanel();
                    if (panel != null)
                        panel.ReplaceLastLine(GetFormattedMessage(message, timestamp, false));
                }
            }
            else
            {
                AddLine(message, color, isSystem);
            }
        }

        public static string GetFormattedMessage(string message, DateTime timestamp, bool isSuggestion = false)
        {
            if (!SettingsManager.UISettings.ShowChatTimestamp.Value || isSuggestion)
                return message;
            MessageBuilder.Clear();
            TimeBuilder.Clear();
            DateTime localTime = timestamp.ToLocalTime();
            TimeBuilder.Append('[').Append(localTime.Hour.ToString("D2")).Append(':').Append(localTime.Minute.ToString("D2")).Append("] ");
            MessageBuilder.Append(GetColorString(TimeBuilder.ToString(), ChatTextColor.System)).Append(message);
            return MessageBuilder.ToString();
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

        public static void IsTalking(Player player, bool isSpeaking)
        {
            if (!IsChatAvailable())
                return;
            var voiceChatPanel = GetVoiceChatPanel();
            if (voiceChatPanel == null)
                return;
            if (isSpeaking)
                voiceChatPanel.AddPlayer(player);
            else
                voiceChatPanel.RemovePlayer(player);
        }

        public static void LoadTheme()
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
            var response = CustomLogicManager.Evaluator.OnChatInput(input);
            if (response is bool && ((bool)response == true))
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
                string processedMessage = ProcessMentions(input);
                SendChatAll(name + ": " + processedMessage);
            }
        }

        private static string ProcessMentions(string message)
        {
            int index = message.IndexOf('@');
            if (index == -1) return message;
            MentionBuilder.Clear();
            MentionBuilder.Append(message);
            while (index != -1)
            {
                int endIndex = MentionBuilder.ToString().IndexOf(' ', index);
                if (endIndex == -1)
                    endIndex = MentionBuilder.Length;
                string mention = MentionBuilder.ToString(index + 1, endIndex - index - 1);
                if (string.IsNullOrWhiteSpace(mention))
                {
                    index = MentionBuilder.ToString().IndexOf('@', index + 1);
                    continue;
                }
                var matchingPlayers = PhotonNetwork.PlayerList
                    .Where(p => 
                    {
                        string playerId = p.ActorNumber.ToString();
                        string playerName = p.GetStringProperty(PlayerProperty.Name).FilterSizeTag().StripRichText().ToLower();
                        mention = mention.ToLower();
                        return playerId == mention || playerName == mention;
                    })
                    .ToList();

                if (matchingPlayers.Count == 1)
                {
                    string playerName = matchingPlayers[0].GetStringProperty(PlayerProperty.Name).FilterSizeTag();
                    string coloredName = GetColorString("@" + playerName, ChatTextColor.MyPlayer);
                    MentionBuilder.Remove(index, endIndex - index)
                                 .Insert(index, coloredName);
                    index = MentionBuilder.ToString().IndexOf('@', index + coloredName.Length);
                }
                else
                {
                    index = MentionBuilder.ToString().IndexOf('@', index + 1);
                }
            }
            
            return MentionBuilder.ToString();
        }

        private static string FormatChatMessage(string message)
        {
            return message;
        }

        public static void UpdateChatPanel()
        {
            if (IsChatAvailable())
            {
                var panel = GetChatPanel();
                if (panel != null)
                    panel.Sync();
            }
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

        [CommandAttribute("closelobby", "/closelobby: Kicks all players and ends the lobby.")]
        private static void CloseLobby(string[] args)
        {
            if (CheckMC())
            {
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    if (!player.IsLocal)
                    {
                        KickPlayer(player, false);
                    }
                }
                _instance.StartCoroutine(_instance.WaitAndLeave());
            }
        }

        private IEnumerator WaitAndLeave()
        {
            yield return new WaitForSeconds(2f);
            InGameManager.LeaveRoom();
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

        [CommandAttribute("kick", "/kick [ID]: Kick the player with ID")]
        private static void Kick(string[] args)
        {
            var player = GetPlayer(args);
            if (player == null) return;
            if (PhotonNetwork.IsMasterClient)
                KickPlayer(player);
            else if (CanVoteKick(player))
                RPCManager.PhotonView.RPC(nameof(RPCManager.VoteKickRPC), RpcTarget.MasterClient, new object[] { player.ActorNumber });
        }

        [CommandAttribute("ban", "/ban [ID]: Ban the player with ID")]
        private static void Ban(string[] args)
        {
            var player = GetPlayer(args);
            if (player == null) return;
            if (PhotonNetwork.IsMasterClient)
                KickPlayer(player, ban: true);
        }

        private static bool CanVoteKick(Player player)
        {
            if (!SettingsManager.InGameCurrent.Misc.AllowVoteKicking.Value)
            {
                AddLine("Server does not allow vote kicking.", ChatTextColor.Error);
                return false;
            }
            if (player == PhotonNetwork.LocalPlayer)
            {
                AddLine("Cannot vote to kick yourself.", ChatTextColor.Error);
                return false;
            }
            if (player.IsMasterClient)
            {
                AddLine("Cannot vote to kick the Master Client.", ChatTextColor.Error);
                return false;
            }
            return true;
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
            MusicManager.ChatNextSong();
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

        [CommandAttribute("resetkd", "/resetkd: Reset your own stats.")]
        private static void Resetkd(string[] args)
        {
            InGameManager.ResetPlayerKD(PhotonNetwork.LocalPlayer);
        }

        [CommandAttribute("resetkdall", "/resetkdall: Reset all player stats.")]
        private static void Resetkdall(string[] args)
        {
            RPCManager.PhotonView.RPC("ResetKDRPC", RpcTarget.All);
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

        [CommandAttribute("savechat", "/savechat: Save chat history to Downloads folder")]
        private static void SaveChatHistory(string[] args)
        {
            var messages = RawMessages.Select((msg, i) => 
                System.Text.RegularExpressions.Regex.Replace(
                    GetFormattedMessage(msg, Timestamps[i], false), 
                    "<.*?>", 
                    string.Empty
                ));
            var (success, message) = Util.SaveChatHistory(messages);
            AddLine(message, success ? ChatTextColor.System : ChatTextColor.Error);
        }

        [CommandAttribute("verifychat", "/verifychat [filename]: Verify if a chat history file has been modified")]
        private static void VerifyChatHistory(string[] args)
        {
            if (args.Length != 2)
            {
                AddLine("Usage: /verifychat [filename]", ChatTextColor.Error);
                return;
            }

            var (success, message) = Util.VerifyChatHistory(args[1]);
            AddLine(message, success ? ChatTextColor.System : ChatTextColor.Error);
        }

        public static void KickPlayer(Player player, bool print = true, bool ban = false, string reason = ".")
        {
            if (PhotonNetwork.IsMasterClient && player != PhotonNetwork.LocalPlayer)
            {
                AnticheatManager.KickPlayer(player, ban);
                if (print)
                {
                    if (ban)
                        SendChatAll($"{player.GetStringProperty(PlayerProperty.Name)} has been banned{reason}", ChatTextColor.System);
                    else
                        SendChatAll($"{player.GetStringProperty(PlayerProperty.Name)} has been kicked{reason}", ChatTextColor.System);
                }
            }
        }

        public static void VoteKickPlayer(Player voter, Player target)
        {
            if (target == null) return;
            if (target.IsMasterClient) return;
            if (!PhotonNetwork.IsMasterClient) return;
            if (!SettingsManager.InGameCurrent.Misc.AllowVoteKicking.Value) return;

            var result = AnticheatManager.TryVoteKickPlayer(voter, target);
            var msg = result.ToString();
            RPCManager.PhotonView.RPC(nameof(RPCManager.AnnounceRPC), voter, new object[] { msg });
            if (result.IsSuccess)
                SendChatAll(target.GetStringProperty(PlayerProperty.Name) + " has been vote kicked.", ChatTextColor.System);
        }

        public static void MutePlayer(Player player, string muteType)
        {
            if (player == PhotonNetwork.LocalPlayer) return;

            if (muteType == "Emote")
            {
                InGameManager.MuteEmote.Add(player.ActorNumber);
            }
            else if (muteType == "Text")
            {
                InGameManager.MuteText.Add(player.ActorNumber);
            }
            else if (muteType == "Voice")
            {
                InGameManager.MuteVoiceChat.Add(player.ActorNumber);
            }

            AddLine($"{player.GetStringProperty(PlayerProperty.Name)} has been muted ({muteType}).", ChatTextColor.System);
        }

        public static void UnmutePlayer(Player player, string muteType)
        {
            if (player == PhotonNetwork.LocalPlayer) return;

            if (muteType == "Emote" && InGameManager.MuteEmote.Contains(player.ActorNumber))
            {
                InGameManager.MuteEmote.Remove(player.ActorNumber);
            }
            else if (muteType == "Text" && InGameManager.MuteText.Contains(player.ActorNumber))
            {
                InGameManager.MuteText.Remove(player.ActorNumber);
            }
            else if (muteType == "Voice" && InGameManager.MuteVoiceChat.Contains(player.ActorNumber))
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

        public static Player GetPlayer(string[] args)
        {
            if (args.Length > 1 && int.TryParse(args[1], out int id) && 
                PhotonNetwork.CurrentRoom.GetPlayer(id, true) is Player player)
            {
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
            if (!IsChatAvailable() || UIManager.CurrentMenu == null)
                return null;
        
            var menu = UIManager.CurrentMenu as InGameMenu;
            return menu?.ChatPanel;
        }

        private static FeedPanel GetFeedPanel()
        {
            if (!IsChatAvailable() || UIManager.CurrentMenu == null)
                return null;
        
            var menu = UIManager.CurrentMenu as InGameMenu;
            return menu?.FeedPanel;
        }

        private static VoiceChatPanel GetVoiceChatPanel()
        {
            return ((InGameMenu)UIManager.CurrentMenu).VoiceChatPanel;
        }

        private static KDRPanel GetKDRPanel()
        {
            return ((InGameMenu)UIManager.CurrentMenu).KDRPanel;
        }

        public static string GetIDString(int id, bool includeMC = false, bool myPlayer = false)
        {
            string str = "[" + id.ToString() + "] ";
            if (includeMC)
                str = "[M]" + str;
            if (myPlayer)
                return GetColorString(str, ChatTextColor.MyPlayer);
            return GetColorString(str, ChatTextColor.ID);
        }

        public static string GetColorString(string str, ChatTextColor color)
        {
            if (color == ChatTextColor.Default)
                return str;
            return "<color=#" + ColorTags[color] + ">" + str + "</color>";
        }

        private static string GetTimeString(DateTime time)
        {
            return time.ToString("HH:mm");
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

        public static void HandleTyping(string input)
        {
            if (string.IsNullOrEmpty(input) || input == "@" || input == "/")
            {
                ClearLastSuggestions();
                return;
            }
            if (input.StartsWith("/"))
            {
                int spaceIndex = input.IndexOf(' ');
                string command = spaceIndex == -1 ? input.Substring(1).ToLower() : input.Substring(1, spaceIndex - 1).ToLower();
                if (string.IsNullOrEmpty(command))
                {
                    ClearLastSuggestions();
                    return;
                }
                if (spaceIndex != -1 && IsPlayerIdCommand(command))
                {
                    string partial = input.Substring(spaceIndex + 1);
                    if (partial != SuggestionState.PartialText)
                    {
                        ClearLastSuggestions();
                        SuggestionState.PartialText = partial;
                        SuggestionState.Type = SuggestionType.PlayerID;
                        var players = new List<Player>();
                        foreach (var p in PhotonNetwork.PlayerList)
                        {
                            if (string.IsNullOrEmpty(partial) || 
                                p.ActorNumber.ToString().StartsWith(partial))
                            {
                                players.Add(p);
                            }
                        }
                        players.Sort((a, b) => a.ActorNumber.CompareTo(b.ActorNumber));

                        if (players.Count > 0)
                        {
                            AddLine("Matching players:", ChatTextColor.MyPlayer, true, isSuggestion: true);
                            foreach (var player in players)
                            {
                                string name = player.GetStringProperty(PlayerProperty.Name).FilterSizeTag();
                                AddLine($"{player.ActorNumber}. {name}", ChatTextColor.MyPlayer, true, isSuggestion: true);
                            }
                            
                            SuggestionState.Suggestions.Clear();
                            foreach (var player in players)
                            {
                                SuggestionState.Suggestions.Add(player.ActorNumber.ToString());
                            }
                        }
                    }
                }
                else if (spaceIndex == -1)
                {
                    if (command != SuggestionState.PartialText)
                    {
                        ClearLastSuggestions();
                        SuggestionState.PartialText = command;
                        SuggestionState.Type = SuggestionType.Command;
                        var matchingCommands = new List<KeyValuePair<string, CommandAttribute>>();
                        foreach (var cmd in CommandsCache)
                        {
                            if (!cmd.Value.IsAlias && cmd.Key.ToLower().StartsWith(command))
                            {
                                matchingCommands.Add(cmd);
                            }
                        }
                        matchingCommands.Sort((a, b) => string.Compare(a.Key, b.Key, StringComparison.Ordinal));
                        if (matchingCommands.Count > 0)
                        {
                            foreach (var cmd in matchingCommands)
                            {
                                MessageBuilder.Clear();
                                MessageBuilder.Append('/').Append(cmd.Key);
                                
                                if (cmd.Value.Parameters.Length > 0)
                                {
                                    MessageBuilder.Append(' ');
                                    for (int i = 0; i < cmd.Value.Parameters.Length; i++)
                                    {
                                        if (i > 0) MessageBuilder.Append(' ');
                                        MessageBuilder.Append('[').Append(cmd.Value.Parameters[i]).Append(']');
                                    }
                                }
                                AddLine(MessageBuilder.ToString(), ChatTextColor.MyPlayer, true, isSuggestion: true);
                            }
                            SuggestionState.Suggestions.Clear();
                            foreach (var cmd in matchingCommands)
                            {
                                SuggestionState.Suggestions.Add(cmd.Key);
                            }
                        }
                    }
                }
            }
            else if (input.Contains("@"))
            {
                int lastAt = input.LastIndexOf('@');
                string partial = input.Substring(lastAt + 1);
                int spaceIdx = partial.IndexOf(' ');
                if (spaceIdx >= 0 || string.IsNullOrWhiteSpace(partial))
                {
                    ClearLastSuggestions();
                    SuggestionState.PartialText = "";
                    return;
                }
                if (partial != SuggestionState.PartialText)
                {
                    ClearLastSuggestions();
                    SuggestionState.PartialText = partial;
                    SuggestionState.Type = SuggestionType.Mention;
                    var players = new List<Player>();
                    string partialLower = partial.ToLower();
                    bool isNumeric = partial.All(char.IsDigit);
                    foreach (var p in PhotonNetwork.PlayerList)
                    {
                        if (isNumeric)
                        {
                            if (p.ActorNumber.ToString().StartsWith(partial))
                                players.Add(p);
                        }
                        else
                        {
                            string name = p.GetStringProperty(PlayerProperty.Name)
                                         .FilterSizeTag()
                                         .StripRichText()
                                         .ToLower();
                            if (name.StartsWith(partialLower))
                                players.Add(p);
                        }
                    }
                    players.Sort((a, b) => a.ActorNumber.CompareTo(b.ActorNumber));
                    if (players.Count > 0)
                    {
                        AddLine("Matching players:", ChatTextColor.MyPlayer, true, isSuggestion: true);
                        SuggestionState.Suggestions.Clear();
                        foreach (var player in players)
                        {
                            string name = player.GetStringProperty(PlayerProperty.Name).FilterSizeTag();
                            AddLine($"{player.ActorNumber}. {name}", ChatTextColor.MyPlayer, true, isSuggestion: true);
                            SuggestionState.Suggestions.Add(player.GetStringProperty(PlayerProperty.Name).FilterSizeTag().StripRichText());
                        }
                    }
                }
            }
            else
            {
                ClearLastSuggestions();
            }
        }

        public static void HandleTabComplete()
        {
            if (SuggestionState.Type == SuggestionType.None || 
                SuggestionState.Suggestions.Count == 0)
                return;
            var chatPanel = GetChatPanel();
            if (chatPanel == null) return;
            string currentInput = chatPanel.GetInputText();
            if ((currentInput.StartsWith("/") && SuggestionState.Type == SuggestionType.Mention) ||
                (!currentInput.StartsWith("/") && (SuggestionState.Type == SuggestionType.Command || SuggestionState.Type == SuggestionType.PlayerID)))
            {
                SuggestionState.Type = SuggestionType.None;
                SuggestionState.Suggestions.Clear();
                SuggestionState.CurrentIndex = -1;
                return;
            }
            SuggestionState.CurrentIndex++;
            if (SuggestionState.CurrentIndex >= SuggestionState.Suggestions.Count)
                SuggestionState.CurrentIndex = 0;
            string chosen = SuggestionState.Suggestions[SuggestionState.CurrentIndex];
            switch (SuggestionState.Type)
            {
                case SuggestionType.PlayerID:
                    string[] parts = currentInput.Split(' ');
                    if (parts.Length > 0)
                    {
                        string command = parts[0];
                        chatPanel.SetInputText($"{command} {chosen}");
                    }
                    break;
                case SuggestionType.Command:
                    int firstSpace = currentInput.IndexOf(' ');
                    if (firstSpace < 0) firstSpace = currentInput.Length;
                    string prefix = currentInput.Substring(0, 1);
                    string suffix = firstSpace < currentInput.Length ? currentInput.Substring(firstSpace) : "";
                    chatPanel.SetInputText(prefix + chosen + suffix);
                    break;
                case SuggestionType.Mention:
                    if (!currentInput.StartsWith("/"))
                    {
                        int lastAt = currentInput.LastIndexOf('@');
                        if (lastAt >= 0)
                        {
                            string beforeAt = currentInput.Substring(0, lastAt + 1);
                            string afterMention = "";
                            int spaceAfterAt = currentInput.IndexOf(' ', lastAt);
                            if (spaceAfterAt >= 0)
                                afterMention = currentInput.Substring(spaceAfterAt);
    
                            string playerName = chosen.Trim();
                            chatPanel.SetInputText(beforeAt + playerName + afterMention);
                        }
                    }
                    break;
            }
            chatPanel.MoveCaretToEnd();
        }

        public static void ClearLastSuggestions()
        {
            for (int i = RawMessages.Count - 1; i >= 0; i--)
            {
                if (SuggestionFlags[i])
                {
                    RawMessages.RemoveAt(i);
                    Colors.RemoveAt(i);
                    SystemFlags.RemoveAt(i);
                    Timestamps.RemoveAt(i);
                    SenderIDs.RemoveAt(i);
                    SuggestionFlags.RemoveAt(i);
                    PrivateFlags.RemoveAt(i);
                    PMPartnerIDs.RemoveAt(i);
                }
            }
            SuggestionState.Clear();

            if (IsChatAvailable())
            {
                var panel = GetChatPanel();
                if (panel != null)
                    panel.Sync();
            }
        }

        private static bool IsPlayerIdCommand(string command)
        {
            return command == "kick" || command == "ban" || command == "mute" || command == "unmute" || command == "revive" || command == "pm";
        }

        public static void SendPrivateMessage(Player target, string message)
        {
            if (target == null)
            {
                AddLine("Invalid private message target.", ChatTextColor.Error);
                return;
            }
            
            string senderName = PhotonNetwork.LocalPlayer.GetStringProperty(PlayerProperty.Name);
            RPCManager.PhotonView.RPC("PrivateChatRPC", RpcTarget.All,
                new object[] { senderName, message, PhotonNetwork.LocalPlayer.ActorNumber, target.ActorNumber });
        }

        public static void OnPrivateChatRPC(string senderName, string message, int senderID, int targetID)
        {
            int localID = PhotonNetwork.LocalPlayer.ActorNumber;
            
            if (localID == senderID)
            {
                Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(targetID);
                if (targetPlayer != null)
                {
                    string targetName = targetPlayer.GetStringProperty(PlayerProperty.Name);
                    AddLine($"{GetColorString("To ", ChatTextColor.System)}{targetName}{GetColorString(": ", ChatTextColor.System)}{message}", 
                        ChatTextColor.Default, false, DateTime.UtcNow, senderID, false, true, targetID);
                    
                    var panel = GetChatPanel();
                    if (panel != null && !panel.IsInPMMode())
                    {
                        panel.EnterPMMode(targetPlayer);
                    }
                }
            }
            else if (localID == targetID)
            {
                // Add the actual PM message
                AddLine($"{GetColorString("From ", ChatTextColor.System)}{senderName}{GetColorString(": ", ChatTextColor.System)}{message}", 
                    ChatTextColor.Default, false, DateTime.UtcNow, senderID, false, true, senderID);
                    
                var panel = GetChatPanel();
                if (panel != null)
                {
                    Player senderPlayer = PhotonNetwork.CurrentRoom.GetPlayer(senderID);
                    if (senderPlayer != null)
                    {
                        panel.AddPMPartner(senderPlayer);
                        if (!panel.IsInPMMode() || panel.GetCurrentPMTarget().ActorNumber != senderPlayer.ActorNumber)
                        {
                            AddLine($"{GetColorString("New message from ", ChatTextColor.System)}{senderName}{GetColorString(". Press Esc to view conversation.", ChatTextColor.System)}", 
                                ChatTextColor.Default, true, DateTime.UtcNow, -1, false, false, -1);
                        }
                    }
                }
            }
        }

        public static void ResetAllPMState()
        {
            var chatPanel = GetChatPanel();
            if (chatPanel != null)
            {
                chatPanel.ResetPMState();
            }
            PrivateFlags.Clear();
            PMPartnerIDs.Clear();
        }

        public static void SyncPMPartnersOnJoin()
        {
            var chatPanel = GetChatPanel();
            if (chatPanel == null) return;
            chatPanel.ResetPMState();
            for (int i = 0; i < RawMessages.Count; i++)
            {
                if (PrivateFlags[i])
                {
                    int partnerId = PMPartnerIDs[i];
                    var partner = PhotonNetwork.CurrentRoom.GetPlayer(partnerId);
                    if (partner != null)
                    {
                        chatPanel.AddPMPartner(partner);
                    }
                }
            }
        }
        public static string GetPlayerIdentifier(Player player)
        {
            string clickableId = $"<link=\"{player.ActorNumber}\">{GetColorString($"[{player.ActorNumber}]", ChatTextColor.ID)}</link>";
            return $"{clickableId} {player.GetStringProperty(PlayerProperty.Name)}";
        }

        [CommandAttribute("pm", "/pm [ID]: Send a private message to player with ID")]
        private static void PM(string[] args)
        {
            if (args.Length < 2)
            {
                AddLine("Usage: /pm [ID] [message]", ChatTextColor.Error);
                return;
            }

            var player = GetPlayer(args);
            if (player != null)
            {
                if (player == PhotonNetwork.LocalPlayer)
                {
                    AddLine("Cannot send private messages to yourself.", ChatTextColor.Error);
                    return;
                }

                var chatPanel = GetChatPanel();
                if (chatPanel != null)
                {
                    chatPanel.EnterPMMode(player);
                    
                    // If a message was provided, send it
                    if (args.Length > 2)
                    {
                        string message = string.Join(" ", args.Skip(2));
                        SendPrivateMessage(player, message);
                    }
                }
            }
        }
    }

    public enum ChatTextColor
    {
        Default,
        ID,
        MyPlayer,
        System,
        Error,
        TeamRed,
        TeamBlue
    }
}
