using System.Net.Http.Json;

namespace Infrastructure;

public static class HttpResponseMessageExtensions
{
    public static async Task<T> FromJsonOrThrow<T>(this HttpContent content)
    {
        var deserialized = await content.ReadFromJsonAsync<T>();
        if (deserialized == null)
            throw new ArgumentException($"Deserialized content is null, but not expected");

        return deserialized;
    }
}