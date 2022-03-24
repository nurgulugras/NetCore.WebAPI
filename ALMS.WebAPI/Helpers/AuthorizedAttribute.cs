using ALMS.Model;
using Microsoft.AspNetCore.Authorization;

namespace ALMS.WebAPI
{
    public class AuthorizedAttribute : AuthorizeAttribute
    {
        public AuthorizedAttribute() { }
        public AuthorizedAttribute(params RoleType[] userRoles)
        {
            foreach (var role in userRoles)
            {
                if (base.Roles != null)
                {
                    base.Roles += ", ";
                }
                base.Roles += ((int)role).ToString();
            }
        }
    }
}