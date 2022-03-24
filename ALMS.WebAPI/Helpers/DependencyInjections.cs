using ALMS.Data;
using ALMS.Data.EFCore;
using ALMS.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ALMS.WebAPI
{
    internal static class DependencyInjections
    {
        internal static void AddDependencyInjects(this IServiceCollection services)
        {

            services.TryAddTransient(typeof(IEMSRepository<>), typeof(EMSRepository<>));
            services.TryAddTransient(typeof(IEMSRepository<,>), typeof(EMSRepository<,>));

            services.TryAddScoped<IAuthenticationService, AuthenticationService>();
            services.TryAddTransient<IUserService, UserService>();
            services.TryAddScoped<IJwtService, JwtService>();
            services.TryAddScoped<IPasswordHasherService, PasswordHasherService>();
            services.TryAddScoped<IUserCredentialService, UserCredentialService>();
            services.TryAddScoped<IAppService, AppService>();

            services.TryAddScoped<IContextUserIdentity, ContextUserIdentity>();
            services.TryAddScoped<IAppProductService, AppProductService>();
            services.TryAddScoped<IAppLimitService, AppLimitService>();


            services.TryAddScoped<ICompanyService, CompanyService>();
            services.TryAddScoped<IOrganizationService, OrganizationService>();
            services.TryAddScoped<ILicenseService, LicenseService>();


            services.TryAddScoped<ILicenseProductService, LicenseProductService>();
            services.TryAddScoped<ILicenseProductRepositoryService, LicenseProductRepositoryService>();
            services.TryAddScoped<ILicenseLimitService, LicenseLimitService>();
            services.TryAddScoped<ILicenseLimitRepositoryService, LicenseLimitRepositoryService>();


            services.TryAddScoped<ISessionService, SessionService>();

            services.AddSingleton(typeof(IEntityCacheService<,>), typeof(MicrosoftCacheService<,>));


            services.TryAddScoped<ILicenseValidations, LicenseValidations>();
            services.TryAddScoped<ICompanyValidations, CompanyValidations>();

            services.AddSingleton<ISessionUpdateQueueService, SessionUpdateQueueService>();
            services.TryAddScoped<IMailService, MailService>();
        }
    }
}