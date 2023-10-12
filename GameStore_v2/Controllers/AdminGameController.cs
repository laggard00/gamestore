using BLL.Interfaces;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore_v2.Controllers
{
    [Route("api/games/")]
    [ApiController]
    public class AdminGameController : Controller
    {
        private readonly IGameService _service;

        public AdminGameController(IGameService cs)
        {
            _service = cs;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameEntity>>> Get()
        {
            var customers = await _service.GetAll();

            if (customers != null) { return Ok(customers); }
            else { return StatusCode(500); }


        }
    }
}
