using AutoMapper;
using GameStore.DAL.Models;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using Newtonsoft.Json;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using GameStore.BLL.DTO.BankRequest;
using GameStore.BLL.DTO.Order;
using GameStore.BLL.DTO.OrderGame;
using Microsoft.Extensions.Configuration;

namespace GameStore.BLL.Services
{
    public class OrderCartService
    {
        public IUnitOfWork uow { get; set; }
        public IMapper mapper { get; set; }

        private Microsoft.Extensions.Configuration.IConfiguration configuration { get; set; }
        public Guid userId { get; set; }
        public OrderCartService(IUnitOfWork uow, IMapper mapper, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            configuration = config;
            this.uow = uow;
            this.mapper = mapper;
            userId = new Guid("01f33a2f-2c43-4ae1-9fd7-08396921d328");
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
            var orderDetailsMapped = orderDetails.Select(x => mapper.Map<OrderGameDTO>(x));
            return orderDetailsMapped;
        }

        public async Task<IEnumerable<OrderGameDTO>> GetCurrentUsersCart()
        {
            //GET CURRENT USER

            var currentUsersCartId = uow.OrderCartRepository.GetCurrentUsersOpenCartId(userId);
            var currentUsersCart = uow.OrderCartRepository.GetOrderDetails(currentUsersCartId.Value);
            return currentUsersCart.Select(x => mapper.Map<OrderGameDTO>(x));

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

            var gameInCart = uow.OrderCartRepository.FindGameInTheCart(cart.Value, game.Id);
            if(gameInCart == null) { uow.OrderCartRepository.AddGameToTheCart(cart.Value, game); }
            else { gameInCart.Quantity++; }
            await uow.SaveAsync();

        }

        public async Task DeleteGameFromCart(string key)
        {

            var game = await uow.GamesRepository.GetGameByAlias(key);
            var cart = uow.OrderCartRepository.GetCurrentUsersOpenCartId(userId);
            var gameInCart = uow.OrderCartRepository.FindGameInTheCart(cart.Value, game.Id);
            if (gameInCart.Quantity > 1) { gameInCart.Quantity--; }
            else {
                uow.OrderCartRepository.RemoveGameFromCart(gameInCart);
            }
            await uow.SaveAsync();
        }

        public async Task<BankInvoice> GetInvoiceData()
        {
            var orderId = uow.OrderCartRepository.GetCurrentUsersOpenCartId(this.userId);
            var userId = this.userId;
            var date = await GetOrderById(orderId.Value);
            var dateTime = date.date;
            var userCart = await GetCurrentUsersCart();

            var Sum = Math.Round(userCart.Select(x => x.price * x.quantity * ((double)(100 - x.discount) / 100)).Sum(), 2);
            return new BankInvoice { CreationDate = dateTime, UserId = userId, OrderId = orderId.Value, Sum = Sum };

        }

        public async Task<HttpResponseMessage> ProcessIBoxTerminal(BankInvoice userInfo)
        {
            var boxRequest = new AnIBoxRequest() { accountNumber = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), transactionAmount = userInfo.Sum, invoiceNumber = userInfo.OrderId };
            var jsonBoxRequest = JsonConvert.SerializeObject(boxRequest);
            using (HttpClient httpClient = new HttpClient())
            {
                //please remember to put links in the file
                
                var uri = new Uri(configuration["BaseApi:base"]+"ibox");
                var content = new StringContent(jsonBoxRequest, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode) { await UpdateCartAndGameInStock(); }
                return response;
            }

        }
        public async Task<bool> IsItPossibleToPurchase()
        {

            var usersCart = await GetCurrentUsersCart();
            var usersCartMapped = usersCart.Select(x => mapper.Map<OrderGame>(x)).ToList();
            var isItLarger = uow.GamesRepository.UnitInStockIsLargerThanOrder(usersCartMapped);
            return isItLarger;
        }
        public async Task UpdateCartAndGameInStock()
        {
            var usersCart = await GetCurrentUsersCart();
            var userCartMapped = usersCart.Select(x => mapper.Map<OrderGame>(x)).ToList();
            uow.OrderCartRepository.UpdateCartAndGameInStock(userCartMapped);
            var usersCartid = uow.OrderCartRepository.GetCurrentUsersOpenCartId(userId);
            await uow.OrderCartRepository.SetOrderToPaid(usersCartid);
            await uow.SaveAsync();
        }

        public async Task<HttpResponseMessage> ProcessVisaPayment(Model? model, double sum)
        {
            var visaRequest = new VisaRequest() { cardHolderName = model.holder, cardNumber = model.cardNumber, cvv = model.cvv2, expirationMonth = model.monthExpire, expirationYear = model.yearExpire, transactionAmount = 56 };
            var jsonVisaRequest = JsonConvert.SerializeObject(visaRequest);
            using (HttpClient httpClient = new HttpClient())
            {
                var uri = new Uri(configuration["BaseApi:base"]+"visa");
                var content = new StringContent(jsonVisaRequest, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode) { await UpdateCartAndGameInStock(); }
                return response;
            }

        }
        public MemoryStream GenerateInvoicePdf(BankInvoice UserInfo)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var UserData = new BankInvoice { UserId = UserInfo.UserId, OrderId = UserInfo.OrderId, CreationDate = DateTime.Now, Sum = UserInfo.Sum };
            MemoryStream ms = new MemoryStream();
            QuestPDF.Fluent.Document.Create(x => x.Page(page =>
            {
                page.Margin(50);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(16));
                page.Header()
                       .AlignCenter()
                       .Text($"Invoice #: {UserData.OrderId}")
                       .SemiBold().FontSize(24).FontColor(Colors.Grey.Darken4);
                page.Content().Text(text =>
                {
                    text.Line($"UserId= {UserData.UserId}");
                    text.Line($"Sum={UserData.Sum}");
                    text.Line($"CreationDate = {UserData.CreationDate}");
                });
            })).GeneratePdf(ms);
            ms.Position = 0;
            return ms;
        }
    }
}
