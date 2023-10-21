using AutoMapper;
using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore_v2.Controllers.AdminControllers
{
    [Route("api/admin/genre")]
    [ApiController]
    public class AdminGenreController : Controller
    {

        private readonly IAdminGenreService _service;

        

        private readonly IMapper mapper;

        public AdminGenreController(IAdminGenreService cs, GameStoreDbContext _context, IMapper maps)
        {
            _service = cs;
            
            mapper = maps;
        }


        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreEntity>>> Get()
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
        public async Task<ActionResult<GenreEntity>> GetById(int id)
        {
            var ret = await _service.GetByIdAsync(id);
            if (ret != null) { return Ok(ret); }
            else { return StatusCode(404); }
        }

        [HttpPost("new")]
        public async Task<ActionResult> Post([FromBody] GenreEntity value)
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
        public async Task<ActionResult> Delete([FromBody] GenreEntity value)
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
        public async Task<ActionResult> Update([FromBody] GenreEntity value)
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
