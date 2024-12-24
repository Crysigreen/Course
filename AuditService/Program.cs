using EasyNetQ;

namespace AuditService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Audit Service is starting...");

            var bus = RabbitHutch.CreateBus("host=localhost");

            string queueName = "audit_queue";

            bus.PubSub.Subscribe<string>("audit_subscription", message =>
            {
                Console.WriteLine($"[Audit] Received: {message}");
            });

            Console.WriteLine("Audit Service is waiting for messages. Press [enter] to exit.");
            Console.ReadLine();

            bus.Dispose();
        }
    }
}
