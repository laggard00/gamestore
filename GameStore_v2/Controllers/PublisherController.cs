using AutoMapper;

using DAL.Models;
using DAL.Repositories;
using FluentAssertions.Common;
using GameStore.BLL.DTO;
using GameStore.BLL.Services;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MySqlConnector;
using System.Security.AccessControl;

namespace GameStore.WEB.Controllers
{

    [ApiController]
    public class PublisherController : Controller
    {
        private PublisherService service;
        private IMapper mapper;
        public PublisherController(PublisherService service, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpPost("publishers")]
        public async Task<ActionResult> Post([FromBody] AddPublisherRequest value)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            var doesExisit = await service.CheckIfPublisherExists(value.publisher.companyName);
            if (doesExisit)
            {
                return Conflict("Publisher already exists");
            }

            await service.AddAsync(mapper.Map<Publisher>(value.publisher));
            return Ok();
        }
        [HttpGet("publishers/{companyName}")]
        public async Task<ActionResult<Publisher>> Get(string companyName)
        {
            var publisher = await service.GetPublisherByCompanyName(companyName);
            return Ok(publisher);
        }

        [HttpGet("publishers")]
        public async Task<ActionResult<Publisher>> GetAllPublishers()
        {
            var publisher = await service.GetAllPublishers();
            return Ok(publisher);
        }

        [HttpGet("games/{key}/publisher")]
        public async Task<ActionResult<Publisher>> GetPublisherByGameKey(string key)
        {
            var publisherByGameGuid = await service.GetPublisherByGameKey(key);
            return Ok(publisherByGameGuid);
        }

        [HttpPut("publishers")]
        public async Task<ActionResult> Put([FromBody] object value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // await service.Update(value.publisher);
            return Ok();

        }
        [HttpDelete("publishers/{id}")]
        public async Task<ActionResult> RemovePublisher(Guid id)
        {
            service.DeletePublisher(id);
            return Ok();

        }
        [HttpGet("publishers/{name}/games")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByPublisherName(string name)
        {
            var allGames = await service.GetGamesByPublisherName(name);
            return Ok(allGames);
        }
    }
}
