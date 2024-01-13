using Characters;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicPlayerBuiltin : CustomLogicBaseBuiltin
    {
        public Player Player;

        public CustomLogicPlayerBuiltin(Player player) : base("Player")
        {
            Player = player;
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (!PhotonNetwork.IsMasterClient && Player != PhotonNetwork.LocalPlayer)
                return null;
            if (methodName == "GetCustomProperty")
                return Player.GetCustomProperty((string)parameters[0]);
            if (methodName == "SetCustomProperty")
            {
                object param = parameters[1];
                if (!(param is float || param is int || param is string || param is bool))
                    throw new System.Exception("Player.SetCustomProperty only supports float, int, string, or bool values.");
                Player.SetCustomProperty((string)parameters[0], param);
                return null;
            }
            if (methodName == "ClearKDR")
            {
                var properties = new Dictionary<string, object>
                    {
                        { PlayerProperty.Kills, 0 },
                        { PlayerProperty.Deaths, 0 },
                        { PlayerProperty.HighestDamage, 0 },
                        { PlayerProperty.TotalDamage, 0 }
                    };
                Player.SetCustomProperties(properties);
                return null;
            }
            return base.CallMethod(methodName, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "Character")
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
            if (name == "Connected")
                return Player != null;
            if (name == "ID")
                return Player.ActorNumber;
            if (name == "Name")
                return Player.GetStringProperty(PlayerProperty.Name);
            if (name == "Guild")
                return Player.GetStringProperty(PlayerProperty.Guild);
            if (name == "Team")
                return Player.GetStringProperty(PlayerProperty.Team);
            if (name == "Status")
                return Player.GetStringProperty(PlayerProperty.Status);
            if (name == "CharacterType")
                return Player.GetStringProperty(PlayerProperty.Character);
            if (name == "Loadout")
                return Player.GetStringProperty(PlayerProperty.Loadout);
            if (name == "Kills")
                return Player.GetIntProperty(PlayerProperty.Kills);
            if (name == "Deaths")
                return Player.GetIntProperty(PlayerProperty.Deaths);
            if (name == "HighestDamage")
                return Player.GetIntProperty(PlayerProperty.HighestDamage);
            if (name == "TotalDamage")
                return Player.GetIntProperty(PlayerProperty.TotalDamage);
            if (name == "SpawnPoint")
            {
                if (Player.HasSpawnPoint())
                    return new CustomLogicVector3Builtin(Player.GetSpawnPoint());
                return null;
            }
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (!PhotonNetwork.IsMasterClient && Player != PhotonNetwork.LocalPlayer)
                return;
            if (name == "Kills")
                Player.SetCustomProperty(PlayerProperty.Kills, (int)value);
            else if (name == "Deaths")
                Player.SetCustomProperty(PlayerProperty.Deaths, (int)value);
            else if (name == "HighestDamage")
                Player.SetCustomProperty(PlayerProperty.HighestDamage, (int)value);
            else if (name == "TotalDamage")
                Player.SetCustomProperty(PlayerProperty.TotalDamage, (int)value);
            else if (name == "SpawnPoint")
            {
                if (value == null)
                    Player.SetCustomProperty(PlayerProperty.SpawnPoint, "null");
                else
                {
                    Vector3 vector = ((CustomLogicVector3Builtin)value).Value;
                    string str = string.Join(",", new string[] { vector.x.ToString(), vector.y.ToString(), vector.z.ToString() });
                    Player.SetCustomProperty(PlayerProperty.SpawnPoint, str);
                }
            }
            else
                base.SetField(name, value);
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
