using GameStore.BLL.DTO.UserViews;
using GameStore.DAL.Repositories.AuthRepositories;
using GameStore.WEB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Services {
    public class RoleService {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PermissionRepository _permissionRepository;
        private readonly PermissionRoleRepository _permissionRoleRepository;
                         
        public RoleService(RoleManager<IdentityRole> roleManger, PermissionRepository permissionRepository, PermissionRoleRepository permissionRoleRepository)
        {
            _roleManager = roleManger;
            _permissionRepository = permissionRepository;
            _permissionRoleRepository = permissionRoleRepository;
        }

        public async Task AddRoleAsync(AddRoleRequest addRoleRequest) {
           var role = await _roleManager.CreateAsync(new IdentityRole() { Name = addRoleRequest.role.name });
            var roleInDb = await _roleManager.FindByNameAsync(addRoleRequest.role.name);
            foreach (var item in addRoleRequest.permissions) {
                var stringToEnum = Enum.TryParse(typeof(PermissionEnum), item, out var enumItem);
                if (stringToEnum) {
                    await _permissionRoleRepository.AddPermissionRoleAsync(addRoleRequest.role.name, (int)enumItem, roleInDb.Id );
                }
            }
            await _permissionRoleRepository.SaveChangesAsync();

        }

        public async Task<bool> DeleteRoleByIdAsync(string id) {
            var exists = await _roleManager.FindByIdAsync(id);
            if (exists != null) {
                await _roleManager.DeleteAsync(exists);
                return true;
            } else return false;

        }

        public async Task<IEnumerable<string>> GetAllPermissionAsync() {
            var allPermissions = await _permissionRepository.GetAllPermissionsAsync(_=> true);
            return allPermissions.Select(x=> x.Name).ToList();
        }

        public async Task<IEnumerable<string>> GetAllPermissionByRoleIdAsync(string id) {
            var role = await _roleManager.FindByIdAsync(id);
            var allPermissionsForARole = await _permissionRoleRepository.GetAllAsync(x => x.RoleName == role.Name);
            return allPermissionsForARole.Select(x => ((PermissionEnum)x.PermissionId).ToString());
        }

        public async Task<IEnumerable<GetRolesModel>> GetAllRolesAsync() {
            var allRoles = _roleManager.Roles.ToList();
            return allRoles.Select(x => new GetRolesModel { id = x.Id, name = x.Name });
        }

        public async Task<GetRolesModel> GetRoleByIdAsync(string id) {
            var roleById = await _roleManager.FindByIdAsync(id);
            return new GetRolesModel() { id = roleById?.Id, name = roleById?.Name };
        }

        public async Task UpdateRoleAsync(UpdateRoleRequest updateRoleRequest) {
            var role = await _roleManager.FindByIdAsync(updateRoleRequest.role.id);
            var oldRoleName = role.Name;
            await _roleManager.SetRoleNameAsync(role, updateRoleRequest.role.name);
            var allPermissionsForTheRole = await _permissionRoleRepository.GetAllAsync(x => x.RoleName == oldRoleName);
            await _permissionRoleRepository.DeleteAllAsync(allPermissionsForTheRole);
            foreach (var permission in updateRoleRequest.permissions) {
                var stringToEnum = Enum.TryParse(typeof(PermissionEnum), permission, out var enumPermission);
                if (stringToEnum) {
                    
                    await _permissionRoleRepository.AddPermissionRoleAsync(updateRoleRequest.role.name, (int)enumPermission, role.Id);
                }
            }
            await _permissionRoleRepository.SaveChangesAsync();

            
        }
    }
}
