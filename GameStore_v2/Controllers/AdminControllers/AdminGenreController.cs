using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using Microsoft.AspNetCore.Mvc;

namespace GameStore_v2.Controllers.AdminControllers
{
    [Route("api/admingenre/")]
    [ApiController]
    public class AdminGenreController : Controller
    {

        private readonly IAdminGenreService _service;

        public AdminGenreController(IAdminGenreService cs)
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

            if (genres != null) { return Ok(genres); }
            else { return StatusCode(404); }

            throw new Exception();


        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> GetById(int id)
        {
            var ret = await _service.GetByIdAsync(id);
            if (ret != null) { return Ok(ret); }
            else { return StatusCode(404); }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreDTO value)
        {

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
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] GenreDTO value, int id)
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
