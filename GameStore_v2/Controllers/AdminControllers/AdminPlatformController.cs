using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using Microsoft.AspNetCore.Mvc;

namespace GameStore_v2.Controllers.AdminControllers
{
    [Route("api/adminplatform/")]
    [ApiController]
    public class AdminPlatformController : Controller
    {
        private readonly IAdminPlatformService _service;

        public AdminPlatformController(IAdminPlatformService cs)
        {
            _service = cs;
        }
        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformDTO>>> Get()
        {
            var genres = await _service.GetAllAsync();

            if (genres != null) { return Ok(genres); }
            else { return StatusCode(404); }

            throw new Exception();


        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformDTO>> GetById(int id)
        {
            var ret = await _service.GetByIdAsync(id);
            if (ret != null) { return Ok(ret); }
            else { return StatusCode(404); }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PlatformDTO value)
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
        public async Task<ActionResult> Put([FromBody] PlatformDTO value, int id)
        {

            if (id != value.Id)
            { 
                return BadRequest();
            }

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
