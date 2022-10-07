using Serilog;
using Common.Logging;
using Product.API.Extensions;

var builder = WebApplication.CreateBuilder(args);


Log.Information("Starting Product API up");

try
{
    builder.Host.UseSerilog(SeriLogger.Configure);
    builder.Host.AddAppConfigurations();
    builder.Services.AddInfrastructure();

    var app = builder.Build();
    app.UseInfrastructure();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shup down Product API complete");
    Log.CloseAndFlush();
}

