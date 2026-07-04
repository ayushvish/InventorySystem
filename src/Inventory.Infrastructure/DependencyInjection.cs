using System.Text;
using Inventory.Application.Common.Interfaces;
using Inventory.Infrastructure.Data;
using Inventory.Infrastructure.Data.Repositories;
using Inventory.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind JWT settings
        var jwtSettings = new JwtSettings();
        configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        // DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
            "Server=(localdb)\\mssqllocaldb;Database=InventorySystemDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(InventoryDbContext).Assembly.FullName)));

        // Repositories & Unit of Work
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Security
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // Auth
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            };
        });

        return services;
    }
}
