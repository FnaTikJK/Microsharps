using AbstractTaskService.Models.Models;

namespace AbstractTaskService.Models.Request;

public class AddTaskRequestModel
{
    public string Description { get; set; }
    public int TTLInMillisecond { get; set; }
}