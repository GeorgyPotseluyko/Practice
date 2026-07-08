using System.Reflection;

namespace Task09.Tests;

[Version(1, 0)]
[DisplayName("Класс для теста")]
public class TestClass
{
    public TestClass(int number){}

    [DisplayName("Метод для теста")]
    public void TestMethod(string text, int count){}
}
