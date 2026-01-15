using System;
using System.Reflection;

namespace CustomLogic.Editor
{
    public static class CustomLogicReflectionUtils
    {
        /// <summary>
        /// Checks if the type is a CustomLogic type
        /// </summary>
        /// <returns>True if the type is under the "CustomLogic" namespace and has the CLType attribute</returns>
        public static bool IsCustomLogicType(Type type)
        {
            return type.Namespace == "CustomLogic" && HasAttribute<CLTypeAttribute>(type);
        }

        /// <summary>
        /// Checks if the member is a CustomLogic property
        /// </summary>
        /// <returns>true if the member has the CLProperty attribute, false otherwise</returns>
        public static bool IsCustomLogicProperty(MemberInfo member)
        {
            return HasAttribute<CLPropertyAttribute>(member);
        }

        public static bool IsCustomLogicMethod(MethodInfo methodInfo)
        {
            return HasAttribute<CLMethodAttribute>(methodInfo);
        }

        public static bool IsObsolete(Type type) => HasAttribute<ObsoleteAttribute>(type);
        public static bool IsObsolete(MemberInfo member) => HasAttribute<ObsoleteAttribute>(member);

        public static string GetObsoleteMessage(Type type)
        {
            if (IsObsolete(type))
            {
                var message = GetAttribute<ObsoleteAttribute>(type).Message;
                return string.IsNullOrEmpty(message) ? "Obsolete" : message;
            }
            return string.Empty;
        }

        public static string GetObsoleteMessage(MemberInfo member)
        {
            if (IsObsolete(member))
            {
                var message = GetAttribute<ObsoleteAttribute>(member).Message;
                return string.IsNullOrEmpty(message) ? "Obsolete" : message;
            }
            return string.Empty;
        }

        public static bool HasAttribute<T>(Type type, bool inherit = false) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), inherit).Length > 0;
        }

        public static T GetAttribute<T>(Type type, bool inherit = false) where T : Attribute
        {
            return (T)type.GetCustomAttributes(typeof(T), inherit)[0];
        }

        public static bool HasAttribute<T>(MemberInfo member, bool inherit = false) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit).Length > 0;
        }

        public static T GetAttribute<T>(MemberInfo member, bool inherit = false) where T : Attribute
        {
            return (T)member.GetCustomAttributes(typeof(T), inherit)[0];
        }

        public static bool IsPropertyStatic(PropertyInfo propertyInfo)
        {
            return (propertyInfo.GetGetMethod(true)?.IsStatic ?? false) ||
                (propertyInfo.GetSetMethod(true)?.IsStatic ?? false);
        }

        public static string GetDefaultValueAsString(ParameterInfo parameterInfo)
        {
            if (parameterInfo.HasDefaultValue)
            {
                if (parameterInfo.DefaultValue != null)
                {
                    if (parameterInfo.DefaultValue is string s)
                        return $"\"{s}\"";

                    return parameterInfo.DefaultValue.ToString();
                }
                else
                    return "null";
            }
            return string.Empty;
        }

        public static bool IsVariadicParameter(ParameterInfo parameterInfo)
        {
            return parameterInfo.IsDefined(typeof(ParamArrayAttribute), false);
        }
    }
}
