using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shared;

namespace Indexer
{
    public class App
    {
        public void Run()
        {
            var rootDir = new DirectoryInfo(Config.DATA);
            var fileExtensions = new List<string> { ".txt" };
            var folders = rootDir.GetDirectories().ToDictionary(dir => dir.Name);

            while (true)
            {
                Console.Write("Enter the name of the folder you want to index, or 'q' to quit: ");
                string input = Console.ReadLine();

                if (input == "q") break;

                if (folders.TryGetValue(input, out var folder))
                {
                    var database = new Database(folder.Name);
                    var crawler = new Crawler(database);

                    DateTime start = DateTime.Now;

                    // Perform the indexing
                    crawler.IndexFilesIn(folder, fileExtensions);

                    TimeSpan used = DateTime.Now - start;

                    var words = database.GetAllWords();

                    // Inform about indexing completion
                    var output = new StringBuilder();
                    output.AppendLine($"\nIndexing completed in {used.TotalSeconds} seconds.");
                    output.AppendLine($"Indexed {words.Count} word(s).");
                    output.AppendLine($"Indexed {crawler.docCount} document(s).");
                    output.AppendLine($"Indexed {crawler.dirCount} folder(s).\n");
                    Console.WriteLine(output.ToString());
                }
                else
                {
                    var folderNames = string.Join(", ", folders.Keys);
                    Console.WriteLine($"Invalid input. Please enter a valid folder name: {folderNames}\n");
                }
            }
        }
    }
}
