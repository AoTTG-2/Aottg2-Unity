using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace CustomLogic.Editor.Models
{
    class XmlInfo
    {
        public string Summary { get; set; }
        public string Remarks { get; set; }
        public string Code { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public string Returns { get; set; }

        public static XmlInfo FromTypeXml(XmlDocument xmlDocument, Type type, CLTypeAttribute attr)
        {
            return new XmlInfo
            {
                Summary = XmlDocumentUtils.GetTypeNodeText(xmlDocument, type, "summary", attr.Description),
                Remarks = XmlDocumentUtils.GetTypeNodeText(xmlDocument, type, "remarks"),
                Code = XmlDocumentUtils.GetTypeNodeText(xmlDocument, type, "code")
            };
        }

        public static XmlInfo FromConstructorXml(XmlDocument xmlDocument, Type type, ConstructorInfo ctorInfo)
        {
            return new XmlInfo
            {
                Summary = XmlDocumentUtils.GetConstructorNodeText(xmlDocument, type, ctorInfo, "summary", string.Empty),
                Remarks = XmlDocumentUtils.GetConstructorNodeText(xmlDocument, type, ctorInfo, "remarks", string.Empty),
                Code = XmlDocumentUtils.GetConstructorNodeText(xmlDocument, type, ctorInfo, "code", string.Empty)
            };
        }

        public static XmlInfo FromPropertyXml(XmlDocument xmlDocument, Type type, PropertyInfo propertyInfo, CLPropertyAttribute attr)
        {
            return new XmlInfo
            {
                Summary = XmlDocumentUtils.GetPropertyNodeText(xmlDocument, type, propertyInfo, "summary", attr.Description),
                Remarks = XmlDocumentUtils.GetPropertyNodeText(xmlDocument, type, propertyInfo, "remarks", string.Empty),
                Code = XmlDocumentUtils.GetPropertyNodeText(xmlDocument, type, propertyInfo, "code", string.Empty)
            };
        }

        public static XmlInfo FromFieldXml(XmlDocument xmlDocument, Type type, FieldInfo fieldInfo, CLPropertyAttribute attr)
        {
            return new XmlInfo
            {
                Summary = XmlDocumentUtils.GetFieldNodeText(xmlDocument, type, fieldInfo, "summary", attr.Description),
                Remarks = XmlDocumentUtils.GetFieldNodeText(xmlDocument, type, fieldInfo, "remarks", string.Empty),
                Code = XmlDocumentUtils.GetFieldNodeText(xmlDocument, type, fieldInfo, "code", string.Empty)
            };
        }

        public static XmlInfo FromMethodXml(XmlDocument xmlDocument, Type type, MethodInfo methodInfo, CLMethodAttribute attr)
        {
            return new XmlInfo
            {
                Summary = XmlDocumentUtils.GetMethodNodeText(xmlDocument, type, methodInfo, "summary", attr.Description),
                Remarks = XmlDocumentUtils.GetMethodNodeText(xmlDocument, type, methodInfo, "remarks", string.Empty),
                Code = XmlDocumentUtils.GetMethodNodeText(xmlDocument, type, methodInfo, "code", string.Empty),
                Parameters = XmlDocumentUtils.GetMethodParamTexts(xmlDocument, type, methodInfo).ToDictionary(x => x.Key, x => x.Value),
                Returns = XmlDocumentUtils.GetMethodNodeText(xmlDocument, type, methodInfo, "returns", string.Empty)
            };
        }
    }
}
