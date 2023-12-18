using BLL.DTO;
using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.IAdminINTERFACES
{
    public interface IAdminGameService : ICrud<GameDTO>
    {
        Task<IEnumerable<GameDTO>> GetGamesByGenre(int genreId);

        Task<IEnumerable<GameDTO>> GetGamesByPlatform(int platformId);

        Task<GameDTO> GetGameByAlias(string alias);


    }
}
