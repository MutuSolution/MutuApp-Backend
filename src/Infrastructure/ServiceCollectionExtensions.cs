using Application.Services;
using Application.Services.Identity;
using Infrastructure.Context;
using Infrastructure.Services;
using Infrastructure.Services.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
            .AddTransient<ApplicationDbSeeeder>();
        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services
                .AddTransient<ITokenService, TokenService>();
        return services;
    }

    public static IServiceCollection AddEmployeeService(this IServiceCollection services)
    {
        services
            .AddTransient<IEmployeeService, EmployeeService>();
        return services;
    }

}
