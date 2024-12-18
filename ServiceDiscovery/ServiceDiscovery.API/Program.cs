using System.Reflection;
using Infrastructure.API.Configuration.Application;
using ServiceDiscovery.API.Logic.ServicesModule;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(opt => 
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSingleton<IRoutingService, RoutingService>(_ => new RoutingService(TimeSpan.FromSeconds(10))); // TODO: Config

var app = builder.Build();

app.BaseConfiguration(
    useHttps: false,
    isPrivateHosted: true);

app.Run();