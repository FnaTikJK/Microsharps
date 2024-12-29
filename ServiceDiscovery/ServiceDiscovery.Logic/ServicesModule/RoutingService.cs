using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Infrastructure;
using Microsoft.Extensions.Logging;
using ServiceDiscovery.Logic.ServicesModule.DTO;
using ServiceDiscovery.Logic.ServicesModule.Models;

namespace ServiceDiscovery.Logic.ServicesModule;

public interface IRoutingService
{
    Result<RegisterServiceResponse> RegisterHosts(RegisterServiceRequest request);
    Result<GetServiceHostResponse> GetServiceHost(string serviceName);
    Result<ServiceRoutingInfo[]> GetAllServicesWithHosts();
    EmptyResult RemoveHosts(RemoveServiceRequest request);
}

public class RoutingService : IRoutingService
{
    private readonly ILogger<RoutingService> logger;
    private readonly ConcurrentDictionary<string, List<IPEndPoint>> hostsByService; // TODO: Add ThreadSafeList
    private readonly Random random;

    public RoutingService(ILogger<RoutingService> logger, TimeSpan healthCheckingDelay) // TODO: logger
    {
        this.logger = logger;
        hostsByService = new ConcurrentDictionary<string, List<IPEndPoint>>();
        random = new Random();
        StartHealthPing(healthCheckingDelay, new CancellationToken());
    }

    public Result<RegisterServiceResponse> RegisterHosts(RegisterServiceRequest request)
    {
        var existedHosts = GetHosts(request.ServiceName);
        foreach (var newHost in request.Hosts.Select(IPEndPoint.Parse))
        {
            if (!existedHosts.Contains(newHost))
            {
                lock (existedHosts)
                {
                    existedHosts.Add(newHost);
                    logger.LogInformation($"Registered service: {request.ServiceName} with host: {newHost}");
                }
            }
            else
            {
                return Result.BadRequest<RegisterServiceResponse>("Same host already used for this service");
            }
        }
        
        return Result.Ok(new RegisterServiceResponse
        {
            Hosts = existedHosts.Select(e => e.ToString()).ToArray(),
        });
    }

    public Result<GetServiceHostResponse> GetServiceHost(string serviceName)
    {
        var hosts = GetHosts(serviceName);
        if (hosts.Count == 0)
            return Result.NotFound<GetServiceHostResponse>("Service has not registered any instances");
        
        var randomHost = hosts[random.Next(hosts.Count)];
        return Result.Ok(new GetServiceHostResponse()
        {
            Host = randomHost.ToString()
        });
    }

    public Result<ServiceRoutingInfo[]> GetAllServicesWithHosts()
    {
        return Result.Ok(hostsByService.Select(e => new ServiceRoutingInfo
        {
            ServiceName = e.Key,
            Hosts = e.Value.Select(ip => ip.ToString()).ToArray(),
        }).ToArray());
    }

    public EmptyResult RemoveHosts(RemoveServiceRequest request)
    {
        var hosts = GetHosts(request.ServiceName);
        foreach (var hostToDel in request.Hosts.Select(IPEndPoint.Parse))
        {
            if (!hosts.Contains(hostToDel))
                continue;
            
            lock (hosts)
            {
                hosts.Remove(hostToDel);
            }
        }
        
        return EmptyResult.Success();
    }
    
    private List<IPEndPoint> GetHosts(string serviceName) 
        => hostsByService.GetOrAdd(serviceName, _ => new List<IPEndPoint>());

    private async Task StartHealthPing(TimeSpan healthCheckingDelay, CancellationToken token)
    {
        async Task Ping()
        {
            foreach (var serviceWithHosts in hostsByService)
            {
                var hosts = serviceWithHosts.Value;
                foreach (var host in hosts)
                {
                    var connectionResult = await TryPing(host);
                    if (connectionResult.IsSuccess) continue;
                    
                    logger.LogWarning($"Can not connect to host: {host}. It will be removed.{Environment.NewLine}Error: {connectionResult.Error}");
                    lock (hosts)
                    {
                        hosts.Remove(host);
                    }
                }
            }
            
        }

        while (!token.IsCancellationRequested)
        {
            await Task.Delay(healthCheckingDelay); // TODO: пинги падают после 1 неудачного
            await Ping();
        }
    }

    private async Task<EmptyResult> TryPing(IPEndPoint host)
    {
        try
        {
            using var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(host);
            if (!tcpClient.Connected)
                return EmptyResult.BadRequest("Can not establish connection with host");
            
            return EmptyResult.Success();
        }
        catch (Exception e)
        {
            return EmptyResult.BadRequest(e.Message + Environment.NewLine + e.StackTrace);
        }
    }
}