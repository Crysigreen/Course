using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Serialization.SystemTextJson;
using GrpcService.Services;

namespace GrpcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        services.AddGrpc();
                        services.AddSingleton<IBus>(_ => RabbitHutch.CreateBus("host=localhost", serviceRegister =>
                            serviceRegister.Register<ISerializer, SystemTextJsonSerializer>()));
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapGrpcService<PaymentService>();
                            endpoints.MapGet("/", async context =>
                            {
                                await context.Response.WriteAsync("Payment gRPC Server is running...");
                            });
                        });
                    });
                });
    }
}