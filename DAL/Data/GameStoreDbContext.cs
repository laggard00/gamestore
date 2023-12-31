using DAL.Models;
using GameStore.DAL.Models;
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
            modelBuilder.Entity<Game>(x => x.HasIndex(y => y.Name).IsUnique());

            modelBuilder.Entity<GenreEntity>(x=> x.HasIndex(y=>y.Name).IsUnique());

            modelBuilder.Entity<Platform>(x => x.HasIndex(y => y.Type).IsUnique());

            modelBuilder.Entity<GameGenre>(x => x.HasKey(y => new { y.GenreId, y.GameId }));
         
            
            modelBuilder.Entity<GamePlatform>(x => x.HasKey(y => new { y.PlatformId, y.GameId }));
            

            modelBuilder.Entity<Publisher>(x => x.HasIndex(y => y.CompanyName).IsUnique());

            modelBuilder.Entity<OrderGame>(x => x.HasKey(y => new { y.OrderId, y.ProductId }));

            modelBuilder.Entity<PaymentMethods>(x => x.HasNoKey());

            modelBuilder.Entity<Comment>().HasOne(p => p.ParentComment)
                                          .WithMany(p => p.Children)
                                          .HasForeignKey(p => p.ParentCommentId);

        }

        public DbSet<Game> Games { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<GamePlatform> GamePlatforms { get; set; }
        public DbSet<GameGenre> GameGenre{ get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderGame> OrderGames { get; set; }
        public DbSet<PaymentMethods> PaymentMethods { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
