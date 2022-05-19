using AuthService.Api.Models;
using AuthService.Domain;
using AuthService.Infrastructure;
using Bogus;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAuthService, MyAuthService>();
builder.Services.AddSingleton<IUserRepository, FakeUserRepository>();
builder.Services.AddSingleton<Faker<User>, UserFaker>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("api/token/create", async (AuthModel model, IAuthService authService, ITokenService tokenService, HttpContext context) =>
{
    var result = await authService.TryAuthorizeAsync(model.Login, model.Password);

    if (result.isValid)
    {
        var token = tokenService.Create(result.user);

        // var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        context.Response.Cookies.Append("X-Access-Token", token,
            new CookieOptions { 
                Expires = DateTime.Now.AddMinutes(30),
                SameSite = SameSiteMode.Strict,
                Secure = true
            });

        return Results.Ok(token);        
    }

    return Results.BadRequest(new { message = "Username or password is incorrect." });

});



app.Run();

