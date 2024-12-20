using System.Net;
using JetBrains.Annotations;

namespace Infrastructure;


[PublicAPI]
public struct EmptyResult
{
    public bool IsSuccess => Error == null;
    
    public string? Error { get; }
    public HttpStatusCode StatusCode { get; }

    public EmptyResult(string? error, HttpStatusCode statusCode)
    {
        Error = error;
        StatusCode = statusCode;
    }


    public void Deconstruct(
        out string? error,
        out HttpStatusCode statusCode)
    {
        error = Error;
        statusCode = StatusCode;
    }

    public static EmptyResult Success()
    {
        return new EmptyResult(null, HttpStatusCode.NoContent);
    }

    public static EmptyResult BadRequest(string e)
    {
        return new EmptyResult(e, HttpStatusCode.BadRequest);
    }

    public static EmptyResult NotFound(string e)
    {
        return new EmptyResult(e, HttpStatusCode.NotFound);
    }
}
