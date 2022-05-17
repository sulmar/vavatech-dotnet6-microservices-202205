
using Microsoft.Extensions.Primitives;
using OCRService.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Logger (Middleware)
//app.Use(async (context, next) =>
//{
//    Console.WriteLine($"{context.Request.Method} {context.Request.Path}");

//    await next();

//    Console.WriteLine($"{context.Response.StatusCode}");

//});



//app.Use(async (context, next) =>
//{
//    if (context.Request.Headers.TryGetValue("Authorization", out StringValues value ))
//    {
//        await next();
//    }
//    else
//    {
//        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//    }
//});

// app.UseMiddleware<LoggerMiddleware>();
// app.UseMiddleware<AuthorizationMiddleware>();

app.UseLogger();
app.UseMyAuthorization();

app.Run(async context =>
{
    if (context.Request.Path == "/api/documents")
    {        
        await context.Response.WriteAsync("Hello OCR Api!");
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
    }
});

app.Run();
