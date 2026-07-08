using System.Reflection;

namespace Task10;

[PluginLoad("Поиск файлов", Depends = new[] {"Размер папки"})]
public class FindFilesCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Файл найден.");
    }
}
