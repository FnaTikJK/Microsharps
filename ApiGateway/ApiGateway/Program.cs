using AbstractTaskService.Client;
using ApiGateway.Logic;
using ApiGateway.Logic.Tasks;
using ApiGateway.Logic.Users;
using Infrastructure.API.Configuration.Authentication;
using Infrastructure.API.Configuration.Builder;
using Users.Client;

const string serviceName = "api-gateway";

var builder = MicrosharpsWebAppBuilder.Create(serviceName, false, args)
    .BaseConfiguration(
        isPrivateHosted: false
    )
    .UseLogging(true)
    .UseAuthorization()
    .ConfigureDi(ConfigureDi);

builder.BuildAndRun();

void ConfigureDi(IServiceCollection services)
{
    services.AddSingleton<IAbstractTaskServiceClient, AbstractTaskServiceClient>(); // TODO:  add DI to Logic and move into it  
    services.AddSingleton<IUsersService, UsersService>();
    services.AddSingleton<IUsersClient, UsersClient>();
    services.AddSingleton<ITaskService, TasksService>();
}