using ConsoleSearch;

namespace ConsoleSearch
{
    public class SearchFactory
    {
        public static ISearchLogic GetSearchLogic() // static for at undgå, at lave et objekt
        {
            return new SearchLogic();
        }
    }
}
