using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore_DAL.Interfaces
{
    public interface IGamesRepository : IRepository<GameEntity>
    {

        Task<IEnumerable<GameEntity>> GetGamesByGenre(int genreId);
        Task<GameEntity> GetGameByAlias(string alias);
    }
}
