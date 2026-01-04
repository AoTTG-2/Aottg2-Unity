using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using System;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Networking functions.
    /// </summary>
    [CLType(Name = "Network", Static = true, Abstract = true)]
    partial class CustomLogicNetworkBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicNetworkBuiltin(){}

        /// <summary>
        /// Is the player the master client.
        /// </summary>
        [CLProperty(Static = true)]
        public static bool IsMasterClient => PhotonNetwork.IsMasterClient;

        /// <summary>
        /// The list of players in the room.
        /// </summary>
        [CLProperty(Static = true, TypeArguments = new[] { "Player" })]
        public static CustomLogicListBuiltin Players
        {
            get
            {
                CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    list.List.Add(new CustomLogicPlayerBuiltin(player));
                }
                return list;
            }
        }

        /// <summary>
        /// The master client.
        /// </summary>
        [CLProperty(Static = true)]
        public static CustomLogicPlayerBuiltin MasterClient => new CustomLogicPlayerBuiltin(PhotonNetwork.MasterClient);

        /// <summary>
        /// The local player.
        /// </summary>
        [CLProperty(Static = true)]
        public static CustomLogicPlayerBuiltin MyPlayer => new CustomLogicPlayerBuiltin(PhotonNetwork.LocalPlayer);

        /// <summary>
        /// The network time.
        /// </summary>
        [CLProperty(Static = true)]
        public static double NetworkTime => PhotonNetwork.Time;

        /// <summary>
        /// The local player's ping.
        /// </summary>
        [CLProperty(Static = true)]
        public static int Ping => PhotonNetwork.GetPing();

        /// <summary>
        /// Send a message to a player.
        /// </summary>
        /// <param name="player">The player to send the message to.</param>
        /// <param name="message">The message to send.</param>
        [CLMethod(Static = true)]
        public static void SendMessage(CustomLogicPlayerBuiltin player, string message)
        {
            RPCManager.PhotonView.RPC(nameof(RPCManager.SendMessageRPC), player.Player, new object[] { message });
        }

        /// <summary>
        /// Send a message to all players.
        /// </summary>
        /// <param name="message">The message to send.</param>
        [CLMethod(Static = true)]
        public static void SendMessageAll(string message)
        {
            RPCManager.PhotonView.RPC(nameof(RPCManager.SendMessageRPC), RpcTarget.All, new object[] { message });
        }

        /// <summary>
        /// Send a message to all players except the sender.
        /// </summary>
        /// <param name="message">The message to send.</param>
        [CLMethod(Static = true)]
        public static void SendMessageOthers(string message)
        {
            RPCManager.PhotonView.RPC(nameof(RPCManager.SendMessageRPC), RpcTarget.Others, new object[] { message });
        }

        /// <summary>
        /// Finds a player in the room by id.
        /// </summary>
        /// <param name="id">The player ID to find.</param>
        /// <returns>The player if found, null otherwise.</returns>
        [CLMethod(Static = true)]
        public static CustomLogicPlayerBuiltin FindPlayer(int id)
        {
            Player player = Util.FindPlayerById(id);
            if (player != null)
                return new CustomLogicPlayerBuiltin(player);
            return null;
        }

        /// <summary>
        /// Get the difference between two photon timestamps.
        /// </summary>
        /// <param name="timestamp1">The first timestamp.</param>
        /// <param name="timestamp2">The second timestamp.</param>
        /// <returns>The difference between the two timestamps.</returns>
        [CLMethod(Static = true)]
        public static double GetTimestampDifference(double timestamp1, double timestamp2)
        {
            // Handle the wrap around case photon timestamps have for the user since most will likely ignore it otherwise.
            return Util.GetPhotonTimestampDifference(timestamp1, timestamp2);
        }

        /// <summary>
        /// Kick the given player by id or player reference.
        /// </summary>
        /// <param name="target">The player to kick (can be Player object or int ID).</param>
        /// <param name="reason">The reason for kicking the player (default: '.').</param>
        [CLMethod(Static = true)]
        public static void KickPlayer([CLParam(Type = "Player|int")] object target, string reason = ".")
        {
            Photon.Realtime.Player player = null;
            if (target is int)
                player = PhotonNetwork.CurrentRoom.GetPlayer(target.UnboxToInt(), true);
            else if (target is CustomLogicPlayerBuiltin)
                player = ((CustomLogicPlayerBuiltin)target).Player;
            else
                throw new ArgumentException($"Invalid player parameter type {target.GetType()}. Valid types are {nameof(CustomLogicPlayerBuiltin)}, int (id).");

            if (PhotonNetwork.IsMasterClient)
                ChatManager.KickPlayer(player, reason: reason);
            else
                throw new Exception("Only the master client can kick players.");
        }
    }
}
