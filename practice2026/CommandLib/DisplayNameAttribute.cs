using System.Reflection;

namespace Task09;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Constructor)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; set; }
    public DisplayNameAttribute(string displayname)
    {
        DisplayName = displayname;
    }
}
