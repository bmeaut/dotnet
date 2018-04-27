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
