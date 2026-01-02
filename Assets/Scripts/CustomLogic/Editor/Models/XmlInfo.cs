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
        public string Returns { get; set; }

        public static XmlInfo FromTypeXml(XmlDocument xmlDocument, Type type)
        {
            return new XmlInfo
            {
                Summary = XmlDocumentUtils.GetTypeNodeText(xmlDocument, type, "summary"),
                Remarks = XmlDocumentUtils.GetTypeNodeText(xmlDocument, type, "remarks"),
                Code = XmlDocumentUtils.GetTypeNodeText(xmlDocument, type, "code")
            };
        }

        public static XmlInfo FromConstructorXml(XmlDocument xmlDocument, Type type, ConstructorInfo ctorInfo)
        {
            return new XmlInfo
            {
                Summary = XmlDocumentUtils.GetConstructorNodeText(xmlDocument, type, ctorInfo, "summary"),
                Remarks = XmlDocumentUtils.GetConstructorNodeText(xmlDocument, type, ctorInfo, "remarks"),
                Code = XmlDocumentUtils.GetConstructorNodeText(xmlDocument, type, ctorInfo, "code")
            };
        }

        public static XmlInfo FromPropertyXml(XmlDocument xmlDocument, Type type, PropertyInfo propertyInfo)
        {
            return new XmlInfo
            {
                Summary = XmlDocumentUtils.GetPropertyNodeText(xmlDocument, type, propertyInfo, "summary"),
                Remarks = XmlDocumentUtils.GetPropertyNodeText(xmlDocument, type, propertyInfo, "remarks"),
                Code = XmlDocumentUtils.GetPropertyNodeText(xmlDocument, type, propertyInfo, "code")
            };
        }

        public static XmlInfo FromFieldXml(XmlDocument xmlDocument, Type type, FieldInfo fieldInfo)
        {
            return new XmlInfo
            {
                Summary = XmlDocumentUtils.GetFieldNodeText(xmlDocument, type, fieldInfo, "summary"),
                Remarks = XmlDocumentUtils.GetFieldNodeText(xmlDocument, type, fieldInfo, "remarks"),
                Code = XmlDocumentUtils.GetFieldNodeText(xmlDocument, type, fieldInfo, "code")
            };
        }

        public static XmlInfo FromMethodXml(XmlDocument xmlDocument, Type type, MethodInfo methodInfo)
        {
            return new XmlInfo
            {
                Summary = XmlDocumentUtils.GetMethodNodeText(xmlDocument, type, methodInfo, "summary"),
                Remarks = XmlDocumentUtils.GetMethodNodeText(xmlDocument, type, methodInfo, "remarks"),
                Code = XmlDocumentUtils.GetMethodNodeText(xmlDocument, type, methodInfo, "code"),
                Returns = XmlDocumentUtils.GetMethodNodeText(xmlDocument, type, methodInfo, "returns")
            };
        }
    }
}
