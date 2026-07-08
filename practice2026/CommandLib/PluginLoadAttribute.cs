using System.Reflection;

namespace Task10;

[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute : Attribute
{
    public string Name { get; }

    public string[] Depends { get; set; } 

    public PluginLoadAttribute(string name)
    {
        Name = name;
        Depends = new string[0];
    }
}
