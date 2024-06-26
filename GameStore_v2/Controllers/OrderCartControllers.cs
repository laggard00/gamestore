﻿using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using GameStore.BLL.Services;
using GameStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using AutoMapper;
using GameStore.BLL.DTO.BankRequest;
using GameStore.BLL.DTO.Order;
using GameStore.BLL.DTO.OrderGame;
using GameStore.DAL.Filters;
using GameStore.WEB.AuthUtilities;

namespace GameStore.WEB.Controllers {

    [ApiController]
    public class OrderCartControllers : Controller {
        private readonly OrderCartService service;
        public OrderCartControllers(OrderCartService _service) {
            service = _service;
        }
        [HttpPost("games/{key}/buy")]
        public async Task<ActionResult> AddGameToCart(string key) {
            await service.AddGameToCart(key);
            return Ok();
        }
        [HttpDelete("orders/cart/{key}")]
        public async Task<ActionResult> DeleteGameFromCart(string key) {
            await service.DeleteGameFromCart(key);
            return Ok();
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetPaidAndCancelledOrders() {
            var allPaidAndCancelledOrders = await service.GetPaidAndCancelledOrders();
            return allPaidAndCancelledOrders == null ? Ok("there are current paid or cancelled orders") : Ok(allPaidAndCancelledOrders);
        }
        [HasPremission(PermissionEnum.ViewOrderHistory)]
        [HttpGet("orders/history")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrderHistory([FromQuery] OrderFilter orderFilter ) {
           
            var allPaidOrders = await service.GetAllPaidOrders(orderFilter);
            return allPaidOrders!=null ? Ok(allPaidOrders) : Ok("there are not orders matching the filter");
        }
        [HttpGet("orders/{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(Guid id) {
            var orderById = await service.GetOrderById(id);
            return orderById != null ? Ok(orderById) : Ok("no order by that ID");
        }

        [HttpGet("orders/{id}/details")]
        public async Task<ActionResult<IEnumerable<OrderGameDTO>>> GetOrderDetails(Guid id) {
            var orderDetails = await service.GetOrderDetails(id);
            return orderDetails == null ? Ok("no products in current order") : Ok(orderDetails);
        }

        [HttpGet("orders/cart")]
        public async Task<ActionResult<IEnumerable<OrderGameDTO>>> GetCurrentUsersCart() {
            var cartDetails = await service.GetCurrentUsersCart();
            return cartDetails == null ? Ok("cart is empty") : Ok(cartDetails);
        }

        [HttpGet("orders/payment-methods")]

        public async Task<ActionResult<object>> GetPaymentMethods() {
            var allPaymentMethods = await service.GetAllPaymentMethods();
            return new { paymentMethods = allPaymentMethods };
        }
        [HasPremission(PermissionEnum.ChangeOrderStatus)]
        [HttpPost("orders/{orderId}/ship")]
        public async Task<ActionResult> ShipOrder(string orderId) {
            await service.ShipOrderAsync(orderId);
            return Ok();
        }
        [HttpPost("orders/{orderId}/details/{gameKey}")]
        public async Task<ActionResult> AddGameToOrder(string orderId, string gameKey) { 
            await service.AddGameToOrder(orderId, gameKey);
            return Ok();
        }
        [HttpPost("orders/payment")]
        public async Task<ActionResult<IResult>> ProcessPayment([FromBody] PaymentRequest paymentRequest) {

            var isItPossible = await service.IsItPossibleToPurchase();
            if (!isItPossible) { return BadRequest("not possible to buy"); }

            var UserInfo = await service.GetInvoiceData();
            switch (paymentRequest.method.ToLower()) {
                case "bank":
                    using (MemoryStream memoryStreamPdf = service.GenerateInvoicePdf(UserInfo)) {
                        await service.UpdateCartAndGameInStock();
                        return File(memoryStreamPdf.ToArray(), "application/pdf", $"INVOICE -{UserInfo.OrderId}.pdf");
                    }
                case "ibox terminal":
                    var response = await service.ProcessIBoxTerminal(UserInfo);
                    return response.IsSuccessStatusCode ? Ok(new { userId = UserInfo.UserId, orderId = UserInfo.OrderId, paymentDate = DateTime.Now, sum = UserInfo.Sum }) : StatusCode((int)response.StatusCode, response.Content);

                case "visa":
                    var processVisa = await service.ProcessVisaPayment(paymentRequest.model, UserInfo.Sum);
                    return processVisa.IsSuccessStatusCode ? Ok() : StatusCode((int)processVisa.StatusCode, processVisa.Content);
                default: return Ok();
            }
        }

    }
}

