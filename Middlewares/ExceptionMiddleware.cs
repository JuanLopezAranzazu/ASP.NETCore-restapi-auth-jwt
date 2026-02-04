using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ConflictException ex)
        {
            await WriteError(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            await WriteError(context, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (Exception)
        {
            await WriteError(context, HttpStatusCode.InternalServerError,
                "Ocurrió un error inesperado en el servidor.");
        }
    }

    private static async Task WriteError(
        HttpContext context,
        HttpStatusCode status,
        string message)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = context.Response.StatusCode,
            error = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}