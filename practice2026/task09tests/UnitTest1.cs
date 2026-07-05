using System;
using System.IO;
using Xunit;

public class Tests
{
    [Fact]
    public void PluginRunner_ShouldExecutePluginsInCorrectOrder()
    {
        string pluginFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Plugins"));

        TextWriter console = Console.Out;

        using(StringWriter sw = new StringWriter())
        {
            try 
            {
                Console.SetOut(sw);
                PluginLoad runner = new PluginLoad();
                runner.Run(pluginFolder);

                string output = sw.ToString();

                Assert.Contains("Вычислен размер папки.", output);
                Assert.Contains("Файл найден.", output);

                int firstStr = output.IndexOf("Вычислен размер папки.");
                int secondStr = output.IndexOf("Файл найден.");
            
                Assert.True(firstStr < secondStr);
            }
            finally
            {
                Console.SetOut(console);
            }
        }
    }
}
