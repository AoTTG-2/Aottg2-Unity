using System.Collections.Generic;

namespace CustomLogic
{
    internal class CLBindingCache
    {
        private static CLBindingCache instance;
        private static CLBindingCache Instance => instance ??= new CLBindingCache();

        private readonly Dictionary<string, Dictionary<string, ICLMemberBinding>> _bindings = new();

        public static bool GetOrCreateBinding(string typeName, string varName, out ICLMemberBinding binding)
        {
            if (Instance._bindings.ContainsKey(typeName))
            {
                if (Instance._bindings[typeName].ContainsKey(varName))
                {
                    binding = Instance._bindings[typeName][varName];
                    return true;
                }
            }

            binding = CustomLogicBuiltinTypes.CreateBinding(typeName, varName);
            Instance._bindings[typeName] = Instance._bindings.GetValueOrDefault(typeName, new Dictionary<string, ICLMemberBinding>());
            Instance._bindings[typeName][varName] = binding;
            return true;
        }
    }
}