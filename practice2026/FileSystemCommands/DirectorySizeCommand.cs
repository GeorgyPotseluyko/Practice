using System.Reflection;

namespace Task10;

[PluginLoad("Размер папки")]
public class DirectorySizeCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Вычислен размер папки.");
    }
}
