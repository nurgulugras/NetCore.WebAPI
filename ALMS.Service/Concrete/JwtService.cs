using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ALMS.Core;
using ALMS.Model;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ALMS.Service
{
    public class JwtService : IJwtService
    {
        private readonly IMapper _mapper;

        public JwtService(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public void Dispose() { }

        public string GenerateToken(JWTClient jwtClient, RoleType loginRoleType)
        {
            var jwtUserString = JsonConvert.SerializeObject(jwtClient, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),

                },
                Formatting = Formatting.None
            });

            var parameters = new Claim[] {
                new Claim (ClaimTypes.UserData, jwtUserString),
                new Claim (ClaimTypes.Role, ((int) loginRoleType).ToString ())
            };
            return GetToken(parameters);
        }
        private string GetToken(Claim[] parameters)
        {
            var jwtConfig = GlobalFields.ApiConfig.JwtTokenConfig;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((string)Convert.ChangeType(jwtConfig.IssuerSigningKey, typeof(string))));
            var tokenLifeTime = (double)Convert.ChangeType(jwtConfig.TokenLifeTime, typeof(double));

            var token = new JwtSecurityToken(
                issuer: jwtConfig.ValidIssuer,
                audience: jwtConfig.ValidAudience,
                claims: parameters,
                notBefore: DateTime.UtcNow,
                expires: jwtConfig.ValidateLifetime ? (DateTime?)DateTime.UtcNow.AddMinutes(tokenLifeTime) : null,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}