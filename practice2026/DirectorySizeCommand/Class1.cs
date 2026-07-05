[PluginLoad("Размер папки")]
public class DirectorySizeCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Вычислен размер папки.");
    }
}
