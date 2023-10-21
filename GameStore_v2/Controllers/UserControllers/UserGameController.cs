using BLL.DTO;
using BLL.Interfaces;
using BLL.Interfaces.IAdminINTERFACES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GameStore_v2.Controllers.UserController
{
    [Route("api/games")]
    [ApiController]
    public class UserGameController : Controller
    {

        private readonly IUserService _service;

        private readonly IMemoryCache _cache;

        private readonly string cacheKey = "Games";



        public UserGameController(IUserService cs, IMemoryCache cache)
        {
            _service = cs;
            _cache = cache;

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
        public async Task<ActionResult> Put([FromBody] GameDTO value)
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


        [HttpDelete("/remove/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {

                await _service.DeleteAsync(id);

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
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<GameDTO> result))
            {
                HttpContext.Response.Headers["Games-Cache-Counter"] = result.Count().ToString();
                return Ok(new { result, a= result.Count().ToString() });

            }
            else
            {
                result = await _service.GetAllAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions().
                                                        SetSlidingExpiration(TimeSpan.FromSeconds(45)).
                                                        SetAbsoluteExpiration(TimeSpan.FromSeconds(600)).
                                                        SetPriority(CacheItemPriority.Normal);

                _cache.Set(cacheKey, result, cacheEntryOptions);

                HttpContext.Response.Headers["Games-Cache-Counter"] = result.Count().ToString();
                return Ok(result);
            }




        }



        [HttpGet("id/{id}")]

        public async Task<ActionResult<IEnumerable<GameDTO>>> GetById(int id)
        {

            var result = await _service.GetByIdAsync(id);

            return Ok(result);

        }
    }
}
