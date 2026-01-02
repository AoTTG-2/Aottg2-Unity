using Characters;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Player", Abstract = true, Description = "Represents a network player. Only master client or player may modify fields.")]
    partial class CustomLogicPlayerBuiltin : BuiltinClassInstance, ICustomLogicEquals
    {
        public readonly Player Player;

        [CLConstructor("Creates a new Player instance.")]
        public CustomLogicPlayerBuiltin(
            [CLParam("The Photon player to wrap.")]
            Player player)
        {
            Player = player;
        }

        [CLProperty("Player's current character, if alive.")]
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

        [CLProperty("Player is still connected to the room.")]
        public bool Connected => Player != null;

        [CLProperty("Player unique ID.")]
        public int ID => Player.ActorNumber;

        [CLProperty("Player name.")]
        public string Name => Player.GetStringProperty(PlayerProperty.Name);

        [CLProperty("Player guild.")]
        public string Guild => Player.GetStringProperty(PlayerProperty.Guild);

        [CLProperty("Player's chosen team (\"None\", \"Blue\", \"Red\", \"Titan\", \"Human\"). Note that this may be different from the character's final team (Character.Team field) if the character's team field is modified.")]
        public string Team => Player.GetStringProperty(PlayerProperty.Team);

        [CLProperty("Player's spawn status (\"Alive\", \"Dead\", \"Spectating\").")]
        public string Status => Player.GetStringProperty(PlayerProperty.Status);

        [CLProperty("Player's chosen character (\"Human\", \"Titan\", \"Shifter\")")]
        public string CharacterType => Player.GetStringProperty(PlayerProperty.Character);

        [CLProperty("Player's chosen loadout (\"Blades\", \"AHSS\", \"APG\", \"Thunderspears\").")]
        public string Loadout => Player.GetStringProperty(PlayerProperty.Loadout);

        [CLProperty("Player's kills.")]
        public int Kills
        {
            get => Player.GetIntProperty(PlayerProperty.Kills);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.Kills, value);
            }
        }

        [CLProperty("Player's deaths.")]
        public int Deaths
        {
            get => Player.GetIntProperty(PlayerProperty.Deaths);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.Deaths, value);
            }
        }

        [CLProperty("Player's highest damage.")]
        public int HighestDamage
        {
            get => Player.GetIntProperty(PlayerProperty.HighestDamage);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.HighestDamage, value);
            }
        }

        [CLProperty("Player's total damage.")]
        public int TotalDamage
        {
            get => Player.GetIntProperty(PlayerProperty.TotalDamage);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.TotalDamage, value);
            }
        }

        [CLProperty("The player's connection ping.")]
        public int Ping => Player.GetIntProperty(PlayerProperty.Ping);

        [CLProperty("The player's spectating ID. If not spectating anyone, returns -1.")]
        public int SpectateID => Player.GetIntProperty(PlayerProperty.SpectateID);

        [CLProperty("Player's respawn point. Is initially null and can be set back to null, at which point map spawn points are used.")]
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

        [CLMethod("Get a custom property at given key. Must be a primitive type. This is synced to all clients.")]
        public object GetCustomProperty(
            [CLParam("The property key to get.")]
            string property)
        {
            return Player.GetCustomProperty("CL:" + property);
        }

        [CLMethod("Sets a custom property at given key. Must be a primitive type. This is synced to all clients.")]
        public void SetCustomProperty(
            [CLParam("The property key to set.")]
            string property,
            [CLParam("The value to set (must be a primitive type).")]
            object value)
        {
            if (!PhotonNetwork.IsMasterClient && Player != PhotonNetwork.LocalPlayer)
                return;
            if (value is not (float or int or string or bool))
                throw new System.Exception("Player.SetCustomProperty only supports float, int, string, or bool values.");
            CheckPropertyRateLimit("CL:" + property);
            Player.SetCustomProperty("CL:" + property, value);
        }

        [CLMethod("Clears kills, deaths, highestdamage, and totaldamage properties.")]
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
