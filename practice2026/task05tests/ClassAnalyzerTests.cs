using Xunit;

namespace Task05.Tests;

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetMethodParams_ReturnsParamsNameAndType()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parametres = analyzer.GetMethodParams("Method");

        Assert.Empty(parametres);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetProperties_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();

        Assert.Contains("Property", properties);
    }

    [Fact]
    public void HasAttribute_CheckAttribute()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        var attribute = analyzer.HasAttribute<SerializableAttribute>();

        Assert.True(attribute);
    }
}
