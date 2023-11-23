namespace Shared
{
    public class Search
    {
        public string Query { get; set; }
        public SearchSettings Settings { get; set; }
        public SearchResult Result { get; set; }

        public Search() { }
    }
}