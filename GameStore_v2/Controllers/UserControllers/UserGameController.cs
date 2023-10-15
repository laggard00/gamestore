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
           if (!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }

            try
           {
               await _service.AddAsync(value);
       
               return NoContent();
       
           }
           catch (Exception ex)
           {
               return StatusCode(400, $"{ex.Message}");
           }
       
       }

        [HttpGet("{gameAlias}")]
        public async Task<ActionResult<string>> GetDescritptionByAlias(string gameAlias)
        {
            
            var ret = await _service.GetGameDescritpionByAlias(gameAlias);


            if (ret != null) { return Ok(ret); }


            else { return StatusCode(404); }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] GameDTO value, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != value.Id) { return BadRequest(); }
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
                return StatusCode(500, $" {ex.Message}");
            }
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<GameDTO>>> Get()
        {
           
            if (_cache.TryGetValue(cacheKey, out IEnumerable<GameDTO> result))
            {
                HttpContext.Response.Headers["Games-Cache-Counter"] = result.Count().ToString();
                return Ok(result);
                
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
    }
}
