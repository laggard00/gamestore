using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.MongoRepositories {
    public class MongoFreeContentRepository {

        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly IMongoDatabase _database;

        public MongoFreeContentRepository(IMongoDatabase dataBase, string collectionName) {
            _database = dataBase;
            _collection = _database.GetCollection<BsonDocument>(collectionName);

        }
        public async Task<IEnumerable<dynamic>> GetDocuments(FilterDefinition<BsonDocument> filter) {
            var documents = await _collection.Find(filter).ToListAsync();
            return documents.Select(BsonTypeMapper.MapToDotNetValue);
        }

    }
}
