using System;
using System.Reflection;
using System.IO;

class Program
{
    static void Main()
    {
        string dllPath = Path.Combine(AppContext.BaseDirectory, "FileSystemCommands.dll");
        
        try
        {
            Assembly assembly = Assembly.LoadFrom(dllPath);

            Console.WriteLine("Доступные команды: DirectorySize, FindFiles");
            Console.Write("Введите имя команды: ");
            string commandName = Console.ReadLine();

            Console.Write("Введите путь к папке: ");
            string path = Console.ReadLine();

            string? mask = null;
            if (commandName == "FindFiles")
            {
                Console.Write("Введите маску: ");
                mask = Console.ReadLine();
            }

            foreach (Type type in assembly.GetExportedTypes())
            {
                if (typeof(ICommand).IsAssignableFrom(type) && type.IsClass)
                {
                    if (type.Name != commandName + "Command")
                    {
                        continue;
                    }

                    object instance;

                    if (commandName == "FindFiles")
                    {
                        instance = Activator.CreateInstance(type, path, mask)!;
                    }
                    else
                    {
                        instance = Activator.CreateInstance(type, path)!;
                    }

                    var command = (ICommand)instance;
                    command.Execute();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
