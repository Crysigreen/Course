using Grpc.Net.Client;
using gRPC_Client.Services;

namespace gRPC_Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Запуск GrpcPaymentClient...");

            var consumer = new PaymentConsumer();
            consumer.StartListening();
        }
    }
}
