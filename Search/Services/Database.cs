using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Shared;
using Shared.BE;

namespace Search.Services
{
    public class Database
    {
        private readonly SqliteConnection _connection;

        public Database()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = Config.DATABASE
            };

            _connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            _connection.Open();
        }

        private void Execute(string sql)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        /** Will perform a search based on a list of word id's. The result
         * is a list of KeyValuePair, where the key part is the id of the 
         * documents and the value part is the number of words in the document
        */
        public List<KeyValuePair<int, int>> GetDocuments(List<int> wordIds)
        {
            var res = new List<KeyValuePair<int, int>>();

            /* Here is an example where we search for documents
             * containing words with id 2 or id 3.
             * SELECT docId, COUNT(wordId) as count
                FROM Occ
                where wordId in (2,3)
                GROUP BY docId
                ORDER BY COUNT(wordId) DESC
             * 
             */

            var sql = "SELECT docId, ";
            sql += "COUNT(wordId) as count ";
            sql += "FROM Occ ";
            sql += "WHERE wordId in " + AsString(wordIds) + " ";
            sql += "GROUP BY docId ";
            sql += "ORDER BY count DESC;";

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = sql;

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var docId = reader.GetInt32(0);
                    var count = reader.GetInt32(1);

                    res.Add(new KeyValuePair<int, int>(docId, count));
                }
            }
            return res;
        }

        /**
         * will return x as a ',' separated string. For instance
         * will AsString([1,2,3]) return "(1,2,3)".
         */
        private string AsString(List<int> x)
        {
            string res = "(";

            for (int i = 0; i < x.Count - 1; i++)
                res += x[i] + ",";

            if (x.Count > 0)
                res += x[x.Count - 1];

            res += ")";

            return res;
        }
        /*
         * SELECT wordId, COUNT(docId) as count
            FROM Occ
            where wordId in (2,3)
            GROUP BY wordId
            ORDER BY COUNT(docId) DESC;
        */

        public Dictionary<string, int> GetAllWords()
        {
            Dictionary<string, int> res = new Dictionary<string, int>();

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM word";

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var w = reader.GetString(1);

                    res.Add(w, id);
                }
            }
            return res;
        }

        public List<BEDocument> GetDocDetails(List<int> docIds)
        {
            var res = new List<BEDocument>();

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM document where id in " + AsString(docIds);

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var url = reader.GetString(1);
                    var idxTime = reader.GetString(2);
                    var creationTime = reader.GetString(3);

                    res.Add(new BEDocument
                    {
                        mId = id,
                        mUrl = url,
                        mIdxTime = idxTime,
                        mCreationTime = creationTime
                    });
                }
            }
            return res;
        }
    }
}
