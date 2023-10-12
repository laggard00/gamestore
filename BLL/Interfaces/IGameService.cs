using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGameService
    {
         Task AddAsync(GameEntity model);

         Task<IEnumerable<GameEntity>> GetAll();

    }
}
