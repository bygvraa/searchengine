using System;
using System.Collections.Generic;
using System.IO;
using Shared.BE;

namespace Indexer
{
    public class Crawler
    {
        private readonly char[] sep = " \\\n\t\"$'!,?;.:-_**+=)([]{}<>/@&%€#".ToCharArray();

        // Contains the words and their id's
        private readonly Dictionary<string, int> words = new();

        // Contains the documents
        private readonly List<BEDocument> documents = new();

        public int docCount;

        public int dirCount;

        readonly Database database;

        public Crawler(Database _database)
        {
            database = _database;
        }

        /*
         * Return a set containing all words in the file.
        */
        private ISet<string> ExtractWordsInFile(FileInfo f)
        {
            var res = new HashSet<string>();
            var content = File.ReadAllLines(f.FullName);

            foreach (var line in content)
            {
                foreach (var aWord in line.Split(sep, StringSplitOptions.RemoveEmptyEntries))
                {
                    res.Add(aWord);
                }
            }
            return res;
        }

        /**
         * Convert a set of words to a set of id's using the words
         * dictionary
         */
        private ISet<int> GetWordIdFromWords(ISet<string> src)
        {
            var res = new HashSet<int>();

            foreach (var p in src)
            {
                res.Add(words[p]);
            }
            return res;
        }

        /**
         * Will index all files contained in the directory [dir] and
         * having an extension in [extensions]. Will update the words and
         * documents */
        public void IndexFilesIn(DirectoryInfo dir, List<string> extensions)
        {
            Console.WriteLine("Crawling " + dir.FullName);
            dirCount++;

            foreach (var file in dir.EnumerateFiles())
                if (extensions.Contains(file.Extension))
                {
                    var newDoc = new BEDocument
                    {
                        mId = documents.Count + 1,
                        mUrl = file.FullName,
                        mIdxTime = DateTime.Now.ToString(),
                        mCreationTime = file.CreationTime.ToString()
                    };
                    documents.Add(newDoc);
                    database.InsertDocument(newDoc);
                    docCount++;

                    var newWords = new Dictionary<string, int>();
                    var wordsInFile = ExtractWordsInFile(file);

                    foreach (var aWord in wordsInFile)
                    {
                        if (!words.ContainsKey(aWord))
                        {
                            words.Add(aWord, words.Count + 1);
                            newWords.Add(aWord, words[aWord]);
                        }
                    }
                    database.InsertAllWords(newWords);
                    database.InsertAllOcc(newDoc.mId, GetWordIdFromWords(wordsInFile));
                }

            // Recursion
            foreach (var subDir in dir.EnumerateDirectories())
            {
                IndexFilesIn(subDir, extensions);
            }
        }

    }
}
