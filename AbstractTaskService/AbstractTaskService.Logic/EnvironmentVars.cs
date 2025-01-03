namespace AbstractTaskService.Logic;

public static class EnvironmentVars
{
    public static string RabbitQueueName => Environment.GetEnvironmentVariable("QUEUE_NAME") ?? "hello";
    public static string DataBase => Environment.GetEnvironmentVariable("DB_HOST") ??
                                     "Server=localhost;Database=AbstractTaskService;Port=5432;User Id=postgres;Password=123";

    public static string RedisConfig => Environment.GetEnvironmentVariable("REDIS_CONF") ?? "localhost";
    public static string RedisInstanceName => Environment.GetEnvironmentVariable("REDIS_NAME") ?? "redis";
    public static string RabbitHost => Environment.GetEnvironmentVariable("RABBIT_HOST") ?? "localhost";
}