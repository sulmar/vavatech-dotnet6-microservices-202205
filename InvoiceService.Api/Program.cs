using Hangfire;
using InvoiceService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IInvoiceService, PdfInvoiceService>();

// dotnet add package Hangfire.AspNetCore
// dotnet add package Hangfire.InMemory
// builder.Services.AddHangfire(options => options.UseInMemoryStorage());

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HangfireDemo;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
// dotnet add package Hangfire.SqlServer
builder.Services.AddHangfire(options => options.UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("api/customers/{id}/invoice", async (int id, 
    IInvoiceService invoiceService,
    IBackgroundJobClient jobClient) =>
{
    // Uwaga: wersja statyczna, nietestowalna!
    // BackgroundJob.Enqueue(() => invoiceService.CreateInvoice(id));
    
    // wersja instancyjna, testowalna
    string jobId = jobClient.Enqueue(() => invoiceService.CreateInvoice(id));

    return Results.Accepted();
});

app.MapHangfireDashboard();

app.Run();

