using ApplicationManagers;
using Events;
using Photon.Realtime;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using GameManagers;

namespace Anticheat
{
    class AnticheatManager : MonoBehaviour
    {
        private static AnticheatManager _instance;
        private static readonly Dictionary<int, Dictionary<PhotonEventType, BaseEventFilter>> _IdToEventFilters = new();
        private static readonly BallotBox kicks = new();

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            EventManager.OnLoadScene += OnLoadScene;
        }

        private static void OnLoadScene(SceneName sceneName) => _IdToEventFilters.Clear();

        public static bool CheckPhotonEvent(Player sender, PhotonEventType eventType, object[] data)
        {
            if (!_IdToEventFilters.ContainsKey(sender.ActorNumber))
                _IdToEventFilters.Add(sender.ActorNumber, new Dictionary<PhotonEventType, BaseEventFilter>());
            var filters = _IdToEventFilters[sender.ActorNumber];
            if (!filters.ContainsKey(eventType))
            {
                if (eventType == PhotonEventType.Instantiate)
                    filters.Add(eventType, new InstantiateEventFilter(sender, eventType));
            }
            return filters[eventType].CheckEvent(data);
        }

        public static void KickPlayer(Player player, bool ban = false, string reason = "")
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            if (PhotonNetwork.IsMasterClient && player == PhotonNetwork.LocalPlayer && reason != string.Empty)
            {
                DebugConsole.Log("Attempting to ban myself for: " + reason + ", please report this to the devs.", true);
                return;
            }
            PhotonNetwork.DestroyPlayerObjects(player);
            PhotonNetwork.CloseConnection(player);
            if (reason != string.Empty)
            {
                DebugConsole.Log("Player " + player.ActorNumber.ToString() + " was autobanned. Reason:" + reason, true);
            }
        }

        public static BallotBox.Result TryVoteKickPlayer(Player voter, Player target)
        {
            var result = kicks.TryCastBallot(voter, target);
            if (result.IsSuccess) KickPlayer(target);
            return result;
        }

        public static void ResetVoteKicks(Player voter) => kicks.ResetBallots(voter);
    }

    public enum PhotonEventType
    {
        Instantiate,
        RPC
    }

    class BallotBox
    {
        private const int ConcurrentVoteLimit = 1;
        private static readonly List<Player> removals = new();

        private readonly Dictionary<Player, HashSet<Ballot>> BallotsByTargetPlayer = new();
        private readonly Dictionary<Player, DateTime> LastBallotCastTimestampByPlayer = new();
        private readonly TimeSpan BallotTimeout = TimeSpan.FromMinutes(5.0);
        private readonly TimeSpan BallotCooldown = TimeSpan.FromMinutes(1.0);

        public Result TryCastBallot(Player voter, Player target)
        {
            RemoveOldBallots();

            var required = PhotonNetwork.CurrentRoom.PlayerCount / 2 + 1;
            var hasBucket = BallotsByTargetPlayer.TryGetValue(target, out var ballots);

            var cast = ballots != null ? ballots.Count : 0;

            if (!PhotonNetwork.IsMasterClient) return Result.MissingAuthority((cast, required), target);
            if (target == PhotonNetwork.LocalPlayer || target == voter) return Result.InvalidTarget((cast, required), target);
            if (HasCooldown(voter)) return Result.UnderCooldown((cast, required), BallotCooldown);
            if (CountBallotsCast(voter) >= ConcurrentVoteLimit) return Result.ExceededConcurrentVotesLimit((cast, required));

            if (hasBucket)
                ballots.Add(voter);
            else
                BallotsByTargetPlayer.Add(target, ballots = new() { voter });

            LastBallotCastTimestampByPlayer.TryAdd(voter, DateTime.UtcNow);

            cast = ballots.Count;
            if (cast >= required)
            {
                BallotsByTargetPlayer.Remove(target);
                return Result.Success(required, target);
            }
            else return Result.InsufficientVotes((cast, required), target);
        }

        public void ResetBallots(Player player)
        {
            foreach (var ballots in BallotsByTargetPlayer.Values)
                ballots.Remove(player);
        }

        private void RemoveOldBallots()
        {
            removals.Clear();

            foreach (var kvp in BallotsByTargetPlayer)
            {
                var target = kvp.Key;
                var ballots = kvp.Value;
                ballots.RemoveWhere(ballot => DateTime.UtcNow - ballot.Timestamp > BallotTimeout);
                if (ballots.Count == 0) removals.Add(target);
            }

            foreach (var target in removals)
                BallotsByTargetPlayer.Remove(target);
        }

        private int CountBallotsCast(Player player)
        {
            int count = 0;

            foreach (var ballots in BallotsByTargetPlayer.Values)
            {
                if (ballots.Contains(player)) ++count;
            }

            return count;
        }

        private bool HasCooldown(Player voter) =>
            LastBallotCastTimestampByPlayer.TryGetValue(voter, out var timestamp) && DateTime.UtcNow - timestamp < BallotCooldown;

        public readonly struct Result
        {
            private readonly Type type;
            private readonly (int cast, int required) progress;
            private readonly Player target;
            private readonly TimeSpan cooldown;

            private Result(Type type, (int, int) progress, Player target, TimeSpan cooldown = default)
            {
                this.type = type;
                this.progress = progress;
                this.target = target;
                this.cooldown = cooldown;
            }

            public static Result Success(int required, Player target) =>
                new(Type.Success, (required, required), target);

            public static Result InsufficientVotes((int cast, int required) progress, Player target) =>
                new(Type.InsufficientVotes, progress, target);

            public static Result ExceededConcurrentVotesLimit((int cast, int required) progress) =>
                new(Type.ExceededConcurrentVotesLimit, progress, default);

            public static Result UnderCooldown((int cast, int required) progress, TimeSpan cooldown) =>
                new(Type.UnderCooldown, progress, default, cooldown);

            public static Result MissingAuthority((int cast, int required) progress, Player target) => new(Type.MissingAuthority, progress, target);

            public static Result InvalidTarget((int cast, int required) progress, Player target) => new(Type.InvalidTarget, progress, target);

            public bool IsSuccess => type == Type.Success;

            public override string ToString() => type switch
            {
                Type.Success => ChatManager.GetColorString($"Voted to kick {target.GetStringProperty(PlayerProperty.Name)} ({progress.cast}/{progress.required}).", ChatTextColor.System),
                Type.InsufficientVotes => ChatManager.GetColorString($"Voted to kick {target.GetStringProperty(PlayerProperty.Name)} ({progress.cast}/{progress.required}).", ChatTextColor.System),
                Type.ExceededConcurrentVotesLimit => ChatManager.GetColorString($"Cannot vote for more than {BallotBox.ConcurrentVoteLimit} players at a time ({progress.cast}/{progress.required}).", ChatTextColor.Error),
                Type.UnderCooldown => ChatManager.GetColorString($"Voting is limited to once every {cooldown.TotalMinutes:N1} minutes ({progress.cast}/{progress.required}).", ChatTextColor.Error),
                Type.MissingAuthority => ChatManager.GetColorString($"Cannot {target.GetStringProperty(PlayerProperty.Name)}: missing authority.", ChatTextColor.Error),
                Type.InvalidTarget => ChatManager.GetColorString($"Cannot kick {target.GetStringProperty(PlayerProperty.Name)}: invalid target.", ChatTextColor.Error),
                _ => ChatManager.GetColorString("Unknown error.", ChatTextColor.Error),
            };

            public enum Type
            {
                Success,
                InsufficientVotes,
                ExceededConcurrentVotesLimit,
                UnderCooldown,
                MissingAuthority,
                InvalidTarget
            }
        }

        readonly struct Ballot
        {
            public readonly Player Voter;
            public readonly DateTime Timestamp;

            public Ballot(Player player)
            {
                Voter = player;
                Timestamp = DateTime.UtcNow;
            }

            public override int GetHashCode() => Voter.GetHashCode();

            public override bool Equals(object obj)
            {
                if (obj is not Ballot other) return false;
                return Voter.Equals(other.Voter);
            }

            public override string ToString() => $"{Voter} ({Timestamp})";

            public static implicit operator Ballot(Player voter) => new(voter);
        }
    }
}