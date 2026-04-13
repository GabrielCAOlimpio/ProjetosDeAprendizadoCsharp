namespace SocialMedia.API.Middleware;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Message = "Invalid argument provided.",
                Details = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (KeyNotFoundException ex)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Message = "Resource not found.",
                Details = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Message = "Unauthorized access.",
                Details = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (InvalidOperationException ex)
        {
            context.Response.StatusCode = 409;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Message = "Conflict occurred.",
                Details = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Message = "An unexpected error occurred.",
                Details = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}