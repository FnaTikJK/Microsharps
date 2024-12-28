﻿using System.Diagnostics;
using System.Text;
using System.Text.Json;
using AbstractTaskWorker.Model;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AbstractTaskWorker;

public class TaskExecutor
{
    private static readonly Random rnd = new();
    private readonly Stopwatch stopwatch = new();
    private readonly IDistributedCache cache;
    public TaskExecutor(IDistributedCache cache)
    {
        this.cache = cache;
    }
    
    public async Task ExecuteTask(AbstractTaskW taskW)
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        source.CancelAfter(taskW.TTLInMillisecond);
        stopwatch.Restart();
        try
        {   
            var delay = rnd.Next(500, 1500);
            var progressUpdateStep = delay / 20;
            while (stopwatch.ElapsedMilliseconds <= delay)
            {
                Console.WriteLine($"delay {delay}");
                await Task.Delay(progressUpdateStep, token);
                var progress = Math.Round((double)stopwatch.ElapsedMilliseconds / delay * 100);
                taskW.Status = $"In progress: {progress} % done";
                await cache.SetStringAsync(taskW.Id.ToString(), taskW.Status, token);
            }
        }
        catch (OperationCanceledException)
        {
            taskW.Status = "Canceled: Timeout";
        }
    }
}