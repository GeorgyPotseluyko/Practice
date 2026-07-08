using System;
using System.IO;
using System.Reflection;

namespace Task09;

public class Program
{
    public static void Main(string[] args)
    {
        Run(args);
    }

    public static bool Run(string[] args)
    {
        string dllPath;

        if (args.Length > 0)
        {
            dllPath = args[0];
        }
        else
        {
            dllPath = Path.Combine(AppContext.BaseDirectory, "FileSystemCommands.dll");
        }

        try
        {
            PrintLibraryInfo(dllPath);
            return true;
        }
        catch (ReflectionTypeLoadException ex)
        {
            Console.WriteLine($"Ошибка загрузки сборки: {ex.Message}");

            foreach (Exception? loaderException in ex.LoaderExceptions)
            {
                if (loaderException is not null)
                {
                    Console.WriteLine($"Ошибка загрузчика: {loaderException.Message}");
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки сборки: {ex.Message}");
            return false;
        }
    }

    public static void PrintLibraryInfo(string dllPath)
    {
        Assembly assembly = Assembly.LoadFrom(dllPath);

        foreach (Type type in assembly.GetTypes())
        {
            if (type.IsClass)
            {
                Console.WriteLine($"Класс: {type.Name}");

                Console.WriteLine("Атрибуты класса:");
                foreach (object attribute in type.GetCustomAttributes(false))
                {
                    Console.WriteLine($"Атрибут: {attribute.GetType().Name}");
                }
                Console.WriteLine();

                Console.WriteLine("Конструкторы:");
                ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);

                foreach (ConstructorInfo constructor in constructors)
                {
                    Console.WriteLine($"Конструктор: {constructor.Name}");

                    Console.WriteLine("Параметры конструктора:");
                    foreach (ParameterInfo parameter in constructor.GetParameters())
                    {
                        Console.WriteLine($"Параметр: {parameter.Name} - {parameter.ParameterType}");
                    }

                    Console.WriteLine("Атрибуты конструктора:");
                    foreach (object attribute in constructor.GetCustomAttributes(false))
                    {
                        Console.WriteLine($"Атрибут: {attribute.GetType().Name}");
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Методы:");
                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);

                foreach (MethodInfo method in methods)
                {
                    if (!method.IsSpecialName)
                    {
                        Console.WriteLine($"Метод: {method.Name}");
                        Console.WriteLine($"Возвращаемый тип: {method.ReturnType}");

                        Console.WriteLine("Параметры метода:");
                        foreach (ParameterInfo parameter in method.GetParameters())
                        {
                            Console.WriteLine($"Параметр: {parameter.Name} - {parameter.ParameterType}");
                        }

                        Console.WriteLine("Атрибуты метода:");
                        foreach (object attribute in method.GetCustomAttributes(false))
                        {
                            Console.WriteLine($"Атрибут: {attribute.GetType().Name}");
                        }
                    }
                }
            }
        }
    }
}
