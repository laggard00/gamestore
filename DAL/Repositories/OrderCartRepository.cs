﻿using GameStore.DAL.Filters;
using GameStore.DAL.Models;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameStore.DAL.Repositories {
    public class OrderCartRepository : IOrderCartRepository {
        protected readonly GameStoreDbContext context;
        public OrderCartRepository(GameStoreDbContext _context) {
            context = _context;
        }

        public OrderGame? FindGameInTheCart(Guid cartId, Guid gameId) {
            return context.Find<OrderGame>(cartId, gameId);
        }

        public void AddGameToTheCart(Guid cartId, Game game) {

            context.Add(new OrderGame { OrderId = cartId, ProductId = game.Id, Quantity = 1, Discount = game.Discount, Price = game.Price });

        }
        public void RemoveGameFromCart(OrderGame orderGame) {

            context.Remove(orderGame);

        }

        public Order CreateNewCartForUser(Guid userId) {
            var userCart = new Order { CustomerId = userId, Date = DateTime.Now, Status = Order.Statuses.Open.ToString() };
            context.Set<Order>().Add(userCart);
            return userCart;
        }

        public void DeleteGameFromCartOrDecreaseQuantity(Guid cartId, Game game) {
            var gameInTheCart = context.Find<OrderGame>(cartId, game.Id);
            if (gameInTheCart.Quantity == 1) { context.Remove(gameInTheCart); } else { gameInTheCart.Quantity--; }
        }

        public IEnumerable<PaymentMethods> GetAllPaymentMethods() {
            return context.PaymentMethods.ToList();
        }

        public Guid? GetCurrentUsersOpenCartId(Guid userId) {
            var user = context.Orders.SingleOrDefault(x => x.CustomerId == userId && x.Status == Order.Statuses.Open.ToString());
            return user?.Id;
        }

        public Order GetOrderById(Guid id) {
            return context.Orders.Find(id);
        }

        public IEnumerable<OrderGame> GetOrderDetails(Guid guid) {
            return context.OrderGames.Where(x => x.OrderId == guid);
        }

        public IEnumerable<Order> GetPaidAndCancelledOrdes() {
            return context.Orders.Where(order => order.Status == Order.Statuses.Paid.ToString()
                                              || order.Status == Order.Statuses.Cancelled.ToString()).ToList();
        }

        public async Task SetOrderToPaid(Guid? cartId) {
            var openOrder = context.Orders.Find(cartId);
            openOrder.Status = Order.Statuses.Paid.ToString();
        }

        public void UpdateCartAndGameInStock(IEnumerable<OrderGame> userCartMapped) {
            foreach (var item in userCartMapped) {
                var product = context.Games.Find(item.ProductId);
                product.UnitInStock -= item.Quantity;
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrders(Expression<Func<Order, bool>> predicate) {

            return await context.Orders.Where(predicate).ToListAsync();

        }
    }
}
