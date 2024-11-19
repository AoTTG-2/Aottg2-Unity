using System;
using System.Reflection;

public static class ReflectionExtensions
{
    public static readonly Type IntType = typeof(int);
    public static readonly Type FloatType = typeof(float);
    
    public static bool HasAttribute<T>(this Type member) where T : Attribute
    {
        return member.GetCustomAttribute<T>() != null;
    }
    
    public static bool HasAttribute<T>(this MemberInfo member) where T : Attribute
    {
        return member.GetCustomAttribute<T>() != null;
    }
}