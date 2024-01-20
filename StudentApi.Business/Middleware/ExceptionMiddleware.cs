using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StudentApi.Business.Contracts;

namespace StudentApi.Business.Middleware
{
    public class ExceptionMiddleware
    {
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
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception {0}", ex.Message);
                _logger.LogError("Inner Exception {0}", ex.InnerException?.Message);
				_logger.LogError("Inner Exception {0}", ex.StackTrace);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new ErrorDetails
            {
                Code = context.Response.StatusCode,
                Message = exception.Message ?? exception.InnerException?.Message
            };

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }

    }
}
