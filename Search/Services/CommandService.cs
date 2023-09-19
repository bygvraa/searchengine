using System;
using Shared;

namespace Search.Services
{
    public class CommandService
    {
        readonly SearchSettings searchSettings;
        readonly Dictionary<string, Func<string, string>> commandHandlers;
        public CommandService(SearchSettings _searchSettings)
        {
            searchSettings = _searchSettings;

            commandHandlers = new Dictionary<string, Func<string, string>>
            {
                { "casesensitive", HandleCaseSensitive },
                { "timestamp", HandleTimestamp },
                { "results", HandleResults }
            };
        }

        public string ProcessCommand(string input)
        {
            string[] parts = input.Split('=');

            if (parts.Length != 2)
                return "Invalid command format. Use '/command=value'.";

            string command = parts[0].ToLower();
            string value = parts[1].ToLower();

            if (!commandHandlers.ContainsKey(command))
                return "Invalid command. Type '/casesensitive=[on|off]', '/timestamp=[on|off]', or '/results=<limit>'.";

            return commandHandlers[command](value);
        }

        private string HandleCaseSensitive(string value)
        {
            if (value != "on" && value != "off")
                return "Invalid command. Use '/casesensitive=[on|off]' to toggle case sensitivity.";

            searchSettings.CaseSensitive = value == "on";
            return $"Case sensitivity is '{value.ToUpper()}'.";
        }

        private string HandleTimestamp(string value)
        {
            if (value != "on" && value != "off")
                return "Invalid command. Use '/timestamp=[on|off]' to toggle timestamps.";

            searchSettings.ShowTimestamps = value == "on";
            return $"Timestamps are '{value.ToUpper()}'.";
        }

        private string HandleResults(string value)
        {
            if (!int.TryParse(value, out int limit) || limit < 0)
                return "Invalid value. Please enter a positive integer for '/results=<limit>'.";

            searchSettings.ResultLimit = limit;
            return $"Result limit set to {limit}.";
        }
    }
}