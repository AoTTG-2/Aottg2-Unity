﻿using Characters;
using Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicShifterBuiltin : CustomLogicCharacterBuiltin
    {
        public BaseShifter Shifter;

        public CustomLogicShifterBuiltin(BaseShifter shifter) : base(shifter, "Shifter")
        {
            Shifter = shifter;
        }
        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (Shifter != null && !Shifter.Dead)
            {
                if (Shifter.IsMine())
                {
                    if (methodName == "MoveTo")
                    {
                        if (!Shifter.AI)
                            return null;
                        var position = ((CustomLogicVector3Builtin)parameters[0]).Value;
                        var range = parameters[1].UnboxToFloat();
                        bool ignoreEnemies = (bool)parameters[2];
                        Shifter.GetComponent<BaseTitanAIController>().MoveTo(position, range, ignoreEnemies);
                        return null;
                    }
                    if (methodName == "Target")
                    {
                        if (!Shifter.AI)
                            return null;
                        var enemy = (CustomLogicCharacterBuiltin)parameters[0];
                        var focus = parameters[1].UnboxToFloat();
                        Shifter.GetComponent<BaseTitanAIController>().SetEnemy(enemy.Character, focus);
                        return null;
                    }
                    if (methodName == "Idle")
                    {
                        if (!Shifter.AI)
                            return null;
                        var time = parameters[0].UnboxToFloat();
                        Shifter.GetComponent<BaseTitanAIController>().ForceIdle(time);
                        return null;
                    }
                    if (methodName == "Wander")
                    {
                        if (!Shifter.AI)
                            return null;
                        Shifter.GetComponent<BaseTitanAIController>().CancelOrder();
                        return null;
                    }
                    if (methodName == "Blind")
                    {
                        Shifter.Blind();
                        return null;
                    }
                    if (methodName == "Cripple")
                    {
                        Shifter.Cripple();
                        return null;
                    }
                    if (methodName == "Emote")
                    {
                        Shifter.Emote((string)parameters[0]);
                        return null;
                    }
                }
                return base.CallMethod(methodName, parameters);
            }
            return null;
        }

        public override object GetField(string name)
        {
            if (name == "Size")
                return Shifter.Size;
            if (name == "DetectRange")
            {
                if (Shifter.IsMine() && Shifter.AI)
                    return Shifter.GetComponent<BaseTitanAIController>().DetectRange;
                return null;
            }
            if (name == "FocusRange")
            {
                if (Shifter.IsMine() && Shifter.AI)
                    return Shifter.GetComponent<BaseTitanAIController>().FocusRange;
                return null;
            }
            if (name == "NapePosition")
            {
                return new CustomLogicVector3Builtin(Shifter.BaseTitanCache.NapeHurtbox.transform.position);
            }
            if (name == "DeathAnimLength")
            {
                return Shifter.DeathAnimationLength;
            }
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (!Shifter.IsMine())
                return;
            if (name == "Size")
                Shifter.SetSize((float)value);
            else if (name == "DetectRange")
            {
                if (Shifter.AI)
                    Shifter.GetComponent<BaseTitanAIController>().SetDetectRange(value.UnboxToFloat());
            }
            else if (name == "FocusRange")
            {
                if (Shifter.AI)
                    Shifter.GetComponent<BaseTitanAIController>().FocusRange = value.UnboxToFloat();
            }
            if (name == "DeathAnimLength")
            {
                Shifter.DeathAnimationLength = value.UnboxToFloat();
            }
            else
                base.SetField(name, value);
        }
    }
}
