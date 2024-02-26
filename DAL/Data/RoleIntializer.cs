
using GameStore.DAL.Models.AuthModels;
using GameStore.DAL.Repositories.AuthRepositories;
using GameStore.WEB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Data {
    public class RoleIntializer {

        public static async Task AddNewRoles(IApplicationBuilder appBuilder) {
            using (var servicescope = appBuilder.ApplicationServices.CreateScope()) {
                var roleManager = servicescope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync("Manager")) {
                    await roleManager.CreateAsync(new IdentityRole("Manager"));
                }
                if (!await roleManager.RoleExistsAsync("Administrator")) {
                    await roleManager.CreateAsync(new IdentityRole("Administrator"));
                }
                if (!await roleManager.RoleExistsAsync("User")) {
                    await roleManager.CreateAsync(new IdentityRole("User"));

                }
                if (!await roleManager.RoleExistsAsync("Guest")) {
                    await roleManager.CreateAsync(new IdentityRole("Guest"));
                }
                if (!await roleManager.RoleExistsAsync("Moderator")) {
                    await roleManager.CreateAsync(new IdentityRole("Moderator"));
                }


                AuthDbContext context = servicescope.ServiceProvider.GetRequiredService<AuthDbContext>();

                foreach (var item in Enum.GetValues(typeof(PermissionEnum))) {
                    var permission = context.Permissions.FirstOrDefault(x => x.Name == item.ToString());
                    if (permission == null) {
                        context.Permissions.Add(new Permission() { Id = (int)item, Name = item.ToString() });
                    }
                    context.SaveChanges();
                }
                
                var administratorRoleId = await roleManager.FindByNameAsync("Administrator");
                var arrayOfAdministratorPermissions = new[] { PermissionEnum.ManageRole, PermissionEnum.ManageUser };

                foreach(var permission in arrayOfAdministratorPermissions) {
                    var checkIfExists = context.PermissionRole.SingleOrDefault(x => x.RoleName == administratorRoleId.Name && x.PermissionId == (int)permission);
                    if (checkIfExists == null) {

                        context.Add(new PermissionRole() { RoleName= administratorRoleId.Name,PermissionId=(int)permission, RoleId = administratorRoleId.Id});
                    }
                }
                context.SaveChanges();

                var managerRoleId = await roleManager.FindByNameAsync("Manager");
                var arrayOfManagerPermissions = new[] { 
                    PermissionEnum.AddGame, PermissionEnum.UpdateGame, PermissionEnum.DeleteGame,
                    PermissionEnum.AddGenre, PermissionEnum.UpdateGenre, PermissionEnum.DeleteGenre,
                    PermissionEnum.AddPublisher, PermissionEnum.UpdatePublisher, PermissionEnum.DeletePublisher,
                    PermissionEnum.AddPlatform, PermissionEnum.UpdatePlatform, PermissionEnum.DeletePlatform,
                    PermissionEnum.ViewOrderHistory, PermissionEnum.ChangeOrderStatus};
                foreach (var permission in arrayOfManagerPermissions) {
                    var checkIfExists = context.PermissionRole.SingleOrDefault(x => x.RoleName == managerRoleId.Name && x.PermissionId == (int)permission);
                    if (checkIfExists == null) {

                        context.Add(new PermissionRole() { RoleName = managerRoleId.Name, PermissionId = (int)permission,RoleId=managerRoleId.Id });
                    }
                }
                context.SaveChanges();

                var moderatorRoleId = await roleManager.FindByNameAsync("Moderator");
                var arrayOfModeratorPermissions = new []{ PermissionEnum.BanUser, PermissionEnum.ManageGameComments };
                foreach (var permission in arrayOfModeratorPermissions) {
                    var checkIfExists = context.PermissionRole.SingleOrDefault(x => x.RoleName == moderatorRoleId.Name && x.PermissionId == (int)permission);
                    if (checkIfExists == null) {

                        context.Add(new PermissionRole() { RoleName = moderatorRoleId.Name, PermissionId = (int)permission, RoleId = moderatorRoleId.Id });
                    }
                }
                context.SaveChanges();

                var userRoleId = await roleManager.FindByNameAsync("User");
                var arrayOfUserPermissions = new[] {PermissionEnum.Comment, PermissionEnum.ViewGames };
                foreach(var permission in arrayOfUserPermissions) {
                    var checkIfExists = context.PermissionRole.SingleOrDefault(x=> x.RoleName == userRoleId.Name && x.PermissionId == (int)permission);
                    if(checkIfExists == null) {
                        context.Add(new PermissionRole() { RoleName = userRoleId.Name, PermissionId = (int)permission, RoleId= userRoleId.Id });
                    }
                }
                context.SaveChanges();

                var guestRoleId = await roleManager.FindByNameAsync("Guest");
                var arrayOfGuestPermissions = new[] { PermissionEnum.ReadOnlyAccess };
                foreach (var permission in arrayOfGuestPermissions) {
                    var checkIfExists = context.PermissionRole.SingleOrDefault(x => x.RoleName == guestRoleId.Name && x.PermissionId == (int)permission);
                    if (checkIfExists == null) {
                        context.Add(new PermissionRole() { RoleName = guestRoleId.Name, PermissionId = (int)permission, RoleId = guestRoleId.Id });
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
