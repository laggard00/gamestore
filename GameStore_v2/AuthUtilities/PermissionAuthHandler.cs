using GameStore.DAL.Models.AuthModels;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace GameStore.WEB.AuthUtilities
{
    public class PermissionAuthHandler : AuthorizationHandler<PermissionRequirment>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirment requirement)
        {
            
            var customClaim = context.User.Claims.Where(x => x.Type == "permission");
            if (customClaim.Any(x => x.Value == requirement?.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
