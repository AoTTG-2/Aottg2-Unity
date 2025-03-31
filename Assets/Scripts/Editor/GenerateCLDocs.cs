
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using CustomLogic;
using NUnit.Framework.Constraints;
using static UnityEngine.Rendering.DebugUI.Table;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime;
using System.Xml;
using NUnit.Framework.Internal;
using UnityEngine.Rendering;
using Utility;
using System;

#if UNITY_EDITOR
public static class PropertyInfoExtensions
{
    public static bool IsStatic(this PropertyInfo source, bool nonPublic = false)
        => source.GetAccessors(nonPublic).Any(x => x.IsStatic);
}

public class GenerateCLDocs : EditorWindow
{
    private static string _xmlFolder = "Temp\\Bin\\Debug\\";
    private static string _xmlFile = "Scripts.xml";
    private Dictionary<string, XmlDocument> _loadedDocs = new();
    private List<string> _priorityXML = new()
    {
        "Scripts.xml",
        "UnityEngine.xml",
        "UnityEngine.PhysicsModule.xml",
        "UnityEditor.CoreModule.xml",
        "Photon3Unity3D.xml"
    };

    private Dictionary<string, string> TypeReference = new();
    private Dictionary<string, string> CSTypeReference = new()
    {
        { "Single", "float" },
        { "Int32", "int" },
        { "Double", "double" },
        { "Boolean", "bool" },
        { "String", "string" },
        { "Void", "null" },
        { "void", "null" },
        { nameof(UserMethod), "function" },
        { nameof(CustomLogicComponentInstance), "Component" }

    };


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
            GenerateDocs("Docs/");
        }
    }

    private void GenerateDocs(string output)
    {
        // Read the xml
        string xmlPath = $"{_xmlFolder}{_xmlFile}";
        var XMLdoc = LoadXML(xmlPath);

        // For all inheritdoc tags, attempt to resolve them using script.xml and any other xml files in the same folder
        var inheritDocs = XMLdoc.SelectNodes("//inheritdoc");
        foreach (System.Xml.XmlNode inheritDoc in inheritDocs)
        {
            try
            {
                var cref = inheritDoc.Attributes["cref"].Value;
                var resolved = ResolveInheritDoc(cref);
                if (resolved == null)
                {
                    Debug.LogError($"Could not resolve cref {cref}");
                    continue;
                }

                XmlNode importNode = inheritDoc.ParentNode.OwnerDocument.ImportNode(resolved, true);
                if (resolved != null)
                    inheritDoc.ParentNode.ReplaceChild(importNode, inheritDoc);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error resolving cref {e.Message}");
            }
        }

        // Get all classes under the CustomLogic namespace where the class has the CLType attribute
        var classes = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.Namespace == "CustomLogic" && x.GetCustomAttributes(typeof(CLTypeAttribute), false).Length > 0)
            .ToArray();

        // Clear the existing files in Object and Static folders
        if (System.IO.Directory.Exists($"{output}/objects"))
            System.IO.Directory.Delete($"{output}/objects", true);
        if (System.IO.Directory.Exists($"{output}/static"))
            System.IO.Directory.Delete($"{output}/static", true);
        if (System.IO.File.Exists($"{output}/callbacks"))
            System.IO.File.Delete($"{output}/callbacks");

        // Update Type reference dictionary with the class names and their paths.
        TypeReference.Clear();
        foreach (var type in classes)
        {
            // Map the className to the folder it will be in
            var clType = (CLTypeAttribute)type.GetCustomAttributes(typeof(CLTypeAttribute), false)[0];
            var className = clType.Name;
            bool isInStatic = clType.Static && clType.Abstract;
            string subfolder = isInStatic ? "static" : "objects";
            string path = $"[{className}](../{subfolder}/{className}.md)";
            Debug.Log($"{type.Name}, {path}");
            TypeReference.Add(type.Name, path);
            TypeReference.Add(className, path);
        }

        foreach (var cl in classes)
        {
            var clType = (CLTypeAttribute)cl.GetCustomAttributes(typeof(CLTypeAttribute), false).First();
            var className = clType.Name;
            bool isInStatic = clType.Static && clType.Abstract;
            string subfolder = isInStatic ? "static" : "objects";
            string folder = $"{output}/{subfolder}";

            // create folders if they dont exist
            System.IO.Directory.CreateDirectory(folder);
            string path = $"{folder}/{className}.md";
            GenerateClassDoc(path, cl, XMLdoc);
        }

        // Create README.md files in both Object and Static with text # Object / # Static
        System.IO.File.WriteAllText($"{output}/objects/README.md", "# Objects");
        System.IO.File.WriteAllText($"{output}/static/README.md", "# Static");
    }

    private void GenerateClassDoc(string path, System.Type type, System.Xml.XmlDocument XMLdoc)
    {
        // Get the CLType attribute
        var clType = (CLTypeAttribute)type.GetCustomAttributes(typeof(CLTypeAttribute), false)[0];
        var className = clType.Name;
        var isStatic = clType.Static;
        var isAbstract = clType.Abstract;
        var inheritBaseMembers = clType.InheritBaseMembers;
        var constructorCount = type.GetConstructors()
            .Where(x => x.GetCustomAttributes(typeof(CLConstructorAttribute), false).Length > 0).Count();

        string doc = string.Empty;
        doc += $"# {className}\n";

        if (inheritBaseMembers && type.BaseType != null)
        {
            doc += GenerateInheritance(type.BaseType);
        }

        if (!isAbstract && (!isStatic || constructorCount > 0))
        {
            doc += GenerateInitializers(type, className, XMLdoc);
        }
        else if (isAbstract)
        {
            doc += "## Initialization\n";
            doc += WrapColor("This class is abstract and cannot be instantiated.", "red") + "\n";
        }
        else if (isStatic && constructorCount == 0)
        {
            doc += "## Initialization\n";
            doc += WrapColor("This class is static and cannot be instantiated.", "red") + "\n";
        }

        doc += GenerateFields(type, XMLdoc);
        doc += GenerateMethods(type, XMLdoc, isStatic: false);
        doc += GenerateMethods(type, XMLdoc, isStatic: true);
        System.IO.File.WriteAllText(path, doc);
    }

    private string GenerateInheritance(System.Type parentType)
    {
        string doc = string.Empty;
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
        return doc;
    }

    private string GenerateInitializers(System.Type type, string className, System.Xml.XmlDocument XMLdoc)
    {
        // Add initializer header
        string doc = "## Initialization\n";

        // Grab all constructors
        var constructors = type.GetConstructors()
            .Where(x => x.GetCustomAttributes(typeof(CLConstructorAttribute), false).Length > 0)
            .ToArray();


        string typeSummary = DelimitStyled(ResolveClassNodeText(type, XMLdoc, "summary"));
        string codeSnippet = ResolveClassNodeText(type, XMLdoc, "code");
        string codeExample = ResolveClassNodeText(type, XMLdoc, "example");

        if (typeSummary != string.Empty)
        {
            doc += $"> {typeSummary}\n";
        }

        if (codeSnippet != string.Empty)
        {
            doc += $"> Constructors:\n";
            doc += CreateCodeExample(codeSnippet);
        }
        else
        {
            // create code examples for each constructor
            codeSnippet = string.Empty;
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                var parameterNames = parameters.Select(x => x.Name).ToList();
                var parameterTypes = parameters.Select(x => ResolveType(x.ParameterType.Name)).ToList();
                var parameterValues = parameters.Select(x => $"{x.ParameterType.Name}").ToList();
                var constructorSignature = $"example = {className}({string.Join(", ", parameterValues)})";

                codeSnippet += $"# {className}({string.Join(", ", parameterValues)})\n";
                codeSnippet += $"example = {className}({string.Join(", ", parameterValues)})\n";
                codeSnippet += "\n";
            }

            if (codeSnippet == string.Empty) doc += WrapColor("No constructor given, object can only be accessed via builtins.", "red") + "\n";
            else doc += CreateCodeExample(codeSnippet);
        }

        if (codeExample != string.Empty)
        {
            doc += $"> Example:\n";
            doc += CreateCodeExample(codeExample);
        }

        return doc;
    }

    private string GenerateFields(System.Type type, System.Xml.XmlDocument XMLdoc)
    {
        string doc = string.Empty;

        var properties = type.GetProperties()
            .Where(x => x.GetCustomAttributes(typeof(CLPropertyAttribute), false).Length > 0)
            .ToArray();

        var fields = type.GetFields()
            .Where(x => x.GetCustomAttributes(typeof(CLPropertyAttribute), false).Length > 0)
            .ToArray();

        List<string> headers = new List<string> { "Field", "Type", "Readonly", "Description" };
        List<List<string>> fieldRows = new List<List<string>>();
        List<List<string>> staticFieldRows = new List<List<string>>();

        foreach (var property in properties)
        {
            var clProperty = (CLPropertyAttribute)property.GetCustomAttributes(typeof(CLPropertyAttribute), false)[0];
            var isStatic = property.IsStatic();
            var field = DelimitStyled(property.Name);
            var varType = ResolveType(property.PropertyType.Name);
            var readOnly = clProperty.ReadOnly || !property.CanWrite;
            var description = ResolvePropertyDescription(type, property, XMLdoc, clProperty.Description);


            if (isStatic)
                staticFieldRows.Add(new List<string> { field, varType, readOnly.ToString(), description });
            else
                fieldRows.Add(new List<string> { field, varType, readOnly.ToString(), description });
        }

        foreach (var field in fields)
        {
            var clProperty = (CLPropertyAttribute)field.GetCustomAttributes(typeof(CLPropertyAttribute), false)[0];
            var isStatic = field.IsStatic;
            var fieldName = DelimitStyled(field.Name);
            var varType = ResolveType(field.FieldType.Name);
            var readOnly = clProperty.ReadOnly;
            var description = ResolveFieldDescription(type, field, XMLdoc, clProperty.Description);
            if (isStatic)
                staticFieldRows.Add(new List<string> { fieldName, varType, readOnly.ToString(), description });
            else
                fieldRows.Add(new List<string> { fieldName, varType, readOnly.ToString(), description });
        }

        if (fieldRows.Count > 0)
        {
            doc += "## Fields\n";
            doc += CreateTable(headers, fieldRows);
        }

        if (staticFieldRows.Count > 0)
        {
            doc += "## Static Fields\n";
            doc += CreateTable(headers, staticFieldRows);
        }

        return doc;
    }

    private string GenerateMethods(System.Type type, System.Xml.XmlDocument XMLdoc, bool isStatic=false)
    {
        string doc = string.Empty;
        var methods = type.GetMethods()
            .Where(x => x.GetCustomAttributes(typeof(CLMethodAttribute), false).Length > 0)
            .ToArray();

        string header = "## Methods";
        // Filter out static methods
        if (isStatic)
        {
            methods = methods.Where(x => x.IsStatic).ToArray();
            header = "## Static Methods";
        }
        else
        {
            methods = methods.Where(x => !x.IsStatic).ToArray();
        }

        if (methods.Length > 0)
        {
            doc += $"{header}\n";
            List<string> headers = new List<string> { "Function", "Returns", "Description" };
            List<float> sizing = new List<float> { 30, 20, 50 };
            List<List<string>> rows = new List<List<string>>();
            foreach (var method in methods)
            {
                var clMethod = (CLMethodAttribute)method.GetCustomAttributes(typeof(CLMethodAttribute), false)[0];
                var obselete = method.GetCustomAttributes(typeof(ObsoleteAttribute), false);
                var methodName = DelimitStyled(method.Name);
                var parameters = method.GetParameters();
                string signature = $"{WrapColor(methodName, "yellow")}(";
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    var paramType = ResolveType(param.ParameterType.Name);
                    string paramDefaultValue = param.DefaultValue == null? "null" : param.DefaultValue.ToString();

                    if (param.HasDefaultValue)
                        signature += $"{param.Name}: {WrapColor(paramType, "blue")} = {WrapColor(paramDefaultValue, "blue")}";
                    else
                        signature += $"{param.Name}: {WrapColor(paramType, "blue")}";
                    if (i < parameters.Length - 1)
                        signature += ", ";
                }
                signature += ")";
                var returnType = ResolveType(method.ReturnType.Name, isReturned: true);
                var description = DelimitStyled(ResolveMethodNodeText(type, method, XMLdoc, "summary", clMethod.Description));
                var codeExample = ResolveMethodNodeText(type, method, XMLdoc, "code");

                doc += $"#### function {signature} \u2192 {WrapColor(returnType, "blue")}\n";

                if (description == string.Empty) description = WrapColor("Missing description, please ping dev to fix this or if you need clarification :)", "red");

                if (obselete.Length > 0)
                {
                    // If obsolete, add a note
                    var obsoleteAttr = (ObsoleteAttribute)obselete[0];
                    string warningStr = WrapColor("This method is obselete", "red");
                    if (!string.IsNullOrEmpty(obsoleteAttr.Message))
                    {
                        doc += $"> {warningStr}: {obsoleteAttr.Message}\n\n";
                    }
                    else doc += $"> {warningStr}\n\n";
                }
                doc += $"> {description}\n\n";

                if (codeExample != string.Empty)
                {
                    doc += $"> Example:\n";
                    doc += CreateCodeExample(codeExample);
                }

                if (method == methods.Last()) doc += "\n---\n\n";
            }

            //doc += CreateHTMLTable(headers, rows, sizing);
        }
        return doc;
    }

    #region XMLParsingOrUtility
    private System.Xml.XmlNode TryResolve(string cref, string file)
    {
        var doc = LoadXML(file);
        var node = doc.SelectSingleNode($"//member[@name='{cref}']");
        return node;
    }

    private System.Xml.XmlNode ResolveInheritDoc(string cref)
    {
        // search common assemblies first
        foreach (var file in _priorityXML)
        {
            var node = TryResolve(cref, $"{_xmlFolder}{file}");
            if (node != null)
            {
                return node;
            }
        }

        List<string> searched = new();

        // Search all with xml that start with UnityEngine first.
        var unityengineFiles = System.IO.Directory.GetFiles(_xmlFolder, "UnityEngine*.xml");
        foreach (var file in unityengineFiles)
        {
            if (_priorityXML.Contains(System.IO.Path.GetFileName(file)))
                continue;
            searched.Add(file);
            var node = TryResolve(cref, file);
            if (node != null)
            {
                return node;
            }
        }

        // exhaustive search other xml files in the same directory
        //var files = System.IO.Directory.GetFiles(_xmlFolder, "*.xml");
        //foreach (var file in files)
        //{
        //    if (_priorityXML.Contains(System.IO.Path.GetFileName(file)) || searched.Contains(System.IO.Path.GetFileName(file)))
        //        continue;
        //    var node = TryResolve(cref, file);
        //    if (node != null)
        //    {
        //        return node;
        //    }
        //}
        return null;
    }

    private XmlDocument LoadXML(string path)
    {
        if (_loadedDocs.ContainsKey(path))
            return _loadedDocs[path];
        var xml = new System.Xml.XmlDocument();
        xml.Load(path);
        _loadedDocs.Add(path, xml);
        return xml;
    }

    private string DelimitStyled(string val)
    {
        val = val.Replace("_", "\\_");
        val = val.Trim();
        val = val.Replace("\n", " ");
        val = val.Replace("\t", "");
        val = val.Replace("|", "");
        return val;
    }

    private string DelimitForTable(string val)
    {
        // This is used for delimiting values in a table
        // Remove new lines, tabs and pipes
        val = val.Replace("\r", ""); // remove carriage returns
        val = val.Replace("\n", " ");
        val = val.Replace("\t", "");
        val = val.Replace("|", "\\|"); // modify pipes
        val = val.Trim();

        // Replace all multiple whitespace occurences with a single space
        // This ensures that we don't have multiple spaces in the output
        val = System.Text.RegularExpressions.Regex.Replace(val, @"\s+", " ");

        return val;
    }

    private string ResolveType(string type, bool isReturned = false)
    {
        if (CSTypeReference.ContainsKey(type))
        {
            type = CSTypeReference[type];
        }
        else if (TypeReference.ContainsKey(type))
        {
            type = TypeReference[type];
        }

        if (isReturned && (type.ToLower() == "null" || type.ToLower() == "none") || type.ToLower() == "void")
            type = "null";

        return DelimitStyled(type);
    }

    private string ResolvePropertyDescription(System.Type type, PropertyInfo property, System.Xml.XmlDocument XMLdoc, string description)
    {
        var propertyName = property.Name;
        string path = $"//member[@name=\"P:{type.FullName}.{propertyName}\"]";
        var propertyNode = XMLdoc.SelectSingleNode(path);

        if (propertyNode != null)
        {
            description = propertyNode.InnerText;
        }

        return DelimitStyled(description);
    }

    private string ResolveFieldDescription(System.Type type, FieldInfo field, System.Xml.XmlDocument XMLdoc, string description)
    {
        var fieldName = field.Name;
        string path = $"//member[@name=\"P:{type.FullName}.{fieldName}\"]";
        var fieldNode = XMLdoc.SelectSingleNode(path);
        if (fieldNode != null)
        {
            description = fieldNode.InnerText;
        }
        return DelimitStyled(description);
    }

    private string GetFullyQualifiedMethod(MethodBase method)
    {
        var methodName = method.Name;

        // Create params signature with fully qualified name
        string qualifiedParameters = string.Empty;
        if (method.GetParameters().Length > 0)
            qualifiedParameters = "(" + string.Join(",", method.GetParameters().Select(x => x.ParameterType.FullName)) + ")";
        var methodNameWithParams = $"{methodName}{qualifiedParameters}";
        return methodNameWithParams;
    }

    /// <summary>
    /// TODO: This will work for now but will need to be replaced if we ever add function overloading.
    /// </summary>
    private string ResolveMethodNodeText(System.Type type, MethodBase method, System.Xml.XmlDocument XMLdoc, string nodeType = "summary", string defaultText = "")
    {
        var fqn = GetFullyQualifiedMethod(method);
        var methodNameWithParams = $"M:{type.FullName}.{fqn}";
        string path = $"//member[@name=\"{methodNameWithParams}\"]";
        var methodNode = XMLdoc.SelectSingleNode(path);

        var node = ResolveMethodNode(type, method, XMLdoc, nodeType);

        if (node != null)
        {
            return node.InnerText;
        }

        // Check if the method is implementing an interface and find if there is a description on that
        var interfaces = type.GetInterfaces();
        foreach (var inter in interfaces)
        {
            var interMethod = inter.GetMethod(method.Name);
            if (interMethod != null)
            {
                node = ResolveMethodNode(type, interMethod, XMLdoc, nodeType);
                if (node != null)
                {
                    return node.InnerText;
                }
            }
        }
        return defaultText;
    }

    private string ResolveClassNodeText(System.Type type, System.Xml.XmlDocument XMLdoc, string nodeType = "summary", string defaultText = "")
    {
        var typeInfo = $"T:{type.FullName}";
        string path = $"//member[@name=\"{typeInfo}\"]";
        var node = XMLdoc.SelectSingleNode(path);

        if (node != null)
        {
            node = node.SelectSingleNode($".//{nodeType}");
            if (node != null)
            {
                return node.InnerText;
            }
        }

        return defaultText;
    }

    private XmlNode ResolveMethodNode(System.Type type, MethodBase method, System.Xml.XmlDocument XMLdoc, string nodeType = "summary")
    {
        var methodName = method.Name;
        // Create params signature with fully qualified name
        string qualifiedParameters = string.Empty;
        if (method.GetParameters().Length > 0)
            qualifiedParameters = "(" + string.Join(",", method.GetParameters().Select(x => x.ParameterType.FullName)) + ")";
        var methodNameWithParams = $"M:{type.FullName}.{methodName}{qualifiedParameters}";
        string path = $"//member[@name=\"{methodNameWithParams}\"]";
        var methodNode = XMLdoc.SelectSingleNode(path);
        if (methodNode != null)
        {
            // search for any child node called summary
            return methodNode.SelectSingleNode($".//{nodeType}");
        }
        return null;
    }

    private string WrapColor(string text, string color)
    {
        return $"<span style=\"color:{color};\">{text}</span>";
    }

    #endregion

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

    private string CreateCodeExample(string lines)
    {
        // Split the input into individual lines
        var lineSplit = lines.Split("\n");

        // Calculate the minimum shared indentation (spaces + tabs), ignoring empty lines
        int minIndent = int.MaxValue;
        foreach (var line in lineSplit)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                int spaceCount = line.Length - line.TrimStart(' ').Length;
                int tabCount = line.Length - line.TrimStart('\t').Length;
                int totalIndent = spaceCount + tabCount; // Combine spaces and tabs for total indentation
                minIndent = Math.Min(minIndent, totalIndent);
            }
        }

        // Remove the minimum shared indentation from all lines
        var adjustedLines = lineSplit.Select(line =>
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                int actualRemove = Math.Min(minIndent, line.Length);
                return line.Substring(actualRemove);
            }
            return line; // Preserve empty lines as they are
        });

        // Combine adjusted lines into a formatted code block
        string code = string.Empty;
        code += "```csharp\n";
        code += string.Join("\n", adjustedLines).TrimEnd(); // Trim trailing whitespace
        code += "\n```\n";
        return code;
    }

    private string CreateHTMLTable(List<string> headers, List<List<string>> rows, List<float> columnWidths)
    {
        string table = string.Empty;
        table += "<table>\n";
        table += "<colgroup>";
        foreach (var width in columnWidths)
        {
            table += $"<col style=\"width: {width}%\"/>\n";
        }
        table += "</colgroup>\n";

        // header
        table += "<thead>\n";
        table += "<tr>\n";
        foreach (var header in headers)
        {
            table += $"<th>{DelimitForTable(header)}</th>\n";
        }
        table += "</tr>\n";
        table += "</thead>\n";

        // body
        table += "<tbody>\n";
        foreach (var row in rows)
        {
            table += "<tr>\n";
            foreach (var cell in row)
            {
                table += $"<td>{DelimitForTable(cell)}</td>\n";
            }
            table += "</tr>\n";
        }
        table += "</tbody>\n";
        table += "</table>\n";
        return table;
    }

    private string CreateTable(List<string> headers, List<List<string>> rows)
    {
             
        string table = string.Empty;
        table += "|";
        foreach (var header in headers)
        {
            table += $"{DelimitForTable(header)}|";
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
                table += DelimitForTable(cell) + "|";
            }
            table += "\n";
        }
        return table;
    }

}
#endif