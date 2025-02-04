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
                Alias = commandAttribute.Alias;
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
        private static string _lastPartialName = "";
        private static int _lastSuggestionCount = 0;
        private static List<string> _currentSuggestions = new List<string>();
        private static int _currentSuggestionIndex = -1;

        private static readonly StringBuilder MessageBuilder = new StringBuilder(256);
        private static readonly StringBuilder TimeBuilder = new StringBuilder(8);
        private static readonly StringBuilder MentionBuilder = new StringBuilder(256);

        private static class SuggestionState
        {
            public static string LastPartialText = "";
            public static int LastCount = 0;
            public static List<string> CurrentSuggestions = new List<string>();
            public static int CurrentIndex = -1;
            public static SuggestionType Type = SuggestionType.None;
        }

        private enum SuggestionType
        {
            None,
            Command,
            Mention
        }

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
            return GetChatPanel().IsInputActive();
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
            
            string formattedMessage = GetIDString(info.Sender.ActorNumber) + message;
            DateTime timestamp = DateTime.UtcNow.AddSeconds(-Util.GetPhotonTimestampDifference(info.SentServerTime, PhotonNetwork.Time));
            
            AddLine(formattedMessage, ChatTextColor.Default, false, timestamp, info.Sender.ActorNumber);
        }

        public static void OnAnnounceRPC(string message)
        {
            AddLine(message, ChatTextColor.System, true);
        }

        public static void AddLine(string message, ChatTextColor color = ChatTextColor.Default, bool isSystem = false, DateTime? timestamp = null, int senderID = -1, bool isSuggestion = false)
        {
            message = message.FilterSizeTag();
            string formattedMessage = GetColorString(message, color);
            DateTime messageTime = timestamp ?? DateTime.UtcNow;

            // Add to history
            RawMessages.Add(formattedMessage);
            Colors.Add(color);
            SystemFlags.Add(isSystem);
            Timestamps.Add(messageTime);
            SenderIDs.Add(senderID);
            SuggestionFlags.Add(isSuggestion);

            if (RawMessages.Count > MaxLines)
            {
                RawMessages.RemoveAt(0);
                Colors.RemoveAt(0);
                SystemFlags.RemoveAt(0);
                Timestamps.RemoveAt(0);
                SenderIDs.RemoveAt(0);
                SuggestionFlags.RemoveAt(0);
            }

            // Update UI if available
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
                MessageBuilder.Append(line)
                            .Append('(')
                            .Append(LastExceptionCount)
                            .Append(')');
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
            TimeBuilder.Append('[')
                      .Append(localTime.Hour.ToString("D2"))
                      .Append(':')
                      .Append(localTime.Minute.ToString("D2"))
                      .Append("] ");
            
            MessageBuilder.Append(GetColorString(TimeBuilder.ToString(), ChatTextColor.System))
                         .Append(message);
            
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

        public static string GetAutocompleteSuggestion(string currentInput)
        {
            if (string.IsNullOrEmpty(currentInput))
            {
                ClearSuggestionState();
                ClearLastSuggestions();
                return null;
            }

            // Determine suggestion type
            SuggestionType newType = DetermineSuggestionType(currentInput);
            
            // If type changed, reset state
            if (newType != SuggestionState.Type)
            {
                ClearLastSuggestions();
                ClearSuggestionState();
                SuggestionState.Type = newType;
            }

            switch (newType)
            {
                case SuggestionType.Command:
                    var commandMatch = CommandRegex.Match(currentInput);
                    string command = commandMatch.Groups[1].Value.ToLower();
                    string parameters = commandMatch.Groups[2].Success ? commandMatch.Groups[2].Value : "";
                    return HandleCommandSuggestions(command, parameters);

                case SuggestionType.Mention:
                    var mentionMatch = MentionRegex.Match(currentInput);
                    if (mentionMatch.Success)
                    {
                        string partialName = mentionMatch.Groups[1].Value.ToLower();
                        return HandlePlayerMentionSuggestions(currentInput, mentionMatch.Index, partialName);
                    }
                    break;
            }

            ClearLastSuggestions();
            return null;
        }

        private static SuggestionType DetermineSuggestionType(string input)
        {
            if (input.StartsWith("/"))
                return SuggestionType.Command;
            if (input.Contains("@"))
                return SuggestionType.Mention;
            return SuggestionType.None;
        }

        private static void ClearSuggestionState()
        {
            SuggestionState.LastPartialText = "";
            SuggestionState.CurrentSuggestions.Clear();
            SuggestionState.CurrentIndex = -1;
            SuggestionState.LastCount = 0;
            SuggestionState.Type = SuggestionType.None;
        }

        private static string HandleCommandSuggestions(string partialCommand, string parameters)
        {
            if (partialCommand != _lastPartialName)
            {
                ClearLastSuggestions();
                _lastPartialName = partialCommand;
                _currentSuggestions.Clear();
                _currentSuggestionIndex = -1;
            }

            if (string.IsNullOrEmpty(parameters))
            {
                // Show command suggestions
                var matchingCommands = CommandsCache
                    .Where(cmd => !cmd.Value.IsAlias && 
                                 (partialCommand == "" || cmd.Key.ToLower().StartsWith(partialCommand.ToLower())))
                    .OrderBy(cmd => cmd.Key)
                    .ToList();

                if (_lastSuggestionCount == 0)
                {
                    ClearLastSuggestions(); // Clear previous suggestions first
                    
                    if (matchingCommands.Count > 0)
                    {
                        _currentSuggestions = matchingCommands.Select(cmd => "/" + cmd.Key).ToList();
                        
                        foreach (var cmd in matchingCommands)
                        {
                            string paramHint = string.Join(" ", cmd.Value.Parameters.Select(p => $"[{p}]"));
                            string suggestion = $"/{cmd.Key}";
                            if (!string.IsNullOrEmpty(paramHint))
                            {
                                suggestion += " " + paramHint;
                            }
                            
                            AddLine(suggestion, ChatTextColor.MyPlayer, true, isSuggestion: true);
                        }

                        _lastSuggestionCount = matchingCommands.Count;
                        UpdateChatPanel();
                    }
                }

                // Only return a tab completion if we have enough characters or cycling through empty suggestions
                if (_currentSuggestions.Count > 0 && 
                    (partialCommand.Length >= 2 || (partialCommand == "" && _currentSuggestionIndex != -1)))
                {
                    _currentSuggestionIndex = (_currentSuggestionIndex + 1) % _currentSuggestions.Count;
                    return _currentSuggestions[_currentSuggestionIndex];
                }
            }
            return null;
        }

        private static string HandlePlayerMentionSuggestions(string input, int lastAtSymbol, string partialName)
        {
            if (partialName != _lastPartialName || _lastSuggestionCount == 0)
            {
                ShowPlayerSuggestions(partialName);
            }
            
            var matchingPlayers = GetMatchingPlayers(partialName);
            if (matchingPlayers.Count == 1 && partialName.Length >= 2)
            {
                // Get the text before the @ symbol
                string prefix = input.Substring(0, lastAtSymbol);
                string playerName = matchingPlayers[0].GetStringProperty(PlayerProperty.Name)
                                            .FilterSizeTag()
                                            .StripRichText();
                return prefix + "@" + playerName;
            }
            
            return null;
        }

        private static List<Player> GetMatchingPlayers(string partial)
        {
            if (string.IsNullOrEmpty(partial)) return PhotonNetwork.PlayerList.ToList();
            
            return PhotonNetwork.PlayerList
                .Where(p => 
                {
                    string playerId = p.ActorNumber.ToString();
                    string playerName = p.GetStringProperty(PlayerProperty.Name)
                                       .FilterSizeTag()
                                       .StripRichText()
                                       .ToLower();
                    partial = partial.ToLower();
                    
                    // Match either ID or name
                    return playerId == partial || 
                           playerId.StartsWith(partial) || 
                           playerName.StartsWith(partial);
                })
                .OrderBy(p => p.ActorNumber) // Sort by ID for consistent ordering
                .ToList();
        }

        private static void ShowPlayerSuggestions(string partial)
        {
            ClearLastSuggestions();
            _lastPartialName = partial;

            var players = GetMatchingPlayers(partial);
            if (players.Count == 0)
            {
                // No matches => do NOT create any suggestion lines
                _lastSuggestionCount = 0;
                return;
            }

            // Only add the header if there's at least 1 match
            AddLine("Matching players:", ChatTextColor.MyPlayer, true, isSuggestion: true);

            foreach (var player in players)
            {
                string playerName = player.GetStringProperty(PlayerProperty.Name).FilterSizeTag();
                AddLine($"{player.ActorNumber}. {playerName}", ChatTextColor.MyPlayer, true, isSuggestion: true);
            }

            // +1 for the "Matching players:" line
            _lastSuggestionCount = players.Count + 1;
            UpdateChatPanel();
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

        [CommandAttribute("pm", "/pm [ID]: Private message player with ID")]
        private static void PrivateMessage(string[] args)
        {
            var player = GetPlayer(args);
            if (args.Length > 2 && player != null)
            {
                string[] msgArgs = new string[args.Length - 2];
                Array.ConstrainedCopy(args, 2, msgArgs, 0, msgArgs.Length);
                string message = string.Join(' ', msgArgs);

                SendChat("From " + PhotonNetwork.LocalPlayer.GetStringProperty(PlayerProperty.Name) + ": " + message, player);
                AddLine("To " + player.GetStringProperty(PlayerProperty.Name) + ": " + message);
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

        public static void ResetTabCompletion()
        {
            _currentSuggestionIndex = -1;
            _currentSuggestions.Clear();
            _lastPartialName = "";
        }

        public static void ClearLastSuggestions()
        {
            // Remove all previous suggestions
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
                }
            }
            _lastSuggestionCount = 0;
        }

        public static string GetNextTabCompletion(string currentInput)
        {
            // Reset suggestions if input is empty or no visible suggestions
            if (string.IsNullOrEmpty(currentInput) || _lastSuggestionCount == 0)
            {
                _currentSuggestions.Clear();
                _currentSuggestionIndex = -1;
                _lastPartialName = "";
                return null;
            }

            if (currentInput.StartsWith("/"))
            {
                string[] parts = currentInput.Split(' ');
                if (parts.Length == 1)
                {
                    string partialCommand = currentInput.Substring(1).ToLower();
                    if (!partialCommand.Equals(_lastPartialName))
                    {
                        return null;
                    }
                    if (_currentSuggestions.Count == 0 && _lastSuggestionCount > 0)
                    {
                        _currentSuggestions = CommandsCache
                            .Where(cmd => !cmd.Value.IsAlias && cmd.Key.ToLower().StartsWith(partialCommand))
                            .Select(cmd => "/" + cmd.Key)
                            .OrderBy(cmd => cmd)
                            .ToList();
                        _currentSuggestionIndex = -1;
                    }
                    if (_currentSuggestions.Count > 0)
                    {
                        _currentSuggestionIndex = (_currentSuggestionIndex + 1) % _currentSuggestions.Count;
                        return _currentSuggestions[_currentSuggestionIndex];
                    }
                }
                else if ((parts.Length == 2 && IsPlayerIdCommand(parts[0].Substring(1))))
                {
                    string partialId = parts[1].ToLower();
                    string beforeId = parts[0] + " ";
                    // Only allow tabcompletion if it matches the current visible suggestions
                    if (!partialId.Equals(_lastPartialName))
                    {
                        return null;
                    }

                    // Build suggestions list if it's empty but we have visible suggestions
                    if (_currentSuggestions.Count == 0 && _lastSuggestionCount > 0)
                    {
                        _currentSuggestions = GetMatchingPlayers(partialId)
                            .Select(p => beforeId + p.ActorNumber)
                            .ToList();
                        _currentSuggestionIndex = -1;
                    }

                    if (_currentSuggestions.Count > 0)
                    {
                        _currentSuggestionIndex = (_currentSuggestionIndex + 1) % _currentSuggestions.Count;
                        return _currentSuggestions[_currentSuggestionIndex];
                    }
                }
            }
            else
            {
                int lastAtSymbol = currentInput.LastIndexOf('@');
                if (lastAtSymbol != -1)
                {
                    string partialName = currentInput.Substring(lastAtSymbol + 1).ToLower();
                    // Only allow tab completion if it matches the current visible suggestions
                    if (!partialName.Equals(_lastPartialName))
                    {
                        return null;
                    }

                    // Build suggestions list if it's empty but we have visible suggestions
                    if (_currentSuggestions.Count == 0 && _lastSuggestionCount > 0)
                    {
                        string beforeMention = currentInput.Substring(0, lastAtSymbol + 1);
                        _currentSuggestions = GetMatchingPlayers(partialName)
                            .Select(p => beforeMention + p.GetStringProperty(PlayerProperty.Name).FilterSizeTag().StripRichText())
                            .ToList();
                        _currentSuggestionIndex = -1;
                    }

                    if (_currentSuggestions.Count > 0)
                    {
                        _currentSuggestionIndex = (_currentSuggestionIndex + 1) % _currentSuggestions.Count;
                        return _currentSuggestions[_currentSuggestionIndex];
                    }
                }
            }

            return null;
        }

        private static bool IsPlayerIdCommand(string command)
        {
            string[] playerIdCommands = new string[] {
                "pm", "kick", "ban", "mute", "unmute", "revive"
            };
            return playerIdCommands.Contains(command.ToLower());
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
                var matchingPlayers = PhotonNetwork.PlayerList
                    .Where(p => 
                    {
                        string playerId = p.ActorNumber.ToString();
                        string playerName = p.GetStringProperty(PlayerProperty.Name)
                                           .FilterSizeTag()
                                           .StripRichText()
                                           .ToLower();
                        mention = mention.ToLower();
                        
                        return playerId == mention || playerName.StartsWith(mention);
                    })
                    .ToList();

                if (matchingPlayers.Count == 1)
                {
                    string playerName = matchingPlayers[0].GetStringProperty(PlayerProperty.Name).FilterSizeTag();
                    string coloredName = GetColorString("@" + playerName, ChatTextColor.MyPlayer);
                    MentionBuilder.Remove(index, endIndex - index - 1)
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
