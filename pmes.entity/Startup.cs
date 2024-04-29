using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using pmes.entity.Context;

namespace pmes.entity
{
    public static class Startup
    {
        public static IServiceCollection AddEntity(this IServiceCollection services, string connectionString)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            services.AddDbContext<PmesContext>(
                        dbContextOptions => dbContextOptions
                        .UseMySql(connectionString, serverVersion)
                        .EnableDetailedErrors()
                    );

            return services;
        }
    }
}
