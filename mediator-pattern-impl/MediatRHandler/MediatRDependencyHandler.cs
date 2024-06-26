using MediatRHandler.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MediatRHandler;
public static class MediatRDependencyHandler
{
    public static IServiceCollection RegisterDatabaseServices(
        this IServiceCollection services,
        ConfigurationManager config, 
        ILogger logger       
    ) 
    {
        string? connectionString = config.GetConnectionString("SqliteConnection");        
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        logger.LogInformation("{Project} services registered", "Data");
        return services;
    }

    public static IServiceCollection RegisterRequestHandlers(
        this IServiceCollection services,        
        ILogger logger       
    ) 
    {
        services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(MediatRDependencyHandler).Assembly));        
        logger.LogInformation("{Project} services registered", "MediatR");
        return services;
    }
}