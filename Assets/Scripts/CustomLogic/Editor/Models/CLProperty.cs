using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CustomLogic.Editor.Models
{
    class CLProperty : BaseModel
    {
        public TypeReference Type { get; set; }

        [JsonProperty("label")]
        public string Name { get; set; }
        public XmlInfo Info { get; set; }

        [JsonProperty("readonly")]
        public bool IsReadonly { get; set; }

        [JsonIgnore]
        public string[] EnumNames { get; set; }
    }
}
