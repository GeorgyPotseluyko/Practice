using System;
using System.IO;
using Xunit;

[Version(1, 0)]
[DisplayName("Класс для теста")]
public class TestClass
{
    public TestClass(int number){}

    [DisplayName("Метод для теста")]
    public void TestMethod(string text, int count){}
}

public class Tests
{
    [Fact]
    public void Program_ShouldPrintLibraryInfo()
    {
        string dllPath = typeof(TestClass).Assembly.Location;
        TextWriter console = Console.Out;

        using (StringWriter sw = new StringWriter())
        {
            try
            {
                Console.SetOut(sw);
                Program.PrintLibraryInfo(dllPath);

                string output = sw.ToString();

                Assert.Contains("Класс: TestClass", output);
                Assert.Contains("Атрибут: VersionAttribute", output);
                Assert.Contains("Атрибут: DisplayNameAttribute", output);
                Assert.Contains("Конструктор: .ctor", output);
                Assert.Contains("Параметр: number - System.Int32", output);
                Assert.Contains("Метод: TestMethod", output);
                Assert.Contains("Параметр: text - System.String", output);
                Assert.Contains("Параметр: count - System.Int32", output);
            }
            finally
            {
                Console.SetOut(console);
            }
        }
    }
}
