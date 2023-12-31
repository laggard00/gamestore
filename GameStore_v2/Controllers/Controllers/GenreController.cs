using AutoMapper;
using BLL.Services;
using GameStore.BLL.DTO;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore_v2.Controllers.AdminControllers
{
    
    [ApiController]
    public class GenreController : Controller
    {

        private readonly GenreService _service;
        public GenreController(GenreService cs )
        {
            _service = cs;
            
            
        }
        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("genres")]
        public async Task<ActionResult<IEnumerable<GET_Genre>>> Get()
        { 
            var genres = await _service.GetAllAsync();
            
            if (genres != null) 
            {
                return Ok(genres);
            }
            else 
            {
                return StatusCode(404); 
            }
        }
        [HttpGet("genres/{Id}")]
        public async Task<ActionResult<GenreEntity>> GetById(Guid id)
        {
            try
            {
                var genreById = await _service.GetByIdAsync(id);

                if (genreById != null)
                {
                    return Ok(genreById);
                }

                else
                {
                    return StatusCode(404);
                }
            }
            catch(Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("genres")]
        public async Task<ActionResult> Post([FromBody] POST_GenreDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                await _service.AddAsync(value.genre);

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(404, $"{ex.Message}");
            }
        }
        [HttpDelete("genres/{Id}")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {

                await _service.DeleteAsync(Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(404, $" {ex.Message}");
            }
        }
        [HttpPut("genres")]
        public async Task<ActionResult> Update([FromBody] PUT_Genre value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateAsync(value.genre);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($" {ex.Message}");
            }

        }

        [HttpGet("games/{key}/genres")]
        public async Task<ActionResult<IEnumerable<GET_Genre>>> GetGenresByGameGuid(Guid GameId)
        {
            try
            {
                var genreByGame = await _service.GetGenresByGameGuid(GameId);
                return Ok(genreByGame);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("genres/{id}/genres")]
        public async Task<ActionResult<IEnumerable<GET_Genre>>> GetGenresByParentGenre(Guid parentGenreId)
        {
            try
            {
                var genreByParentGenre = await _service.GetGenresByParentGenre(parentGenreId);
                return Ok(genreByParentGenre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
