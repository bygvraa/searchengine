using System;
using System.Collections.Generic;
using Shared.BE;

namespace ConsoleSearch
{
    public class SearchLogic
    {
        readonly Database database;

        // a cache for all words in the documents
        readonly Dictionary<string, int> words;

        public SearchLogic(Database _database)
        {
            database = _database;
            words = database.GetAllWords();
        }

        /* Perform search of documents containing words from query. The result will
         * contain details about up to maxAmount of documents.
         */
        public SearchResult Search(String[] query, int maxAmount)
        {
            List<string> ignored;

            DateTime start = DateTime.Now;

            // Convert words to wordids
            var wordIds = GetWordIds(query, out ignored);

            // perform the search - get all docIds
            var docIds = database.GetDocuments(wordIds);

            // get ids for the first maxAmount             
            var top = new List<int>();
            foreach (var p in docIds.GetRange(0, Math.Min(maxAmount, docIds.Count)))
                top.Add(p.Key);

            // compose the result.
            // all the documentHit
            var docresult = new List<DocumentHit>();
            int idx = 0;

            foreach (var doc in database.GetDocDetails(top))
            {
                docresult.Add(new DocumentHit(doc, docIds[idx++].Value));
            }
            return new SearchResult(query, docIds.Count, docresult, ignored, DateTime.Now - start);
        }

        private List<int> GetWordIds(String[] query, out List<string> outIgnored)
        {
            var result = new List<int>();
            var ignored = new List<string>();

            foreach (var aWord in query)
            {
                if (words.ContainsKey(aWord))
                    result.Add(words[aWord]);
                else
                    ignored.Add(aWord);
            }
            outIgnored = ignored;
            return result;
        }

    }
}
