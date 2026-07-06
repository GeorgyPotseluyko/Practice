using System.IO;

namespace Task08;

public class DirectorySizeCommand : ICommand
{
    public string Path;

    public DirectorySizeCommand(string path)
    {
        Path = path;
    }

    public long DirectorySize()
    {
        if (!Directory.Exists(Path))
        {
            return 0;
        }

        long size = 0;
        string[] files = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            FileInfo info = new FileInfo(file);
            size += info.Length;
        }
        
        return size;
    }

    public void Execute()
    {
        long size = DirectorySize();
        Console.WriteLine($"Размер: {size} байт");
    }
}
