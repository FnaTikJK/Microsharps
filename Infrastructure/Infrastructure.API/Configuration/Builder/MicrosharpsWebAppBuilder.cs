using System.Reflection;
using Infrastructure.API.Configuration.Logging;
using Infrastructure.API.Configuration.ServiceDiscovery;
using Infrastructure.API.RightsChecker;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.API.Configuration.Builder;

/// <summary>
/// Билдер веб-аппа для стандартизации, облегчения заведения сервисов и централизованого изменения их базовой конфигурации
/// </summary>
public class MicrosharpsWebAppBuilder
{
    private readonly string serviceName;
    private readonly WebApplicationBuilder builder;
    private readonly WebApplicationConfig appConfig;

    /// <summary>
    /// Создаёт инстанс билдера
    /// </summary>
    /// <param name="serviceName">Название сервиса</param>
    /// <param name="isStrictBuild">Нужно ли проверять корректность конфигурации</param>
    /// <param name="args"></param>
    public static MicrosharpsWebAppBuilder Create(string serviceName, bool isStrictBuild, string[] args)
        => new(serviceName, isStrictBuild, args);
    
    private MicrosharpsWebAppBuilder(string serviceName, bool isStrictBuild, string[] args)
    {
        this.serviceName = serviceName;
        builder = WebApplication.CreateBuilder(args);
        appConfig = new();
    }

    public MicrosharpsWebAppBuilder BaseConfiguration(bool isPrivateHosted)
    {
        builder.Services.AddEndpointsApiExplorer();
        UseControllers();
        UseSwagger();
        UseLogging(true, LogSinks.OnlyLocal);
        appConfig.Add(app =>
        {
            app.UseAuthentication();
            app.UseAuthorization();
        });
        
        if (isPrivateHosted)
            appConfig.Add(app => app.UseCors(opt => opt.AllowAll()));
        else
            appConfig.Add(app => app.UseHttpsRedirection());
        

        return this;
    }

    public MicrosharpsWebAppBuilder UseControllers()
    {
        builder.Services.AddControllers();
        appConfig.Add(app => app.MapControllers());

        return this;
    }

    public MicrosharpsWebAppBuilder UseSwagger()
    {
        builder.Services.AddSwaggerGen(opt => 
        {
            var xmlFilename = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        
        appConfig.Add(app =>
        {
            // if (app.Environment.IsDevelopment()) TODO
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            } 
        });

        return this;
    }

    public MicrosharpsWebAppBuilder UseLogging(
        bool withEndpoints, 
        LogSink[]? customLogSinks = null)
    {
        builder.ConfigureSerilog(serviceName, customLogSinks ?? LogSinks.All);
        appConfig.Add(app => app.UseSerilogRequestLogging());
        if (withEndpoints)
        {
            builder.Services.AddSingleton<ILogsService, LogsService>();
            appConfig.Add(app => app.MapLoggingEndpoints());
        }
        return this;
    }

    public MicrosharpsWebAppBuilder RegisterInServiceDiscovery(
        string? ownHost = null,
        string? serviceDiscoveryHost = null)
    {
        ownHost ??= EnvironmentVars.OwhHost;
        serviceDiscoveryHost ??= EnvironmentVars.SdHost;
        builder.Services.RegisterServiceDiscoveryConfigurationClient(serviceName, ownHost, serviceDiscoveryHost);
        appConfig.Add(app => app.MapServiceDiscoveryEndpoints());
        appConfig.Add(app => app.ConfigureLifetimeInServiceDiscovery());
        
        return this;
    }

    public MicrosharpsWebAppBuilder ConfigureDi(Action<IServiceCollection> configuration)
    {
        configuration(builder.Services);
        return this;
    }


    public void BuildAndRun()
    {
        var app = builder.Build();
        appConfig.Apply(app);
        
        app.Run();
    }
    
    public MicrosharpsWebAppBuilder UseRightsChecker()
    {
        builder.Services.AddSingleton<IRightsChecker, RightsChecker.RightsChecker>();
        appConfig.Add(app => app.UseRightsChecker());  // Применяем middleware для проверки прав
        return this;
    }
}