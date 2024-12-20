using AbstractTaskService.Models.Models;

namespace AbstractTaskService.Models.Request;

public class TestPostResponseModel
{
    public AbstractTaskModel[] Tasks { get; set; }
}