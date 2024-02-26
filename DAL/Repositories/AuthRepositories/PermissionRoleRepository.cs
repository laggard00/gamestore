using GameStore.DAL.Data;
using GameStore.DAL.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories.AuthRepositories {
    public class PermissionRoleRepository {

        private readonly AuthDbContext _authContext;
        public PermissionRoleRepository(AuthDbContext authDbContext)
        {
            _authContext= authDbContext;
        }

        public async Task AddPermissionRoleAsync(string RoleName, int PermissionId, string roleId) {

            _authContext.Add(new PermissionRole() { PermissionId = PermissionId, RoleName = RoleName, RoleId= roleId });
        }

        public async Task<PermissionRole> GetPermissionAsync(Expression<Func<PermissionRole, bool>> query) {
            return _authContext.PermissionRole.SingleOrDefault(query);
        }
        public async Task<IEnumerable<PermissionRole>> GetAllAsync(Expression<Func<PermissionRole, bool>> query) {
            return _authContext.PermissionRole.Where(query);
        }

        public async Task DeleteAllAsync(IEnumerable<PermissionRole> roles) {
            _authContext.PermissionRole.RemoveRange(roles);
        }

        public async Task SaveChangesAsync() { 
            await _authContext.SaveChangesAsync();
        }
    }
}
