using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLogic
{
    /// <summary>
    /// All CLClassInstances will maintain their own static dictionary cache for builtins, on creation, they will be added to this in order of inheritance.
    /// The child class should appear first in the list, so that it can override the parent class.
    /// </summary>
    class CustomLogicClassVariables
    {
        private Dictionary<string, object> _variables = new Dictionary<string, object>();
        private List<Dictionary<string, object>> _builtinVariables = new List<Dictionary<string, object>>();

        public CustomLogicClassVariables() {}

        public object this[string key]
        {
            get
            {
                if (_variables.ContainsKey(key))
                {
                    return _variables[key];
                }
                
                // Try to get from builtin variables in order of merge (child class, parent class, etc...)
                foreach (var builtin in _builtinVariables)
                {
                    if (builtin.ContainsKey(key))
                    {
                        return builtin[key];
                    }
                }
                throw new KeyNotFoundException();
            }
            set
            {
                _variables[key] = value;
            }
        }

        public void MergeCachedBuiltinVariables(Dictionary<string, object> builtinVariables)
        {
            _builtinVariables.Add(builtinVariables);
        }

        public object RemoveVariable(string key)
        {
            if (_variables.ContainsKey(key))
                return _variables.Remove(key);
            throw new KeyNotFoundException();
        }

        public void ClearVariables()
        {
            _variables.Clear();
        }

        public void ClearBuiltinVariables()
        {
            // Clear the list, not the static referenced dictionaries.
            _builtinVariables.Clear();
        }

        public void ClearAllVariables()
        {
            ClearVariables();
            ClearBuiltinVariables();
        }

        public bool ContainsVariable(string key)
        {
            if (_variables.ContainsKey(key))
                return true;
            return ContainsBuiltinVariable(key);
        }

        public bool ContainsBuiltinVariable(string key)
        {
            foreach (var builtin in _builtinVariables)
            {
                if (builtin.ContainsKey(key))
                    return true;
            }
            return false;
        }

        public void Add(string key, object value)
        {
            _variables.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return ContainsVariable(key);
        }

        public bool BuiltinIsEmpty()
        {
            return _builtinVariables.Count == 0;
        }

        public bool BuiltinVariablesAreEmpty()
        {
            foreach (var builtin in _builtinVariables)
            {
                if (builtin.Count > 0)
                    return false;
            }
            return true;
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
