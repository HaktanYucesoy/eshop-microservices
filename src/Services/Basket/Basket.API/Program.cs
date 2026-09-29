using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.Dependencies;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.IoC;
using BuildingBlocks.Modules;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
var dbConnection = builder.Configuration.GetConnectionString("DatabaseConnection")!;
var redisConnection = builder.Configuration.GetConnectionString("Redis")!;

builder.Services.AddDependencyResolvers([
    new MediatorModule(assembly),
    new MartenModule(config =>
    {
        config.Connection(dbConnection);
        config.Schema.For<ShoppingCart>().Identity(s => s.UserName);
    }),
    new CarterDependencyModule(assembly),
    new BasketDataModule()
]);


builder.Services.AddStackExchangeRedisCache(config =>
{
    config.Configuration = redisConnection;
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(dbConnection)
    .AddRedis(redisConnection);


var app = builder.Build();
app.UseHttpsRedirection();
app.MapCarter();
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseExceptionHandler((options) => { });
app.Run();