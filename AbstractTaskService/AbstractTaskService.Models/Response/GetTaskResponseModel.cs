using AbstractTaskService.Models.Models;

namespace AbstractTaskService.Models.Response;

public class GetTaskResponseModel
{
    public string Description { get; set; }
    public int TTlInMilliseconds { get; set; }
    public string Status { get; set; }
}