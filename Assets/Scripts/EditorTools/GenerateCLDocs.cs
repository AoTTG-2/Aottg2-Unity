
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using CustomLogic;
using UnityEngine.WSA;


public class GenerateCLDocs : EditorWindow
{
    private Dictionary<string, string> TypeReference = new();
    [MenuItem("AoTTG2/Editor/GenerateCLDocs")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GenerateCLDocs));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Generate Docs"))
        {
            // Generate docs in the project folder/Docs
            GenerateDocs("Assets/Docs");
        }
    }

    private void GenerateDocs(string output)
    {
        // Get all classes under the CustomLogic namespace where the class has the CLType attribute
        var classes = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.Namespace == "CustomLogic" && x.GetCustomAttributes(typeof(CLTypeAttribute), false).Length > 0)
            .ToArray();

        TypeReference.Clear();

        foreach (var type in classes)
        {
            // Map the className to the folder it will be in
            var clType = (CLTypeAttribute)type.GetCustomAttributes(typeof(CLTypeAttribute), false)[0];
            var className = clType.Name;
            var isStatic = clType.Static;
            string subfolder = isStatic ? "Static" : "Object";
            string path = $"[{className}](../{subfolder}/{className}.md)";
            Debug.Log($"{type.Name}, {path}");
            TypeReference.Add(type.Name, path);
            TypeReference.Add(className, path);
        }

        // iterate
        foreach (var cl in classes)
        {
            var clType = (CLTypeAttribute)cl.GetCustomAttributes(typeof(CLTypeAttribute), false)[0];
            var className = clType.Name;
            var isStatic = clType.Static;
            string subfolder = isStatic ? "Static" : "Object";
            string folder = $"{output}/{subfolder}";

            // create folders if they dont exist
            System.IO.Directory.CreateDirectory(folder);

            string path = $"{folder}/{className}.md";
            GenerateClassDoc(path, cl);
        }
    }

    private string DelimitStyled(string val)
    {
        return val.Replace("_", "\\_");
    }

    private void GenerateClassDoc(string path, System.Type type)
    {
        // Get the CLType attribute
        var clType = (CLTypeAttribute)type.GetCustomAttributes(typeof(CLTypeAttribute), false)[0];
        var className = clType.Name;
        var isStatic = clType.Static;
        var isAbstract = clType.Abstract;
        var inheritBaseMembers = clType.InheritBaseMembers;

        string doc = string.Empty;
        doc += $"# {className}\n";

        if (inheritBaseMembers)
        {
            // Get parent class type and check if there is a CLType attribute
            var parentType = type.BaseType;
            if (parentType != null)
            {
                if (parentType == typeof(BuiltinClassInstance))
                {
                    doc += "Inherits from object\n";
                }
                else
                {
                    var parentClTypes = parentType.GetCustomAttributes(typeof(CLTypeAttribute), false);
                    if (parentClTypes.Length > 0)
                    {
                        var parentClType = (CLTypeAttribute)parentClTypes[0];
                        var parentClassName = parentClType.Name;
                        if (TypeReference.ContainsKey(parentClassName))
                        {
                            parentClassName = TypeReference[parentClassName];
                        }
                        doc += $"Inherits from {parentClassName}\n";
                    }
                    
                }
            }
        }

        if (!isStatic && !isAbstract)
        {
            // Add initializer header
            doc += "## Initialization\n";

            // Grab all constructors
            var constructors = type.GetConstructors()
                .Where(x => x.GetCustomAttributes(typeof(CLConstructorAttribute), false).Length > 0)
                .ToArray();

            // create code examples for each constructor
            List<string> lines = new List<string>();
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                var parameterNames = parameters.Select(x => x.Name).ToList();
                var parameterTypes = parameters.Select(x => x.ParameterType.Name).ToList();
                var parameterValues = parameters.Select(x => $"({x.ParameterType.Name})").ToList();
                var code = $"new {className}({string.Join(", ", parameterValues)})";
                lines.Add(code);
            }

            doc += CreateCodeExample(lines);

        }

        // Get all properties with the CLProperty attribute
        var properties = type.GetProperties()
            .Where(x => x.GetCustomAttributes(typeof(CLPropertyAttribute), false).Length > 0)
            .ToArray();

        if (properties.Length > 0)
        {
            // Field, Type, Readonly, Description
            doc += "## Fields\n";
            List<string> headers = new List<string> { "Field", "Type", "Readonly", "Description" };
            List<List<string>> rows = new List<List<string>>();
            foreach (var property in properties)
            {
                var clProperty = (CLPropertyAttribute)property.GetCustomAttributes(typeof(CLPropertyAttribute), false)[0];
                var field = DelimitStyled(property.Name);
                var varType = property.PropertyType.Name;
                if (TypeReference.ContainsKey(varType))
                {
                    varType = TypeReference[varType];
                }
                var readOnly = clProperty.ReadOnly;
                var description = clProperty.Description;
                rows.Add(new List<string> { field, varType, readOnly.ToString(), description });
            }

            doc += CreateTable(headers, rows);
        }

        // Get all methods with the CLMethod attribute
        var methods = type.GetMethods()
            .Where(x => x.GetCustomAttributes(typeof(CLMethodAttribute), false).Length > 0)
            .ToArray();

        if (methods.Length > 0)
        {
            doc += "## Methods\n";
            List<string> headers = new List<string> { "Function", "Returns", "Description" };
            List<List<string>> rows = new List<List<string>>();
            foreach (var method in methods)
            {
                var clMethod = (CLMethodAttribute)method.GetCustomAttributes(typeof(CLMethodAttribute), false)[0];
                var methodName = DelimitStyled(method.Name);

                // methodName should look like name(param1 : type1, param2 : type2)
                // optional params look like name(param1: type1, param2 : type2 = defaultValue)
                var parameters = method.GetParameters();

                string signature = $"{methodName}(";
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    var paramType = param.ParameterType.Name;
                    if (TypeReference.ContainsKey(paramType))
                    {
                        paramType = TypeReference[paramType];
                    }
                    if (param.HasDefaultValue)
                        signature += $"{param.Name} : {paramType} = {param.DefaultValue}";
                    else
                        signature += $"{param.Name} : {paramType}";
                    if (i < parameters.Length - 1)
                        signature += ", ";
                }
                signature += ")";
                methodName = signature;
                var returnType = method.ReturnType.Name;
                if (TypeReference.ContainsKey(returnType))
                {
                    returnType = TypeReference[returnType];
                }
                var description = clMethod.Description;
                rows.Add(new List<string> { methodName, returnType, description });
            }

            doc += CreateTable(headers, rows);

        }

        System.IO.File.WriteAllText(path, doc);
    }

    private string CreateCodeExample(List<string> lines)
    {
        string code = string.Empty;
        code += "```csharp\n";
        foreach (var line in lines)
        {
            code += line + "\n";
        }
        code += "```\n";
        return code;
    }

    /// <summary>
    /// Create a markdown table
    /// </summary>
    /// <param name="headers"></param>
    /// <param name="rows"></param>
    /// <returns></returns>
    private string CreateTable(List<string> headers, List<List<string>> rows)
    {
        string table = string.Empty;
        table += "|";
        foreach (var header in headers)
        {
            table += header + "|";
        }
        table += "\n";
        table += "|";
        for (int i = 0; i < headers.Count; i++)
        {
            table += "---|";
        }
        table += "\n";
        foreach (var row in rows)
        {
            table += "|";
            foreach (var cell in row)
            {
                table += cell + "|";
            }
            table += "\n";
        }
        return table;
    }

}
