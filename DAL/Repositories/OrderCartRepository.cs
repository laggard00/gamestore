using GameStore.DAL.Models;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameStore.DAL.Repositories
{
    public class OrderCartRepository
    {
        protected readonly GameStoreDbContext context;
        public OrderCartRepository(GameStoreDbContext _context)
        {
            context=_context;
        }

        public void AddGameToTheCartOrIncreaseQuantity(Guid cartId, Game game)
        {
            var gameInTheCart = context.Find<OrderGame>(cartId, game.Id);
            if (gameInTheCart == null)
            {
                if (game.UnitInStock > 0)
                {
                    context.Add(new OrderGame { OrderId = cartId, ProductId = game.Id, Quantity = 1, Discount = game.Discount, Price = game.Price });

                }
            }
            else 
            {
                if(game.UnitInStock > 0) { gameInTheCart.Quantity++; }
            }

        }

        public Order CreateNewCartForUser(Guid userId)
        {
            var userCart = new Order { Id = new Guid(), CustomerId = userId, Date=DateTime.Now, Status=Order.Statuses.Open.ToString() };
            context.Set<Order>().Add(userCart);
            return userCart;
        }

        public void DeleteGameFromCartOrDecreaseQuantity(Guid cartId, Game game)
        {
            var gameInTheCart = context.Find<OrderGame>(cartId, game.Id);
            if (gameInTheCart.Quantity == 1) { context.Remove(gameInTheCart); }
            else { gameInTheCart.Quantity--; }
        }

        public IEnumerable<PaymentMethods> GetAllPaymentMethods()
        {
            return context.PaymentMethods.ToList();
        }

        public Guid GetCurrentUsersOpenCartId(Guid userId)
        {
            return context.Orders.Where(x => x.CustomerId == userId && x.Status == Order.Statuses.Open.ToString()).SingleOrDefault().Id;
            
           
        }

        public Order GetOrderById(Guid id)
        {
            return context.Orders.Find(id);
        }

        public IEnumerable<OrderGame> GetOrderDetails(Guid guid)
        {
            return context.OrderGames.Where(x=>x.OrderId== guid);
        }

        public IEnumerable<Order> GetPaidAndCancelledOrdes()
        {
            return context.Orders.Where(order => order.Status == Order.Statuses.Paid.ToString()
                                              || order.Status == Order.Statuses.Cancelled.ToString()).ToList();
        }
    }
}
