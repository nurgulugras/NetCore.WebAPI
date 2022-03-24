using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;
using ALMS.Model;
using ALMS.Core;

namespace ALMS.Service
{
    public class HttpContextResolver
    {
        private HttpContext _context;

        public HttpContextResolver(HttpContext httpContext)
        {
            _context = httpContext;
        }

        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string OwinContext = "MS_OwinContext";

        #region  [ IP ]

        public string GetIPAddress()
        {
            if (_context != null)
            {
                return _context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
            }
            return null;
        }
        public string GetIPAddress(HttpRequestMessage request)
        {
            //Web-hosting
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }
            //Self-hosting
            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }
            //Owin-hosting
            if (request.Properties.ContainsKey(OwinContext))
            {
                dynamic ctx = request.Properties[OwinContext];
                if (ctx != null)
                {
                    return ctx.Request.RemoteIpAddress;
                }
            }

            return GetIPAddress();
        }
        #endregion

        #region  [ MACHINE_NAME ] 
        public string GetMachineName(string IPAddress)
        {
            string hName = "";
            try
            {
                System.Net.IPHostEntry host = new System.Net.IPHostEntry();
                host = System.Net.Dns.GetHostEntry(IPAddress);

                //Split out the host name from the FQDN
                if (host.HostName.Contains("."))
                {
                    string[] sSplit = host.HostName.Split('.');
                    hName = sSplit[0].ToString();
                }
                else
                {
                    hName = host.HostName.ToString();
                }
            }
            catch (Exception exception)
            {
                hName = "Dışardan erişim: " + exception.Message;
            }
            return hName;
        }
        #endregion

        #region  [ USERAGENT ] 

        public string GetUserAgent()
        {
            if (_context != null)
            {
                return _context.Request.Headers["User-Agent"].FirstOrDefault();
            }
            return null;
        }

        public string GetUserAgent(HttpRequestMessage request)
        {
            //Web-hosting
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserAgent;
                }
            }
            //Self-hosting
            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.UserAgent;
                }
            }
            //Owin-hosting
            if (request.Properties.ContainsKey(OwinContext))
            {
                dynamic ctx = request.Properties[OwinContext];
                if (ctx != null)
                {
                    return ctx.Request.UserAgent;
                }
            }

            return GetUserAgent();
        }
        #endregion

        #region  [ JWT Token ]
        public string GetJwtToken()
        {
            string authorization = _context.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorization))
            {
                return string.Empty;
            }

            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return authorization.Substring("Bearer ".Length).Trim();
            }

            return string.Empty;
        }
        #endregion

        public string GetOrigin()
        {
            var header = _context.Request.GetTypedHeaders();
            var uriReferer = header.Referer;
            return uriReferer == null ? string.Empty : $"{uriReferer.Scheme}://{uriReferer.Authority}";
        }
        public JWTClient GetContextUserFromContext()
        {
            return GetContextUserFromContext(_context.User);
        }
        public JWTClient GetContextUserFromContext(ClaimsPrincipal userPrincipal)
        {
            var identity = ((ClaimsIdentity)userPrincipal.Identity);
            var userData = identity.GetUserData();
            var jwtClient = JsonConvert.DeserializeObject<JWTClient>(userData);

            var ip = GetIPAddress();
            jwtClient.RequestUserInfo = new HttpRequestUserInfo(ip, GetUserAgent(), GetMachineName(ip));

            return jwtClient;
        }

        public void SetUserRole(RoleType userRole)
        {
            var principal = _context.User;
            var identity = ((ClaimsIdentity)principal.Identity);
            identity.AddClaim(new Claim(ClaimTypes.Role, ((int)userRole).ToString()));
        }
        public bool IsAuthenticated()
        {
            return _context.User.Identity.IsAuthenticated;
        }

    }
}