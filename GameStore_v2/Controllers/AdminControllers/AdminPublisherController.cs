using AutoMapper;
using BLL.DTO;
using DAL.Interfaces;
using DAL.Models;
using FluentAssertions.Common;
using GameStore_DAL.Data;
using GameStore_DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.AccessControl;

namespace GameStore_v2.Controllers.AdminControllers
{
    [Route("admin/publisher")]
    [ApiController]
    public class AdminPublisherController : Controller
    {
        private IPublisherRepository publisherRepository { get; set; }
        private IMapper mapper { get; set; }

        private IUnitOfWork uow {get;set;}
        private GameStoreDbContext dbContext { get; set; }
        public AdminPublisherController(IUnitOfWork _uow, IMapper _mapper, GameStoreDbContext context)
        {
            uow = _uow;
            publisherRepository = _uow.PublisherRepository;
            mapper = _mapper;
            dbContext = context;
        }

        [HttpPost("new")]
        public async Task<ActionResult> Post([FromBody] PublisherDTO value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var doesExisit= publisherRepository.CheckIfPublisherExists(value.CompanyName);
            if (doesExisit) 
            {
                return Conflict("Publisher already exists");
            }
            try 
            {
                await publisherRepository.AddAsync(mapper.Map<PublisherEntity>(value));
                await uow.SaveAsync();
                return Ok();
            }
            catch(Exception ex) 
            { 
                return BadRequest("Error Happended while saving publisher");
            }



        }
        [HttpGet("{companyName}")]
        public async Task<ActionResult<PublisherDTO>> Get(string companyName)
        {
            try 
            {
               var publisher = publisherRepository.GetPublisherByCompanyName(companyName);
                var publisherMapped = mapper.Map<PublisherDTO>(publisher);
                return Ok(publisherMapped);
               
            }
            catch(Exception ex) { return BadRequest("error happended while getting publisher by name"); }
        }

    }
}
