using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

using static System.Net.Mime.MediaTypeNames;

namespace GameStore_v2.Controllers.AdminControllers
{
    [Route("api/admingames/")]
    [ApiController]
    public class AdminGameController : Controller
    {
        private readonly IAdminGameService _service;
        private readonly IMemoryCache _cache;

        private readonly string cacheKey = "GameCache";
        public AdminGameController(IAdminGameService cs, IMemoryCache cache)
        {
            _service = cs;
            _cache = cache;
        }




        /// <summary>
        /// Get all games, with their Details
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
            
        public async Task<ActionResult<IEnumerable<GameDTO>>> Get()
        {
            if (_cache.TryGetValue(cacheKey, out IEnumerable<GameDTO> result))
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


        /// <summary>
        /// Get a game By Id,
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDTO>> GetById(int id)
        {

            var ret = await _service.GetByIdAsync(id);


            if (ret != null) { return Ok(ret); }


            else { return StatusCode(404); }
        }




        /// <summary>
        /// Creates a new Game in Database!
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GameDTO value)
        {
            if (!ModelState.IsValid) 
            {
                
                return BadRequest(ModelState);
            }
            try
            {
                await _service.AddAsync(value);

                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);

            }
            catch (Exception ex)
            {
                return StatusCode(404, $"{ex.Message}");
            }

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

                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
            }
            catch (Exception ex)
            {
                return BadRequest($" {ex.Message}");
            }

        }

        /// <summary>
        /// Delete a game by Id!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                return StatusCode(404, $" {ex.Message}");
            }
        }
        /// <summary>
        /// Get all the games by Genre, uses AdminPanelServices, and GamesRepository
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        [HttpGet("bygenre/{genreId}")]
        public async Task<ActionResult<IEnumerable<GameDTO>>> GetGamesByGenre(int genreId)
        {
            var gamesByGenre = await _service.GetGamesByGenre(genreId);

            if (gamesByGenre != null) { 
                                            return Ok(gamesByGenre);
                                       
            }


            else { return StatusCode(404); }



        }
        /// <summary>
        /// Get all the games by Platforms, uses AdminPanelService, and GamePlatformRepository
        /// </summary>
        /// <param name="platformId"></param>
        /// <returns></returns>
        [HttpGet("byplatform/{platformId}")]
        public async Task<ActionResult<IEnumerable<GameDTO>>> GetGamesByPlatform(int platformId)
        {
            var gamesByGenre = await _service.GetGamesByPlatfrom(platformId);

            if (gamesByGenre != null) { return Ok(gamesByGenre); }


            else { return StatusCode(500); }



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

            
            var jsonString = JsonSerializer.Serialize(game);

            
            var contentType = "application/octet-stream";

            
            return File(Encoding.UTF8.GetBytes(jsonString), contentType, fileName);





        }

    }
}
