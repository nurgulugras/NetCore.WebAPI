using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ALMS.Core;
using ALMS.Model;

namespace ALMS.WebAPI.Middlewares
{
    internal class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = new ApiResponseParameter<bool>();
            var internalMessage = exception.TryResolveExceptionMessage(); //exception.GetInnerException ();
            var httpStatusCode = (exception is UnauthorizedException) ? HttpStatusCode.Unauthorized : HttpStatusCode.OK;

            result.HttpStatusCode = httpStatusCode;
            result.Message = internalMessage;

            result.ResultType = ResultType.Fail;

            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result.ToString());
        }
    }
}