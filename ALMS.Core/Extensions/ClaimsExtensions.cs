using System.Linq;
using System.Security.Claims;

namespace ALMS.Core
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsIdentity claimsIdentity)
        {

            return claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).SingleOrDefault();
        }
        public static string GetRole(this ClaimsIdentity claimsIdentity)
        {
            return claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).SingleOrDefault();
        }
        public static string GetVerificationCode(this ClaimsIdentity claimsIdentity)
        {
            return claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.Hash).Select(x => x.Value).SingleOrDefault();
        }
        public static string GetUserData(this ClaimsIdentity claimsIdentity)
        {
            return claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.UserData).Select(x => x.Value).SingleOrDefault();
        }
    }
}