using System.ComponentModel;

namespace AbstractTaskService.Logic.Requests;

public class AddTaskRequest
{
    public string Description { get; set; }
    public int TTLInMillisecond { get; set; }
}