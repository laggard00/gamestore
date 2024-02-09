

using BLL.Services;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Games;
using GameStore.BLL.Validators;
using GameStore.DAL.Filters;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

using static System.Net.Mime.MediaTypeNames;

namespace GameStore.WEB.Controllers {

    [ApiController]
    public class GameController : Controller {
        private readonly GameService _service;

        private readonly IAppCache _appCache;

        public GameController(GameService cs, IAppCache appCache) {
            _service = cs;
            _appCache = appCache;
        }
        /// <summary>
        /// Get all games, with their Details
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("games")]
        ///you changed this recently carefull
        public async Task<ActionResult<GetAllGames>> Get([FromQuery] GameFilter filters) {
            var result = await _service.GetAllAsync(filters);

            // var result = await _appCache.GetOrAddAsync("gamesGet", async () => await _service.GetAllAsync(filters), DateTime.Now.AddMinutes(1));
            return Ok(result);
        }

        /// <summary>
        /// Get a game By Id,
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("games/find/{id}")]
        public async Task<ActionResult<Game>> GetById(Guid id) {
            var gameById = await _service.GetByIdAsync(id);
            if (gameById != null) { return Ok(gameById); } else { return StatusCode(404); }
        }

        /// <summary>
        /// Creates a new Game in Database!
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("games")]
        public async Task<ActionResult> Post([FromBody] AddGameRequest value) {
            if (value == null) {
                return BadRequest("The request body must not be null.");
            }

            if (ModelState.IsValid) {
                // If alias is not provided, generate it from the game name
                //add fluentvalidation here
                if (string.IsNullOrEmpty(value.Game.Key)) {
                    value.Game.Key = value.Game.Name.Replace(' ', '-').ToLower();
                }


                var existingGame = await _service.GetGameByAlias(value.Game.Key);
                if (existingGame != null) {
                    return Conflict("Game with this alias already exists!");
                }
                try {
                    await _service.AddAsync(value);
                } catch (Exception ex) { throw ex; }
                return Ok();

            } else { return BadRequest(ModelState.ValidationState); }
        }

        [HttpGet("games/{key}")]
        public async Task<ActionResult<Game>> GetGameByAlias(string key) {
            var ret = await _service.GetGameByAlias(key);
            if (ret != null) { return Ok(ret); } else { return StatusCode(404); }
        }

        [HttpPut("games")]
        public async Task<ActionResult> Update([FromBody] UpdateGameRequest value) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            await _service.UpdateAsync(value);
            return Ok();
        }

        /// <summary>
        /// Delete a game by Id!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("games/{key}")]
        public async Task<ActionResult> Remove(string key) {
            await _service.DeleteAsync(key);
            return Ok();
        }
        /// <summary>
        /// Get all the games by Genre, uses AdminPanelServices, and GamesRepository
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        [HttpGet("genres/{id}/games")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByGenre(Guid id) {
            var gamesByGenre = await _service.GetGamesByGenre(id);
            return Ok(gamesByGenre);
        }
        /// <summary>
        /// Get all the games by Platforms, uses AdminPanelService, and GamePlatformRepository
        /// </summary>
        /// <param name="platformId"></param>
        /// <returns></returns>
        [HttpGet("platforms/{id}/games")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByPlatform(Guid platformId) {
            var gamesByGenre = await _service.GetGamesByPlatform(platformId);
            if (gamesByGenre != null) { return Ok(gamesByGenre); } else { return StatusCode(500); }
        }

        [HttpGet("games/{key}/file")]
        public async Task<ActionResult> DownloadGame(string key) {
            var game = await _service.GetGameByAlias(key);
            if (game == null) {
                return NotFound($"Game with alias '{key}' not found.");
            }
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{game.Name}_{timestamp}.txt";
            var contentType = "application/octet-stream";
            return File(Encoding.UTF8.GetBytes(string.Empty), contentType, fileName);
        }

    }
}
