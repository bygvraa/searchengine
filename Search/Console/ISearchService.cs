using Shared;

namespace ConsoleSearch
{
    public interface ISearchService
    {
        SearchResult Search(string[] query, SearchSettings settings);
    }
}