using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Infrastructure.Persistence;

namespace WorkFlow360.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                     configuration.GetConnectionString("Database");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'Database' was not found.");
            }

            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                });

            services.AddScoped<IApplicationDbContext>(
                serviceProvider =>
                    serviceProvider.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }
}
