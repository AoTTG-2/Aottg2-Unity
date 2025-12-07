using ApplicationManagers;
using GameManagers;
using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace CustomLogic
{
    internal partial class CustomLogicEvaluator
    {
        public string GetLineNumberString(int lineNumber)
        {
            if (Compiler != null)
                return Compiler.FormatLineNumber(lineNumber);
            return lineNumber.ToString();
        }

        private void LogCustomLogicError(string errorMessage, bool showInChat)
        {
            // Always log full error to debug console using the new method
            DebugConsole.LogCustomLogic(errorMessage, false);

            // Show in chat based on ShowErrorInChat setting
            if (showInChat && ChatManager.IsChatAvailable())
            {
                if (SettingsManager.UISettings.ChatCLErrors.Value)
                {
                    // Show full error message in chat
                    ChatManager.AddException(errorMessage);
                }
                else
                {
                    // Show simplified notification in chat
                    ChatManager.AddException($"CL Runtime Exception, press {SettingsManager.InputSettings.General.DebugWindow.GetKey()} to view in debug console");
                }
            }
        }

        public CustomLogicStartAst GetStartAst()
        {
            return _start;
        }

        public Dictionary<string, CustomLogicClassInstance> GetStaticClasses()
        {
            return _staticClasses;
        }

        public Dictionary<string, BaseSetting> GetModeSettings()
        {
            var instance = CreateClassInstance("Main", EmptyArgs, false);
            try
            {
                RunAssignmentsClassInstance(instance);
                Dictionary<string, BaseSetting> settings = new Dictionary<string, BaseSetting>();
                foreach (string variableName in instance.Variables.Keys)
                {
                    if (!variableName.StartsWith("_") && variableName != "Type")
                    {
                        object value = instance.Variables[variableName];
                        if (value is float)
                            settings.Add(variableName, new FloatSetting((float)value));
                        else if (value is string)
                            settings.Add(variableName, new StringSetting((string)value));
                        else if (value is int)
                            settings.Add(variableName, new IntSetting((int)value));
                        else if (value is bool)
                            settings.Add(variableName, new BoolSetting((bool)value));
                    }
                }
                return settings;
            }
            catch (Exception e)
            {
                LogCustomLogicError("Custom logic error getting main logic settings", true);
                return new Dictionary<string, BaseSetting>();
            }
        }

        public Dictionary<string, BaseSetting> GetComponentSettings(string component, List<string> parameters)
        {
            Dictionary<string, BaseSetting> settings = new Dictionary<string, BaseSetting>();
            Dictionary<string, string> parameterDict = new Dictionary<string, string>();
            try
            {
                var instance = CreateClassInstance(component, EmptyArgs, false);
                RunAssignmentsClassInstance(instance);
                foreach (string str in parameters)
                {
                    string[] strArr = str.Split(':');
                    parameterDict.Add(strArr[0], strArr[1]);
                }
                foreach (string variableName in instance.Variables.Keys)
                {
                    if (!variableName.StartsWith("_") && variableName != "Type")
                    {
                        object value = instance.Variables[variableName];
                        if (parameterDict.ContainsKey(variableName))
                            value = CustomLogicComponentInstance.DeserializeValue(value, parameterDict[variableName]);
                        if (value is float)
                            settings.Add(variableName, new FloatSetting((float)value));
                        else if (value is string)
                            settings.Add(variableName, new StringSetting((string)value));
                        else if (value is int)
                            settings.Add(variableName, new IntSetting((int)value));
                        else if (value is bool)
                            settings.Add(variableName, new BoolSetting((bool)value));
                        else if (value is CustomLogicColorBuiltin)
                            settings.Add(variableName, new ColorSetting(((CustomLogicColorBuiltin)value).Value));
                        else if (value is CustomLogicVector3Builtin)
                            settings.Add(variableName, new Vector3Setting(((CustomLogicVector3Builtin)value).Value));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Custom logic error getting component settings for " + component + ": " + e.Message + "\n" + e.InnerException);
            }
            return settings;
        }

        public List<string> GetComponentNames()
        {
            var componentNames = new List<string>();
            foreach (string className in _start.Classes.Keys)
            {
                if ((int)_start.Classes[className].Token.Value == (int)CustomLogicSymbol.Component)
                    componentNames.Add(className);
            }
            return componentNames;
        }

        /// <summary>
        /// Gets a static class instance by name (for testing purposes).
        /// </summary>
        public CustomLogicClassInstance GetStaticClass(string className)
        {
            if (_staticClasses.ContainsKey(className))
                return _staticClasses[className];
            return null;
        }

        /// <summary>
        /// Creates a static class instance by name (for testing purposes).
        /// </summary>
        public void CreateStaticClass(string className)
        {
            if (!_staticClasses.ContainsKey(className))
            {
                CustomLogicSourceType? classNamespace = null;
                if (_start.ClassNamespaces.TryGetValue(className, out var ns))
                    classNamespace = ns;

                var instance = CreateClassInstance(className, EmptyArgs, false, classNamespace);
                instance.Namespace = classNamespace;
                _staticClasses.Add(className, instance);
            }
        }
    }
}
