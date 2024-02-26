
using DAL.Models;
using GameStore.DAL.Models;
using GameStore.DAL.Models.AuthModels;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Data {
    public class AuthDbContext : IdentityDbContext<User>{

        public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options)
        {
            
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<Permission>().HasKey(x => x.Id);
            ///modelBuilder.Entity<Role>().HasMany<Permission>().WithMany().UsingEntity<PermissionRole>();
            modelBuilder.Entity<PermissionRole>().HasKey(x => new { x.RoleName, x.PermissionId });
            modelBuilder.Entity<PermissionRole>().HasOne<IdentityRole>().WithMany().HasForeignKey(x=> x.RoleId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PermissionRole>().HasOne<Permission>().WithMany().HasForeignKey(x=> x.PermissionId).OnDelete(DeleteBehavior.Cascade);
           

        }
        
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionRole> PermissionRole { get; set; }

    }
}
