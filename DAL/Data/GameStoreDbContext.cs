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

            modelBuilder.Entity<Game>().HasOne<Publisher>().WithMany().HasForeignKey(x => x.PublisherId);

            modelBuilder.Entity<GenreEntity>(x=> x.HasIndex(y=>y.Name).IsUnique());

            modelBuilder.Entity<Platform>(x => x.HasIndex(y => y.Type).IsUnique());

            modelBuilder.Entity<Publisher>(x => x.HasIndex(y => y.CompanyName).IsUnique());

            modelBuilder.Entity<PaymentMethods>(x => x.HasNoKey());

            modelBuilder.Entity<Comment>().HasOne(p => p.ParentComment)
                                          .WithMany(p => p.Children)
                                          .HasForeignKey(p => p.ParentCommentId)
                                          .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameGenre>()
                        .HasKey(gg => new { gg.GameId, gg.GenreId });

            modelBuilder.Entity<GameGenre>()
                .HasOne<Game>()
                .WithMany()
                .HasForeignKey(gg => gg.GameId);
                
            modelBuilder.Entity<GameGenre>()
                .HasOne<GenreEntity>()
                .WithMany()
                .HasForeignKey(gg => gg.GenreId);

            modelBuilder.Entity<GamePlatform>()
                .HasKey(gp => new {gp.GameId, gp.PlatformId });

            modelBuilder.Entity<GamePlatform>()
                 .HasOne<Platform>()
                 .WithMany()
                 .HasForeignKey(gp => gp.PlatformId);

            modelBuilder.Entity<GamePlatform>()
                 .HasOne<Game>()
                 .WithMany()
                 .HasForeignKey(gp => gp.GameId);

            modelBuilder.Entity<OrderGame>()
                .HasKey(og => new { og.OrderId, og.ProductId });
           
            modelBuilder.Entity<OrderGame>()
                .HasOne<Order>()
                .WithMany() 
                .HasForeignKey(og => og.OrderId);
           
            modelBuilder.Entity<OrderGame>()
                .HasOne<Game>()
                .WithMany() 
                .HasForeignKey(og => og.ProductId);



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
