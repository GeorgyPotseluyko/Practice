using System.Reflection;

namespace Task07;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; set; }
    public DisplayNameAttribute(string displayname)
    {
        DisplayName = displayname;
    }
}
