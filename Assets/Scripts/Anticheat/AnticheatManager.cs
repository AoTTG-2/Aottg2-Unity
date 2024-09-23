using ApplicationManagers;
using Events;
using Photon.Realtime;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

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

        public static bool TryVoteKickPlayer(Player voter, Player target, out (int submitted, int required) progress)
        {
            var kick = kicks.CastBallot(voter, target, out progress);
            if (kick) KickPlayer(target);
            return kick;
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

        public bool CastBallot(Player voter, Player target, out (int submitted, int required) progress)
        {
            RemoveOldBallots();

            HashSet<Ballot> ballots = null;
            if (target != PhotonNetwork.LocalPlayer && CountBallotsCast(voter) < ConcurrentVoteLimit && !HasCooldown(voter))
            {
                if (BallotsByTargetPlayer.TryGetValue(target, out ballots))
                    ballots.Add(voter);
                else
                    BallotsByTargetPlayer.Add(target, ballots = new HashSet<Ballot> { voter });

                LastBallotCastTimestampByPlayer.TryAdd(voter, DateTime.UtcNow);
            }

            progress.submitted = ballots != null ? ballots.Count : 0;
            progress.required = PhotonNetwork.CurrentRoom.PlayerCount / 2 + 1;

            var votePassed = progress.submitted >= progress.required;
            if (votePassed) BallotsByTargetPlayer.Remove(target);
            return votePassed;
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