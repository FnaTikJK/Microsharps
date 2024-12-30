using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.API.RightsChecker
{
    public static class RightsCheckerMiddlewareExtensions
    {
        public static IApplicationBuilder UseRightsChecker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RightsCheckerMiddleware>();
        }
    }

    public class RightsCheckerMiddleware
    {
        private readonly RequestDelegate _nextRequest;
        private readonly IRightsChecker _rightsChecker;

        public RightsCheckerMiddleware(RequestDelegate nextRequest, IRightsChecker rightsChecker)
        {
            _nextRequest = nextRequest;
            _rightsChecker = rightsChecker;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User?.Identity?.Name;
            var requiredRight = "required_right"; // Здесь логика получения необходимых прав из контекста, например, из пути или запроса

            if (userId != null && await _rightsChecker.HasAccessAsync(userId, requiredRight, "read"))
            {
                await _nextRequest(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
        }
    }
}