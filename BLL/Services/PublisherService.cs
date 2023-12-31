using DAL.Models;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Services
{
    public class PublisherService
    {
        private IUnitOfWork uow { get; set; }
        public PublisherService(IUnitOfWork unitofwork)
        {
            uow = unitofwork;
        }
        public async Task<bool> CheckIfPublisherExists(string name)
        {
            return uow.PublisherRepository.CheckIfPublisherExists(name);
        }
        public async Task AddAsync(Publisher publisher)
        {
            uow.PublisherRepository.AddAsync(publisher);
            await uow.SaveAsync();
        }

        public async Task<Publisher> GetPublisherByCompanyName(string name)
        {
            return await uow.PublisherRepository.GetPublisherByCompanyName(name);
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishers()
        {
            return await uow.PublisherRepository.GetAllPublishers();
        }
        public async Task<Publisher> GetPublisherByGameKey(string key) 
        {
            var publisherId = await uow.GamesRepository.GetGameByAlias(key);
            return await uow.PublisherRepository.GetPublisherById(publisherId.PublisherId);
        }

        public async Task Update(Publisher publisher)
        {
            uow.PublisherRepository.Update(publisher);
            await uow.SaveAsync();
        }

        public async Task DeletePublisher(Guid id)
        {
            uow.PublisherRepository.DeletePublisher(id);
            await uow.SaveAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByPublisherName(string name)
        {
            var publisher =await  GetPublisherByCompanyName(name);
            return await uow.GamesRepository.GetAllGamesWithSamePublisher(publisher.Id);

        }
    }
}
