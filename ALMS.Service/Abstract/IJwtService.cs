using ALMS.Model;

namespace ALMS.Service
{
    public interface IJwtService : IServiceBase
    {
        string GenerateToken(JWTClient jwtClient, RoleType loginRoleType);
    }
}