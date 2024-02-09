using GameStore.DAL.MongoRepositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Services {
    public class ShippersService {

        private readonly MongoFreeContentRepository _freeContentRepository;
        private const string collectionName = "shippers";
        public ShippersService(IMongoDatabase database) {
            _freeContentRepository = new MongoFreeContentRepository(database, collectionName);
        }

        public async Task<IEnumerable<dynamic>> GetAllShippersAsync() {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var allShippers = await _freeContentRepository.GetDocuments(filter);
            return allShippers;
        }

    }
}
