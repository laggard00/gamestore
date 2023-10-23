using DAL.Models;
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

            modelBuilder.Entity<PlatformEntity>().Ignore(x => x.Type);

            modelBuilder.Entity<GenreEntity>()
                        .HasOne(genre => genre.ParentGenre)
                        .WithMany(parent => parent.SubGenre)
                        .HasForeignKey(genre => genre.ParentGenreId);
                        


            modelBuilder.Entity<GameEntity>()
                .HasIndex(x => x.Name)
                .IsUnique(true);


            modelBuilder.Entity<GameEntity>()
                .HasIndex(x=> x.GameAlias)
                .IsUnique(true);


            modelBuilder.Entity<GamePlatform>()
                        .HasKey(gp => new { gp.GameId, gp.PlatformId });



            modelBuilder.Entity<GamePlatform>()
                .HasOne(gp => gp.Game)
                .WithMany(g => g.GamePlatforms)
                .HasForeignKey(gp => gp.GameId);


            modelBuilder.Entity<GamePlatform>()
                .HasOne(gp => gp.Platform)
                .WithMany(p => p.GamePlatforms)
                .HasForeignKey(gp => gp.PlatformId);

            modelBuilder.Entity<GameGenre>()
            .HasKey(gg => new { gg.GameId, gg.GenreId });

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg => gg.Games)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GameId);

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg => gg.Genre)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GenreId);


        }

        public DbSet<GameEntity> Games { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<PlatformEntity> Platforms { get; set; }

        public DbSet<GamePlatform> GamePlatforms { get; set; }
        public DbSet<GameGenre> GameGenre{ get; set; }
    }
}
