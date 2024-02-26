using Microsoft.AspNetCore.Authorization;

namespace GameStore.WEB.AuthUtilities
{
    public class PermissionRequirment : IAuthorizationRequirement
    {

        public string Permission { get; set; }
        public PermissionRequirment(string permission)
        {
            Permission = permission;
        }
    }
}
