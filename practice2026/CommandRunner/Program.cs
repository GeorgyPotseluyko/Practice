using System.IO;

namespace Task10;

public class Program
{
    public static void Main(string[] args)
    {
        string pluginFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

        PluginLoad runner = new PluginLoad();
        runner.Run(pluginFolder);
    }
}
