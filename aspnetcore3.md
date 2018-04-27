## WriteAsJsonAsync
```csharp
private async Task WriteAsJsonAsync(
    HttpContext context, HttpStatusCode statusCode, object payload, bool clearBeforeWrite = true)
{
    if (clearBeforeWrite)
    {
        context.Response.Clear();
    }

    context.Response.StatusCode = (int)statusCode;

    context.Response.ContentType = "application/json";
    var json = JsonConvert.SerializeObject(payload);
    await context.Response.WriteAsync(json);
}
```
## UseApiExceptionHandler

```csharp
public static class ApiExceptionHandlerExtensions
{
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseWhen(
            ctx => ctx.Request.Path.HasValue 
                   && ctx.Request.Path.StartsWithSegments(new PathString("/api")),
            b => b.UseMiddleware<ApiExceptionHandlerMiddleware>());
    }
}
```
