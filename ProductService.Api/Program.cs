using Bogus;
using ProductService.Domain;
using ProductService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProductRepository, FakeProductRepository>();
builder.Services.AddSingleton<Faker<Product>, ProductFaker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/products/ping", () => "Pong");

app.MapGet("api/products", async (IProductRepository repository) => Results.Ok(await repository.GetAsync()));

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

// Przyk�ad z u�yciem operatora is
/*
app.MapGet("api/products/{id}", async (IProductRepository repository, int id) => await repository.GetAsync(id) is Product product
    ? Results.Ok(product)
    : Results.NotFound()
)
    .Produces<Product>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);
*/

// Przyk�ad z u�yciem operatora switch

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

app.Run();