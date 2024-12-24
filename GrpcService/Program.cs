using GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация Kestrel для gRPC
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5055, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
    options.ListenAnyIP(5051, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

// Добавляем gRPC
builder.Services.AddGrpc();

var app = builder.Build();

// Маршруты gRPC
app.MapGrpcService<PaymentServiceImpl>();
app.MapGet("/", () => "gRPC Payment Service is running");

app.Run();
