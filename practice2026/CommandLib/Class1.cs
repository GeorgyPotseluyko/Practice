using System;
using System.Reflection;

public interface ICommand
{
    void Execute();
}

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
