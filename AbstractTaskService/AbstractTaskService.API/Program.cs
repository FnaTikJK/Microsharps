using System.Reflection;
using AbstractTaskService.Logic;
using Infrastructure.API.Configuration.Application;
using Infrastructure.API.Configuration.ApplicationBuilder;

const string applicationName = "abstract-task-service";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(opt => 
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.RegisterServiceDiscovery(applicationName);
builder.Services.AddSingleton<IAbstractTaskService, AbstractTaskService.Logic.AbstractTaskService>();

var app = builder.Build();

app.BaseConfiguration(useHttps:false, true);
app.ConfigureServiceDiscoveryLifetime();

app.Run();

