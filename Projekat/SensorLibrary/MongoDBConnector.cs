using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SensorLibrary
{
    public class MongoDBConnector
    {
        private static IMongoDatabase _db = null;
        private static readonly object objLock = new object();

        public static IMongoDatabase GetInstance()
        {
            if (_db == null)
            {
                lock (objLock)
                {
                    if (_db == null)
                    {
                        _db = CreateDB();
                    }
                }
            }

            return _db;
        }

        private static IMongoDatabase CreateDB()
        {
            string connectionString = string.Empty;
            using (var sr = new StreamReader("connectionString.txt"))
            {
                connectionString = sr.ReadToEnd();
            }
            var client = new MongoClient("mongodb://root:example@mongo:27017");
            var db = client.GetDatabase("SOA_PROJEKAT");
            return db;
        }
    }
}
