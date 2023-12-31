
using BLL.Services;
using DAL.Models;
using GameStore.BLL.DTO;
using GameStore.BLL.Services;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore_v2.Controllers.AdminControllers
{
    
    [ApiController]
    public class PlatformController : Controller
    {
        private readonly PlatformService _service;
        private readonly PublisherService publisherService;

        public PlatformController(PlatformService cs, PublisherService ser)
        {
            _service = cs;
            publisherService = ser;
        }
        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("api/publishers")]
        public async Task<ActionResult<IEnumerable<DAL.Models.Publisher>>> GetAllPublishers()
        {
            try
            {
                var publisher = await publisherService.GetAllPublishers();
                return Ok(publisher);

            }
            catch (Exception ex) { return BadRequest("error happended while getting publisher by name"); }
        }
        [Route("platforms")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameStore_DAL.Models.Platform>>> Get()
        {
            var platforms = await _service.GetAllAsync();

            if (platforms != null) { return Ok(platforms); }
            else { return StatusCode(404); }

            throw new Exception();


        }
        [HttpGet("platforms/{Id}")]
        public async Task<ActionResult<GameStore_DAL.Models.Platform>> GetById(Guid id)
        {
            var platformById = await _service.GetByIdAsync(id);
            if (platformById != null) { return Ok(platformById); }
            else { return StatusCode(404); }
        }

        [HttpPost("platforms")]
        public async Task<ActionResult> Post([FromBody] POST_Platform value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {

                await _service.AddAsync(value.platform);

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }
        [HttpGet("/games/{key}/platforms")]
        public async Task<ActionResult<IEnumerable<GameStore_DAL.Models.Platform>>> GetPlatformByGameGuid(Guid GameId)
        {
            try
            {
                var gamesByGenre = await _service.GetPlatformByGameGuid(GameId);
                return Ok(gamesByGenre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("platforms/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
        [HttpPut("platforms")]
        public async Task<ActionResult> Put([FromBody] PUT_Platform value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _service.UpdateAsync(value.platform);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($" {ex.Message}");
            }

        }
    }
}
