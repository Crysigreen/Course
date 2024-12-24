using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Serialization.SystemTextJson;
using Newtonsoft.Json;

namespace Course.AuditLogger
{
    public class AuditProducer : IDisposable
    {
        private readonly IBus _bus;

        public AuditProducer()
        {
            _bus = RabbitHutch.CreateBus("host=localhost", serviceRegister =>
                serviceRegister.Register<ISerializer, SystemTextJsonSerializer>());
        }

        public void SendAuditMessage(string message)
        {
            if (_bus == null)
            {
                throw new InvalidOperationException("RabbitMQ bus is not initialized.");
            }

            _bus.PubSub.Publish(message);
            Console.WriteLine($"[Producer] Sent Audit Message: {message}");
        }

        public void Dispose()
        {
            _bus?.Dispose();
        }
    }
}
