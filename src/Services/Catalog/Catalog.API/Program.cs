using BuildingBlocks.Dependencies;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.IoC;
using BuildingBlocks.Modules;
using Carter;
using Catalog.API.Data;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
string dbConnection = builder.Configuration.GetConnectionString("DatabaseConnection")!;

builder.Services.AddDependencyResolvers([

    new MediatorModule(assembly),
    new MartenModule(config =>
    {
        config.Connection(dbConnection);
        
    }),
    new CarterDependencyModule(assembly)
]);

builder.Services.AddValidatorsFromAssembly(assembly);

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();



builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(dbConnection);

var app = builder.Build();

app.UseHttpsRedirection();
app.MapCarter();
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseExceptionHandler(options => { });
app.Run();