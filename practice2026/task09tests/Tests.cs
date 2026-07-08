using System;
using System.IO;
using Xunit;

namespace Task09.Tests;

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
                bool result = Program.Run(new[] { dllPath });

                string output = sw.ToString();

                Assert.Contains("Класс: TestClass", output);
                Assert.Contains("Атрибут: VersionAttribute", output);
                Assert.Contains("Атрибут: DisplayNameAttribute", output);
                Assert.Contains("Конструктор: .ctor", output);
                Assert.Contains("Параметр: number - System.Int32", output);
                Assert.Contains("Метод: TestMethod", output);
                Assert.Contains("Параметр: text - System.String", output);
                Assert.Contains("Параметр: count - System.Int32", output);
                Assert.True(result);
            }
            finally
            {
                Console.SetOut(console);
            }
        }
    }

    [Fact]
    public void Run_ShouldPrintError_WhenAssemblyPathIsInvalid()
    {
        TextWriter console = Console.Out;

        using (StringWriter sw = new StringWriter())
        {
            try
            {
                Console.SetOut(sw);
                bool result = Program.Run(new[] { "fake_library.dll" });

                string output = sw.ToString();

                Assert.Contains("Ошибка загрузки сборки:", output);
                Assert.False(result);
            }
            finally
            {
                Console.SetOut(console);
            }
        }
    }
}
