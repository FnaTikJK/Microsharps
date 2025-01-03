using AbstractTaskService.DAL;
using AbstractTaskService.DAL.Repositories;
using AbstractTaskWorker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<AbstractTaskWorker.Services.AbstractTaskWorker>();
builder.Services.AddScoped<IAbstractTaskRepository, AbstractTaskRepository>();
builder.Services.AddDbContext("Server=localhost;Database=AbstractTaskService;Port=5432;User Id=postgres;Password=123");
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost";
    options.InstanceName = "redis";
});

var host = builder.Build();
host.Run();