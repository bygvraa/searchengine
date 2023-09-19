using System;
using Shared;

namespace ConsoleSearch
{
    public class CommandLogic
    {
        readonly SearchSettings searchSettings;

        public CommandLogic(SearchSettings _searchSettings) {
            searchSettings = _searchSettings;
        }

        public void HandleCommand(string input)
        {
            string[] parts = input.Split('=');
            if (parts.Length == 2)
            {
                string command = parts[0].ToLower();
                string value = parts[1].ToLower();

                if (command == "/casesensitive")
                {
                    if (value == "on")
                    {
                        searchSettings.CaseSensitive = true;
                        Console.WriteLine("Case sensitivity is 'ON'.");
                    }
                    else if (value == "off")
                    {
                        searchSettings.CaseSensitive = false;
                        Console.WriteLine("Case sensitivity is 'OFF'.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Use '/casesensitive=[on|off]' to toggle case sensitivity.");
                    }
                }
                else if (command == "/timestamp")
                {
                    if (value == "on")
                    {
                        searchSettings.ShowTimestamps = true;
                        Console.WriteLine("Timestamps are 'ON'.");
                    }
                    else if (value == "off")
                    {
                        searchSettings.ShowTimestamps = false;
                        Console.WriteLine("Timestamps are 'OFF'.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Use '/timestamp=[on|off]' to toggle timestamps.");
                    }
                }
                else if (command == "/results")
                {
                    if (int.TryParse(value, out int limit))
                    {
                        if (limit <= 0)
                        {
                            Console.WriteLine("Invalid result limit. Please enter a positive integer.");
                        }
                        else
                        {
                            searchSettings.ResultLimit = limit;
                            Console.WriteLine("Result limit set to " + searchSettings.ResultLimit + ".");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Use '/results=<limit>' to set the result limit.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command. Type '/casesensitive=[on|off]', '/timestamp=[on|off]', or '/results=<limit>'.");
                }
            }
            else
            {
                Console.WriteLine("Invalid command. Type '/casesensitive=[on|off]', '/timestamp=[on|off]', or '/results=<limit>'.");
            }
        }

    }
}