using AutoMapper;

using DAL.Models;
using DAL.Repositories;
using FluentAssertions.Common;
using GameStore.BLL.DTO;
using GameStore.BLL.Services;
using GameStore_DAL.Data;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MySqlConnector;
using System.Security.AccessControl;

namespace GameStore_v2.Controllers.AdminControllers
{
    
    [ApiController]
    public class PublisherController : Controller
    {
        private PublisherService service;
        private IUnitOfWork uow;
        private IMapper mapper;
        public PublisherController(PublisherService service, IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.service = service;
            this.uow = unitOfWork;
            this.mapper = mapper;
        }
       
        [HttpPost("publishers")]
        public async Task<ActionResult> Post([FromBody] POST_Publisher value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var doesExisit= await service.CheckIfPublisherExists(value.publisher.companyName);
            if (doesExisit) 
            {
                return Conflict("Publisher already exists");
            }
            try 
            {
                await service.AddAsync(mapper.Map<DAL.Models.Publisher>(value.publisher));
                return Ok();
            }
            catch(Exception ex) 
            { 
                return BadRequest("Error Happended while saving publisher");
            }



        }
        [HttpGet("publishers/{companyName}")]
        public async Task<ActionResult<DAL.Models.Publisher>> Get(string companyName)
        {
            try 
            {
               var publisher = uow.PublisherRepository.GetPublisherByCompanyName(companyName);
               return Ok(publisher);
               
            }
            catch(Exception ex) { return BadRequest("error happended while getting publisher by name"); }
        }
        
        [HttpGet("publishers")]
        public async Task<ActionResult<DAL.Models.Publisher>> GetAllPublishers()
        {
            try
            {
                var publisher = await service.GetAllPublishers();
                return Ok(publisher);

            }
            catch (Exception ex) { return BadRequest("error happended while getting publisher by name"); }
        }

        [HttpGet("games/{key}/publisher")]
        public async Task<ActionResult<DAL.Models.Publisher>> GetPublisherByGameKey(string key)
        {
            try
            {
                var publisherByGameGuid = await service.GetPublisherByGameKey(key);
                return Ok(publisherByGameGuid);

            }
            catch (Exception ex) { return BadRequest("error happended while getting publisher by name"); }
        }
        
        [HttpPut("publishers")]
        public async Task<ActionResult> Put([FromBody] PUT_Publisher value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                await service.Update(value.publisher);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Error Happended while saving publisher");
            }

        }
        [HttpDelete("publishers/{id}")]
        public async Task<ActionResult> RemovePublisher(Guid id)
        {
            try
            {
                service.DeletePublisher(id);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(); }
        }
        [HttpGet("publishers/{companyName}/games")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByPublisherName(string name)
        {
            try
            {
                var allGames = await service.GetGamesByPublisherName(name);
                return Ok(allGames);
            }
            catch (Exception ex) 
            {
                return BadRequest();
            }
        } 
    }
}
