using AbstractTaskWorker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<AbstractTaskWorker.Services.AbstractTaskWorker>();
builder.Services.AddScoped<IAbstractTaskRepository, AbstractTaskRepository>();
builder.Services.AddDbContext<WorkerDbContext>(options
    => options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));


var host = builder.Build();
host.Run();