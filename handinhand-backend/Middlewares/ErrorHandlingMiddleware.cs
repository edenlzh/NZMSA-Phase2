// HandInHand.Middlewares/ErrorHandlingMiddleware.cs
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HandInHand.Middlewares;

/// <summary>
/// 统一 API 异常处理：<br/>
/// • 按异常类型映射到对应 HTTP 状态码<br/>
/// • 以 RFC-9457 ProblemDetails（application/problem+json）格式输出<br/>
/// • 开发 / Testing 环境下输出详细错误信息，生产环境隐藏 Detail
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next   = next;
        _logger = logger;
        _env    = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // 继续后续管道
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex); // 捕获并格式化返回
        }
    }

    private async Task HandleAsync(HttpContext ctx, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception");

        /* ---------- 1. 根据异常类型确定状态码 ---------- */
        var statusCode = ex switch
        {
            ValidationException            => StatusCodes.Status400BadRequest,
            FormatException                => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException    => StatusCodes.Status401Unauthorized,
            SecurityTokenException         => StatusCodes.Status401Unauthorized,
            KeyNotFoundException           => StatusCodes.Status404NotFound,
            _                              => StatusCodes.Status500InternalServerError
        };

        /* ---------- 2. 组装 ProblemDetails ---------- */
        var problem = new ProblemDetails
        {
            Status   = statusCode,
            Title    = GetTitle(statusCode),
            Detail   = (_env.IsDevelopment() || _env.IsEnvironment("Testing")) ? ex.Message : null,
            Instance = ctx.Request.Path
        };

        /* ---------- 3. 写入响应 ---------- */
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode  = statusCode;
        await ctx.Response.WriteAsJsonAsync(problem);
    }

    private static string GetTitle(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest         => "Bad Request",
        StatusCodes.Status401Unauthorized       => "Unauthorized",
        StatusCodes.Status404NotFound           => "Not Found",
        _                                       => "Internal Server Error"
    };
}
