using System.Collections.Generic;
using Characters;
using Cysharp.Threading.Tasks;
using Photon.Realtime;
using UnityEngine;

namespace CustomLogic
{
    partial class CustomLogicEvaluator
    {
        public void OnTick()
        {
            EvaluateMethodForCallbacks("OnTick").Forget();
            CurrentTime += Time.fixedDeltaTime;
        }

        public void OnFrame()
        {
            EvaluateMethodForCallbacks("OnFrame").Forget();
        }

        public void OnLateFrame()
        {
            EvaluateMethodForCallbacks("OnLateFrame").Forget();
        }

        public void OnButtonClick(string name)
        {
            EvaluateMethodForCallbacks("OnButtonClick", new List<object>() { name }).Forget();
        }

        public void OnPlayerSpawn(Player player, BaseCharacter character)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(player);
            var characterBuiltin = GetCharacterBuiltin(character);
            if (characterBuiltin == null)
                return;

            EvaluateMethodForCallbacks("OnPlayerSpawn", new List<object>() { playerBuiltin, characterBuiltin })
                .Forget();
        }

        public void OnCharacterSpawn(BaseCharacter character)
        {
            var builtin = GetCharacterBuiltin(character);
            if (builtin == null)
                return;

            EvaluateMethodForCallbacks("OnCharacterSpawn", new List<object>() { builtin }).Forget();
        }

        public void OnCharacterDie(BaseCharacter victim, BaseCharacter killer, string killerName)
        {
            var victimBuiltin = GetCharacterBuiltin(victim);
            var killerBuiltin = GetCharacterBuiltin(killer);

            EvaluateMethodForCallbacks("OnCharacterDie",
                new List<object>() { victimBuiltin, killerBuiltin, killerName }).Forget();
        }

        public void OnCharacterDamaged(BaseCharacter victim, BaseCharacter killer, string killerName, int damage)
        {
            var victimBuiltin = GetCharacterBuiltin(victim);
            var killerBuiltin = GetCharacterBuiltin(killer);

            EvaluateMethodForCallbacks("OnCharacterDamaged",
                new List<object>() { victimBuiltin, killerBuiltin, killerName, damage }).Forget();
        }

        public object OnChatInput(string message)
        {
            return EvaluateMethod(_staticClasses["Main"], "OnChatInput", new List<object>() { message });
        }

        public void OnPlayerJoin(Player player)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(player);
            EvaluateMethodForCallbacks("OnPlayerJoin", new List<object>() { playerBuiltin }).Forget();
            ((CustomLogicUIBuiltin)_staticClasses["UI"]).OnPlayerJoin(player);
        }

        public void OnPlayerLeave(Player player)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(player);
            EvaluateMethodForCallbacks("OnPlayerLeave", new List<object>() { playerBuiltin }).Forget();
        }

        public void OnNetworkMessage(Player sender, string message, double sentServerTimestamp)
        {
            var playerBuiltin = new CustomLogicPlayerBuiltin(sender);
            EvaluateMethod(_staticClasses["Main"], "OnNetworkMessage",
                new List<object>() { playerBuiltin, message, sentServerTimestamp }).Forget();
        }
    }
}