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
        private static readonly Dictionary<Player, HashSet<Vote>> VotesByTargetPlayer = new();
        private static readonly List<Player> removals = new();
        private static readonly TimeSpan VoteTimeout = TimeSpan.FromMinutes(5.0);

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
            RemoveOldVotes();

            HashSet<Vote> votes = null;
            if (target != PhotonNetwork.LocalPlayer)
            {
                if (VotesByTargetPlayer.TryGetValue(target, out votes))
                    votes.Add(voter);
                else
                    VotesByTargetPlayer.Add(target, votes = new HashSet<Vote> { voter });
            }

            progress.submitted = votes != null ? votes.Count : 0;
            progress.required = PhotonNetwork.CurrentRoom.PlayerCount / 2 + 1;
            if (progress.submitted >= progress.required)
            {
                KickPlayer(target);
                VotesByTargetPlayer.Remove(target);
                return true;
            }

            return false;
        }

        public static void ResetVoteKicks(Player player)
        {
            foreach (var voters in VotesByTargetPlayer.Values)
                voters.Remove(player);
        }

        private static void RemoveOldVotes()
        {
            foreach (var kvp in VotesByTargetPlayer)
            {
                var target = kvp.Key;
                var votes = kvp.Value;
                votes.RemoveWhere(vote => DateTime.UtcNow - vote.Timestamp > VoteTimeout);
                if (votes.Count == 0) removals.Add(target);
            }

            foreach (var target in removals)
                VotesByTargetPlayer.Remove(target);

            removals.Clear();
        }
    }

    public enum PhotonEventType
    {
        Instantiate,
        RPC
    }

    readonly struct Vote
    {
        public readonly Player Voter;
        public readonly DateTime Timestamp;

        public Vote(Player player)
        {
            Voter = player;
            Timestamp = DateTime.UtcNow;
        }

        public override int GetHashCode() => Voter.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is not Vote other) return false;
            return Voter.Equals(other.Voter);
        }

        public override string ToString() => $"{Voter} ({Timestamp})";

        public static implicit operator Vote(Player voter) => new(voter);
    }
}