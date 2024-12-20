using ApiGateway.Models.Models;

namespace ApiGateway.Models.Requests;

public class TestPostResponseModel
{
    public AbstractTaskModel[] Tasks { get; set; }
}