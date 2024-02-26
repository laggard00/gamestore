using Microsoft.EntityFrameworkCore;
using Models;
using MongoDB.Driver;
using nosqlconverter.DBContexts;
using nosqlconverter.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nosqlconverter.Converters
{
    public class MongoToGameStoreCategoriesToGenreConverter
    {

        private readonly IMongoCollection<MongoCategory> mongoDbCategory;
        private readonly GameStoreDbContext gameStoreDbContext;

        public MongoToGameStoreCategoriesToGenreConverter(MongoClient client, GameStoreDbContext gsContext)
        {
            mongoDbCategory = client.GetDatabase("Northwind").GetCollection<MongoCategory>("categories");
            gameStoreDbContext = gsContext;

        }

        private async Task<IEnumerable<MongoCategory>> LoadAllMongoCategoryThatDontExistInGameStore()
        {
            var gameStoreGenreNames = await gameStoreDbContext.Genres.Select(x => x.Name).ToListAsync();
            var allMongoThatDontExistInGameStore = mongoDbCategory.Find(x => !gameStoreGenreNames.Contains(x.CategoryName)).ToList();
            return allMongoThatDontExistInGameStore;
        }

        private GenreEntity CategoryToPublisherMapper(MongoCategory category)
        {
            return new GenreEntity { Name = category.CategoryName };
        }

        public async Task ConvertToGameStore()
        {
            var allMongoThatDontExistInGameStore = await LoadAllMongoCategoryThatDontExistInGameStore();
            var allMongoMapped = allMongoThatDontExistInGameStore.Select(CategoryToPublisherMapper);
            await gameStoreDbContext.Genres.AddRangeAsync(allMongoMapped);
            await gameStoreDbContext.SaveChangesAsync();
        }
    }
}
