using BLL.Interfaces;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class GameService: IGameService
    {
        public IUnitOfWork uow { get; set; }
        public IGamesRepository repository { get; set; }

        public GameService(IUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            
            repository = unitOfWork.GamesRepository;
            
        }

        public async Task AddAsync(GameEntity model)
        {
           
            await repository.AddAsync(model);
            await uow.SaveAsync();

        }
        public async Task<IEnumerable<GameEntity>> GetAll()
        {

            return await repository.GetAllAsync();

        }

    }
}
