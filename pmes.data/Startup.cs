using Microsoft.Extensions.DependencyInjection;

namespace pmes.data
{
    public static class Startup
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            return services;
        }
    }
}
