using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LocationCheck.Data
{
    public class DbInitializer
    {
        private readonly LocationCheckDb _context;        

        public DbInitializer(IServiceScope scope)
        {
            _context = scope.ServiceProvider.GetService<LocationCheckDb>();            
        }

        public async Task InitAsync()
        {            

            await _context.Database.MigrateAsync();            
        }
    }
}
