namespace spliteasy.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DataServiceExtensions
{
    public static IServiceCollection AddDataServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Get connection string from configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Register DbContext
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        // Register repositories
        // services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();

        return services;
    }
}
