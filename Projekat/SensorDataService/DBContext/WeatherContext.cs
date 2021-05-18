using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SensorDataService.Model;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace SensorDataService.DBContext
{
    public class WeatherContext : DbContext
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
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("SOA_PROJEKAT");
            return db;
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            
        }

        
    }
}
