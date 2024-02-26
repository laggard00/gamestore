using Microsoft.EntityFrameworkCore;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using nosqlconverter.DBContexts;
using nosqlconverter.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace nosqlconverter.Converters {
    public class MongoToGameStoreProductsToGamesConverter {

        private readonly IMongoCollection<MongoProducts> mongoDbProducts;
        private readonly IMongoCollection<MongoGame> mongoDBGame;
        private readonly GameStoreDbContext gameStoreDbContext;
        public MongoToGameStoreProductsToGamesConverter(GameStoreDbContext gsContext, MongoClient client) {
            mongoDbProducts = client.GetDatabase("Northwind").GetCollection<MongoProducts>("products");
            mongoDBGame = client.GetDatabase("Northwind").GetCollection<MongoGame>("Games");

            gameStoreDbContext = gsContext;
        }

        public async Task LoadAllMongoProductsNotExistingInSQL() {
        
           var gameNamesInGameStore = gameStoreDbContext.Games.Select(x => x.Name).ToList();
           var notInMatchArray = new BsonArray(gameNamesInGameStore);
           var bsonDocument = new BsonDocument {
                                                 {
                                                     "ProductName",
                                                                 new BsonDocument
                                                                 {
                                                         { "$nin", notInMatchArray }
                                                     }
           
                                                   },{"Discontinued",0}
                                               };

            var allMongoProductsWithSuppliersAndCategory = mongoDbProducts.Aggregate()

                                                                          .Lookup("categories", "CategoryID", "CategoryID", "asCategories")
                                                                          .Lookup("suppliers", "SupplierID", "SupplierID", "asSupplier")

                                                                          .Unwind("asCategories", new AggregateUnwindOptions<MongoProducts>() { PreserveNullAndEmptyArrays = true })
                                                                          .Unwind("asSupplier", new AggregateUnwindOptions<MongoProducts>() { PreserveNullAndEmptyArrays = true })
                                                                          // .Match(bsonDocument)
                                                                          .ToList();
        
           
            foreach (var game in allMongoProductsWithSuppliersAndCategory) 
            {
                var genreGuid = await FindCategoryGenreAndUpdateGameGenre(game.asCategories.CategoryName);
                var gameGuid = gameStoreDbContext.Games.Add(ProductToGamesMapper(game)).Entity.Id;
                gameStoreDbContext.GameGenre.Add(new GameGenre {
                    GameId = gameGuid, GenreId = genreGuid
                });
            }
            gameStoreDbContext.SaveChanges();
        }

        public async Task FindGameByGuid() 
        {
            var guid = new Guid("f20eea49-7b74-4379-8662-f82b142550c4");
            var filter = Builders<BsonDocument>.Filter.Eq(new StringFieldDefinition<BsonDocument, Guid>("PublisherId"), guid);
            var doc = new BsonDocument { { "PublisherId", guid } };
            var b =  mongoDBGame.Find(x=> x._id==guid).ToList();
          
        }

        private Game ProductToGamesMapper(MongoProducts product) {
        
            var guid = FindSupplierGuid(product.asSupplier.CompanyName);
            return new Game {
                Id = Guid.NewGuid(),
                Description = product.QuantityPerUnit,
                Key = product.ProductName,
                Name = product.ProductName,
                Price = product.UnitPrice,
                Discount = 0,
                PublisherId = guid,
                UnitInStock = product.UnitsInStock
            };
        }



        private Guid FindSupplierGuid(string companyName) {
            var publisher =  gameStoreDbContext.Publishers.SingleOrDefault(x => x.CompanyName == companyName);
            return publisher == null ? Guid.Empty : publisher.Id;
        }
        private async Task<Guid> FindCategoryGenreAndUpdateGameGenre(string genreName) {

            var genre = await gameStoreDbContext.Genres.SingleOrDefaultAsync(x => x.Name == genreName);
            return genre.Id;

        }


        private async Task StoreKeyIdPairIntoNorthWindSQLDataBase() {
        }



    }

}

