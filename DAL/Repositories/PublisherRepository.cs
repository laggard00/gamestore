﻿
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using DAL.Models;
using GameStore.DAL.Models;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories {
    public class PublisherRepository : IPublisherRepository {
        protected readonly GameStoreDbContext context;
        private DbSet<Publisher> dbset;
        public PublisherRepository(GameStoreDbContext _context) {
            this.context = _context;
            dbset = context.Publishers;
        }

        public async Task AddAsync(Publisher entity) {
            await context.Publishers.AddAsync(entity);
        }

        public async Task<Publisher> GetPublisherByCompanyName(string companyName) {
            var publisherByCompanyName = context.Publishers.SingleOrDefault(x => x.CompanyName == companyName);
            return publisherByCompanyName;
        }

        public bool CheckIfPublisherExists(string companyName) {
            return context.Publishers.Any(x => x.CompanyName == companyName);

        }
        public bool CheckIfPublisherExists(Guid id) {
            return context.Publishers.Any(x => x.Id == id);
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishers() {
            return dbset.ToList();
        }

        public async Task<Publisher> GetPublisherById(Guid publisherId) {
            return dbset.Find(publisherId);
        }

        public void Update(Publisher publisher) {
            context.Update(publisher);
        }

        public void DeletePublisher(Guid id) {
            var b = dbset.Find(id);
            if (b == null) { return; }
            context.Remove(b);
        }
    }
}
