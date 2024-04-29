using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using pmes.core.Middlewares;

namespace pmes.core
{
    public static class Startup
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            // temporary allow any origin
            services.AddCors(p => p.AddPolicy("allowAll", b =>
            {
                b.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ExceptionMiddleware>();
            services.AddSingleton<RequestLoggingMiddleware>();
            services.AddScoped<ResponseLoggingMiddleware>();

            return services;
        }
    }
}
