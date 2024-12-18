using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Infrastructure;
using ServiceDiscovery.API.Logic.ServicesModule.DTO;
using ServiceDiscovery.API.Logic.ServicesModule.Models;

namespace ServiceDiscovery.API.Logic.ServicesModule;

public interface IRoutingService
{
    Result<RegisterServiceResponse> RegisterHosts(RegisterServiceRequest request);
    Result<GetServiceHostResponse> GetServiceHost(string serviceName);
    Result<ServiceRoutingInfo[]> GetAllServicesWithHosts();
    EmptyResult RemoveHosts(RemoveServiceRequest request);
}

public class RoutingService : IRoutingService
{
    private readonly ConcurrentDictionary<string, List<IPEndPoint>> hostsByService; // TODO: Add ThreadSafeList
    private readonly Random random;

    public RoutingService(TimeSpan healthCheckingDelay) // TODO: logger
    {
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
                    var isSuccessConnected = await TryPing(host);
                    if (!isSuccessConnected)
                    {
                        Console.WriteLine($"Can not connect to host: {host}. It will be removed");
                        lock (hosts)
                        {
                            hosts.Remove(host);
                        }
                    }
                    else // TODO: remove
                    {
                        Console.WriteLine($"Success connection with host: {host}.");
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

    private async Task<bool> TryPing(IPEndPoint host)
    {
        try
        {
            using var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(host);
            if (!tcpClient.Connected)
                return false;
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
            return false;
        }
    }
}