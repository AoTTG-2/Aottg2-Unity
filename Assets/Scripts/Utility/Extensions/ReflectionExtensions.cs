using System;
using System.Reflection;

public static class ReflectionExtensions
{
    public static bool HasAttribute<T>(this Type member) where T : Attribute
    {
        return member.GetCustomAttribute<T>() != null;
    }

    public static bool TryGetAttribute<T>(this Type type, out T attribute) where T : Attribute
    {
        if (type.HasAttribute<T>())
        {
            attribute = type.GetCustomAttribute<T>();
            return true;
        }

        attribute = null;
        return false;
    }
}