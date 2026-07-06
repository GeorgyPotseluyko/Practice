using System.IO;

namespace Task08;

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
