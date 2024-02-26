using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nosqlconverter.MongoModels;
using nosqlconverter.DBContexts;

namespace nosqlconverter.Converters
{
    public class NoSqlToSqlOrdersEntityConverter
    {


        private readonly IMongoCollection<MongoOrders> mongoDbOrders;

        private readonly GameStoreDbContext gameStoreDbContext;

        private readonly NorthwindDbContext northwindDbContext;




        public NoSqlToSqlOrdersEntityConverter(MongoClient client, GameStoreDbContext _gScontext, NorthwindDbContext nwContext)
        {

            mongoDbOrders = client.GetDatabase("Northwind").GetCollection<MongoOrders>("orders");
            gameStoreDbContext = _gScontext;
            northwindDbContext = nwContext;
        }

        public async Task ConvertToGameStore()
        {

            await AddAllNotExistingNoSqlOrdersToSql();
            northwindDbContext.SaveChanges();
            await UpdateGameStore();
            gameStoreDbContext.SaveChanges();


        }
        private async Task<IEnumerable<MongoOrders>> LoadAllNoSqlOrders()
        {

            var allNoSqlOrders = mongoDbOrders.Find(_ => true);
            return allNoSqlOrders.ToList();
        }

        private async Task<IEnumerable<MongoSQLOrders>> LoadAllNorthwindOrders()
        {


            return await northwindDbContext.SQLOrders.ToListAsync();


        }

        private async Task AddAllNotExistingNoSqlOrdersToSql()
        {

            var allMongoDbOrders = await LoadAllNoSqlOrders();
            var allNorthwindInSQL = await LoadAllNorthwindOrders();
            var mongoOrdersToUpdateNorthWind = allMongoDbOrders.Where(x => !allNorthwindInSQL.Any(y => y.NoSqlOrderId == x.OrderId));
            await northwindDbContext.SQLOrders.AddRangeAsync(mongoOrdersToUpdateNorthWind.Select(x => NoSqlToSqlOrderMapper(x)));
            northwindDbContext.SaveChanges();


        }


        public async Task UpdateGameStore()
        {
            await AddAllNotExistingNoSqlOrdersToSql();
            var gameStoreOrders = gameStoreDbContext.Orders.ToList();
            var northWindOrders = northwindDbContext.SQLOrders.ToList();
            var northWindOrdersToUpdateGameStore = northWindOrders.Where(orderId => !gameStoreOrders.Any(x => x.Id == orderId.Id)).ToList();
            await gameStoreDbContext.AddRangeAsync(northWindOrdersToUpdateGameStore.Select(NorthwindToGameStoreMapper));
            gameStoreDbContext.SaveChanges();

        }

        private MongoSQLOrders NoSqlToSqlOrderMapper(MongoOrders nosqlOrder)
        {
            return new MongoSQLOrders
            {
                CustomerId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                NoSqlOrderId = nosqlOrder.OrderId,
                NoSqlCustomerId = nosqlOrder.CustomerId,
                Date = DateTime.Parse(nosqlOrder.OrderDate)
            };
        }

        private Order NorthwindToGameStoreMapper(MongoSQLOrders sqlOrder)
        {
            return new Order { CustomerId = sqlOrder.CustomerId, Id = sqlOrder.Id, Date = sqlOrder.Date, Status = "Paid" };
        }


    }
}
