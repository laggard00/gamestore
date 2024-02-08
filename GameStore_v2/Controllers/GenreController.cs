using AutoMapper;
using BLL.Services;
using GameStore.BLL.DTO.Genres;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers {

    [ApiController]
    public class GenreController : Controller {

        private readonly GenreService _service;
        public GenreController(GenreService cs) {
            _service = cs;
        }
        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("genres")]
        public async Task<ActionResult<IEnumerable<GetGenreRequest>>> Get() {
            var genres = await _service.GetAllAsync();
            if (genres != null) {
                return Ok(genres);
            } else {
                return StatusCode(404);
            }
        }
        [HttpGet("genres/{Id}")]
        public async Task<ActionResult<GenreEntity>> GetById(Guid id) {
            var genreById = await _service.GetByIdAsync(id);
            if (genreById != null) {
                return Ok(genreById);
            } else {
                return StatusCode(404);
            }
        }

        [HttpPost("genres")]
        public async Task<ActionResult> Post([FromBody] AddGenreRequest value) {
            var b = ModelState.IsValid;
            await _service.AddAsync(value.genre);
            return Ok();
        }
        [HttpDelete("genres/{Id}")]
        public async Task<ActionResult> Delete(Guid Id) {
            await _service.DeleteAsync(Id);
            return Ok();
        }
        [HttpPut("genres")]
        public async Task<ActionResult> Update([FromBody] UpdateGenreRequest value) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            await _service.UpdateAsync(value.genre);
            return NoContent();
        }

        [HttpGet("games/{key}/genres")]
        public async Task<ActionResult<IEnumerable<GetGenreRequest>>> GetGenresByGameGuid(Guid GameId) {
            var genreByGame = await _service.GetGenresByGameGuid(GameId);
            return Ok(genreByGame);
        }

        [HttpGet("genres/{id}/genres")]
        public async Task<ActionResult<IEnumerable<GetGenreRequest>>> GetGenresByParentGenre(Guid parentGenreId) {
            var genreByParentGenre = await _service.GetGenresByParentGenre(parentGenreId);
            return Ok(genreByParentGenre);
        }
    }
}
