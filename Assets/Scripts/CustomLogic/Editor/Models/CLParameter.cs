namespace CustomLogic.Editor.Models
{
    class CLParameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TypeReference Type { get; set; }
        public string DefaultValue { get; set; }
        public bool IsOptional { get; set; }
        public bool IsVariadic { get; set; }
        public string[] EnumNames { get; set; }
    }
}
