using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GameStore_v2.Controllers.UserControllers
{
    [Route("api/platforms/")]
    [ApiController]
    public class UserPlatformController : Controller
    {
        private readonly IAdminPlatformService _service;
        private readonly IMemoryCache _cache;

        private readonly string cacheKey = "PlatformCache";

        public UserPlatformController(IAdminPlatformService cs, IMemoryCache cache)
        {
            _service = cs;
            _cache = cache;
        }
        /// <summary>
        /// Get all platforms
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformDTO>>> Get()
        {
            if (_cache.TryGetValue(cacheKey, out IEnumerable<PlatformDTO> result))
                {
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

                    return Ok(result);
                }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformDTO>> GetById(int id)
        {
            var ret = await _service.GetByIdAsync(id);

            if (ret != null) 
            { 
                return Ok(ret); 
            }

            else 
            { 
                return StatusCode(404);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PlatformDTO value)
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
        [HttpDelete("{id}")]
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
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] PlatformDTO value, int id)
        {

            if (id != value.Id) { return BadRequest(); }
            try
            {


                await _service.UpdateAsync(value);
                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
            }
            catch (Exception ex)
            {
                return BadRequest($" {ex.Message}");
            }

        }
    }
}
