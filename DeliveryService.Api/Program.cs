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

app.MapGet("/", () => "Hello World!");

app.Run();
