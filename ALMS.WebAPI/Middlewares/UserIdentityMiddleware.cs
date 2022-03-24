using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;
using ALMS.Model;
using ALMS.Service;
using AutoMapper;
using Elsa.NNF.Data.ORM;
using Microsoft.AspNetCore.Http;

namespace ALMS.WebApi.Middlewares
{
    internal class UserIdentityMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider, IContextUserIdentity contextUserIdentity, IUserService userService, IEntityRepositoryContext entityRepositoryContext, IAppService appService)
        {
            var contextUser = default(JWTClient);
            var ignoreContinue = false;

            var contextResolver = new HttpContextResolver(context);
            var isAuthenticated = contextResolver.IsAuthenticated();
            if (isAuthenticated)
            {
                var token = contextResolver.GetJwtToken();
                contextUserIdentity.SetToken(token);

                var handler = new JwtSecurityTokenHandler();
                var tokens = handler.ReadJwtToken(token);

                contextUser = contextResolver.GetContextUserFromContext();
                if (contextUser == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("User is unauthorized!");
                    ignoreContinue = true;
                }
                else
                {
                    if (contextUser.LoginType == LoginType.UI)
                    {
                        var isValidUser = await userService.CheckUserIsValid(contextUser.UserClient.Username, contextUser.UserClient.Hash);
                        if (isValidUser == null)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            await context.Response.WriteAsync("User is unauthorized!");
                            ignoreContinue = true;
                        }
                        else
                        {
                            contextResolver.SetUserRole(contextUser.UserClient.UserType);
                            contextUserIdentity.SetContextUser(new ContextUser(isValidUser, contextUser.RequestUserInfo));

                            entityRepositoryContext.SetCurrentUser(contextUser.UserClient.Username);
                        }
                    }
                    else if (contextUser.LoginType == LoginType.API)
                    {
                        var app = await appService.GetAppByIdAsync(contextUser.AppClient.AppId);
                        if (app != null && app.IsActive && app.PasswordHashCode.Equals(contextUser.AppClient.Hash))
                        {
                            contextResolver.SetUserRole(RoleType.API);
                            contextUserIdentity.SetContextUser(new ContextUser(app, contextUser.RequestUserInfo));
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            await context.Response.WriteAsync("App is unauthorized!");
                            ignoreContinue = true;
                        }
                    }
                }
            }
            if (!ignoreContinue)
            {
                await _next(context);
            }
        }
    }
}