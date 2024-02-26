using GameStore.DAL.Data;
using GameStore.DAL.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories.AuthRepositories {
    public class PermissionRepository {
        private readonly AuthDbContext _authContext;
        public PermissionRepository(AuthDbContext authDbContext) {
            _authContext = authDbContext;

        }

        public async Task AddPermissionAsync(Permission permission) { 
           
            _authContext.Permissions.Add(permission);
        }
        public async Task<Permission> GetPermissionAsync(Expression<Func<Permission,bool>> query) {
            return _authContext.Permissions.SingleOrDefault(query);
        }
        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync(Expression<Func<Permission, bool>> query) {
            return _authContext.Permissions.Where(query);
        }


    }
}
