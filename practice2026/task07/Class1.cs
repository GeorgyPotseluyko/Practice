using System;
using System.Reflection;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; set; }
    public DisplayNameAttribute(string displayname)
    {
        DisplayName = displayname;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major { get; set; }
    public int Minor { get; set; }
    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }
}

[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass
{
    [DisplayName("Числовое свойство")]
    public int Number { get; set; }

    [DisplayName("Тестовый метод")]
    public void TestMethod(){}

}

public static class ReflectionHelper
{
    public static string PrintTypeInfo(Type type)
    {
        List<string> info = new List<string>();

        var classDisplayName = type.GetCustomAttribute<DisplayNameAttribute>();
        if (classDisplayName != null)
        {
            info.Add($"DisplayName: {classDisplayName.DisplayName}");
        }

        var classVersion = type.GetCustomAttribute<VersionAttribute>();
        if (classVersion != null)
        {
            info.Add($"Version: {classVersion.Major}.{classVersion.Minor}");
        }

        foreach (var property in type.GetProperties())
        {
            var propertyDisplayName = property.GetCustomAttribute<DisplayNameAttribute>();
            if (propertyDisplayName != null)
            {
                info.Add($"Свойство: {property.Name}, DisplayName: {propertyDisplayName.DisplayName}");
            }
        }

        foreach (var method in type.GetMethods())
        {
            var methodDisplayName = method.GetCustomAttribute<DisplayNameAttribute>();
            if (methodDisplayName != null)
            {
                info.Add($"Метод: {method.Name}, DisplayName: {methodDisplayName.DisplayName}");
            }
        }

        return string.Join("\n", info);
    }
}