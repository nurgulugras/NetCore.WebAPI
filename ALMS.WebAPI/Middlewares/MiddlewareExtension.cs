using ALMS.WebAPI.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ALMS.WebApi.Middlewares
{
    internal static class MiddlewareExtension
    {
        public static void UseUserIdentityAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserIdentityMiddleware>();
        }
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}