namespace SocialMedia.API.Middleware;

public class ResumeMiddleware
{
    private readonly RequestDelegate _next;

    public ResumeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.ToString().ToLower();
        var method = context.Request.Method.ToUpper();
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";

        Console.WriteLine("=================================================================");
        Console.WriteLine($"[{DateTime.UtcNow}] - {method} - {path} from {ip}");
        Console.WriteLine("=================================================================");


        await _next(context);

        var statusCode = context.Response.StatusCode;
        Console.WriteLine("=================================================================");
        Console.WriteLine($"[{DateTime.UtcNow}] - Response Status: {statusCode} for {method} - {path} from {ip}");
        Console.WriteLine("=================================================================");
    }
}