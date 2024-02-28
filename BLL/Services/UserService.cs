using Amazon.Auth.AccessControlPolicy;
using GameStore.BLL.DTO.UserViews;
using GameStore.DAL.Data;
using GameStore.DAL.Models.AuthModels;
using GameStore.DAL.Repositories.AuthRepositories;
using GameStore.WEB;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Services {
    public class UserService {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly AuthDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _hasher;
        private readonly PermissionRoleRepository _premissionRoleRepository;

        public UserService(UserManager<User> usrMngr, RoleManager<IdentityRole> roleMngr,
            AuthDbContext context, IConfiguration config, PasswordHasher<User> hasher, PermissionRoleRepository prRepository) {
            _userManager = usrMngr;
            _roleManager = roleMngr;
            _context = context;
            _configuration = config;
            _hasher = hasher;
            _premissionRoleRepository = prRepository;


        }

        public async Task AddUserAsync(RegisterDTO registerRequest) {
            var user = new User() {
                FirstName = string.Empty,
                LastName = string.Empty,
                UserName = registerRequest.user.name
            };
            user.PasswordHash = _hasher.HashPassword(user, registerRequest.password);
            try {
                await _userManager.CreateAsync(user);
            } catch (Exception ex) { var b = ex; }
            var listOfRoleNames = new List<string>();
            foreach (var roles in registerRequest.roles) {
                var role = await _roleManager.FindByIdAsync(roles);
                listOfRoleNames.Add(role.Name);
            }
            await _userManager.AddToRolesAsync(user, listOfRoleNames);
        }

        public async Task<bool> DeleteUserByIdAsync(string id) {
            var userExists = await _userManager.FindByIdAsync(id);
            if (userExists is not null) {
                await _userManager.DeleteAsync(userExists);
                return true;
            }
            return false;

        }

        public async Task<IEnumerable<GetUsersModel>> GetAllUsersAsync() {
            var allUsers = await _userManager.Users.ToListAsync();
            return allUsers.Select(x => new GetUsersModel() { id = x.Id, name = x.UserName });
        }

        public async Task<IEnumerable<GetRolesModel>> GetAllUsersRolesAsync(string id) {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Where(x => roles.Contains(x.Name));
            return allRoles.Select(x => new GetRolesModel() { name = x.Name, id = x.Id });
        }

        public async Task<GetUsersModel> GetUserByIdAsync(string id) {
            var userById = await _userManager.FindByIdAsync(id);
            var getUserModelMapped = new GetUsersModel() { id = userById.Id ?? string.Empty, name = userById.FirstName ?? string.Empty };
            return getUserModelMapped;
        }

        public async Task<string> ProcessLoginRequestAsync(UserLoginRequest loginRequest) {

            var userExists = await _userManager.FindByNameAsync(loginRequest.model.login);
            var passwordCorrect = await _userManager.CheckPasswordAsync(userExists, loginRequest.model.password);
            if (userExists is not null && passwordCorrect) {
                var tokenValue = await GenerateJWTTokenAsync(userExists);
                return tokenValue;
            } else return string.Empty;

        }

        public async Task UpdateUserAsync(UpdateUserDTO updateUserRequest) {
            var userExists = await _userManager.FindByIdAsync(updateUserRequest.user.id);
            if (userExists != null) {
                var currentRoles = await _userManager.GetRolesAsync(userExists);
                var listOfRoleNames = new List<string>();
                foreach (var roles in updateUserRequest.roles) {
                    var role = await _roleManager.FindByIdAsync(roles);
                    listOfRoleNames.Add(role.Name);
                }
                userExists.UserName = updateUserRequest.user.name;
                userExists.PasswordHash = _hasher.HashPassword(userExists, updateUserRequest.password);
                await _userManager.RemoveFromRolesAsync(userExists, currentRoles);
                await _userManager.AddToRolesAsync(userExists, listOfRoleNames);

            }
        }
        private async Task<string> GenerateJWTTokenAsync(User userExists) {
            var authClaims = new List<Claim>() {
            new Claim(ClaimTypes.Name, userExists.UserName),
            new Claim(ClaimTypes.NameIdentifier, userExists.Id),
            
            //new Claim(ClaimTypes.Role, "Admin"),
            // new Claim(JwtRegisteredClaimNames.Email, userExists.Email),
            new Claim(JwtRegisteredClaimNames.Sub, userExists.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(userExists);
            var listOfPermissions = new List<int>();
            foreach (var role in userRoles) {
                var allPermissions = await _premissionRoleRepository.GetAllAsync(x => x.RoleName == role);
                listOfPermissions.AddRange(allPermissions.Select(x => x.PermissionId));
            }

            foreach (var item in listOfPermissions) {
                authClaims.Add(new Claim("permission", ((PermissionEnum)item).ToString()));
            }

            var secret = _configuration["JWT:Secret"];
            var secret2 = _configuration["JWT:Issuer"];
            var secret3 = _configuration["JWT:Audience"];
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(15),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
