using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace NewsTrack.WebApi
{
    internal class ExceptionMiddleware
    {
        private const string Message = "An unhandled exception has been thrown";

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await WriteResponse(httpContext, ex).ConfigureAwait(false);
            }
        }

        private async Task WriteResponse(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "text/plain";
            httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");

            _logger.LogError(ex, Message);

            await httpContext.Response.WriteAsync(Message).ConfigureAwait(false);
        }
    }
}