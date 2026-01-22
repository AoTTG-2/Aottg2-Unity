using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using CustomLogic.Editor.Models;
using Unity.VisualScripting;
using UnityEditor;

namespace CustomLogic.Editor
{
    class CLTypeProvider
    {
        private const BindingFlags MemberFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private static readonly Dictionary<string, string> _typeNameMap = new()
        {
            ["Int32"] = "int",
            ["Single"] = "float",
            ["Boolean"] = "bool",
            ["String"] = "string",
            ["Void"] = "null",
            ["Double"] = "double",
            [nameof(CustomLogicClassInstance)] = "class",
            [nameof(CustomLogicComponentInstance)] = "component",
            [nameof(UserMethod)] = "function",
        };

        private static readonly CLType ObjectType = new()
        {
            Name = "Object",
            Info = new XmlInfo
            {
                Summary = "The base type of all objects in the game."
            },
            IsStatic = false,
            IsAbstract = true,
            InheritBaseMembers = true,
            BaseType = null,
            Constructors = Array.Empty<CLConstructor>(),
            InstanceProperties = new CLProperty[]
            {
                new()
                {
                    Name = "Type",
                    Info = new XmlInfo
                    {
                        Summary = "The type of the object (such as \"Human\")"
                    },
                    IsReadonly = false,
                    Type = new TypeReference("string")
                },
                new()
                {
                    Name = "IsCharacter",
                    Info = new XmlInfo
                    {
                        Summary = "Whether or not the object is a Character type or any of its inheritors"
                    },
                    IsReadonly = false,
                    Type = new TypeReference("bool")
                },
            }
        };

        private static readonly CLType ComponentType = new()
        {
            Name = "Component",
            Info = new XmlInfo
            {
                Summary = "Represents a component script attached to a MapObject."
            },
            IsStatic = false,
            IsAbstract = true,
            InheritBaseMembers = true,
            BaseType = ObjectType,
            Constructors = Array.Empty<CLConstructor>(),
            InstanceProperties = new CLProperty[]
            {
                new()
                {
                    Name = "MapObject",
                    Info = new XmlInfo
                    {
                        Summary = "The MapObject the component is attached to."
                    },
                    IsReadonly = true,
                    Type = new TypeReference("MapObject")
                },
                new()
                {
                    Name = "NetworkView",
                    Info = new XmlInfo
                    {
                        Summary = "The NetworkView attached to the MapObject, if Networked is enabled."
                    },
                    IsReadonly = true,
                    Type = new TypeReference("NetworkView")
                },
            }
        };

        private readonly Dictionary<Type, CLType> _resolvedTypes = new();

        private string ResolveTypeName(Type type)
        {
            if (type.Name == "Object")
                return ObjectType.Name;

            if (_resolvedTypes.ContainsKey(type))
                return _resolvedTypes[type].Name;

            if (_typeNameMap.ContainsKey(type.Name))
                return _typeNameMap[type.Name];

            return type.Name;
        }

        private string[] ResolveEnumNames(Type[] enumTypes)
        {
            if (enumTypes == null || enumTypes.Length == 0)
                return null;

            var enumNames = new List<string>();
            foreach (var enumType in enumTypes)
            {
                var enumClTypeAttribute = CustomLogicReflectionUtils.GetAttribute<CLTypeAttribute>(enumType);
                var enumName = enumClTypeAttribute != null ? enumClTypeAttribute.Name : enumType.Name;
                enumNames.Add(enumName);
            }

            return enumNames.Count > 0 ? enumNames.ToArray() : null;
        }

        /// <summary>
        /// Parses a type reference string that may contain generic type arguments.
        /// Examples: "K", "List&lt;string&gt;", "List&lt;K&gt;", "Dict&lt;K,V&gt;"
        /// </summary>
        private TypeReference ParseTypeReferenceString(string typeString)
        {
            if (string.IsNullOrWhiteSpace(typeString))
                return new TypeReference("Object");

            typeString = typeString.Trim();

            var openBracketIndex = typeString.IndexOf('<');
            if (openBracketIndex < 0)
            {
                return new TypeReference(typeString);
            }

            var typeName = typeString.Substring(0, openBracketIndex).Trim();
            
            var closeBracketIndex = typeString.LastIndexOf('>');
            if (closeBracketIndex < openBracketIndex)
            {
                // Malformed, treat as simple type
                return new TypeReference(typeString);
            }

            var argumentsString = typeString.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1).Trim();
            
