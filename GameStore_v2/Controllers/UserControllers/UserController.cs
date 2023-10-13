using BLL.DTO;
using BLL.Interfaces;
using BLL.Interfaces.IAdminINTERFACES;
using Microsoft.AspNetCore.Mvc;

namespace GameStore_v2.Controllers.UserController
{
    [Route("api/games")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _service;


        public UserController(IUserService cs)
        {
            _service = cs;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GameDTO>> GetById(int id)
        {

            var ret = await _service.GetByIdAsync(id);


            if (ret != null) { return Ok(ret); }


            else { return StatusCode(404); }
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GameDTO value)
        {

            try
            {
                await _service.AddAsync(value);

                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);

            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }

        }
    }
}
