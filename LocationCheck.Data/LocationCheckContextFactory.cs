using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LocationCheck.Data.Constants;

namespace LocationCheck.Data
{
    // Used to keep Migrations within the Data sub project
    public class LocationCheckContextFactory : IDesignTimeDbContextFactory<LocationCheckDb>
    {
        public LocationCheckDb CreateDbContext(string[] args)
        {           

            var optionsBuilder = new DbContextOptionsBuilder<LocationCheckDb>();
            var configuration = new ConfigurationBuilder()                
                .Build();           

            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString(StringConstants.ApplicationDb)
            );

            return new LocationCheckDb(optionsBuilder.Options);
        }
    }
}
