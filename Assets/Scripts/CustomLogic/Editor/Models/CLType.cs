using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CustomLogic.Editor.Models
{
    class CLType : BaseModel
    {
        public string Name { get; set; }
        public XmlInfo Info { get; set; }

        [JsonIgnore] public bool IsStatic { get; set; }
        [JsonIgnore] public bool IsAbstract { get; set; }
        [JsonIgnore] public bool InheritBaseMembers { get; set; }

        [JsonIgnore] public CLType BaseType { get; set; }

        public CLConstructor[] Constructors { get; set; }

        [JsonProperty("staticFields")]
        public CLProperty[] StaticProperties { get; set; }

        [JsonProperty("instanceFields")]
        public CLProperty[] InstanceProperties { get; set; }

        public CLMethod[] StaticMethods { get; set; }
        public CLMethod[] InstanceMethods { get; set; }

        public string Kind => IsStatic ? "EXTENSION" : "CLASS";
        public string BaseTypeName => BaseType?.Name ?? string.Empty;
    }
}
