using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Models;
using GameStore_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Services
{
    public class OrderCartService
    {
        public IUnitOfWork uow { get; set; }
        public IMapper mapper { get; set; }
        public Guid userId { get; set; }
        public OrderCartService(IUnitOfWork uow, IMapper mapper)
        { 
            this.uow= uow;
            this.mapper= mapper;
            userId = new Guid();
        }

        public async Task<IEnumerable<OrderDTO>> GetPaidAndCancelledOrders()
        {
            var orders = uow.OrderCartRepository.GetPaidAndCancelledOrdes();
            return orders.Select(x => mapper.Map<OrderDTO>(x));
        }

        public async Task<OrderDTO> GetOrderById(Guid id)
        { 
           var order = uow.OrderCartRepository.GetOrderById(id);
           return mapper.Map<OrderDTO>(order);
        }

        public async Task<IEnumerable<OrderGameDTO>> GetOrderDetails(Guid orderId)
        {
            var orderDetails = uow.OrderCartRepository.GetOrderDetails(orderId);
            return orderDetails.Select(x=> mapper.Map<OrderGameDTO>(x));
        }

        public async Task<IEnumerable<OrderGameDTO>> GetCurrentUsersCart()
        {
            //GET CURRENT USER
            
            var currentUsersCartId = uow.OrderCartRepository.GetCurrentUsersOpenCartId(userId);
            var currentUsersCart = uow.OrderCartRepository.GetOrderDetails(currentUsersCartId);
            return currentUsersCart.Select(x=> mapper.Map<OrderGameDTO>(x));

        }

        public async Task<IEnumerable<PaymentMethods>> GetAllPaymentMethods()
        {
            return uow.OrderCartRepository.GetAllPaymentMethods();
        }

        public async Task AddGameToCart(string key)
        {
            
            var game = await uow.GamesRepository.GetGameByAlias(key);
            var existingCartId = uow.OrderCartRepository.GetCurrentUsersOpenCartId(userId);
            var cart = existingCartId == null ? uow.OrderCartRepository.CreateNewCartForUser(userId).Id : existingCartId;
            uow.OrderCartRepository.AddGameToTheCartOrIncreaseQuantity(cart, game);
            uow.SaveAsync();
            
        }

        public async Task DeleteGameFromCart(string key)
        {
            
            var game = await uow.GamesRepository.GetGameByAlias(key);
            var cart = uow.OrderCartRepository.GetCurrentUsersOpenCartId(userId);
            if (cart == null) { throw new Exception("Youre trying to delete a game from the cart but you never added the game in it"); }
            uow.OrderCartRepository.DeleteGameFromCartOrDecreaseQuantity(cart, game);
        }

        public async Task<BankInvoice> GetInvoiceData()
        {
            var OrderId = uow.OrderCartRepository.GetCurrentUsersOpenCartId(userId);
            var UserId = userId;
            var Date = await GetOrderById(OrderId);
            var dateTime = Date.date;
            var UserCart = await GetCurrentUsersCart();
            var Sum = UserCart.Select(x => x.price * x.quantity * ((100 - x.discount) / 100)).Sum();
           return new BankInvoice { CreationDate = dateTime, UserId = UserId, OrderId = OrderId, Sum = Sum };

        }
    }
}
