using DAL.Models;
using GameStore_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IGamePlatformRepository 
    {

        Task<List<GamePlatform>> GetGamePlatformByPlatfromId(int platformId);
    }
}
