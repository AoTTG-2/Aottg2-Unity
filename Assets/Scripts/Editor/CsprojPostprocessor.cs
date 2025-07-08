using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
#if UNITY_EDITOR
    public class CsprojPostprocessor : AssetPostprocessor
    {
        /// <summary>
        /// This method is called when a .csproj file is generated.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string OnGeneratedCSProject(string path, string content)
        {
            if (!path.EndsWith("Scripts.csproj"))
            {
                return content;
            }
            return Modify(content);
        }

        private static string Modify(string content)
        {
            // find the last occurrence of the </Project> tag
            var index = content.LastIndexOf("</Project>", StringComparison.Ordinal);
            if (index == -1)
            {
                Debug.LogError("Failed to find the </Project> tag in the csproj file");
                return content;
            }

            // insert some text before the </Project> tag
            var generated = new StringBuilder();
            generated.AppendLine($"<!-- Generated via {nameof(CsprojPostprocessor)} -->");
            generated.AppendLine("\t<PropertyGroup>");
            generated.AppendLine("\t\t<GenerateDocumentationFile>true</GenerateDocumentationFile>");
            generated.AppendLine("\t\t<DocumentationFile>Temp\\Bin\\Debug\\Scripts.xml</DocumentationFile>");
            generated.AppendLine("\t</PropertyGroup>");
            generated.AppendLine($"<!-- Generated via {nameof(CsprojPostprocessor)} -->");

            var sb = new StringBuilder(content);
            sb.Insert(index, generated.ToString());
            return sb.ToString();

        }
    }
#endif
}