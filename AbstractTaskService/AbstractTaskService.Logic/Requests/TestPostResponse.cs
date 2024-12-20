using AbstractTaskService.Logic.Models;

namespace AbstractTaskService.Logic.Requests;

public class TestPostResponse
{
    public AbstractTask[] Tasks { get; set; }
}