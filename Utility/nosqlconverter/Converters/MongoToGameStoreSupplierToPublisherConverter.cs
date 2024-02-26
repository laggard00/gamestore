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
    public class MongoToGameStoreSupplierToPublisherConverter
    {

        private readonly IMongoCollection<MongoSuppliers> mongoDbSuppliers;
        private readonly GameStoreDbContext gameStoreDbContext;


        public MongoToGameStoreSupplierToPublisherConverter(MongoClient client, GameStoreDbContext gsContext)
        {
            mongoDbSuppliers = client.GetDatabase("Northwind").GetCollection<MongoSuppliers>("suppliers");
            gameStoreDbContext = gsContext;

        }

        private async Task<IEnumerable<MongoSuppliers>> LoadAllMongoWhereNameDoesntExistInGameStore()
        {
            var gameStorePublisherNames = await gameStoreDbContext.Publishers.Select(x => x.CompanyName).ToListAsync();
            var allMongoThatDontExistInGameStore = mongoDbSuppliers.Find(x => !gameStorePublisherNames.Contains(x.CompanyName)).ToList();
            return allMongoThatDontExistInGameStore;
        }

        private string UrlParser(string url)
        {

            var indexOfPound = url.IndexOf('#') + 1;
            var lastIndexOfPound = url.LastIndexOf('#') - indexOfPound; ;
            var uri = "";

            if (indexOfPound != -1 && lastIndexOfPound != -1 && indexOfPound != lastIndexOfPound)
            {

                uri = url.Substring(indexOfPound, lastIndexOfPound);
            }

            Uri uriResult;
            bool result = Uri.TryCreate(uri, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (result)
            {
                return uriResult.ToString();
            }
            else
            {
                return null;
            }
        }
        private Publisher SuppliersToPublisherMapper(MongoSuppliers supplier)
        {

            return new Publisher { CompanyName = supplier.CompanyName, Description = supplier.ContactName, HomePage = UrlParser(supplier.HomePage) };
        }

        public async Task ConvertToGameStore()
        {
            var allMongoThatDontExistInGameStore = await LoadAllMongoWhereNameDoesntExistInGameStore();
            var allMongoMapped = allMongoThatDontExistInGameStore.Select(SuppliersToPublisherMapper);
            await gameStoreDbContext.Publishers.AddRangeAsync(allMongoMapped);
            await gameStoreDbContext.SaveChangesAsync();
        }
    }
}
