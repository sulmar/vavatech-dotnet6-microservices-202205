using Bogus;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProductService.Api.HealthChecks;
using ProductService.Domain;
using ProductService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

string env = builder.Environment.EnvironmentName;

builder.Configuration.AddJsonFile("appsettings.mateusz.json", optional: true);
builder.Configuration.AddJsonFile("appsettings.marcin.json", optional: true);
builder.Configuration.AddXmlFile("appsettings.xml", optional: true);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddCommandLine(args);
builder.Configuration.AddJsonFile($"appsettings.{env}.json", optional: true);
builder.Configuration.AddUserSecrets<Program>();

// builder.Configuration.AddInMemoryCollection(); // do testów integracyjnych

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProductRepository, FakeProductRepository>();
builder.Services.AddSingleton<Faker<Product>, ProductFaker>();

builder.Services.AddHttpClient();

string instance = builder.Configuration["instance"];

//string nbpApiUrl = builder.Configuration["NBPApi:Url"];
//string nbpApiTable = builder.Configuration["NBPApi:Table"];

string googleMaps = builder.Configuration["GoogleMaps:LicenseKey"];

builder.Services.Configure<NbpApiHealthCheckOptions>(builder.Configuration.GetSection("NBPApi"));


builder.Services.AddHealthChecks()
    .AddCheck<NbpApiHealthCheck>("NbpApi")
    .AddCheck("Sample", () =>
    {
        if (instance=="B")
        {
            if (DateTime.Now.Minute % 2 == 0)
            {
                return HealthCheckResult.Unhealthy();
            }
            else
            {
                return HealthCheckResult.Healthy();
            }
        }

        return HealthCheckResult.Healthy("Lorem ipsum");

    });
    ;

// dotnet add package AspNetCore.HealthChecks.UI
builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage(); // dotnet add package AspNetCore.HealthChecks.UI.InMemory.Storage


builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.Add("X-Instance", instance);
        return Task.CompletedTask;
    });

    await next();
});

app.MapGet("api/products/ping", () => "Pong");

app.MapGet("api/products", async (IProductRepository repository) => Results.Ok(await repository.GetAsync()));
    //.RequireAuthorization();

/*
app.MapGet("api/products/{id}", async (IProductRepository repository, int id) =>
{
    var product = await repository.GetAsync(id);

    if (product == null)
        return Results.NotFound();

    return Results.Ok(product);
})
    .Produces<Product>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

*/

// Przyk³ad z u¿yciem operatora is
/*
app.MapGet("api/products/{id}", async (IProductRepository repository, int id) => await repository.GetAsync(id) is Product product
    ? Results.Ok(product)
    : Results.NotFound()
)
    .Produces<Product>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);
*/

// Przyk³ad z u¿yciem operatora switch

app.MapGet("api/products/{id:int}", async (IProductRepository repository, int id) => await repository.GetAsync(id) switch
{
    Product product => Results.Ok(product),
    null => Results.NotFound()
})
    .Produces<Product>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetProductById");

app.MapPost("api/products", async (IProductRepository repository, Product product) =>
{
    await repository.AddAsync(product);

    return Results.CreatedAtRoute("GetProductById", new { id = product.Id }, product);
});

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.MapHealthChecksUI(); // /healthchecks-ui

app.Run();