            var arguments = ParseTypeArguments(argumentsString);
            
            return new TypeReference(typeName, arguments);
        }

        /// <summary>
        /// Parses a comma-separated list of type arguments, respecting nested generics.
        /// </summary>
        private TypeReference[] ParseTypeArguments(string argumentsString)
        {
            if (string.IsNullOrWhiteSpace(argumentsString))
                return Array.Empty<TypeReference>();

            var arguments = new List<TypeReference>();
            var currentArg = new System.Text.StringBuilder();
            var bracketDepth = 0;

            foreach (var ch in argumentsString)
            {
                if (ch == '<')
                {
                    bracketDepth++;
                    currentArg.Append(ch);
                }
                else if (ch == '>')
                {
                    bracketDepth--;
                    currentArg.Append(ch);
                }
                else if (ch == ',' && bracketDepth == 0)
                {
                    var argStr = currentArg.ToString().Trim();
                    if (!string.IsNullOrEmpty(argStr))
                    {
                        arguments.Add(ParseTypeReferenceString(argStr));
                    }
                    currentArg.Clear();
                }
                else
                {
                    currentArg.Append(ch);
                }
            }

            var lastArgStr = currentArg.ToString().Trim();
            if (!string.IsNullOrEmpty(lastArgStr))
            {
                arguments.Add(ParseTypeReferenceString(lastArgStr));
            }

            return arguments.ToArray();
        }

        private string ExtractCategoryFromType(Type type)
        {
            var typeName = type.Name;
            var typeNameWithoutSuffix = typeName.Replace("Builtin", "").Replace("Instance", "");
            
            // Search for the source file in the project
            var scriptPath = FindScriptPath(typeName);
            if (!string.IsNullOrEmpty(scriptPath))
            {
                // Extract the parent folder name from the path
                // Example: Assets\Scripts\CustomLogic\Builtin\Collections\CustomLogicDictBuiltin.cs
                // Should extract "Collections"
                var normalizedPath = scriptPath.Replace('\\', '/');
                var builtinIndex = normalizedPath.IndexOf("Builtin/");
                
                if (builtinIndex >= 0)
                {
                    var afterBuiltin = normalizedPath.Substring(builtinIndex + "Builtin/".Length);
                    var parts = afterBuiltin.Split('/');
                    
                    if (parts.Length > 1)
                    {
                        // Return the folder name immediately after "Builtin"
                        return parts[0];
                    }
                }
            }
            
            return null;
        }

        private string FindScriptPath(string typeName)
        {
            // Search for .cs files matching the type name
            var guids = AssetDatabase.FindAssets($"{typeName} t:MonoScript");
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var fileName = Path.GetFileNameWithoutExtension(path);
                
                if (fileName == typeName)
                {
                    return path;
                }
            }
            
