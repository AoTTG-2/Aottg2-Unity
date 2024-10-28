using Characters;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using System.Reflection;

namespace CustomLogic
{
    class CustomLogicTitanBuiltin : CustomLogicCharacterBuiltin
    {
        public BasicTitan Titan;

        public CustomLogicTitanBuiltin(BasicTitan titan) : base(titan, "Titan")
        {
            Titan = titan;
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            bool mine = Titan.IsMine() && !Titan.Dead;
            if (methodName == "MoveTo")
            {
                if (!mine || !Titan.AI)
                    return null;
                var position = ((CustomLogicVector3Builtin)parameters[0]).Value;
                var range = parameters[1].UnboxToFloat();
                bool ignoreEnemies = (bool)parameters[2];
                Titan.GetComponent<BaseTitanAIController>().MoveTo(position, range, ignoreEnemies);
                return null;
            }
            if (methodName == "Target")
            {
                if (!mine || !Titan.AI)
                    return null;

                ITargetable enemy;
                if (parameters[0] is CustomLogicMapTargetableBuiltin mapTargetable)
                    enemy = mapTargetable.Value;
                else
                    enemy = ((CustomLogicCharacterBuiltin)parameters[0]).Character;
                var focus = parameters[1].UnboxToFloat();
                Titan.GetComponent<BaseTitanAIController>().SetEnemy(enemy, focus);
                return null;
            }
            if (methodName == "Idle")
            {
                if (!mine || !Titan.AI)
                    return null;
                var time = parameters[0].UnboxToFloat();
                Titan.GetComponent<BaseTitanAIController>().ForceIdle(time);
                return null;
            }
            if (methodName == "Wander")
            {
                if (!mine || !Titan.AI)
                    return null;
                Titan.GetComponent<BaseTitanAIController>().CancelOrder();
                return null;
            }
            if (methodName == "Blind")
            {
                if (mine)
                    Titan.Blind();
                return null;
            }
            if (methodName == "Cripple")
            {
                if (mine)
                {
                    var time = parameters[0].UnboxToFloat();
                    Titan.Cripple(time);
                }
                return null;
            }
            if (methodName == "Emote")
            {
                if (mine)
                    Titan.Emote((string)parameters[0]);
                return null;
            }
            return base.CallMethod(methodName, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "Size")
                return Titan.Size;
            if (name == "DetectRange")
            {
                if (Titan.IsMine() && Titan.AI)
                    return Titan.GetComponent<BaseTitanAIController>().DetectRange;
                return null;
            }
            if (name == "FocusRange")
            {
                if (Titan.IsMine() && Titan.AI)
                    return Titan.GetComponent<BaseTitanAIController>().FocusRange;
                return null;
            }
            if (name == "RunSpeedBase")
                return Titan.RunSpeedBase;
            if (name == "WalkSpeedBase")
                return Titan.WalkSpeedBase;
            if (name == "WalkSpeedPerLevel")
                return Titan.WalkSpeedPerLevel;
            if (name == "RunSpeedPerLevel")
                return Titan.RunSpeedPerLevel;
            if (name == "TurnSpeed")
                return Titan.TurnSpeed;
            if (name == "RotateSpeed")
                return Titan.RotateSpeed;
            if (name == "JumpForce")
                return Titan.JumpForce;
            if (name == "ActionPause")
                return Titan.ActionPause;
            if (name == "AttackPause")
                return Titan.AttackPause;
            if (name == "TurnPause")
                return Titan.TurnPause;
            if (name == "NapePosition")
                return new CustomLogicVector3Builtin(Titan.BasicCache.NapeHurtbox.transform.position);
            if (name == "IsCrawler")
                return Titan.IsCrawler;
            if (name == "FocusTime")
            {
                if (Titan.IsMine() && Titan.AI)
                    return Titan.GetComponent<BaseTitanAIController>().FocusTime;
                return null;
            }
            if (name == "FarAttackCooldown")
            {
                if (Titan.IsMine() && Titan.AI)
                    return Titan.GetComponent<BaseTitanAIController>().FarAttackCooldown;
                return null;
            }
            if (name == "AttackWait")
            {
                if (Titan.IsMine() && Titan.AI)
                    return Titan.GetComponent<BaseTitanAIController>().AttackWait;
                return null;
            }
            if (name == "CanRun")
            {
                if (Titan.IsMine() && Titan.AI)
                    return Titan.GetComponent<BaseTitanAIController>().IsRun;
                return null;
            }
            if (name == "AttackSpeedMultiplier")
            {
                return Titan.AttackSpeedMultiplier;
            }
            if (name == "UsePathfinding")
            {
                if (Titan.IsMine() && Titan.AI)
                    return Titan.GetComponent<BaseTitanAIController>()._usePathfinding;
                return null;
            }
            if (name == "HeadMount")
            {
                return new CustomLogicTransformBuiltin(Titan.BasicCache.Head);
            }
            if (name == "NeckMount")
            {
                return new CustomLogicTransformBuiltin(Titan.BasicCache.Neck);
            }
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (!Titan.IsMine())
                return;
            if (name == "Size")
                Titan.SetSize(value.UnboxToFloat());
            else if (name == "RunSpeedBase")
                Titan.RunSpeedBase = value.UnboxToFloat();
            else if (name == "WalkSpeedBase")
                Titan.WalkSpeedBase = value.UnboxToFloat();
            else if (name == "RunSpeedPerLevel")
                Titan.RunSpeedPerLevel = value.UnboxToFloat();
            else if (name == "WalkSpeedPerLevel")
                Titan.WalkSpeedPerLevel = value.UnboxToFloat();
            else if (name == "TurnSpeed")
                Titan.TurnSpeed = value.UnboxToFloat();
            else if (name == "RotateSpeed")
                Titan.RotateSpeed = value.UnboxToFloat();
            else if (name == "JumpForce")
                Titan.JumpForce = value.UnboxToFloat();
            else if (name == "ActionPause")
                Titan.ActionPause = value.UnboxToFloat();
            else if (name == "AttackPause")
                Titan.AttackPause = value.UnboxToFloat();
            else if (name == "TurnPause")
                Titan.TurnPause = value.UnboxToFloat();
            else if (name == "DetectRange")
            {
                if (Titan.AI)
                    Titan.GetComponent<BaseTitanAIController>().SetDetectRange(value.UnboxToFloat());
            }
            else if (name == "FocusRange")
            {
                if (Titan.AI)
                    Titan.GetComponent<BaseTitanAIController>().FocusRange = value.UnboxToFloat();
            }
            else if (name == "FocusTime")
            {
                if (Titan.AI)
                    Titan.GetComponent<BaseTitanAIController>().FocusTime = value.UnboxToFloat();
            }
            else if (name == "AttackWait")
            {
                if (Titan.AI)
                    Titan.GetComponent<BaseTitanAIController>().AttackWait = value.UnboxToFloat();
            }
            else if (name == "CanRun")
            {
                if (Titan.AI)
                    Titan.GetComponent<BaseTitanAIController>().IsRun = (bool)value;
            }
            else if (name == "AttackSpeedMultiplier")
            {
                Titan.AttackSpeedMultiplier = value.UnboxToFloat();
            }
            else if (name == "UsePathfinding")
            {
                if (Titan.AI)
                    Titan.GetComponent<BaseTitanAIController>()._usePathfinding = (bool)value;
            }
            else
                base.SetField(name, value);
        }

    }
}
