using System;
using System.Collections.Generic;
using System.Linq;
using ALMS.Core;
using ALMS.Model;
using AutoMapper;
using Elsa.ApprovalFlowManagement;

namespace ALMS.WebAPI
{
    /// <summary>
    /// AutoMapper Object-Map Settings
    /// </summary>
    public class DomainProfile : Profile
    {

        /// <summary>
        /// AutoMapper Object-Map Settings
        /// </summary>
        public DomainProfile()
        {
            LoadEntityTypes();
            CreateMap<User, JWTUser>()
                .ForMember(x => x.Username, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.UserType, opt => opt.MapFrom(src => src.Role))
                .ForMember(x => x.Hash, opt => opt.MapFrom(src => src.PasswordHashCode));

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<App, AppDto>()
                .ForMember(dest => dest.ApiSecretKey, opt => opt.MapFrom(src => "*****"))
                .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CreateUser.FullName));

            CreateMap<App, AppDetailDto>()
                .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CreateUser.FullName));

            CreateMap<CreationAppModel, App>();

            CreateMap<App, AppDecryptedDto>()
                .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CreateUser.FullName))
                .ForMember(dest => dest.ApiKey, opt => opt.MapFrom(src => CryptographyHelper.SymmetricDecrypt(src.ApiKey)))
                .ForMember(dest => dest.ApiSecretKey, opt => opt.MapFrom(src => CryptographyHelper.SymmetricDecrypt(src.ApiSecretKey)));

            CreateMap<AppProductDto, AppProduct>();
            CreateMap<AppProduct, AppProductDto>()
                .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CreateUser.FullName))
                .ForMember(dest => dest.AppName, opt => opt.MapFrom(src => src.App.Name));

            CreateMap<AppProduct, AppProductDto2>();

            CreateMap<AppLimit, AppLimitDto>()
                .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CreateUser.FullName))
                .ForMember(dest => dest.AppName, opt => opt.MapFrom(src => src.App.Name));
            CreateMap<AppLimitDto, AppLimit>();

            CreateMap<AppLimit, AppLimitDto2>();

            CreateMap<License, LicenseDto>()
                .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CreateUser.FullName))
                .ForMember(dest => dest.LicenseProducts, opt => opt.MapFrom(src => src.LicenseProductList.Select(x => x.AppProduct)))
                .ForMember(dest => dest.LicenseLimits, opt => opt.MapFrom(src => src.LicenseLimitList.Select(x => x.AppLimit)))
                .ForMember(dest => dest.AppName, opt => opt.MapFrom(src => src.App.Name))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.LicensePeriodTypeDesc, opt => opt.MapFrom(src => src.LicensePeriodType.ToString()));
            CreateMap<LicenseDto, License>();
            CreateMap<CreationLicenseModel, License>();

            CreateMap<License, LicenseInfo>()
                .ForMember(dest => dest.LicenseProducts, opt => opt.MapFrom(src => src.LicenseProductList.Select(x => x.AppProduct.Product)))
                .ForMember(dest => dest.LicenseLimits, opt => opt.MapFrom(src => src.LicenseLimitList))
                .ForMember(dest => dest.UserSessionLimit, opt => opt.MapFrom(src => src.SessionLimit));

            CreateMap<LicenseProduct, LicenseProductDto2>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.AppProduct.Product))
                .ForMember(dest => dest.ProductDesc, opt => opt.MapFrom(src => src.AppProduct.Description));

            CreateMap<LicenseProduct, LicenseProductDto>()
                  .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CreateUser.FullName))
                  .ForMember(dest => dest.AppProductName, opt => opt.MapFrom(src => src.AppProduct.Product))
                  .ForMember(dest => dest.LicenseName, opt => opt.MapFrom(src => src.License.Name))
                  .ForMember(dest => dest.LicenseNo, opt => opt.MapFrom(src => src.License.LicenseNo));
            CreateMap<LicenseProductDto, LicenseProduct>();

            CreateMap<LicenseLimit, LicenseLimitDto>()
                .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CreateUser.FullName))
                .ForMember(dest => dest.AppLimitName, opt => opt.MapFrom(src => src.AppLimit.LimitName))
                .ForMember(dest => dest.AppLimitDescription, opt => opt.MapFrom(src => src.AppLimit.Description));
            // .ForMember(dest => dest.LicenseNo, opt => opt.MapFrom(src => src.License.LicenseNo));
            CreateMap<LicenseLimitDto, LicenseLimit>();

            CreateMap<LicenseLimit, LicenseLimitDto2>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppLimit.LimitName));

            CreateMap<User, ApplicationUser>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(x => x.UserMail, opt => opt.MapFrom(src => src.Email));

            CreateMap<Organization, OrganizationDto>()
                       .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CreateUser.FullName));
            CreateMap<OrganizationDto, Organization>();


            CreateMap<Company, CompanyDto>()
                       .ForMember(dest => dest.CreateUserName, opt => opt.MapFrom(src => src.CreateUser.FullName))
                       .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Organization.Name));

            CreateMap<CompanyDto, Company>();

            CreateMap<Session, SessionApiDto>()
                       .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.StartDate));
        }
        private void LoadEntityTypes()
        {
            var entityTypes = GetEntityTypes();
            foreach (var type in entityTypes)
            {
                CreateMap(type, type);
            }
        }
        private List<Type> GetEntityTypes()
        {
            var type = typeof(IEntityBase);
            return typeof(User).Assembly.GetTypes()
                .Where(p => type.IsAssignableFrom(p) && p.FullName != type.FullName).ToList();
        }
    }
}