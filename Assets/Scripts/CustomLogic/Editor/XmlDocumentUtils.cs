using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace CustomLogic.Editor
{
    public static class XmlDocumentUtils
    {
        public const string XmlPath = "Temp/Bin/Debug";

        private static readonly List<string> _priorityXml = new()
        {
            "Scripts.xml",
            "UnityEngine.xml",
            "UnityEngine.PhysicsModule.xml",
            "UnityEditor.CoreModule.xml",
            "Photon3Unity3D.xml"
        };

        public static XmlDocument LoadXml(string path)
        {
            var xml = new XmlDocument();
            xml.Load(path);
            return xml;
        }

        private static XmlNode TryResolve(string cref, string file)
        {
            var doc = LoadXml(file);
            var node = doc.SelectSingleNode($"//member[@name='{cref}']");
            return node;
        }

        public static async Task ResolveAndReplaceInheritDocNodeAsync(XmlDocument xmlDocument, Action<string> logError = null)
        {
            logError ??= Debug.LogError;

            // For all inheritdoc tags, attempt to resolve them using script.xml and any other xml files in the same folder
            var inheritDocs = xmlDocument.SelectNodes("//inheritdoc");
            foreach (XmlNode inheritDoc in inheritDocs)
            {
                try
                {
                    var cref = inheritDoc.Attributes["cref"].Value;
                    var resolved = ResolveInheritDoc(cref);
                    if (resolved == null)
                    {
                        logError($"Could not resolve cref {cref}");
                        continue;
                    }

                    XmlNode importNode = inheritDoc.ParentNode.OwnerDocument.ImportNode(resolved, true);
                    if (resolved != null)
                        inheritDoc.ParentNode.ReplaceChild(importNode, inheritDoc);
                }
                catch (Exception e)
                {
                    logError($"Error resolving cref={inheritDoc.Attributes["cref"].Value} {e.Message}");
                }

                await Task.Yield();
            }

            await Task.CompletedTask;
        }

        private static XmlNode ResolveInheritDoc(string cref)
        {
            // search common assemblies first
            foreach (var file in _priorityXml)
            {
                var node = TryResolve(cref, $"{XmlPath}/{file}");
                if (node != null)
                {
                    return node;
                }
            }

            // Search all with xml that start with UnityEngine first.
            var unityengineFiles = System.IO.Directory.GetFiles(XmlPath, "UnityEngine*.xml");
            foreach (var file in unityengineFiles)
            {
                if (_priorityXml.Contains(System.IO.Path.GetFileName(file)))
                    continue;

                var node = TryResolve(cref, file);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the text of the xml node for the given type
        /// </summary>
        /// <returns>The text of the xml node (e.g. summary) for the given type or the default text</returns>
        public static string GetTypeNodeText(XmlDocument xmlDocument, Type type, string nodeType, string defaultText = "")
        {
            var typeInfo = $"T:{type.FullName}";
            var path = $"//member[@name=\"{typeInfo}\"]";
            XmlNode typeNode = xmlDocument.SelectSingleNode(path);

            if (TryGetInnerText(typeNode, nodeType, out string innerText))
                return innerText;

            return defaultText;
        }

        public static string GetConstructorNodeText(XmlDocument xmlDocument, Type type, ConstructorInfo ctorInfo, string nodeType, string defaultText = "")
        {
            var parameters = ctorInfo.GetParameters().Select(x => x.ParameterType.FullName).ToArray();
            var signature = parameters.Length > 0 ? $"({string.Join(",", parameters)})" : string.Empty;
            var path = $"{type.FullName}.#ctor{signature}";
            XmlNode ctorNode = xmlDocument.SelectSingleNode($"//member[@name=\"M:{path}\"]");

            if (TryGetInnerText(ctorNode, nodeType, out string innerText))
                return innerText;

            return defaultText;
        }

        public static string GetMethodNodeText(XmlDocument xmlDocument, Type type, MethodInfo methodInfo, string nodeType, string defaultText = "")
        {
            var parameters = methodInfo.GetParameters().Select(x => x.ParameterType.FullName).ToArray();
            var signature = parameters.Length > 0 ? $"({string.Join(",", parameters)})" : string.Empty;
            var path = $"{type.FullName}.{methodInfo.Name}{signature}";
            XmlNode method = xmlDocument.SelectSingleNode($"//member[@name=\"M:{path}\"]");

            if (TryGetInnerText(method, nodeType, out string innerText))
                return innerText;

            return defaultText;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetMethodParamTexts(XmlDocument xmlDocument, Type type, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters().Select(x => x.ParameterType.FullName).ToArray();
            var signature = parameters.Length > 0 ? $"({string.Join(",", parameters)})" : string.Empty;
            var path = $"{type.FullName}.{methodInfo.Name}{signature}";
            XmlNode method = xmlDocument.SelectSingleNode($"//member[@name=\"M:{path}\"]");

            if (method != null)
            {
                foreach (XmlNode p in method.SelectNodes("param"))
                {
                    var name = p.Attributes["name"].Value;
                    yield return new KeyValuePair<string, string>(name, p.InnerText);
                }
            }

            yield break;
        }

        public static string GetPropertyNodeText(XmlDocument xmlDocument, Type type, PropertyInfo property, string nodeType, string defaultText = "")
        {
            var propertyName = property.Name;
            string path = $"//member[@name=\"P:{type.FullName}.{propertyName}\"]";
            var propertyNode = xmlDocument.SelectSingleNode(path);

            if (TryGetInnerText(propertyNode, nodeType, out string innerText))
                return innerText;

            return defaultText;
        }

        public static string GetFieldNodeText(XmlDocument xmlDocument, Type type, FieldInfo field, string nodeType, string defaultText = "")
        {
            var propertyName = field.Name;
            string path = $"//member[@name=\"P:{type.FullName}.{propertyName}\"]";
            var propertyNode = xmlDocument.SelectSingleNode(path);

            if (TryGetInnerText(propertyNode, nodeType, out string innerText))
                return innerText;

            return defaultText;
        }

        private static bool TryGetInnerText(XmlNode node, string nodeType, out string innerText)
        {
            if (node != null && node.TrySelectSingleNode($".//{nodeType}", out XmlNode xmlNode))
            {
                innerText = xmlNode.InnerText;
                return true;
            }
            innerText = string.Empty;
            return false;
        }

        private static bool TrySelectSingleNode(this XmlNode node, string xpath, out XmlNode xNode)
        {
            xNode = node.SelectSingleNode(xpath);
            return xNode != null;
        }
    }
}