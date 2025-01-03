namespace AbstractTaskService.Logic.Response;

public class GetTaskResponse
{
    public string Description { get; set; }
    public int TTlInMilliseconds { get; set; }
    public string Status { get; set; }
}