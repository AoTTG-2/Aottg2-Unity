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
            return base.CallMethod(name, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "TickTime")
                return Time.fixedDeltaTime;
            if (name == "FrameTime")
                return Time.deltaTime;
            if (name == "GameTime")
                return CustomLogicManager.Evaluator.CurrentTime;
            if (name == "TimeScale")
                return Time.timeScale;
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (name == "TimeScale")
                Time.timeScale = value.UnboxToFloat();
            else
                base.SetField(name, value);
        }
    }
}
