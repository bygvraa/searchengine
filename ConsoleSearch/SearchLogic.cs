﻿using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace ConsoleSearch
{
    public class SearchLogic : ISearchLogic
    {
        readonly Database database;
        readonly Dictionary<string, int> words; // a cache for all words in the documents

        public SearchLogic()
        {
            database = new Database();
            words = database.GetAllWords();
        }

        /* Perform search of documents containing words from query. The result will
         * contain details about up to maxAmount of documents.
         */
        public SearchResult Search(string[] query, SearchSettings settings)
        {
            List<string> ignored;

            DateTime start = DateTime.Now;

            // Convert words to wordids
            var wordIds = GetWordIds(query, settings.CaseSensitive, out ignored);

            // perform the search - get all docIds
            var docIds = database.GetDocuments(wordIds);

            // get ids for the first maxAmount             
            var top = new List<int>();
            foreach (var p in docIds.GetRange(0, Math.Min(settings.ResultLimit, docIds.Count)))
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

        private List<int> GetWordIds(string[] query, bool caseSensitive, out List<string> ignored)
        {
            var result = new List<int>();
            ignored = new List<string>();

            foreach (var aWord in query)
            {
                if (caseSensitive)
                {
                    if (words.ContainsKey(aWord))
                        result.Add(words[aWord]);
                    else
                        ignored.Add(aWord);
                }
                else
                {
                    // Case-insensitive search
                    var matchingWord = words.Keys.FirstOrDefault(word => string.Equals(word, aWord, StringComparison.OrdinalIgnoreCase));
                    if (matchingWord != null)
                        result.Add(words[matchingWord]);
                    else
                        ignored.Add(aWord);
                }
            }
            return result;
        }

    }
}
