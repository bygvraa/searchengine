using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace Indexer
{
    public class App
    {
        public void Run()
        {
            var database = new Database();
            var crawler = new Crawler(database);

            var rootDir = new DirectoryInfo(Config.FOLDER);

            DateTime startTime = DateTime.Now;

            // Perform the indexing
            var fileExtensions = new List<string> { ".txt" };
            crawler.IndexFilesIn(rootDir, fileExtensions);

            TimeSpan usedTime = DateTime.Now - startTime;

            var indexedWords = database.GetAllWords();

            // Inform about indexing completion
            Console.WriteLine("\nIndexing completed in " + usedTime.TotalMilliseconds + " milliseconds.");
            Console.WriteLine("Indexed " + indexedWords.Count + " word(s).");
            Console.WriteLine("Indexed " + crawler.docCount + " document(s).");
            Console.WriteLine("Indexed " + crawler.dirCount + " folder(s).\n");

            while (true)
            {
                Console.Write("Enter the number of words you want to see: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int amount))
                {
                    // Display the specified number of words
                    for (int i = 0; i < Math.Min(amount, indexedWords.Count); i++)
                    {
                        var word = indexedWords.ElementAt(i);
                        Console.WriteLine("<" + word.Key + ", " + word.Value + ">");
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

        }

    }
}
