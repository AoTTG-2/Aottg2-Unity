using System.Collections.Generic;
using Utility;

namespace CustomLogic
{
    class CLCommandArgument
    {
        public string type;
        public string defaultValue;
        public bool optional = false;

        public CLCommandArgument(string serialized)
        {
            // Format is type[=defaultValue] where defaultValue being specified makes it optional
            var parts = serialized.Split('=');
            type = parts[0].Trim();
            if (parts.Length > 1)
            {
                optional = true;
                defaultValue = parts[1].Trim();
            }
        }

        public object GetDefault()
        {
            if (optional == false)
                throw new System.Exception("Argument is not optional, no default value");
            return Parse(defaultValue);
        }

        public object Parse(string argument)
        {
            switch (type.ToLower())
            {
                case "string":
                    return argument;
                case "int":
                case "integer":
                    if (int.TryParse(argument, out int intValue))
                        return intValue;
                    throw new System.Exception($"Could not parse '{argument}' as int");
                case "float":
                    if (float.TryParse(argument, out float doubleValue))
                        return doubleValue;
                    throw new System.Exception($"Could not parse '{argument}' as float");
                case "bool":
                case "boolean":
                    if (bool.TryParse(argument, out bool boolValue))
                        return boolValue;
                    throw new System.Exception($"Could not parse '{argument}' as bool");
                default:
                    throw new System.Exception($"Unknown argument type '{type}'");
            }
        }
    }
    class CLCommand
    {
        public string Command;
        public string Description;
        public UserMethod Method;
        public bool HasFormatting = false;
        public string Format;
        List<CLCommandArgument> Arguments = new();

        public CLCommand(string command, UserMethod method, string description = "", string format = "")
        {
            Command = command;
            Method = method;
            Description = description;
            Format = format;
            if (string.IsNullOrWhiteSpace(format) == false)
            {
                HasFormatting = true;
                var args = format.Split(',');
                foreach (var arg in args)
                {
                    Arguments.Add(new CLCommandArgument(arg.Trim()));
                }
            }
        }

        public object[] ParseIntoFormat(string command, string arguments)
        {
            if (HasFormatting == false)
                return new object[] { command, arguments };

            List<string> args = Util.SplitArguments(arguments);

            if (args.Count > Arguments.Count)
                throw new System.Exception($"Too many arguments for command '{Command}', expected {Arguments.Count} but got {args.Count}");

            List<object> parsedArgs = new() { command };
            for (int i = 0; i < Arguments.Count; i++)
            {
                if (i < args.Count)
                {
                    parsedArgs.Add(Arguments[i].Parse(args[i]));
                }
                else
                {
                    if (Arguments[i].optional)
                    {
                        parsedArgs.Add(Arguments[i].GetDefault());
                    }
                    else
                    {
                        throw new System.Exception($"Missing required argument {i + 1} of type '{Arguments[i].type}' for command '{Command}'");
                    }
                }
            }

            return parsedArgs.ToArray();
        }

        public void Execute(string rawCommand)
        {
            if (string.IsNullOrWhiteSpace(rawCommand))
                return;

            // Remove leading/trailing whitespace
            rawCommand = rawCommand.Trim();

            // Split into command and arguments
            string command = rawCommand;
            string arguments = string.Empty;

            int firstSpace = rawCommand.IndexOf(' ');
            if (firstSpace != -1)
            {
                command = rawCommand.Substring(0, firstSpace);
                arguments = rawCommand.Substring(firstSpace + 1).Trim();
            }

            object[] parsedArgs = ParseIntoFormat(command, arguments);

            CustomLogicManager.Evaluator.EvaluateMethod(Method, parsedArgs);
        }

    }


    /// <summary>
    /// References the main game camera.
    /// </summary>
    //[CLType(Name = "Commands", Abstract = true, Static = true)]
    partial class CustomLogicCommandsBuiltin : BuiltinClassInstance
    {
        private Dictionary<string, CLCommand> _commands = new();


        [CLConstructor]
        public CustomLogicCommandsBuiltin()
        {
        }

        [CLMethod(description: "Check if the given command is already registered")]
        public bool IsCommandRegistered(string command) => _commands.ContainsKey(command);

        [CLMethod(description: "Removes the given command from the registry")]
        public void UnregisterCommand(string command)
        {
            if (_commands.ContainsKey(command) == false)
                throw new System.Exception($"Command '{command}' is not registered");
            _commands.Remove(command);
        }

        [CLMethod(description: "Registers the given command and binds it to a CL Method in the format function(command, args)")]
        public void RegisterCommand(string command, UserMethod method, string description = "")
        {
            if (_commands.ContainsKey(command))
                throw new System.Exception($"Command '{command}' is already registered");

            _commands.Add(command, new CLCommand(command, method, description));
        }

        /// <summary>
        /// Registers the given command and binds it to a CL Method which matches the passed format.
        /// Ex: format = "string,int=5,bool=false" -> function(command, stringArg, intArg, boolArg)
        /// Callable as: /command "string value" 10 true or /command "string value" (intArg defaults to 5 and boolArg defaults to false)
        /// </summary>
        [CLMethod]
        public void RegisterFormattedCommand(string command, UserMethod method, string format, string description = "")
        {
            if (_commands.ContainsKey(command))
                throw new System.Exception($"Command '{command}' is already registered");

            _commands.Add(command, new CLCommand(command, method, description, format));
        }
    }
}
