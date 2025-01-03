using AbstractTaskService.DAL;
using AbstractTaskService.DAL.Repositories;
using AbstractTaskWorker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<AbstractTaskWorker.Services.AbstractTaskWorker>();
builder.Services.AddScoped<IAbstractTaskRepository, AbstractTaskRepository>();
builder.Services.AddDbContext(EnvironmentVars.DataBase);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = EnvironmentVars.RedisConfig;
    options.InstanceName = EnvironmentVars.RedisInstanceName;
});

var host = builder.Build();
host.Run();