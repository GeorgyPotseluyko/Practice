using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        string pluginFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

        PluginLoad runner = new PluginLoad();
        runner.Run(pluginFolder);
    }
}

public class PluginLoad
{
    Dictionary<string, Type> pluginTypes = new Dictionary<string, Type>();
    Dictionary<string, string[]> dependencies = new Dictionary<string, string[]>();
    List<string> sortedPlugins = new List<string>();
    HashSet<string> visited = new HashSet<string>();

    public void Run(string pluginFolder)
    {
        if (!Directory.Exists(pluginFolder))
        {
            Console.WriteLine("Папка с плагинами не найдена.");
            return;
        }

        string[] dllFiles = Directory.GetFiles(pluginFolder, "*.dll");
        foreach (string file in dllFiles)
        {
            Assembly assembly = Assembly.LoadFrom(file);
            var commands = assembly.GetExportedTypes().Where(t => t.IsClass && typeof(ICommand).IsAssignableFrom(t));

            foreach (Type type in commands)
            {
                var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                if (attr != null)
                {
                    pluginTypes[attr.Name] = type;
                    dependencies[attr.Name] = attr.Depends;
                }
            }
        }

        try
        {
            foreach (string pluginName in pluginTypes.Keys)
            {
                ResolveDependencies(pluginName);
            }

            foreach (string name in sortedPlugins)
            {
                var command = (ICommand)Activator.CreateInstance(pluginTypes[name]);
                command.Execute();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка графа зависимостей: {ex.Message}");
        }
    }

    void ResolveDependencies(string pluginName)
    {
        if (visited.Contains(pluginName))
        {
            return;
        }

        if (dependencies.ContainsKey(pluginName) && dependencies[pluginName] != null)
        {
            foreach (string neighbor in dependencies[pluginName])
            {
                ResolveDependencies(neighbor);
            }
        }

        visited.Add(pluginName);
        sortedPlugins.Add(pluginName);
    }
}
