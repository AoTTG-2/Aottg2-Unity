namespace CustomLogic.Editor.Models
{
    class CLParameter
    {
        public string Name { get; set; }
        public XmlInfo Info { get; set; }
        public TypeReference Type { get; set; }
        public string DefaultValue { get; set; }
        public bool IsOptional { get; set; }
        public bool IsVariadic { get; set; }
        public string EnumName { get; set; }
    }
}
