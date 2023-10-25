using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IPublisherRepository
    {
        Task AddAsync(PublisherEntity entity);
       PublisherEntity GetPublisherByCompanyName(string companyName);

        bool CheckIfPublisherExists(string companyName);
    }
}
