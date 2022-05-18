using DeliveryService.Api.Services;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

// protobuf-net.Grpc.AspNetCore
builder.Services.AddCodeFirstGrpc();

// protobuf-net.Grpc.AspNetCore.Reflection
builder.Services.AddCodeFirstGrpcReflection();

var app = builder.Build();

app.MapGrpcService<MyDeliveryService>();
app.MapCodeFirstGrpcReflectionService();

/* 
 
> dotnet add package System.ServiceModel.Primitives
 
> dotnet grpc-cli ls https://localhost:5081/

> dotnet grpc-cli ls https://localhost:5081/ DeliveryService.Contracts.DeliveryService

> dotnet grpc-cli dump https://localhost:5081/ DeliveryService.Contracts.DeliveryService

*/

app.MapGet("/", () => "Hello World!");

// https://github.com/grpc/grpc-web

app.Run();
