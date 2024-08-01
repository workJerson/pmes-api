using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pmes.entity.Context;

namespace pmes.entity
{
    public static class Startup
    {
        public static IServiceCollection AddEntity(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            services.AddDbContext<DatabaseContext>(
                        dbContextOptions => dbContextOptions
                        .UseMySql(configuration["Database:ConnectionString"]!, serverVersion)
                        .EnableDetailedErrors()
                    );

            return services;
        }
    }
}
