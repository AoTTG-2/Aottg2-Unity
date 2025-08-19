using System;
using System.Collections.Generic;
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

        // TODO: Generic types
        // TODO: Optional and variadic params

        private static readonly Dictionary<string, string> _typeNameMap = new()
        {
            ["Int32"] = "int",
            ["Single"] = "float",
            ["Boolean"] = "bool",
            ["String"] = "string",
            ["Void"] = "null",
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

        private void ResolveCLType(Type type, XmlDocument xmlDocument)
        {
            var clTypeAttribute = CustomLogicReflectionUtils.GetAttribute<CLTypeAttribute>(type);
            var className = clTypeAttribute.Name;
            var isStatic = clTypeAttribute.Static;
            var isAbstract = clTypeAttribute.Abstract;
            var inheritBaseMembers = clTypeAttribute.InheritBaseMembers;

            var clType = new CLType
            {
                Name = className,
                IsStatic = isStatic,
                IsAbstract = isAbstract,
                InheritBaseMembers = inheritBaseMembers,
                Info = XmlInfo.FromTypeXml(xmlDocument, type, clTypeAttribute),
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
                        clParameters.Add(new CLParameter
                        {
                            Name = parameterNames[j],
                            Type = parameterTypes[j],
                            DefaultValue = parameterValues[j]
                        });
                    }

                    output[i] = new CLConstructor
                    {
                        Info = XmlInfo.FromConstructorXml(xmlDocument, type, ctor),
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
                var isStatic = CustomLogicReflectionUtils.IsPropertyStatic(property);

                var cLProperty = new CLProperty
                {
                    Name = property.Name,
                    Type = new TypeReference(ResolveTypeName(property.PropertyType)),
                    IsReadonly = clPropertyAttribute.ReadOnly || !property.CanWrite,
                    Info = XmlInfo.FromPropertyXml(xmlDocument, type, property, clPropertyAttribute),
                    ObsoleteMessage = CustomLogicReflectionUtils.GetObsoleteMessage(property),
                };

                if (isStatic)
                    staticProperties.Add(cLProperty);
                else
                    instanceProperties.Add(cLProperty);
            }

            foreach (var field in fields)
            {
                var clPropertyAttribute = CustomLogicReflectionUtils.GetAttribute<CLPropertyAttribute>(field);

                var cLProperty = new CLProperty
                {
                    Name = field.Name,
                    Type = new TypeReference(ResolveTypeName(field.FieldType)),
                    IsReadonly = clPropertyAttribute.ReadOnly,
                    Info = XmlInfo.FromFieldXml(xmlDocument, type, field, clPropertyAttribute),
                    ObsoleteMessage = CustomLogicReflectionUtils.GetObsoleteMessage(field),
                };

                var isStatic = field.IsStatic;

                if (isStatic)
                    staticProperties.Add(cLProperty);
                else
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
                var isStatic = method.IsStatic;

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

                var clParameters = new List<CLParameter>(parameterNames.Count);
                for (int j = 0; j < parameterNames.Count; j++)
                {
                    clParameters.Add(new CLParameter
                    {
                        Name = parameterNames[j],
                        Type = parameterTypes[j],
                        DefaultValue = parameterValues[j],
                        IsOptional = parameters[j].IsOptional
                    });
                }

                var info = XmlInfo.FromMethodXml(xmlDocument, type, method, clMethodAttribute);
                if (info.Parameters.Count > 0)
                {
                    foreach (var parameter in clParameters)
                    {
                        if (info.Parameters.ContainsKey(parameter.Name))
                        {
                            parameter.Info ??= new XmlInfo();
                            parameter.Info.Summary = info.Parameters[parameter.Name];
                        }
                    }
                }

                var cLMethod = new CLMethod
                {
                    Name = method.Name,
                    Info = info,
                    ReturnType = new TypeReference(ResolveTypeName(method.ReturnType)),
                    Parameters = clParameters.ToArray(),
                    ObsoleteMessage = CustomLogicReflectionUtils.GetObsoleteMessage(method),
                };

                if (isStatic)
                    staticMethods.Add(cLMethod);
                else
                    instanceMethods.Add(cLMethod);
            }

            clType.StaticMethods = staticMethods.ToArray();
            clType.InstanceMethods = instanceMethods.ToArray();
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