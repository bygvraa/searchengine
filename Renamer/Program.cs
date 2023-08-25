using CommonStuff;

namespace Renamer;

class Program
{
    static void Main(string[] args)
    {
        Renamer renamer = new Renamer();
        renamer.Crawl(new DirectoryInfo(Config.FOLDER));
        Console.WriteLine("Done with");
        Console.WriteLine("Folders: " + renamer.CountFolders);
        Console.WriteLine("Files:   " + renamer.CountFiles);
    }
}
