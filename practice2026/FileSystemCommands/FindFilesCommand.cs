using System;
using System.IO;

namespace Task09;

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
