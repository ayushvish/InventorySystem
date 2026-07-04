using System.Text.Json;
using Inventory.Application.Common.Exceptions;
using Inventory.Shared;

namespace Inventory.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var statusCode = StatusCodes.Status500InternalServerError;
        var message = "An unexpected error occurred.";
        List<string>? errors = null;

        switch (exception)
        {
            case ValidationException valEx:
                statusCode = StatusCodes.Status400BadRequest;
                message = valEx.Message;
                errors = valEx.Errors.SelectMany(x => x.Value.Select(err => $"{x.Key}: {err}")).ToList();
                break;

            case NotFoundException nfEx:
                statusCode = StatusCodes.Status404NotFound;
                message = nfEx.Message;
                break;

            case UnauthorizedException unEx:
                statusCode = StatusCodes.Status401Unauthorized;
                message = unEx.Message;
                break;

            default:
                message = exception.Message;
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<object>(statusCode, message, errors);
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
