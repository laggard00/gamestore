﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore_DAL.Interfaces
{
    public interface IUnitOfWork 
    {
        IGamesRepository GamesRepository { get; }

        Task SaveAsync();
    }
}
