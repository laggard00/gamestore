﻿using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

using static System.Net.Mime.MediaTypeNames;

namespace GameStore_v2.Controllers.AdminControllers
{
    [Route("api/admin")]
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
        [HttpGet("games")]
            
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
        [HttpGet("games/id/{id}")]
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
        [HttpPost("games/new")]
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
                    return Conflict("Game with this alias already exists!");
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

        [HttpGet("games/{gameAlias}")]
        public async Task<ActionResult<GameDTO>> GetGameByAlias(string gameAlias)
        {

            var ret = await _service.GetGameByAlias(gameAlias);


            if (ret != null) { return Ok(ret); }


            else { return StatusCode(404); }
        }

        [HttpPost("games/update")]
        public async Task<ActionResult> Update([FromBody] GameDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
          
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
        [HttpDelete("games/remove")]
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
        /// <summary>
        /// Get all the games by Genre, uses AdminPanelServices, and GamesRepository
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        [HttpGet("games/bygenre/{genreId}")]
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
        [HttpGet("games/byplatform/{platformId}")]
        public async Task<ActionResult<IEnumerable<GameDTO>>> GetGamesByPlatform(int platformId)
        {
            var gamesByGenre = await _service.GetGamesByPlatfrom(platformId);

            if (gamesByGenre != null) { return Ok(gamesByGenre); }


            else { return StatusCode(500); }



        }

        [HttpGet("games/{gameAlias}/download")]
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

            
            return File(Encoding.UTF8.GetBytes(string.Empty), contentType, fileName);;





        }

    }
}
