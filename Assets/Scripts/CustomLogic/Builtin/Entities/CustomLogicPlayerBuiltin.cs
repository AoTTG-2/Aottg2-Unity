using Characters;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents a network player. Only master client or player may modify fields.
    /// </summary>
    [CLType(Name = "Player", Abstract = true)]
    partial class CustomLogicPlayerBuiltin : BuiltinClassInstance, ICustomLogicEquals
    {
        public readonly Player Player;

        /// <summary>
        /// Creates a new Player instance.
        /// </summary>
        /// <param name="player">The Photon player to wrap.</param>
        [CLConstructor]
        public CustomLogicPlayerBuiltin(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Player's current character, if alive.
        /// </summary>
        [CLProperty]
        public CustomLogicCharacterBuiltin Character
        {
            get
            {
                int viewId = Player.GetIntProperty(PlayerProperty.CharacterViewId, 0);
                if (viewId > 0)
                {
                    var photonView = PhotonView.Find(viewId);
                    if (photonView != null)
                    {
                        var character = photonView.GetComponent<BaseCharacter>();
                        if (character == null || character.Dead || character.Cache.PhotonView.Owner != Player)
                            return null;
                        return CustomLogicEvaluator.GetCharacterBuiltin(character);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Player is still connected to the room.
        /// </summary>
        [CLProperty]
        public bool Connected => Player != null;

        /// <summary>
        /// Player unique ID.
        /// </summary>
        [CLProperty]
        public int ID => Player.ActorNumber;

        /// <summary>
        /// Player name.
        /// </summary>
        [CLProperty]
        public string Name => Player.GetStringProperty(PlayerProperty.Name);

        /// <summary>
        /// Player guild.
        /// </summary>
        [CLProperty]
        public string Guild => Player.GetStringProperty(PlayerProperty.Guild);

        /// <summary>
        /// Player's chosen team. Note that this may be different from the character's final team
        /// (Character.Team field) if the character's team field is modified.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicTeamEnum) })]
        public string Team => Player.GetStringProperty(PlayerProperty.Team);

        /// <summary>
        /// Player's spawn status.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicPlayerStatusEnum) })]
        public string Status => Player.GetStringProperty(PlayerProperty.Status);

        /// <summary>
        /// Player's chosen character.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicCharacterTypeEnum) })]
        public string CharacterType => Player.GetStringProperty(PlayerProperty.Character);

        /// <summary>
        /// Player's chosen loadout.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicLoadoutEnum) })]
        public string Loadout => Player.GetStringProperty(PlayerProperty.Loadout);

        /// <summary>
        /// Player's kills.
        /// </summary>
        [CLProperty]
        public int Kills
        {
            get => Player.GetIntProperty(PlayerProperty.Kills);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.Kills, value);
            }
        }

        /// <summary>
        /// Player's deaths.
        /// </summary>
        [CLProperty]
        public int Deaths
        {
            get => Player.GetIntProperty(PlayerProperty.Deaths);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.Deaths, value);
            }
        }

        /// <summary>
        /// Player's highest damage.
        /// </summary>
        [CLProperty]
        public int HighestDamage
        {
            get => Player.GetIntProperty(PlayerProperty.HighestDamage);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.HighestDamage, value);
            }
        }

        /// <summary>
        /// Player's total damage.
        /// </summary>
        [CLProperty]
        public int TotalDamage
        {
            get => Player.GetIntProperty(PlayerProperty.TotalDamage);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.TotalDamage, value);
            }
        }

        /// <summary>
        /// The player's connection ping.
        /// </summary>
        [CLProperty]
        public int Ping => Player.GetIntProperty(PlayerProperty.Ping);

        /// <summary>
        /// The player's spectating ID. If not spectating anyone, returns -1.
        /// </summary>
        [CLProperty]
        public int SpectateID => Player.GetIntProperty(PlayerProperty.SpectateID);

        /// <summary>
        /// Player's respawn point. Is initially null and can be set back to null,
        /// at which point map spawn points are used.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin SpawnPoint
        {
            get
            {
                if (Player.HasSpawnPoint())
                    return new CustomLogicVector3Builtin(Player.GetSpawnPoint());
                return null;
            }
            set
            {
                if (!PhotonNetwork.IsMasterClient && Player != PhotonNetwork.LocalPlayer)
                    return;
                CheckPropertyRateLimit("SpawnPoint");
                if (value == null)
                    Player.SetCustomProperty(PlayerProperty.SpawnPoint, "null");
                else
                {
                    var vector = value.Value;
                    string str = string.Join(",", new string[] { vector.x.ToString(), vector.y.ToString(), vector.z.ToString() });
                    Player.SetCustomProperty(PlayerProperty.SpawnPoint, str);
                }
            }
        }

        /// <summary>
        /// Get a custom property at given key. Must be a primitive type. This is synced to all clients.
        /// </summary>
        /// <param name="property">The property key to get.</param>
        /// <returns>The property value.</returns>
        [CLMethod]
        public object GetCustomProperty(string property)
        {
            return Player.GetCustomProperty("CL:" + property);
        }

        /// <summary>
        /// Sets a custom property at given key. Must be a primitive type. This is synced to all clients.
        /// </summary>
        /// <param name="property">The property key to set.</param>
        /// <param name="value">The value to set (must be a primitive type).</param>
        [CLMethod]
        public void SetCustomProperty(string property, object value)
        {
            if (!PhotonNetwork.IsMasterClient && Player != PhotonNetwork.LocalPlayer)
                return;
            if (value is not (float or int or string or bool))
                throw new System.Exception("Player.SetCustomProperty only supports float, int, string, or bool values.");
            CheckPropertyRateLimit("CL:" + property);
            Player.SetCustomProperty("CL:" + property, value);
        }

        /// <summary>
        /// Clears kills, deaths, highestdamage, and totaldamage properties.
        /// </summary>
        [CLMethod]
        public void ClearKDR()
        {
            if (!PhotonNetwork.IsMasterClient && Player != PhotonNetwork.LocalPlayer)
                return;
            CheckPropertyRateLimit("ClearKDR");
            var properties = new Dictionary<string, object>
                {
                    { PlayerProperty.Kills, 0 },
                    { PlayerProperty.Deaths, 0 },
                    { PlayerProperty.HighestDamage, 0 },
                    { PlayerProperty.TotalDamage, 0 }
                };
            Player.SetCustomProperties(properties);
        }

        private void CheckPropertyRateLimit(string property)
        {
            if (Player == PhotonNetwork.LocalPlayer)
                return;
            var properties = CustomLogicManager.Evaluator.PlayerIdToLastPropertyChanges;
            if (!properties.ContainsKey(Player.ActorNumber))
                properties[Player.ActorNumber] = new Dictionary<string, float>();
            var lastChanges = properties[Player.ActorNumber];
            if (!lastChanges.ContainsKey(property))
                lastChanges[property] = 0;
            float timeElapsed = Time.time - lastChanges[property];
            if (timeElapsed < 1f)
                throw new System.Exception("Exceeded set property rate limit on non-local client: " + property);
            lastChanges[property] = Time.time;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return Player == null;
            if (!(obj is CustomLogicPlayerBuiltin))
                return false;
            var other = ((CustomLogicPlayerBuiltin)obj).Player;
            return Player == other;
        }

        public override int GetHashCode()
        {
            return Player.GetHashCode();
        }

        public bool __Eq__(object self, object other) => self.Equals(other);

        public int __Hash__() => this.GetHashCode();
    }
}
