using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Api.Configuration
{
    public static class DbConfig
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqlDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Api")));

            return services;
        }
    }
}
