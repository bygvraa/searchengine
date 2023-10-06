using ConsoleSearch;

namespace ConsoleSearch
{
    public class SearchFactory
    {
        public static ISearchService GetSearchService() // static for at undgå, at lave et objekt
        {
            return new SearchService();
        }
    }
}
