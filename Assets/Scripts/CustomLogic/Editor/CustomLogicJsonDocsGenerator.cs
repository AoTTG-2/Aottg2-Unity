using CustomLogic.Editor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CustomLogic.Editor
{
    class CustomLogicJsonDocsGenerator : BaseCustomLogicDocsGenerator
    {
        private readonly JsonSerializerSettings _settings = new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };

        public CustomLogicJsonDocsGenerator(CLType[] allTypes) : base(allTypes) { }

        public override string GetRelativeFilePath(CLType type) => $"json/{type.Name}.json";

        public override string Generate(CLType type)
        {
            return JsonConvert.SerializeObject(type, Formatting.Indented, _settings);
        }
    }
}