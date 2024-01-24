﻿using GameStore.DAL.Models;
using GameStore_DAL.Models;

namespace GameStore.DAL.Repositories.RepositoryInterfaces
{
    public interface IOrderCartRepository
    {
        void AddGameToTheCart(Guid cartId, Game game);
        Order CreateNewCartForUser(Guid userId);
        void DeleteGameFromCartOrDecreaseQuantity(Guid cartId, Game game);
        IEnumerable<PaymentMethods> GetAllPaymentMethods();
        Guid? GetCurrentUsersOpenCartId(Guid userId);
        Order GetOrderById(Guid id);
        IEnumerable<OrderGame> GetOrderDetails(Guid guid);
        IEnumerable<Order> GetPaidAndCancelledOrdes();

        void RemoveGameFromCart(OrderGame orderGame);
          Task SetOrderToPaid(Guid? id);
        void UpdateCartAndGameInStock(IEnumerable<OrderGame> userCartMapped);
        OrderGame? FindGameInTheCart(Guid cartId, Guid gameId);
    }
}