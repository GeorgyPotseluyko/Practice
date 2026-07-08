using System;
using System.IO;

namespace Task09;

[Version(0, 0)]
[DisplayName("Размер папки")]
public class DirectorySizeCommand : ICommand
{
    [DisplayName("Путь")]
    public string Path;

    [DisplayName("Размер папки")]
    public DirectorySizeCommand(string path)
    {
        Path = path;
    }

    [DisplayName("Размер папки")]
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
