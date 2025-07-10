using System.Net;
using System.Text.Json;

namespace HandInHand.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try { await next(context); }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var problem = new { message = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
