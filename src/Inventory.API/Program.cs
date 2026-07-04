using Inventory.API;
using Inventory.API.DataSeed;
using Inventory.API.Middleware;
using Inventory.Application;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add Clean Architecture Layers Services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<InventoryDbContext>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacy Warehouse Inventory API v1");
    });
}

// Global Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

// Response Compression
app.UseResponseCompression();

app.UseRouting();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// Migration and Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<InventoryDbContext>();
        await DatabaseInitializer.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database migration or seeding.");
    }
}

try
{
    Log.Information("Starting Pharmacy Warehouse Inventory API...");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API startup failed.");
}
finally
{
    Log.CloseAndFlush();
}
