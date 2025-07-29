using System;

namespace CustomLogic.Editor.Models
{
    class TypeReference
    {
        public string Name;
        public TypeReference[] Arguments;

        public TypeReference(string name)
        {
            Name = name;
            Arguments = Array.Empty<TypeReference>();
        }

        public TypeReference(string name, TypeReference[] arguments)
        {
            Name = name;
            Arguments = arguments;
        }
    }
}
