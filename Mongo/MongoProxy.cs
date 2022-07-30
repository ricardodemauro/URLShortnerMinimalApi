using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortnerMinimalApi.Mongo
{
    public class MongoProxy
    {
        readonly MongoClient _client;

        readonly IMongoDatabase _database;

        public MongoProxy(string connectionString)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("UrlShortner");
        }

        public IMongoDatabase Database => _database;
    }
}
