using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GameStore_DAL.Data
{
    public class GameStoreDbContext :DbContext
    {
        public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options):base(options)
        {
            
        }

       //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       //{
       //    var connectionString = "Server = localhost;Database=NETMYSQL;User=root;Password=1234;";
       //    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
       //
       //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameEntity>().HasData(new GameEntity { Id = 1, Description = "simple descrption", GameAlias = "game alias", Name = "name" });
                                                    
            
        }

        public DbSet<GameEntity> Games { get; set; }
       // public DbSet<Genre> Genres { get; set; }
       // public DbSet<Platform> Platforms { get; set; }
    }
}
