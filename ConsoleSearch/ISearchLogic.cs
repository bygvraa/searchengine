using Shared;

namespace ConsoleSearch
{
    public interface ISearchLogic
    {
        SearchResult Search(string[] query, SearchSettings settings);
    }
}