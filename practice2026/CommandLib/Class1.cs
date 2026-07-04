using System;
using System.Reflection;

public interface ICommand
{
    void Execute();
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Constructor)]
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

