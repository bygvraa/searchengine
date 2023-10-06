using System;
using Shared;

namespace ConsoleSearch
{
    public class App
    {
        public void Run()
        {
            var searchService = SearchFactory.GetSearchService();

            var searchSettings = new SearchSettings();
            var commandService = new CommandService(searchSettings);

            Console.WriteLine("Console Search");

            while (true)
            {
                Console.WriteLine("Enter search terms or commands - 'q' for quit");
                string input = Console.ReadLine();
                if (input.Equals("q")) break;

                if (input.StartsWith("/"))
                {
                    input = input[1..];
                    var result = commandService.ProcessCommand(input);
                    Console.WriteLine(result);
                }
                else
                {
                    var query = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    var result = searchService.Search(query, searchSettings);

                    if (result.Ignored.Count > 0)
                    {
                        Console.WriteLine("Ignored: ");
                        Console.WriteLine(string.Join(", ", result.Ignored));
                    }

                    int idx = 0;
                    foreach (var doc in result.DocumentHits)
                    {
                        Console.WriteLine($"{idx + 1}: {doc.Document.mUrl} -- contains {doc.NoOfHits} search term(s)");
                        if (searchSettings.ShowTimestamps)
                            Console.WriteLine($"Index time: {doc.Document.mIdxTime}. Creation time: {doc.Document.mCreationTime}");

                        Console.WriteLine();
                        idx++;
                    }
                    Console.WriteLine($"Documents: {result.Hits}. Time: {result.TimeUsed.TotalMilliseconds}");
                }
            }
        }
    }
}
