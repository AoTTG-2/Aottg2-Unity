using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLogic
{
    class CustomLogicClassVariables
    {
        public string ClassName { get; set; }
        private Dictionary<string, object> _variables = new Dictionary<string, object>();
        private static Dictionary<string, Dictionary<string, object>> _builtinVariables = new Dictionary<string, Dictionary<string, object>>();


        public CustomLogicClassVariables(string className)
        {
            ClassName = className;
            if (!_builtinVariables.ContainsKey(className))
            {
                _builtinVariables[className] = new Dictionary<string, object>();
            }
        }

        // Implement all methods that exist on dictionary including [] operator. first check in variables for each reference, if it doesn't exist, check in builtin variables scoped to the class name.
        // for assignment, only assign to variables, not builtin variables. Expose a separate method to assign to builtin variables.

        public object this[string key]
        {
            get
            {
                if (_variables.ContainsKey(key))
                {
                    return _variables[key];
                }
                else if (_builtinVariables.ContainsKey(ClassName) && _builtinVariables[ClassName].ContainsKey(key))
                {
                    return _builtinVariables[ClassName][key];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            set
            {
                _variables[key] = value;
            }
        }

        public void AssignToBuiltinVariable(string key, object value)
        {
            if (!_builtinVariables.ContainsKey(ClassName))
            {
                _builtinVariables[ClassName] = new Dictionary<string, object>();
            }
            _builtinVariables[ClassName][key] = value;
        }

        public void RemoveVariable(string key)
        {
            if (_variables.ContainsKey(key))
            {
                _variables.Remove(key);
            }
            else if (_builtinVariables.ContainsKey(ClassName) && _builtinVariables[ClassName].ContainsKey(key))
            {
                _builtinVariables[ClassName].Remove(key);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void ClearVariables()
        {
            _variables.Clear();
        }

        public void ClearBuiltinVariables()
        {
            if (_builtinVariables.ContainsKey(ClassName))
            {
                _builtinVariables[ClassName].Clear();
            }
        }

        public void ClearAllVariables()
        {
            ClearVariables();
            ClearBuiltinVariables();
        }

        public bool ContainsVariable(string key)
        {
            return _variables.ContainsKey(key) || (_builtinVariables.ContainsKey(ClassName) && _builtinVariables[ClassName].ContainsKey(key));
        }

        public bool ContainsBuiltinVariable(string key)
        {
            return _builtinVariables.ContainsKey(ClassName) && _builtinVariables[ClassName].ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            _variables.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _variables.ContainsKey(key) || _builtinVariables[ClassName].ContainsKey(key);
        }

        public bool BuiltinIsEmpty()
        {
            return _builtinVariables.ContainsKey(ClassName) && _builtinVariables[ClassName].Count == 0;
        }

        // Keys
        public Dictionary<string, object>.KeyCollection Keys
        {
            get
            {
                return _variables.Keys;
            }
        }
    }
}
