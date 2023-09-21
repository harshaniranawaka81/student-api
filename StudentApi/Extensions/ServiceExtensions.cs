using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentApi.Business.Services;
using StudentApi.Repository;
using Microsoft.Extensions.Configuration;

namespace WeatherService.Api.Extensions
{
    /// <summary>
    /// Class to store all service extensions
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configure the Swagger endpoint for the API
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Student API",
                    Version = "v1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "Harshani Ranawaka",
                        Email = "harshaniranawaka@gmail.com",
                        Url = new Uri("https://github.com/harshaniranawaka81")
                    }
                });

                // generate the XML docs that'll drive the swagger docs
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Configure CORS policies
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        /// <summary>
        /// Configure the database connection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void ConfigureDb(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<StudentDbContext>();

            var connectionString = config["ConnectionStrings: DefaultConnection"];
            services.AddDbContext<StudentDbContext>(
                options => options.UseSqlServer(connectionString));
        }

        /// <summary>
        /// Register all custom services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(this IServiceCollection services)
        {
	        services.AddScoped<IStudentRepository, StudentRepository>();
			services.AddScoped<IStudentService, StudentService>();
        }

		/// <summary>
		/// Configure the logging
		/// </summary>
		/// <param name="app"></param>
		public static void ConfigureLogging(this WebApplication app)
        {
            var loggerFactory = app.Services.GetService<ILoggerFactory>();
            var logFilePath = app.Configuration["Logging:LogFilePath"];
            if (!string.IsNullOrEmpty(logFilePath))
                loggerFactory.AddFile(logFilePath);
        }
    }
}
