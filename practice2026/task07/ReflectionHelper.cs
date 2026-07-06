using System.Reflection;
using System.Collections.Generic;

namespace Task07;

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
