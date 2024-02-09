

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using nosqlconverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace nosqlconverter.DBContexts
{
    public class GameStoreDbContext : DbContext
    {


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server = localhost;Database=NETMYSQL;User=root;Password=1234;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(x => x.HasIndex(y => y.Name).IsUnique());

            modelBuilder.Entity<Game>().HasOne<Publisher>().WithMany().HasForeignKey(x => x.PublisherId);

            modelBuilder.Entity<GenreEntity>(x => x.HasIndex(y => y.Name).IsUnique());


            modelBuilder.Entity<Publisher>(x => x.HasIndex(y => y.CompanyName).IsUnique());

            

            

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

        public DbSet<Order> Orders { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameGenre> GameGenre { get; set; }

        public DbSet<OrderGame> OrderGames { get; set; }

    }
}
