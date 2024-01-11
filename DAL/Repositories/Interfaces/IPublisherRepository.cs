using DAL.Models;

namespace GameStore.DAL.Repositories.RepositoryInterfaces
{
    public interface IPublisherRepository
    {
        Task AddAsync(Publisher entity);
        bool CheckIfPublisherExists(string companyName);
        bool CheckIfPublisherExists(Guid id);
        void DeletePublisher(Guid id);
        Task<IEnumerable<Publisher>> GetAllPublishers();
        Task<Publisher> GetPublisherByCompanyName(string companyName);
        Task<Publisher> GetPublisherById(Guid publisherId);
        void Update(Publisher publisher);
    }
}