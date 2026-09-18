using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Infrastructure.Authentication;
using WorkFlow360.Infrastructure.Persistence;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WorkFlow360.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure( this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException( "Connection string 'Database' was not found.");
            }

            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                });

            services.AddScoped<IApplicationDbContext>(
                serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? throw new InvalidOperationException("JWT settings were not found.");

            if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey)) throw new InvalidOperationException("JWT secret key was not configured.");

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });


            return services;
        }
    }
}
