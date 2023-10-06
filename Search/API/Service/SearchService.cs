using Shared;

namespace Search.Service
{
    public class SearchService
    {
        private readonly Database _db;
        private readonly Dictionary<string, int> _words; // a cache for all words in the documents

        public SearchService()
        {
            _db = new Database();
            _words = _db.GetAllWords();
        }

        /* Perform search of documents containing words from query. The result will
         * contain details about up to maxAmount of documents.
         */
        public SearchResult Search(string[] query, SearchSettings settings)
        {
            DateTime start = DateTime.Now;

            // Convert words to wordids
            var wordIds = GetWordIds(query, settings.CaseSensitive, out List<string> ignored);

            // perform the search - get all docIds
            var docIds = _db.GetDocuments(wordIds);

            // get ids for the first maxAmount             
            var docs = new List<int>();
            foreach (var doc in docIds.GetRange(0, Math.Min(settings.ResultLimit, docIds.Count)))
            {
                docs.Add(doc.Key);
            }

            // compose the result, all the documentHit
            var docresult = new List<DocumentHit>();
            int idx = 0;

            foreach (var doc in _db.GetDocDetails(docs))
            {
                docresult.Add(new DocumentHit(doc, docIds[idx++].Value));
            }
            return new SearchResult(query, docIds.Count, docresult, ignored, DateTime.Now - start);
        }

        private List<int> GetWordIds(string[] query, bool caseSensitive, out List<string> ignored)
        {
            var result = new List<int>();
            ignored = new List<string>();

            foreach (var aWord in query)
            {
                if (caseSensitive)
                {
                    if (_words.ContainsKey(aWord))
                        result.Add(_words[aWord]);
                    else
                        ignored.Add(aWord);
                }
                else
                {
                    // Case-insensitive search
                    var matchingWord = _words.Keys.FirstOrDefault(word => string.Equals(word, aWord, StringComparison.OrdinalIgnoreCase));
                    if (matchingWord != null)
                        result.Add(_words[matchingWord]);
                    else
                        ignored.Add(aWord);
                }
            }
            return result;
        }
    }
}
