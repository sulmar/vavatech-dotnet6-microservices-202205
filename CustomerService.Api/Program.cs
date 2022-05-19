using Bogus;
using CustomerService.Domain;
using CustomerService.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR.Extensions.Autofac.DependencyInjection
builder.Services.AddMediatR(typeof(Program));

builder.Services.AddSingleton<ICustomerRepository, FakeCustomerRepository>();
builder.Services.AddSingleton<Faker<Customer>, CustomerFaker>();

builder.Services.AddSingleton<IMessageSender, FakeConsoleMessageSender>();

builder.Services.AddHealthChecks();

var secretKey = "your-256-bit-secret";
var key = Encoding.UTF8.GetBytes(secretKey);

// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = "Vavatech",
        ValidateAudience = true,
        ValidAudience = "Vavatech",
    };
})
    
    ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("api/customers/{id}", async context =>
//    {
//        int id = (int)context.Request.RouteValues["id"];

//        ICustomerRepository customerRepository = context.RequestServices.GetRequiredService<ICustomerRepository>();

//        Customer customer = await customerRepository.Get(id);



//    });
//});

//)

app.Run();
