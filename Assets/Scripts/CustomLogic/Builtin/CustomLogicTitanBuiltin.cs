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
            if (Titan != null && !Titan.Dead)
            {
                if (Titan.IsMine())
                {
                    if (methodName == "MoveTo")
                    {
                        if (!Titan.AI)
                            return null;
                        var position = ((CustomLogicVector3Builtin)parameters[0]).Value;
                        var range = parameters[1].UnboxToFloat();
                        bool ignoreEnemies = (bool)parameters[2];
                        Titan.GetComponent<BaseTitanAIController>().MoveTo(position, range, ignoreEnemies);
                        return null;
                    }
                    if (methodName == "Target")
                    {
                        if (!Titan.AI)
                            return null;
                        var enemy = (CustomLogicCharacterBuiltin)parameters[0];
                        var focus = parameters[1].UnboxToFloat();
                        Titan.GetComponent<BaseTitanAIController>().SetEnemy(enemy.Character, focus);
                        return null;
                    }
                    if (methodName == "Idle")
                    {
                        if (!Titan.AI)
                            return null;
                        var time = parameters[0].UnboxToFloat();
                        Titan.GetComponent<BaseTitanAIController>().ForceIdle(time);
                        return null;
                    }
                    if (methodName == "Wander")
                    {
                        if (!Titan.AI)
                            return null;
                        Titan.GetComponent<BaseTitanAIController>().CancelOrder();
                        return null;
                    }
                    if (methodName == "Blind")
                    {
                        Titan.Blind();
                        return null;
                    }
                    if (methodName == "Cripple")
                    {
                        var time = parameters[0].UnboxToFloat();
                        Titan.Cripple(time);
                        return null;
                    }
                    if (methodName == "Emote")
                    {
                        Titan.Emote((string)parameters[0]);
                        return null;
                    }
                }

                if (methodName == "Reveal")
                {
                    Titan.Reveal(0, parameters[0].UnboxToFloat());
                    return null;
                }
                if (methodName == "AddOutline")
                {
                    Titan.AddOutline();
                    return null;
                }
                else if (methodName == "RemoveOutline")
                {
                    Titan.RemoveOutline();
                    return null;
                }
                return base.CallMethod(methodName, parameters);
            }
            return null;
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
            if (name == "NapePosition")
            {
                return new CustomLogicVector3Builtin(Titan.BasicCache.NapeHurtbox.transform.position);
            }
            if (name == "IsCrawler")
            {
                return Titan.IsCrawler;
            }
            if (name == "UsePathfinding")
            {
                if (Titan.IsMine() && Titan.AI)
                    return Titan.GetComponent<BaseTitanAIController>()._usePathfinding;
                return null;
            }
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (!Titan.IsMine())
                return;
            if (name == "Size")
                Titan.SetSize(value.UnboxToFloat());
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
