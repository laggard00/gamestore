using GameStore.BLL.Services;
using GameStore.DAL.MongoRepositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.WEB.Controllers {

    [Route("[controller]")]
    public class ShippersController : Controller {

        private readonly ShippersService shipperService;

        public ShippersController(ShippersService service) {
            shipperService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BsonDocument>>> GetAllShippers() {
            var allShippers = await shipperService.GetAllShippersAsync();
            return Ok(allShippers);
        }
    }
}
