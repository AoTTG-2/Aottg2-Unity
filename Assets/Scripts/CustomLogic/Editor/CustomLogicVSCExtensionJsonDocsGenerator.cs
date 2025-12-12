using System;
using System.Collections.Generic;
using System.Linq;
using CustomLogic.Editor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CustomLogic.Editor
{
    class CustomLogicVSCExtensionJsonDocsGenerator : BaseCustomLogicDocsGenerator
    {
        private readonly JsonSerializerSettings _settings = new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        private readonly Dictionary<string, ClassDefinition> _typeNameMap = new();

        public CustomLogicVSCExtensionJsonDocsGenerator(CLType[] allTypes) : base(allTypes)
        {
            foreach (var type in allTypes)
            {
                _typeNameMap[type.Name] = ConvertToClassDefinition(type);
            }

            foreach (var type in allTypes)
            {
                var classDef = _typeNameMap[type.Name];
                if (type.InheritBaseMembers && type.BaseType != null)
                {
                    classDef.Extends = new List<string> { type.BaseType.Name };
                }
            }
        }

        public override string GetRelativeFilePath(CLType type) => $"vscode-json/{type.Name}.json";

        public override string Generate(CLType type)
        {
            var classDef = _typeNameMap[type.Name];
            return JsonConvert.SerializeObject(classDef, Formatting.Indented, _settings);
        }

        private string DetermineKind(CLType clType)
        {
            if (clType.IsComponent)
                return "component";

            if (clType.IsStatic)
                return "extension";

            return "class";
        }

        private string NormalizeDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return string.Empty;

            var lines = description.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToArray();

            if (lines.Length == 0)
                return string.Empty;

            return string.Join(" ", lines);
        }

        private List<string> ExtractTypeParameters(CLType clType)
        {
            if (clType.TypeParameters != null && clType.TypeParameters.Length > 0)
            {
                return clType.TypeParameters.ToList();
            }

            return new List<string>();
        }

        private ClassDefinition ConvertToClassDefinition(CLType clType)
        {
            var typeParameters = ExtractTypeParameters(clType);
            var kind = DetermineKind(clType);

            var classDef = new ClassDefinition
            {
                Kind = kind,
                Name = clType.Name,
                Description = NormalizeDescription(clType.Info?.Summary),
                TypeParameters = typeParameters.Count > 0 ? typeParameters : null,
                StaticFields = ConvertFields(clType.StaticProperties),
                StaticMethods = ConvertMethods(clType.StaticMethods),
                InstanceFields = ConvertFields(clType.InstanceProperties),
                InstanceMethods = ConvertMethods(clType.InstanceMethods),
                Constructors = ConvertConstructors(clType.Constructors)
            };

            return classDef;
        }

        private List<Field> ConvertFields(CLProperty[] properties)
        {
            if (properties == null || properties.Length == 0)
                return new List<Field>();

            return properties.Select(p => new Field
            {
                Label = p.Name,
                Type = ConvertTypeReference(p.Type),
                Description = NormalizeDescription(p.Info?.Summary),
                Readonly = p.IsReadonly,
                Private = false
            }).ToList();
        }


        private List<Method> ConvertMethods(CLMethod[] methods)
        {
            if (methods == null || methods.Length == 0)
                return new List<Method>();

            return methods
                .Where(m => !m.Name.StartsWith("__")) // Filter operators
                .Select(m => new Method
                {
                    Label = m.Name,
                    ReturnType = ConvertTypeReference(m.ReturnType),
                    Description = NormalizeDescription(m.Info?.Summary),
                    Parameters = ConvertParameters(m.Parameters),
                    Kind = "function"
                }).ToList();
        }

        private List<Constructor> ConvertConstructors(CLConstructor[] constructors)
        {
            if (constructors == null || constructors.Length == 0)
                return new List<Constructor>();

            return constructors.Select(c => new Constructor
            {
                Parameters = ConvertParameters(c.Parameters),
                Description = NormalizeDescription(c.Info?.Summary)
            }).ToList();
        }

        private List<Parameter> ConvertParameters(CLParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return new List<Parameter>();

            return parameters.Select(p => new Parameter
            {
                Name = p.Name,
                Type = ConvertTypeReference(p.Type),
                Description = NormalizeDescription(p.Info?.Summary),
                IsOptional = p.IsOptional,
                IsVariadic = p.IsVariadic
            }).ToList();
        }

        private JsonTypeReference ConvertTypeReference(Models.TypeReference typeRef)
        {
            if (typeRef == null)
                return new JsonTypeReference { Name = "void", TypeArguments = new List<JsonTypeReference>() };

            var name = typeRef.Name;

            if (name == "null" || name == "Void")
                name = "void";

            var typeRefObj = new JsonTypeReference
            {
                Name = name,
                TypeArguments = new List<JsonTypeReference>()
            };

            if (typeRef.Arguments != null && typeRef.Arguments.Length > 0)
            {
                typeRefObj.TypeArguments = typeRef.Arguments.Select(ConvertTypeReference).ToList();
                
                // If the type is "Object" with exactly one type argument that has no arguments itself,
                // replace the name with the type argument (e.g., Object<V> becomes V)
                if (name == "Object" && typeRefObj.TypeArguments.Count == 1)
                {
                    var singleArg = typeRefObj.TypeArguments[0];
                    if (singleArg.TypeArguments == null || singleArg.TypeArguments.Count == 0)
                    {
                        typeRefObj.Name = singleArg.Name;
                        typeRefObj.TypeArguments = new List<JsonTypeReference>();
                    }
                }
            }

            return typeRefObj;
        }


        private class ClassDefinition
        {
            public string Kind { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<string> TypeParameters { get; set; }
            public List<string> Extends { get; set; }
            public List<Field> InstanceFields { get; set; }
            public List<Method> InstanceMethods { get; set; }
            public List<Field> StaticFields { get; set; }
            public List<Method> StaticMethods { get; set; }
            public List<Constructor> Constructors { get; set; }
            // public bool Hidden { get; set; } // No need yet
        }

        private class Field
        {
            public string Label { get; set; }
            public JsonTypeReference Type { get; set; }
            public string Description { get; set; }
            public bool Readonly { get; set; }
            public bool Private { get; set; }
        }

        private class Method
        {
            public string Label { get; set; }
            public JsonTypeReference ReturnType { get; set; }
            public string Description { get; set; }
            public List<Parameter> Parameters { get; set; }
            public string Kind { get; set; }
        }

        private class Constructor
        {
            public List<Parameter> Parameters { get; set; }
            public string Description { get; set; }
        }

        private class Parameter
        {
            public string Name { get; set; }
            public JsonTypeReference Type { get; set; }
            public string Description { get; set; }
            public bool IsOptional { get; set; }
            public bool IsVariadic { get; set; }
        }

        private class JsonTypeReference
        {
            public string Name { get; set; }
            public List<JsonTypeReference> TypeArguments { get; set; }
        }
    }
}
