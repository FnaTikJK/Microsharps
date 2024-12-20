using ApiGateway.Logic.Models;

namespace ApiGateway.Logic.Requests;

public class TestPostResponse
{
    public AbstractTask[] Tasks { get; set; }
}