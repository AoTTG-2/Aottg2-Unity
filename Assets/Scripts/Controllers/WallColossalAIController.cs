using Characters;
using GameManagers;
using Map;
using Settings;
using SimpleJSONFixed;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Controllers
{
    class WallColossalAIController : BaseTitanAIController
    {
        protected override bool _scriptedAI => true;
        protected override bool _stationaryAI => true;

        protected List<string> WallAttacks = new List<string>() { "AttackSweep", "AttackWallSlap1L", "AttackWallSlap1R", "AttackWallSlap2L", "AttackWallSlap2R" };
        protected List<string> LeftHandedAttacks = new List<string>() { "AttackWallSlap1L", "AttackWallSlap2L" };
        protected List<string> RightHandedAttacks = new List<string>() { "AttackSweep", "AttackWallSlap1R", "AttackWallSlap2R" };
        public float WallAttackCooldownLeft = 5f;
        public float WallAttackCooldown = 5f;

        protected override void UpdateScriptedAI()
        {
            WallAttackCooldownLeft -= Time.deltaTime;
            if (WallAttackCooldownLeft > 0f)
                return;
            WallAttack();
        }

        public override void Init(JSONNode data)
        {
            base.Init(data);
            WallAttackCooldown = data["WallAttackCooldown"].AsFloat;
        }

        public void WallAttack()
        {
            WallColossalShifter shifter = (_titan as WallColossalShifter);
            if (_titan.CanAttack())
            {
                List<string> validAttacks = WallAttacks;
                if (shifter.LeftHandState == ColossalHandState.Broken && shifter.RightHandState == ColossalHandState.Broken)
                {
                    return;
                }
                else if (shifter.LeftHandState == ColossalHandState.Broken)
                {
                    validAttacks = RightHandedAttacks;
                }
                else if (shifter.RightHandState == ColossalHandState.Broken)
                {
                    validAttacks = LeftHandedAttacks;
                }

                    string attack = RandomGen.ChooseRandom(validAttacks);
                _titan.Attack(attack);
                WallAttackCooldownLeft = WallAttackCooldown;
            }
        }
    }
}
