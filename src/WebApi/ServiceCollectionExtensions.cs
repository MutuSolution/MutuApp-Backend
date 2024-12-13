using Infrastructure.Context;
using Infrastructure.Models;

namespace WebApi;

public static class ServiceCollectionExtensions
{
    internal static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var seeders = serviceScope.ServiceProvider.GetServices<ApplicationDbSeeder>();
        foreach (var seeder in seeders)
        {
            seeder.SeedDatabaseAsync().GetAwaiter().GetResult();
        }
        return app;
    }
    internal static IServiceCollection AddIdentitySettings(this IServiceCollection services)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
        return services;
    }
}
