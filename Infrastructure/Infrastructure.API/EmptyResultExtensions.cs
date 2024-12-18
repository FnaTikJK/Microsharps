using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.API;

public static class EmptyResultExtensions
{
    public static ActionResult ActionResult(this EmptyResult result)
    {
        var (error, statusCode) = result;
        
        return statusCode switch
        {
            HttpStatusCode.NoContent => new NoContentResult(),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(error),
            HttpStatusCode.NotFound => new NotFoundObjectResult(error),
            _ => throw new ArgumentException($"Result does not support {statusCode}")
        };
    }
}