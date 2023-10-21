using BLL.DTO;
using BLL.Interfaces;
using BLL.Interfaces.IAdminINTERFACES;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

namespace GameStore_v2.Controllers.UserController
{
    [Route("api/games")]
    [ApiController]
    public class UserGameController : Controller
    {

        private readonly IUserService _service;

        private readonly IAppCache _appCache;




        public UserGameController(IUserService cs, IAppCache cache)
        {
            _service = cs;
            _appCache = cache;

        }




        [HttpPost("new")]
        public async Task<ActionResult> Post([FromBody] GameDTO value)
        {
            if (value == null)
            {
                return BadRequest("The request body must not be null.");
            }

            if (ModelState.IsValid)
            {

               
                // If alias is not provided, generate it from the game name

                if (string.IsNullOrEmpty(value.GameAlias))
                {
                    value.GameAlias = value.Name.Replace(' ', '-').ToLower();
                }


                var existingGame = await _service.GetGameByAlias(value.GameAlias);
                if (existingGame != null)
                {
                    return Conflict("This game with this alias already exists!");
                }


                try
                {
                    await _service.AddAsync(value);
                    return Ok();
                }
                catch (Exception ex)
                {
                    // Log the exception here
                    return StatusCode(500, "An error occurred while saving the game.");
                }
            }

            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet("{gameAlias}")]
        public async Task<ActionResult<GameDTO>> GetGameByAlias(string gameAlias)
        {

            var ret = await _service.GetGameByAlias(gameAlias);


            if (ret != null) { return Ok(ret); }


            else { return StatusCode(404) ; }
        }


        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] GameDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            try
            {

                await _service.UpdateAsync(value);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($" {ex.Message}");
            }

        }


        [HttpDelete("/remove")]
        public async Task<ActionResult> Remove([FromBody] GameDTO value)
        {
            try
            {

                await _service.DeleteAsync(value.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(404, $" {ex.Message}");
            }
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<GameDTO>>> Get()
        {
            try
            {
                var result = await _appCache.GetOrAdd("gamesGet", async () => await _service.GetAllAsync(), DateTime.Now.AddMinutes(1));

                return Ok(new { result, gamesInCache = result.Count() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }



        [HttpGet("id/{id}")]

        public async Task<ActionResult<IEnumerable<GameDTO>>> GetById(int id)
        {

            var result = await _service.GetByIdAsync(id);

            return Ok(result);

        }

        [HttpGet("{gameAlias}/download")]
        public async Task<ActionResult> DownloadGame(string gameAlias)
        {
            var game = await _service.GetGameByAlias(gameAlias);


            if (game == null)
            {
                return NotFound($"Game with alias '{gameAlias}' not found.");
            }

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");


            var fileName = $"{game.Name}_{timestamp}.txt";


            var contentType = "application/octet-stream";


            return File(Encoding.UTF8.GetBytes(string.Empty), contentType, fileName);





        }
    }
}
