using Xunit;
using System;
using System.IO;

namespace Task08.Tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        TextWriter console = Console.Out;
        using (StringWriter sw = new StringWriter())
        {
            try
            {
                Console.SetOut(sw);

                var command = new DirectorySizeCommand(testDir);
                command.Execute();

                string consoleOutput = sw.ToString();
                Assert.Contains("Размер: 10 байт", consoleOutput);
            }
            finally
            {
                Console.SetOut(console);
            }
        }

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        TextWriter console = Console.Out;
        using (StringWriter sw = new StringWriter())
        {
            try
            {
                Console.SetOut(sw);

                var command = new FindFilesCommand(testDir, "*.txt");
                command.Execute();

                string consoleOutput = sw.ToString();
                Assert.Contains("file1.txt", consoleOutput);
            }
            finally
            {
                Console.SetOut(console);
            }
        }

        Directory.Delete(testDir, true);
    }
}
