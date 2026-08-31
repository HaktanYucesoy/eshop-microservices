using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Catalog.API.Data;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

string dbConnection = builder.Configuration.GetConnectionString("DatabaseConnection")!;

builder.Services.AddMarten(config =>
{
    config.Connection(dbConnection);
    config.AutoCreateSchemaObjects = JasperFx.AutoCreate.CreateOrUpdate;

    config.CreateDatabasesForTenants(c =>
    {
      
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(dbConnection)
        {
            Database = "postgres"
        };

        
        c.MaintenanceDatabase(connectionStringBuilder.ConnectionString);

        c.ForTenant()
            .CheckAgainstPgDatabase()
            .WithOwner("postgres")
            .WithEncoding("UTF-8");
    });

}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(dbConnection);

var app = builder.Build();

app.UseHttpsRedirection();
app.MapCarter();
app.UseHealthChecks("/health",new HealthCheckOptions
{
    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseExceptionHandler(options => { });
app.Run();