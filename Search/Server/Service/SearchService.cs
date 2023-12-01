using Shared;
using Shared.BE;

namespace Server.Service
{
    public class SearchService
    {
        private readonly ILogger<SearchService> _logger;
        private readonly HttpClient _httpClient;
        private Dictionary<string, int> _wordCache = new(); // a cache for all words in the documents

        public SearchService(ILogger<SearchService> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient { BaseAddress = new Uri(Config.DATABASE_ADDRESS) };
            _wordCache = GetAllWordsAsync().GetAwaiter().GetResult();
        }

        /* Perform search of documents containing words from query. The result will
         * contain details about up to maxAmount of documents. */
        public async Task<SearchResult> Search(string[] query, SearchSettings settings)
        {
            DateTime start = DateTime.Now;

            if (_wordCache.Count == 0 || _wordCache == null)
            {
                _logger.LogInformation("No words in cache, fetching from server...");
                _wordCache = await GetAllWordsAsync();
                if (_wordCache.Count == 0)
                    _logger.LogWarning("Could not fetch words from database server.");
            }

            // Convert words to word ids
            var wordIds = GetWordIds(query, settings.CaseSensitive, out List<string> ignoredWords);
            if (wordIds.Count == 0)
            {
                _logger.LogInformation("No documents found.");
                return new SearchResult(query, 0, new List<DocumentHit>(), ignoredWords, DateTime.Now - start);
            }

            // Get documents containing the words - get all docIds
            var docIds = await GetDocumentsAsync(wordIds);

            // get ids for the first maxAmount             
            var docs = new List<int>();
            foreach (var doc in docIds.GetRange(0, Math.Min(settings.ResultLimit, docIds.Count)))
            {
                docs.Add(doc.Key);
            }

            // compose the result, all the documentHit
            var docResult = new List<DocumentHit>();
            int idx = 0;

            foreach (var doc in await GetDocDetailsAsync(docs))
            {
                docResult.Add(new DocumentHit(doc, docIds[idx++].Value));
            }

            return new SearchResult(query, docIds.Count, docResult, ignoredWords, DateTime.Now - start);
        }

        private List<int> GetWordIds(string[] query, bool caseSensitive, out List<string> ignored)
        {
            var result = new List<int>();
            ignored = new List<string>();

            foreach (var word in query)
            {
                if (caseSensitive)
                {
                    if (_wordCache.TryGetValue(word, out int wordId))
                    {
                        result.Add(wordId);
                    }
                    else
                    {
                        ignored.Add(word);
                    }
                }
                else
                {
                    // Case-insensitive search
                    var matches = _wordCache.Keys.Where(key => string.Equals(word, key, StringComparison.OrdinalIgnoreCase)).ToList();
                    if (matches.Any())
                    {
                        result.AddRange(matches.Select(match => _wordCache[match]));
                    }
                    else
                    {
                        ignored.Add(word);
                    }
                }
            }
            return result;
        }

        private async Task<Dictionary<string, int>> GetAllWordsAsync()
        {
            var route = $"{_httpClient.BaseAddress}database/words";
            _logger.LogInformation($"GET: GetAllWords at Database server: {route}");

            var response = await _httpClient.GetAsync(route);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
                if (result != null)
                    return result;
                else
                    return new Dictionary<string, int>();
            }
            else
            {
                _logger.LogWarning($"GET: GetAllWords at Database server: {route} --- ERROR");
                return new Dictionary<string, int>();
            }
        }

        private async Task<List<KeyValuePair<int, int>>> GetDocumentsAsync(List<int> wordIds)
        {
            var route = $"{_httpClient.BaseAddress}database/documents";
            _logger.LogInformation($"GET: GetDocuments at Database server: {route}");

            var response = await _httpClient.PostAsJsonAsync(route, wordIds);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<KeyValuePair<int, int>>>();
                if (result != null)
                    return result;
            }
            else
            {
                _logger.LogError($"GET: GetDocuments at Database server: {route} --- ERROR");
            }
            return new List<KeyValuePair<int, int>>();
        }

        private async Task<List<BEDocument>> GetDocDetailsAsync(List<int> docIds)
        {
            var route = $"{_httpClient.BaseAddress}database/details";
            _logger.LogInformation($"GET: GetDocDetails at Database server: {route}");

            var response = await _httpClient.PostAsJsonAsync(route, docIds);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<BEDocument>>();
                if (result != null)
                    return result;
            }
            else
            {
                _logger.LogError($"GET: GetDocDetails at Database server: {route} --- ERROR");
            }
            return new List<BEDocument>();
        }
    }
}
