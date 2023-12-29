using ApplicationManagers;
using GameManagers;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicTimeBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicTimeBuiltin(): base("Time")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            return null;
        }

        public override object GetField(string name)
        {
            if (name == "TickTime")
                return Time.fixedDeltaTime;
            else if (name == "FrameTime")
                return Time.deltaTime;
            else if (name == "GameTime")
                return CustomLogicManager.Evaluator.CurrentTime;
            else if (name == "TimeScale")
                return Time.timeScale;
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (name == "TimeScale")
                Time.timeScale = value.UnboxToFloat();
        }
    }
}
