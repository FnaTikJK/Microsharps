using System.Reflection;
using AbstractTaskService.Client;
using ApiGateway.Logic;
using Infrastructure.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(opt => 
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSingleton<ITestService, TestService>();
builder.Services.AddSingleton<IAbstractTaskServiceClient, AbstractTaskServiceClient>(); // TODO:  add DI to Logic and move into it  

var app = builder.Build();

app.BaseConfiguration(
    useHttps: false,
    isPrivateHosted: false
);

app.Run();