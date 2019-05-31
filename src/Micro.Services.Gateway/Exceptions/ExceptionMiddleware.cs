using System;
using System.Net;
using System.Threading.Tasks;
using Micro.Services.Gateway.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Micro.Services.Gateway.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _log;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> log)
        {
            _log = log;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                await HandleException(HttpStatusCode.InternalServerError, httpContext);
            }
        }

        private static Task HandleException(HttpStatusCode statuscode, HttpContext context, Exception exception = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statuscode;
            var model = new ErrorModel
            {
                StatusCode = context.Response.StatusCode,
                Error = !string.IsNullOrWhiteSpace(exception?.Message)
                    ? exception.Message
                    : "An error occurred"
            };
            return context.Response.WriteAsync(JsonConvert.SerializeObject(model));
        }
    }
}
