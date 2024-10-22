using System;
using System.Collections.Generic;
using ApplicationManagers;
using Settings;

namespace CustomLogic
{
    partial class CustomLogicEvaluator
    {
        public Dictionary<string, BaseSetting> GetModeSettings()
        {
            return GetInstanceSettings("Main");
        }

        public Dictionary<string, BaseSetting> GetComponentSettings(string component, List<string> parameters)
        {
            return GetInstanceSettings(component, parameters, false);
        }

        private Dictionary<string, BaseSetting> GetInstanceSettings(string className, List<string> parameters = null, bool catchError = true)
        {
            var isComponent = parameters != null;
            var settings = new Dictionary<string, BaseSetting>();
            var parameterDict = new Dictionary<string, string>();
            try
            {
                var instance = CreateClassInstance(className, new List<object>(), false);
                RunAssignmentsClassInstance(instance);

                if (isComponent)
                {
                    foreach (var str in parameters)
                    {
                        var strArr = str.Split(':');
                        parameterDict.Add(strArr[0], strArr[1]);
                    }
                }

                foreach (var variableName in instance.Variables.Keys)
                {
                    if (variableName.StartsWith("_"))
                        continue;
                    
                    var value = instance.Variables[variableName];
                    if (isComponent && parameterDict.TryGetValue(variableName, out var paramName))
                        value = CustomLogicComponentInstance.DeserializeValue(value, paramName);

                    settings[variableName] = value switch
                    {
                        float f => new FloatSetting(f),
                        string s => new StringSetting(s),
                        int i => new IntSetting(i),
                        bool b => new BoolSetting(b),
                        CustomLogicColorBuiltin c => new ColorSetting(c.Value),
                        CustomLogicVector3Builtin v => new Vector3Setting(v.Value),
                        _ => null
                    };
                }
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                if (catchError)
                {
                    DebugConsole.Log("Custom logic error getting " + className + " settings", true);
                }
            }

            return settings;
        }
    }
}