using System.Net;
using JetBrains.Annotations;

namespace Infrastructure;

[PublicAPI]
public struct Result<T>
{
    public Result(string? error, HttpStatusCode statusCode, T value = default(T))
    {
        Error = error;
        StatusCode = statusCode;
        Value = value;
    }

    public string? Error { get; }
    public HttpStatusCode StatusCode { get; }
    public T Value { get; }
    public bool IsSuccess => Error == null;
    
    public void Deconstruct(
        out string? error,
        out HttpStatusCode statusCode,
        out T value)
    {
        error = Error;
        statusCode = StatusCode;
        value = Value;
    }
}

public static class Result
{
    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(null, HttpStatusCode.OK, value);
    }

    public static Result<T> NoContent<T>()
    {
        return new Result<T>(null, HttpStatusCode.NoContent);
    }

    public static Result<T> BadRequest<T>(string e)
    {
        return new Result<T>(e, HttpStatusCode.BadRequest);
    }

    public static Result<T> NotFound<T>(string e)
    {
        return new Result<T>(e, HttpStatusCode.NotFound);
    }

    public static Result<T> InternalServerError<T>(string e)
    {
        return new Result<T>(e, HttpStatusCode.InternalServerError);
    }


    public static Result<T2> ErrorFrom<T, T2>(Result<T> source)
    {
        if (source.IsSuccess)
            throw new ArgumentException("source Result should be unsuccessful");
        
        return new Result<T2>(source.Error, source.StatusCode);
    }

    public static Result<T> ErrorFromHttp<T>(Result<HttpResponseMessage> source)
        => ErrorFrom<HttpResponseMessage, T>(source);
    
    public static Result<T> ErrorFromHttp<T>(HttpResponseMessage response)
    {
        var errorMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => BadRequest<T>(errorMessage),
            HttpStatusCode.NotFound => NotFound<T>(errorMessage),
            HttpStatusCode.InternalServerError => InternalServerError<T>(errorMessage),
        };
    }
}