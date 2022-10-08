using Microsoft.EntityFrameworkCore;

namespace Product.API.Extensions;

public static class HostExtentions
{
    public static IHost MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();
            try
            {
                logger.LogInformation("Migrating mysql database");
                ExecuteMigrations(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the mysql database");
            }
        };
        return host;
    }

    private static void ExecuteMigrations<TContext>(TContext? context) where TContext : DbContext
    {
        context!.Database.Migrate();
    }
}