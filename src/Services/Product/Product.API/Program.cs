using Serilog;
using Common.Logging;
using Product.API.Extensions;
using Product.API.Persistence;

var builder = WebApplication.CreateBuilder(args);


Log.Information("Starting Product API up");

try
{
    builder.Host.UseSerilog(SeriLogger.Configure);
    builder.Host.AddAppConfigurations();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    app.UseInfrastructure();
    app.MigrateDatabase<ProductContext>(async (context, _) =>
    {
        await ProductContextSeed.SeedProductAsync(context, Log.Logger);
    }).Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shup down Product API complete");
    Log.CloseAndFlush();
}