            return null;
        }

        private void ResolveCLType(Type type, XmlDocument xmlDocument)
        {
            var clTypeAttribute = CustomLogicReflectionUtils.GetAttribute<CLTypeAttribute>(type);
            var className = clTypeAttribute.Name;
            var isStatic = clTypeAttribute.Static;
            var isAbstract = clTypeAttribute.Abstract;
            var inheritBaseMembers = clTypeAttribute.InheritBaseMembers;
            var isComponent = clTypeAttribute.IsComponent;
            
            string[] typeParameters = null;
            if (clTypeAttribute.TypeParameters != null && clTypeAttribute.TypeParameters.Length > 0)
            {
                typeParameters = clTypeAttribute.TypeParameters;
            }

            var typeXmlInfo = XmlInfo.FromTypeXml(xmlDocument, type);
            var category = ExtractCategoryFromType(type);

            var clType = new CLType
            {
                Name = className,
                IsStatic = isStatic,
                IsAbstract = isAbstract,
                InheritBaseMembers = inheritBaseMembers,
                IsComponent = isComponent,
                TypeParameters = typeParameters,
                Category = category,
                Info = typeXmlInfo,
                ObsoleteMessage = CustomLogicReflectionUtils.GetObsoleteMessage(type),
            };

            _resolvedTypes[type] = clType;
        }

        private void ResolveConstructors(Type type, CLType clType, XmlDocument xmlDocument)
        {
            var constructors = type.GetConstructors(MemberFlags)
                .Where(x => x.GetCustomAttributes(typeof(CLConstructorAttribute), false).Length > 0)
                .ToArray();

            var constructorCount = constructors.Count();

            if (!clType.IsAbstract && (!clType.IsStatic || constructorCount > 0))
            {
                var output = new CLConstructor[constructorCount];

                for (int i = 0; i < constructors.Length; i++)
                {
                    ConstructorInfo ctor = constructors[i];

                    var parameters = ctor.GetParameters();
                    var parameterNames = parameters.Select(x => x.Name).ToList();
                    var parameterTypes = parameters.Select(x =>
                    {
                        if (x.ParameterType.IsArray)
                        {
                            var eType = x.ParameterType.GetElementType();
                            return new TypeReference(eType.Name);
                        }
                        return new TypeReference(ResolveTypeName(x.ParameterType));
                    }).ToList();
                    var parameterValues = parameters.Select(x => $"{x.PseudoDefaultValue()}" != string.Empty ? $"{x.PseudoDefaultValue()}" : ResolveTypeName(x.ParameterType)).ToList();

                    var clParameters = new List<CLParameter>(parameterNames.Count);
                    for (int j = 0; j < parameterNames.Count; j++)
                    {
                        var parameterType = parameterTypes[j];
                        var parameterInfo = parameters[j];
                        
                        var clParamAttribute = parameterInfo.GetCustomAttribute<CLParamAttribute>();
                        
                        if (clParamAttribute != null && !string.IsNullOrEmpty(clParamAttribute.Type))
                        {
                            parameterType = ParseTypeReferenceString(clParamAttribute.Type);
                        }
                        
                        string[] enumNames = null;
                        if (clParamAttribute != null && clParamAttribute.Enum != null && clParamAttribute.Enum.Length > 0)
                        {
                            enumNames = ResolveEnumNames(clParamAttribute.Enum);
                        }
                        
                        string parameterDescription = XmlDocumentUtils.GetParameterNodeText(xmlDocument, type, ctor, parameterInfo);
                        
                        clParameters.Add(new CLParameter
                        {
                            Name = parameterNames[j],
                            Description = parameterDescription,
                            Type = parameterType,
                            DefaultValue = parameterValues[j],
                            IsOptional = parameterInfo.IsOptional,
                            IsVariadic = CustomLogicReflectionUtils.IsVariadicParameter(parameters[j]),
                            EnumNames = enumNames
                        });
                    }

                    var ctorAttribute = ctor.GetCustomAttribute<CLConstructorAttribute>();
                    var ctorXmlInfo = XmlInfo.FromConstructorXml(xmlDocument, type, ctor);

                    output[i] = new CLConstructor
                    {
                        Info = ctorXmlInfo,
                        Parameters = clParameters.ToArray(),
                        ObsoleteMessage = CustomLogicReflectionUtils.GetObsoleteMessage(ctor),
                    };
                }

                clType.Constructors = output;
            }
        }

        private void ResolveInheritence(Type type, CLType clType)
        {
            CLType baseType = null;
            if (clType.InheritBaseMembers && type.BaseType != null)
            {
                var parentType = type.BaseType;
                if (parentType == typeof(BuiltinClassInstance))
                {
                    baseType = ObjectType;
                }
                else
                {
                    if (parentType.HasAttribute<CLTypeAttribute>() && _resolvedTypes.ContainsKey(parentType))
                    {
                        baseType = _resolvedTypes[parentType];
                    }
                }
                clType.BaseType = baseType;
            }
        }

        private void ResolveProperties(Type type, CLType clType, XmlDocument xmlDocument)
        {
            var properties = type.GetProperties(MemberFlags)
                .Where(CustomLogicReflectionUtils.IsCustomLogicProperty)
                .ToArray();

            var fields = type.GetFields(MemberFlags)
                .Where(CustomLogicReflectionUtils.IsCustomLogicProperty)
                .ToArray();

            var staticProperties = new List<CLProperty>();
            var instanceProperties = new List<CLProperty>();

            foreach (var property in properties)
            {
                var clPropertyAttribute = CustomLogicReflectionUtils.GetAttribute<CLPropertyAttribute>(property);
                var isStatic = CustomLogicReflectionUtils.IsPropertyStatic(property) || clPropertyAttribute.Static;
                var isHybrid = clPropertyAttribute.Hybrid;

                var typeRef = new TypeReference(ResolveTypeName(property.PropertyType));
                // Apply generic type arguments from attribute if specified
                if (clPropertyAttribute?.TypeArguments != null && clPropertyAttribute.TypeArguments.Length > 0)
                {
                    typeRef.Arguments = clPropertyAttribute.TypeArguments.Select(arg => new TypeReference(arg)).ToArray();
                }

                var propertyXmlInfo = XmlInfo.FromPropertyXml(xmlDocument, type, property);

                string[] enumNames = null;
                if (clPropertyAttribute != null && clPropertyAttribute.Enum != null && clPropertyAttribute.Enum.Length > 0)
                {
                    enumNames = ResolveEnumNames(clPropertyAttribute.Enum);
                }

                var cLProperty = new CLProperty
                {
                    Name = property.Name,
                    Type = typeRef,
                    IsReadonly = clPropertyAttribute.ReadOnly || !property.CanWrite,
                    Info = propertyXmlInfo,
                    ObsoleteMessage = CustomLogicReflectionUtils.GetObsoleteMessage(property),
                    EnumNames = enumNames
                };

                if (isHybrid || isStatic)
                    staticProperties.Add(cLProperty);
                if (isHybrid || !isStatic)
                    instanceProperties.Add(cLProperty);
            }

            foreach (var field in fields)
            {
                var clPropertyAttribute = CustomLogicReflectionUtils.GetAttribute<CLPropertyAttribute>(field);

                var typeRef = new TypeReference(ResolveTypeName(field.FieldType));
                // Apply generic type arguments from attribute if specified
                if (clPropertyAttribute?.TypeArguments != null && clPropertyAttribute.TypeArguments.Length > 0)
                {
                    typeRef.Arguments = clPropertyAttribute.TypeArguments.Select(arg => new TypeReference(arg)).ToArray();
                }

                var fieldXmlInfo = XmlInfo.FromFieldXml(xmlDocument, type, field);

                string[] enumNames = null;
                if (clPropertyAttribute != null && clPropertyAttribute.Enum != null && clPropertyAttribute.Enum.Length > 0)
                {
                    enumNames = ResolveEnumNames(clPropertyAttribute.Enum);
                }

                var cLProperty = new CLProperty
                {
                    Name = field.Name,
                    Type = typeRef,
                    IsReadonly = clPropertyAttribute.ReadOnly,
                    Info = fieldXmlInfo,
                    ObsoleteMessage = CustomLogicReflectionUtils.GetObsoleteMessage(field),
                    EnumNames = enumNames
                };

                var isStatic = field.IsStatic || clPropertyAttribute.Static;
                var isHybrid = clPropertyAttribute.Hybrid;

                if (isHybrid || isStatic)
                    staticProperties.Add(cLProperty);
                if (isHybrid || !isStatic)
                    instanceProperties.Add(cLProperty);
            }

            clType.StaticProperties = staticProperties.ToArray();
            clType.InstanceProperties = instanceProperties.ToArray();
        }

        private void ResolveMethods(Type type, CLType clType, XmlDocument xmlDocument)
        {
            var methods = type.GetMethods(MemberFlags)
                .Where(CustomLogicReflectionUtils.IsCustomLogicMethod)
                .ToArray();

            var staticMethods = new List<CLMethod>();
            var instanceMethods = new List<CLMethod>();

            foreach (var method in methods)
            {
                var clMethodAttribute = CustomLogicReflectionUtils.GetAttribute<CLMethodAttribute>(method);
                var isStatic = method.IsStatic || clMethodAttribute.Static;
                var isHybrid = clMethodAttribute.Hybrid;

                var parameters = method.GetParameters();
                var parameterNames = parameters.Select(x => x.Name).ToList();
                var parameterTypes = parameters.Select(x =>
                {
                    if (x.ParameterType.IsArray)
                    {
                        var eType = x.ParameterType.GetElementType();
                        return new TypeReference(eType.Name);
                    }
                    return new TypeReference(ResolveTypeName(x.ParameterType));
                }).ToList();
                var parameterValues = parameters.Select(x => CustomLogicReflectionUtils.GetDefaultValueAsString(x)).ToList();

                var info = XmlInfo.FromMethodXml(xmlDocument, type, method);

                var clParameters = new List<CLParameter>(parameterNames.Count);
                for (int j = 0; j < parameterNames.Count; j++)
                {
                    var parameterType = parameterTypes[j];
                    var parameterInfo = parameters[j];
                    
                    var clParamAttribute = parameterInfo.GetCustomAttribute<CLParamAttribute>();
                    
                    // Apply parameter type arguments from CLParamAttribute if specified
                    if (clParamAttribute != null && !string.IsNullOrEmpty(clParamAttribute.Type))
                    {
                        parameterType = ParseTypeReferenceString(clParamAttribute.Type);
                    }
                    
                    string[] enumNames = null;
                    if (clParamAttribute != null && clParamAttribute.Enum != null && clParamAttribute.Enum.Length > 0)
                    {
                        enumNames = ResolveEnumNames(clParamAttribute.Enum);
                    }
                    
                    string parameterDescription = XmlDocumentUtils.GetParameterNodeText(xmlDocument, type, method, parameterInfo);
                    
                    clParameters.Add(new CLParameter
                    {
                        Name = parameterNames[j],
                        Description = parameterDescription,
                        Type = parameterType,
                        DefaultValue = parameterValues[j],
                        IsOptional = parameterInfo.IsOptional,
                        IsVariadic = CustomLogicReflectionUtils.IsVariadicParameter(parameterInfo),
                        EnumNames = enumNames
                    });
                }

                var returnTypeRef = new TypeReference(ResolveTypeName(method.ReturnType));
                // Apply generic type arguments from attribute if specified
                if (clMethodAttribute?.ReturnTypeArguments != null && clMethodAttribute.ReturnTypeArguments.Length > 0)
                {
                    returnTypeRef.Arguments = clMethodAttribute.ReturnTypeArguments.Select(arg => new TypeReference(arg)).ToArray();
                }

                var cLMethod = new CLMethod
                {
                    Name = method.Name,
                    Info = info,
                    ReturnType = returnTypeRef,
                    Parameters = clParameters.ToArray(),
                    ObsoleteMessage = CustomLogicReflectionUtils.GetObsoleteMessage(method),
                };

                if (isHybrid || isStatic)
                    staticMethods.Add(cLMethod);
                if (isHybrid || !isStatic)
                    instanceMethods.Add(cLMethod);
            }

            clType.StaticMethods = staticMethods.OrderBy(x => x.IsObsolete).ToArray();
            clType.InstanceMethods = instanceMethods.OrderBy(x => x.IsObsolete).ToArray();
        }

        public CLType[] GetCLTypes(XmlDocument xmlDocument)
        {
            var classes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(CustomLogicReflectionUtils.IsCustomLogicType)
                .ToArray();

            foreach (var type in classes)
            {
                ResolveCLType(type, xmlDocument);
            }

            foreach (var (type, clType) in _resolvedTypes)
            {
                ResolveConstructors(type, clType, xmlDocument);
            }

            foreach (var (type, clType) in _resolvedTypes)
            {
                ResolveInheritence(type, clType);
            }

            foreach (var (type, clType) in _resolvedTypes)
            {
                ResolveProperties(type, clType, xmlDocument);
                ResolveMethods(type, clType, xmlDocument);
            }

            return _resolvedTypes.Values.Append(ObjectType).Append(ComponentType).ToArray();
        }
    }
}
