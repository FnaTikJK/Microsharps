using System.Reflection;
using AbstractTaskService.DAL;
using AbstractTaskService.DAL.Context;
using AbstractTaskService.DAL.Repositories;
using AbstractTaskService.Logic;
using AbstractTaskService.Logic.Services;
using Infrastructure.API.Configuration;
using Infrastructure.API.Configuration.Builder;
using Infrastructure.API.Configuration.ServiceDiscovery;
using Microsoft.EntityFrameworkCore;

const string serviceName = "abstract-task-service";

var builder = MicrosharpsWebAppBuilder.Create(serviceName, false, args)
    .BaseConfiguration(isPrivateHosted: true)
    .UseLogging(true)
    .ConfigureDi(ConfigureDi)
    .RegisterInServiceDiscovery();

builder.BuildAndRun();


void ConfigureDi(IServiceCollection services)
{
    services.AddStackExchangeRedisCache(options => {
        options.Configuration = "localhost";
        options.InstanceName = "redis";
        });
    services.AddDbContext("Server=localhost;Database=AbstractTaskService;Port=5432;User Id=postgres;Password=123");
    services.AddScoped<IAbstractTaskRepository, AbstractTaskRepository>();
    services.AddScoped<IAbstractTaskService, AbstractTaskService.Logic.Services.AbstractTaskService>();
};
