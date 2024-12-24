using EasyNetQ;
using Grpc.Net.Client;
using gRPC_Client.Services.Models;
using GrpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ.Serialization.SystemTextJson;
using EasyNetQ.DI;

namespace gRPC_Client.Services
{
    public class PaymentConsumer
    {
        private readonly IBus _bus;

        public PaymentConsumer()
        {
            _bus = RabbitHutch.CreateBus("host=localhost", serviceRegister =>
                serviceRegister.Register<ISerializer, SystemTextJsonSerializer>());
        }

        public void StartListening()
        {
            Console.WriteLine("[*] Ожидание сообщений о платеже...");

            _bus.PubSub.SubscribeAsync<PaymentMessage>("payment_subscription", async payment =>
            {
                try
                {
                    Console.WriteLine($"[x] Получен запрос на обработку платежа: UserId={payment.UserId}, Amount={payment.Amount}, Method={payment.PaymentMethod}");

                    Console.WriteLine("[*] Отправка запроса на gRPC сервер...");
                    var paymentResult = await ProcessPaymentViaGrpc(payment);

                    Console.WriteLine($"[✓] Ответ от gRPC сервера: TransactionId={paymentResult.TransactionId}, Success={paymentResult.Success}");

                    Console.WriteLine("[*] Отправка результата обратно в RabbitMQ...");
                    PublishPaymentResult(payment.UserId, paymentResult);

                    Console.WriteLine("[✓] Сообщение успешно отправлено обратно в очередь.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Ошибка при обработке платежа: {ex.Message}");
                }
            });

            Console.WriteLine("Нажмите любую кнопку для завершения процесса");
            Console.ReadLine();
        }

        private async Task<PaymentResponse> ProcessPaymentViaGrpc(PaymentMessage payment)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:5051", new GrpcChannelOptions
                {
                    HttpHandler = new SocketsHttpHandler
                    {
                        EnableMultipleHttp2Connections = true
                    }
                });

                var client = new PaymentService.PaymentServiceClient(channel);

                var response = await client.ProcessPaymentAsync(new PaymentRequest
                {
                    UserId = payment.UserId,
                    Amount = payment.Amount,
                    PaymentMethod = payment.PaymentMethod
                });

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Ошибка при вызове gRPC сервиса: {ex.Message}");
                throw;
            }
        }

        private void PublishPaymentResult(string userId, PaymentResponse paymentResult)
        {
            var resultMessage = new PaymentResultMessage
            {
                UserId = userId,
                Success = paymentResult.Success,
                TransactionId = paymentResult.TransactionId,
                Message = paymentResult.Message
            };

            _bus.PubSub.Publish(resultMessage);
            Console.WriteLine($"[✓] Результат обработки платежа отправлен: UserId={userId}, Success={resultMessage.Success}, TransactionId={resultMessage.TransactionId}");
        }
    }
}
