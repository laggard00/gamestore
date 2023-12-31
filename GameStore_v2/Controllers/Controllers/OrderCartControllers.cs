using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using GameStore.BLL.DTO;
using GameStore.BLL.Services;
using GameStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GameStore.WEB.Controllers.Controllers
{
    [ApiController]
    public class OrderCartControllers : Controller
    {
        private readonly OrderCartService service;
        public OrderCartControllers(OrderCartService _service)
        {
            service = _service;
        }
        [HttpPost("games/{key}/buy")]
        public async Task<ActionResult> AddGameToCart(string key)
        {
            try
            {
                await service.AddGameToCart(key);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("orders/cart/{key}")]
        public async Task<ActionResult> DeleteGameFromCart(string key)
        {
            try
            {
                await service.DeleteGameFromCart(key);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetPaidAndCancelledOrders()
        {
            try
            {
                var allPaidAndCancelledOrders =await service.GetPaidAndCancelledOrders();
                return allPaidAndCancelledOrders ==null?Ok("there are current paid or cancelled orders"):Ok(allPaidAndCancelledOrders);

            }
            catch (Exception ex) { return BadRequest("Something happened while getting your orders"); }
            
        }
        [HttpGet("orders/{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(Guid id)
        {
            var orderById = await service.GetOrderById(id);
            return orderById != null ? Ok(orderById) : Ok("no order by that ID");
        }

        [HttpGet("orders/{id}/details")]
        public async Task<ActionResult<IEnumerable<OrderGameDTO>>> GetOrderDetails(Guid id)
        {
            var orderDetails = await service.GetOrderDetails(id);
            return orderDetails == null ? Ok("no products in current order") : Ok(orderDetails);
        }

        [HttpGet("orders/cart")]
        public async Task<ActionResult<IEnumerable<OrderGameDTO>>> GetCurrentUsersCart()
        {
            var cartDetails = await service.GetCurrentUsersCart();
            return cartDetails == null ? Ok("cart is empty") : Ok(cartDetails);
        }

        [HttpGet("orders/payment-methods")]
        
        public async Task<ActionResult<object>> GetPaymentMethods()
        {
            var allPaymentMethods = await service.GetAllPaymentMethods();
            return new { paymentmethods = allPaymentMethods };
        }

        [HttpPost("orders/payment")]
        public async Task<ActionResult<IResult>>ProcessPayment([FromBody] PaymentRequest paymentRequest)
        {
            try
            {
                switch (paymentRequest.method.ToLower())
                {
                    case "bank":
                        QuestPDF.Settings.License = LicenseType.Community;
                        var UserData = new BankInvoice { UserId = new Guid(), OrderId = new Guid(), CreationDate = DateTime.Now, Sum = 287 };
                        MemoryStream ms = new MemoryStream();
                        QuestPDF.Fluent.Document.Create(x => x.Page(page => { page.Margin(50);
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
                        return File(ms, "application/pdf", $"INVOICE -{UserData.OrderId}.pdf");


                    // case "ibox terminal":break;
                    // case "visa":break;
                    default:return Ok();
                }
            }
            catch (Exception ex) { return BadRequest(); }
        }
    }
}
