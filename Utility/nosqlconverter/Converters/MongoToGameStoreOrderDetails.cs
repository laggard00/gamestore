using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using nosqlconverter.DBContexts;
using nosqlconverter.Models;
using nosqlconverter.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace nosqlconverter.Converters {
    public class MongoToGameStoreOrderDetails {

        private readonly GameStoreDbContext gsContext;
        private readonly NorthwindDbContext northwindContext;
        private readonly IMongoCollection<MongoOrderDetails> database;

        public MongoToGameStoreOrderDetails(GameStoreDbContext context, NorthwindDbContext nwContext, MongoClient client) {
            gsContext = context;
            northwindContext = nwContext;
            database = client.GetDatabase("Northwind").GetCollection<MongoOrderDetails>("orderdetails");
        }

        private async Task<IEnumerable<MongoOrderDetails>> LoadAllMongoOrderDetailsWithChild() {
            var mongoOrderDetailsAll = database.Aggregate()
                                               .Lookup("products", "ProductID", "ProductID", "Game")
                                               .Unwind("Game", new AggregateUnwindOptions<MongoOrderDetails>() { PreserveNullAndEmptyArrays = true })
                                               .ToList();

            return mongoOrderDetailsAll;
        }

        public async Task ConvertToGameStore()
        {
            var allOrderDetails = await LoadAllMongoOrderDetailsWithChild();

            foreach(var item in allOrderDetails) {
                var orderGuid = await FindOrderGuidForOrderID(item.OrderID);
                var gameGuid = await FindGameStoreGuidForGameName(item.Game.ProductName);
                if (orderGuid == Guid.Empty || gameGuid==Guid.Empty) { continue; }

                var orderGame = new OrderGame { OrderId = orderGuid, ProductId = gameGuid, Discount = (int)Math.Floor(item.Discount), Price = item.UnitPrice, Quantity = item.Quantity };
                gsContext.OrderGames.Add(orderGame);

            }
            gsContext.SaveChanges();
        }

        private async Task<Guid> FindGameStoreGuidForGameName(string gameName) {
            var gameFromGameStore = await gsContext.Games.SingleOrDefaultAsync(x => x.Name == gameName);
            return gameFromGameStore.Id;
        }
        private async Task<Guid> FindOrderGuidForOrderID(int orderId) {
            var order = await northwindContext.SQLOrders.SingleOrDefaultAsync(x => x.NoSqlOrderId == orderId);
            return order.Id;
        }
    }
}
