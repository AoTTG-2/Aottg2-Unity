using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CustomLogic.Editor.Models
{
    class CLMethod : BaseModel
    {
        public CLParameter[] Parameters { get; set; }
        public TypeReference ReturnType { get; set; }

        [JsonProperty("label")]
        public string Name { get; set; }
        public XmlInfo Info { get; set; }
    }
}
