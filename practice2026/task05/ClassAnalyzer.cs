using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Task05;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        MethodInfo[] methods = _type.GetMethods();
        return methods.Select(method => method.Name);
    }

    public IEnumerable<string> GetMethodParams(string methodname)
    {
        MethodInfo[] methods = _type.GetMethods();
        return methods.Where(method => method.Name == methodname)
            .SelectMany(method => method.GetParameters())
            .Select(p => $"{p.Name}, {p.ParameterType.Name}");
    }

    public IEnumerable<string> GetAllFields()
    {
        FieldInfo[] fields = _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        return fields.Select(field => field.Name);
    }

    public IEnumerable<string> GetProperties()
    {
        PropertyInfo[] properties = _type.GetProperties();
        return properties.Select(property => property.Name);
    }

    public bool HasAttribute<T>() where T : Attribute
    {
        return _type.IsDefined(typeof(T), inherit: false);
    }
    
}
