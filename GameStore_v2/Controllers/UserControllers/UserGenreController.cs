using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GameStore_v2.Controllers.UserControllers
{

    [Route("api/genres")]
    [ApiController]
    public class UserGenreController : Controller
    {
        /// <summary>
        /// temporary using adminGenreService.
        /// not sure if I should make userGenreService to abstract data differently? otherwise functionality is the same.
        /// </summary>
        private readonly IAdminGenreService _service;

      

        public UserGenreController(IAdminGenreService cs)
        {
            _service = cs;
            
        }


        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> Get()
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


        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> GetById(int id)
        {
            var ret = await _service.GetByIdAsync(id);
            if (ret != null) { return Ok(ret); }
            else { return StatusCode(404); }
        }


        [HttpPost("new")]
        public async Task<ActionResult> Post([FromBody] GenreDTO value)
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
        [HttpDelete("remove")]
        public async Task<ActionResult> Delete([FromBody] GenreDTO value)
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
        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] GenreDTO value)
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
    }

}
