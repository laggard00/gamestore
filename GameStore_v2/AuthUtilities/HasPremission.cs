using Microsoft.AspNetCore.Authorization;

namespace GameStore.WEB.AuthUtilities
{
    public class HasPremission : AuthorizeAttribute
    {

        public HasPremission(PermissionEnum permission) : base(policy: permission.ToString())
        {
        }

    }
}
