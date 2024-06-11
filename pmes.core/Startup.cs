using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pmes.core.Cache;
using pmes.core.Helpers;
using pmes.core.Middlewares;
using pmes.core.Services.AWS;
using pmes.core.Services.Email;
using pmes.core.Services.File;

namespace pmes.core
{
    public static class Startup
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfigurationRoot configuration)
        {
            #region Cache Settings
            services.AddOutputCache(options =>
            {
                options.AddPolicy("settings", policy => policy.Expire(TimeSpan.FromHours(24)));
            });

            services.AddDistributedMemoryCache();

            services.AddScoped<CacheService>();
            #endregion

            // HttpClient
            services.AddHttpClient();

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region CORS Settings
            services.AddCors(p => p.AddPolicy("allowAll", b =>
            {
                b.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));
            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ExceptionMiddleware>();

            #region Logger Settings
            if (configuration["StaticSettings:AllowRequestResponseLogging"] == "true")
            {
                services.AddSingleton<RequestLoggingMiddleware>();
                services.AddScoped<ResponseLoggingMiddleware>();
            }
            #endregion

            #region AWS Related Services
            services.AddScoped<ICognitoService, CognitoService>();
            services.AddScoped<IS3Service, S3Service>();
            #endregion

            // Email
            services.AddScoped<IEmailService, EmailService>();

            // Helpers
            services.AddScoped<IHttpClientHelper, HttpClientHelper>();

            #region File Related Services
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ICSVService, CSVService>();
            services.AddScoped<IExcelService, ExcelService>();
            #endregion
            return services;
        }
    }
}
