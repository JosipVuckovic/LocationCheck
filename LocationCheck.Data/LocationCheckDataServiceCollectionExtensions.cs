using LocationCheck.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocationCheck.Data
{

    public static class LocationCheckDataServiceCollectionExtensions
    {
        public static IServiceCollection AddLocationCheckData(
            this IServiceCollection services,
            IConfiguration configuration)
        {            

            services.AddDbContext<LocationCheckDb>(
                opts => opts.UseSqlServer(
                    configuration.GetConnectionString(StringConstants.ApplicationDb),
                    x => x.MigrationsAssembly(
                        typeof(LocationCheckDb).Assembly.GetName().ToString()
                    )
                )
            );            

            return services;
        }
    }
}
