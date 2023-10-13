using AutoMapper;
using BLL.DTO;
using GameStore_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService :ICrud<GameDTO>
    {

        Task<string> GetGameDescritpionByAlias(string alias);
        
    }
}
