using Characters;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Abstract = true)]
    class CustomLogicPlayerBuiltin : CustomLogicClassInstanceBuiltin
    {
        public Player Player;

        public CustomLogicPlayerBuiltin(Player player) : base("Player")
        {
            Player = player;
        }

        [CLProperty("Gets the character associated with the player.")]
        public object Character
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

        [CLProperty("Gets a value indicating whether the player is connected.")]
        public bool Connected => Player != null;

        [CLProperty("Gets the ID of the player.")]
        public int ID => Player.ActorNumber;

        [CLProperty("Gets the name of the player.")]
        public string Name => Player.GetStringProperty(PlayerProperty.Name);

        [CLProperty("Gets the guild of the player.")]
        public string Guild => Player.GetStringProperty(PlayerProperty.Guild);

        [CLProperty("Gets the team of the player.")]
        public string Team => Player.GetStringProperty(PlayerProperty.Team);

        [CLProperty("Gets the status of the player.")]
        public string Status => Player.GetStringProperty(PlayerProperty.Status);

        [CLProperty("Gets the character type of the player.")]
        public string CharacterType => Player.GetStringProperty(PlayerProperty.Character);

        [CLProperty("Gets the loadout of the player.")]
        public string Loadout => Player.GetStringProperty(PlayerProperty.Loadout);

        [CLProperty("Gets or sets the kills of the player.")]
        public int Kills
        {
            get => Player.GetIntProperty(PlayerProperty.Kills);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.Kills, value);
            }
        }

        [CLProperty("Gets or sets the deaths of the player.")]
        public int Deaths
        {
            get => Player.GetIntProperty(PlayerProperty.Deaths);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.Deaths, value);
            }
        }

        [CLProperty("Gets or sets the highest damage of the player.")]
        public int HighestDamage
        {
            get => Player.GetIntProperty(PlayerProperty.HighestDamage);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.HighestDamage, value);
            }
        }

        [CLProperty("Gets or sets the total damage of the player.")]
        public int TotalDamage
        {
            get => Player.GetIntProperty(PlayerProperty.TotalDamage);
            set
            {
                if (PhotonNetwork.IsMasterClient || Player == PhotonNetwork.LocalPlayer)
                    Player.SetCustomProperty(PlayerProperty.TotalDamage, value);
            }
        }

        [CLProperty("Gets the ping of the player.")]
        public int Ping => Player.GetIntProperty(PlayerProperty.Ping);

        [CLProperty("Gets the spectate ID of the player.")]
        public int SpectateID => Player.GetIntProperty(PlayerProperty.SpectateID);

        [CLProperty("Gets or sets the spawn point of the player.")]
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

        [CLMethod("Gets a custom property of the player.")]
        public object GetCustomProperty(string property)
        {
            return Player.GetCustomProperty("CL:" + property);
        }

        [CLMethod("Sets a custom property of the player.")]
        public void SetCustomProperty(string property, object value)
        {
            if (!PhotonNetwork.IsMasterClient && Player != PhotonNetwork.LocalPlayer)
                return;
            if (!(value is float || value is int || value is string || value is bool))
                throw new System.Exception("Player.SetCustomProperty only supports float, int, string, or bool values.");
            CheckPropertyRateLimit("CL:" + property);
            Player.SetCustomProperty("CL:" + property, value);
        }

        [CLMethod("Clears the kill-death ratio properties of the player.")]
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
    }
}
