using MongoDB.Driver;
using System.Collections.Generic;

namespace AiDollar.Infrastructure.Database
{
    public class MongoDbOperation : IDbOperation
    {
        private readonly IMongoDatabase _database;
   
        public MongoDbOperation(string connectionString, string database)
        {
            IMongoClient client = new MongoClient(connectionString);
            _database = client.GetDatabase(database);
        }
        public int SaveItems<T>(IEnumerable<T> items, string tableName)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            collection.InsertManyAsync(items);
            return 1;
        }
        public IEnumerable<T> Select<T>(string query)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            var result = collection.Find<T>(query).ToList();
            return result;
        }
        public void BeginTransaction()
        { }
        public void Commit(){}

        public int Execute(string query)
        {
            return 1;
        }
    }
}
