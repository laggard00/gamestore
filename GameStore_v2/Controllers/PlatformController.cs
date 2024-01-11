
using BLL.Services;
using DAL.Models;
using GameStore.BLL.DTO;
using GameStore.BLL.Services;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
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
        public async Task<ActionResult<IEnumerable<Publisher>>> GetAllPublishers()
        {

            var publisher = await publisherService.GetAllPublishers();
            return Ok(publisher);

        }
        [Route("platforms")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Platform>>> Get()
        {
            var platforms = await _service.GetAllAsync();

            if (platforms != null) { return Ok(platforms); }
            else { return StatusCode(404); }

            throw new Exception();

        }
        [HttpGet("platforms/{Id}")]
        public async Task<ActionResult<Platform>> GetById(Guid id)
        {
            var platformById = await _service.GetByIdAsync(id);
            if (platformById != null) { return Ok(platformById); }
            else { return StatusCode(404); }
        }

        [HttpPost("platforms")]
        public async Task<ActionResult> Post([FromBody] AddPlatformRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.AddAsync(value.platform);

            return NoContent();
        }
        [HttpGet("/games/{key}/platforms")]
        public async Task<ActionResult<IEnumerable<Platform>>> GetPlatformByGameGuid(Guid GameId)
        {

            var gamesByGenre = await _service.GetPlatformByGameGuid(GameId);
            return Ok(gamesByGenre);
        }
        [HttpDelete("platforms/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.DeleteAsync(id);

            return NoContent();
        }
        [HttpPut("platforms")]
        public async Task<ActionResult> Put([FromBody] UpdatePlatformRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.UpdateAsync(value.platform);
            return NoContent();

        }
    }
}
