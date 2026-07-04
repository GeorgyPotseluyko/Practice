using System;
using System.IO;

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

[Version(0, 1)]
[DisplayName("Поиск файлов")]
public class FindFilesCommand : ICommand
{
    [DisplayName("Путь")]
    public string Path;

    [DisplayName("Маска")]
    public string Mask;

    [DisplayName("Поиск Файлов")]
    public FindFilesCommand(string path, string mask)
    {
        Path = path;
        Mask = mask;
    }

    [DisplayName("Поиск файлов")]
    public string[] FindFiles()
    {
        if (!Directory.Exists(Path))
        {
            return new string[0];
        }

        string[] files = Directory.GetFiles(Path, Mask, SearchOption.AllDirectories);
        return files;
    }

    public void Execute()
    {
        string[] files = FindFiles();
        foreach (string file in files)
        {
            Console.Write($"{file} ");
        }
    }
}
