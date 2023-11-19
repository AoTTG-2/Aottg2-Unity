using Characters;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

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
            if (Titan != null && !Titan.Dead && Titan.IsMine())
            {
                if (methodName == "MoveTo")
                {
                    if (!Titan.AI)
                        return null;
                    var position = ((CustomLogicVector3Builtin)parameters[0]).Value;
                    var range = parameters[1].UnboxToFloat();
                    bool ignoreEnemies = (bool)parameters[2];
                    Titan.GetComponent<BaseTitanAIController>().MoveTo(position, range, ignoreEnemies);
                }
                else if (methodName == "Target")
                {
                    if (!Titan.AI)
                        return null;
                    var enemy = (CustomLogicCharacterBuiltin)parameters[0];
                    var focus = parameters[1].UnboxToFloat();
                    Titan.GetComponent<BaseTitanAIController>().SetEnemy(enemy.Character, focus);
                }
                else if (methodName == "Idle")
                {
                    if (!Titan.AI)
                        return null;
                    var time = parameters[0].UnboxToFloat();
                    Titan.GetComponent<BaseTitanAIController>().ForceIdle(time);
                }
                else if (methodName == "Wander")
                {
                    if (!Titan.AI)
                        return null;
                    Titan.GetComponent<BaseTitanAIController>().CancelOrder();
                }
                else if (methodName == "Blind")
                    Titan.Blind();
                else if (methodName == "Cripple")
                    Titan.Cripple();
                else if (methodName == "Emote")
                    Titan.Emote((string)parameters[0]);
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
            }
            if (name == "FocusRange")
            {
                if (Titan.IsMine() && Titan.AI)
                    return Titan.GetComponent<BaseTitanAIController>().FocusRange;
            }
            if (name == "NapePosition")
            {
                return new CustomLogicVector3Builtin(Titan.BasicCache.NapeHurtbox.transform.position);
            }
            if (name == "IsCrawler")
            {
                return Titan.IsCrawler;
            }
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (!Titan.IsMine())
                return;
            if (name == "Size")
                Titan.SetSize((float)value);
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
            else
                base.SetField(name, value);
        }

    }
}
