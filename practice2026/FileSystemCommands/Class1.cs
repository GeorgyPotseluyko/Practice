using System;
using System.IO;

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

public class FindFilesCommand : ICommand
{
    public string Path;
    public string Mask;

    public FindFilesCommand(string path, string mask)
    {
        Path = path;
        Mask = mask;
    }

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
