using ApplicationManagers;
using Characters;
using Effects;
using GameManagers;
using Photon.Pun;
using Projectiles;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicGameBuiltin: CustomLogicBaseBuiltin
    {
        private string _lastSetTopLabel = string.Empty;

        public CustomLogicGameBuiltin(): base("Game")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "Debug")
            {
                DebugConsole.Log((string)parameters[0], true);
                return null;
            }
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (name == "Print")
            {
                string message;
                if (parameters[0] == null)
                    message = "null";
                else
                    message = parameters[0].ToString();
                ChatManager.AddLine(message, ChatTextColor.System);
                return null;
            }
            if (name == "PrintAll")
            {
                string message;
                if (parameters[0] == null)
                    message = "null";
                else
                    message = parameters[0].ToString();
                ChatManager.SendChatAll(message, ChatTextColor.System);
                return null;
            }
            if (name == "End")
            {
                if (PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("EndGameRPC", RpcTarget.All, new object[] { parameters[0].UnboxToFloat() });
                return null;
            }
            if (name == "FindCharacterByViewID")
            {
                int viewID = (int)parameters[0];
                var character = Util.FindCharacterByViewId(viewID);
                if (character == null || character.Dead)
                    return null;
                return CustomLogicEvaluator.GetCharacterBuiltin(character);
            }
            if (name == "SpawnTitan")
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    string type = (string)parameters[0];
                    var titan = new CustomLogicTitanBuiltin(gameManager.SpawnAITitan(type));
                    return titan;
                }
                return null;
            }
            if (name == "SpawnTitanAt")
            {
                string type = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                float rotationY = parameters.Count > 2 ? parameters[2].UnboxToFloat() : 0f;
                if (PhotonNetwork.IsMasterClient)
                {
                    var titan = new CustomLogicTitanBuiltin(gameManager.SpawnAITitanAt(type, position, rotationY));
                    return titan;
                }
                return null;
            }
            if (name == "SpawnTitans")
            {
                string type = (string)parameters[0];
                if (PhotonNetwork.IsMasterClient)
                {
                    CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                    foreach (var titan in gameManager.SpawnAITitans(type, (int)parameters[1]))
                        list.List.Add(new CustomLogicTitanBuiltin(titan));
                    return list;
                }
                return null;
            }
            if (name == "SpawnTitansAsync")
            {
                string type = (string)parameters[0];
                if (PhotonNetwork.IsMasterClient)
                    gameManager.SpawnAITitansAsync(type, (int)parameters[1]);
                return null;
            }
            if (name == "SpawnTitansAt")
            {
                string type = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[2]).Value;
                float rotationY = parameters.Count > 3 ? parameters[3].UnboxToFloat() : 0f;
                if (PhotonNetwork.IsMasterClient)
                {
                    CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                    for (int i = 0; i < (int)parameters[1]; i++)
                    {
                        var titan = new CustomLogicTitanBuiltin(gameManager.SpawnAITitanAt(type, position, rotationY));
                        list.List.Add(titan);
                    }
                    return list;
                }
                return null;
            }
            if (name == "SpawnTitansAtAsync")
            {
                string type = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[2]).Value;
                float rotationY = parameters.Count > 3 ? parameters[3].UnboxToFloat() : 0f;
                if (PhotonNetwork.IsMasterClient)
                    gameManager.SpawnAITitansAtAsync(type, (int)parameters[1], position, rotationY);
                return null;
            }
            if (name == "SpawnShifter")
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    string type = (string)parameters[0];
                    var shifter = new CustomLogicShifterBuiltin(gameManager.SpawnAIShifter(type));
                    return shifter;
                }
                return null;
            }
            if (name == "SpawnShifterAt")
            {
                string type = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                float rotationY = parameters.Count > 2 ? parameters[2].UnboxToFloat() : 0f;
                if (PhotonNetwork.IsMasterClient)
                {
                    var shifter = new CustomLogicShifterBuiltin(gameManager.SpawnAIShifterAt(type, position, rotationY));
                    return shifter;
                }
                return null;
            }
            if (name == "SpawnProjectile")
            {
                string projectileName = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                Vector3 rotation = ((CustomLogicVector3Builtin)parameters[2]).Value;
                Vector3 velocity = ((CustomLogicVector3Builtin)parameters[3]).Value;
                Vector3 gravity = ((CustomLogicVector3Builtin)parameters[4]).Value;
                float liveTime = (float)parameters[5];
                string team = (string)parameters[6];
                object[] settings = null;
                if (projectileName == ProjectilePrefabs.Thunderspear)
                {
                    float radius = (float)parameters[7];
                    Color color = ((CustomLogicColorBuiltin)parameters[8]).Value.ToColor();
                    settings = new object[] { radius, color };
                }
                else if (projectileName == ProjectilePrefabs.Flare)
                {
                    Color color = ((CustomLogicColorBuiltin)parameters[7]).Value.ToColor();
                    settings = new object[] { color };
                }
                ProjectileSpawner.Spawn(projectileName, position, Quaternion.Euler(rotation), velocity, gravity, liveTime, -1, team, settings);
                return null;
            }
            if (name == "SpawnProjectileWithOwner")
            {
                string projectileName = (string)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                Vector3 rotation = ((CustomLogicVector3Builtin)parameters[2]).Value;
                Vector3 velocity = ((CustomLogicVector3Builtin)parameters[3]).Value;
                Vector3 gravity = ((CustomLogicVector3Builtin)parameters[4]).Value;
                float liveTime = (float)parameters[5];
                BaseCharacter character = ((CustomLogicCharacterBuiltin)parameters[6]).Character;
                object[] settings = null;
                if (projectileName == ProjectilePrefabs.Thunderspear)
                {
                    float radius = (float)parameters[7];
                    Color color = ((CustomLogicColorBuiltin)parameters[8]).Value.ToColor();
                    settings = new object[] { radius, color };
                }
                else if (projectileName == ProjectilePrefabs.Flare)
                {
                    Color color = ((CustomLogicColorBuiltin)parameters[7]).Value.ToColor();
                    settings = new object[] { color };
                }
                ProjectileSpawner.Spawn(projectileName, position, Quaternion.Euler(rotation), velocity, gravity, liveTime, character.photonView.ViewID, 
                    character.Team, settings);
                return null;
            }
            if (name == "SpawnEffect")
            {
                string effectName = (string)parameters[0];
                var field = typeof(EffectPrefabs).GetField(effectName);
                if (field == null)
                    return null;
                effectName = (string)field.GetValue(null);
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                Vector3 rotation = ((CustomLogicVector3Builtin)parameters[2]).Value;
                float scale = (float)parameters[3];
                object[] settings = null;
                if (effectName == EffectPrefabs.ThunderspearExplode)
                {
                    Color color = ((CustomLogicColorBuiltin)parameters[4]).Value.ToColor();
                    bool kill = false;
                    if (parameters.Count > 5)
                        kill = (bool)(parameters[5]);
                    settings = new object[] { color, kill };
                }
                EffectSpawner.Spawn(effectName, position, Quaternion.Euler(rotation), scale, true, settings);
                return null;
            }
            if (name == "SpawnPlayer")
            {
                var player = ((CustomLogicPlayerBuiltin)parameters[0]).Player;
                bool force = (bool)parameters[1];
                if (player == PhotonNetwork.LocalPlayer)
                    gameManager.SpawnPlayer(force);
                else if (PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("SpawnPlayerRPC", player, new object[] { force });
                return null;
            }
            if (name == "SpawnPlayerAll")
            {//Zippy: CL spawn error
                bool force = (bool)parameters[0];
                if (PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("SpawnPlayerRPC", RpcTarget.All, new { force });
                return null;
            }
            if (name == "SpawnPlayerAt")
            {
                var player = ((CustomLogicPlayerBuiltin)parameters[0]).Player;
                bool force = (bool)parameters[1];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[2]).Value;
                float rotationY = parameters.Count > 3 ? parameters[3].UnboxToFloat() : 0f;
                if (player == PhotonNetwork.LocalPlayer)
                    gameManager.SpawnPlayerAt(force, position, rotationY);
                else if (PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("SpawnPlayerAtRPC", player, force, position, rotationY);
                return null;
            }
            if (name == "SpawnPlayerAtAll")
            {
                bool force = (bool)parameters[0];
                Vector3 position = ((CustomLogicVector3Builtin)parameters[1]).Value;
                float rotationY = parameters.Count > 2 ? parameters[2].UnboxToFloat() : 0f;
                if (PhotonNetwork.IsMasterClient)
                    RPCManager.PhotonView.RPC("SpawnPlayerAtRPC", RpcTarget.All, force, position, rotationY);
                return null;
            }
            if (name == "SetPlaylist")
            {
                string playlist = (string)parameters[0];
                if (playlist == "Default")
                    playlist = "Default.Ordered";
                MusicManager.SetPlaylist(playlist);
                CustomLogicManager.Evaluator.HasSetMusic = true;
                return null;
            }
            if (name == "SetSong")
            {
                string song = (string)parameters[0];
                MusicManager.SetSong(song);
                CustomLogicManager.Evaluator.HasSetMusic = true;
                return null;
            }
            return base.CallMethod(name, parameters);
        }

        public override object GetField(string name)
        {
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (name == "IsEnding")
                return gameManager.IsEnding;
            if (name == "PVP")
                return SettingsManager.InGameCurrent.Misc.PVP.Value;
            if (name == "EndTimeLeft")
                return gameManager.EndTimeLeft;
            if (name == "Titans")
            {
                var list = new CustomLogicListBuiltin();
                foreach (var titan in gameManager.Titans)
                {
                    if (titan != null && !titan.Dead)
                        list.List.Add(new CustomLogicTitanBuiltin(titan));
                }
                return list;
            }
            if (name == "AITitans")
            {
                var list = new CustomLogicListBuiltin();
                foreach (var titan in gameManager.Titans)
                {
                    if (titan != null && !titan.Dead && titan.AI)
                        list.List.Add(new CustomLogicTitanBuiltin(titan));
                }
                return list;
            }
            if (name == "PlayerTitans")
            {
                var list = new CustomLogicListBuiltin();
                foreach (var titan in gameManager.Titans)
                {
                    if (titan != null && !titan.Dead && !titan.AI)
                        list.List.Add(new CustomLogicTitanBuiltin(titan));
                }
                return list;
            }
            if (name == "Shifters")
            {
                var list = new CustomLogicListBuiltin();
                foreach (var shifter in gameManager.Shifters)
                {
                    if (shifter != null && (!shifter.Dead || shifter.TransformingToHuman))
                        list.List.Add(new CustomLogicShifterBuiltin(shifter));
                }
                return list;
            }
            if (name == "AIShifters")
            {
                var list = new CustomLogicListBuiltin();
                foreach (var shifter in gameManager.Shifters)
                {
                    if (shifter != null && shifter.AI && (!shifter.Dead || shifter.TransformingToHuman))
                        list.List.Add(new CustomLogicShifterBuiltin(shifter));
                }
                return list;
            }
            if (name == "PlayerShifters")
            {
                var list = new CustomLogicListBuiltin();
                foreach (var shifter in gameManager.Shifters)
                {
                    if (shifter != null && !shifter.AI && (!shifter.Dead || shifter.TransformingToHuman))
                        list.List.Add(new CustomLogicShifterBuiltin(shifter));
                }
                return list;
            }
            if (name == "Humans")
            {
                var list = new CustomLogicListBuiltin();
                foreach (var human in gameManager.Humans)
                {
                    if (human != null && !human.Dead)
                        list.List.Add(new CustomLogicHumanBuiltin(human));
                }
                return list;
            }
            if (name == "AIHumans")
            {
                var list = new CustomLogicListBuiltin();
                foreach (var human in gameManager.Humans)
                {
                    if (human != null && !human.Dead && human.AI)
                        list.List.Add(new CustomLogicHumanBuiltin(human));
                }
                return list;
            }
            if (name == "PlayerHumans")
            {
                var list = new CustomLogicListBuiltin();
                foreach (var human in gameManager.Humans)
                {
                    if (human != null && !human.Dead && !human.AI)
                        list.List.Add(new CustomLogicHumanBuiltin(human));
                }
                return list;
            }
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            base.SetField(name, value);
        }
    }
}
