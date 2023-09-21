﻿

using StudentApi.Business.Middleware;

namespace WeatherService.Api.Extensions
{
    /// <summary>
    /// Class to store all extension methods to register custom middleware
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Register the exception middleware
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }

    }
}
