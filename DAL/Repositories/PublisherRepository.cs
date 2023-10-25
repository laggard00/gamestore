using DAL.Interfaces;
using DAL.Models;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class PublisherRepository:IPublisherRepository
    {
        protected readonly GameStoreDbContext context;
        
        public PublisherRepository(GameStoreDbContext _context)
        {
            this.context = _context;
            
        }

        public async Task AddAsync(PublisherEntity entity)
        {
            try
            {
                await context.Publishers.AddAsync(entity);
                
            }
            catch (Exception ex) 
            {
                throw new Exception("Adding publisher failed ");
                    }
            
        }

        public  PublisherEntity GetPublisherByCompanyName(string companyName)
        {
            
               var b = context.Publishers.SingleOrDefault(x => x.CompanyName == companyName);
            
               return b;
            
            
            
        }

        public bool CheckIfPublisherExists(string companyName)
        {
            var b= context.Publishers.SingleOrDefault(x => x.CompanyName == companyName);
            if (b == null)
            {
                return false;
            }
            return true;
        }
    }
}
