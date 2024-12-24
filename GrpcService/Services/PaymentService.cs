using Grpc.Core;
using EasyNetQ;
using EasyNetQ.Serialization.SystemTextJson;
using System;
using System.Threading.Tasks;
using EasyNetQ.DI;
using GrpcService.Messages;
using Payment;



namespace GrpcService.Services
{
    public class PaymentService : Payments.PaymentsBase
    {
        private readonly IBus _bus;

        public PaymentService()
        {
            _bus = RabbitHutch.CreateBus("host=localhost", serviceRegister =>
                serviceRegister.Register<ISerializer, SystemTextJsonSerializer>());
        }

        public override Task<Payment.PaymentResponse> ProcessPayment(Payment.PaymentRequest request, ServerCallContext context)
        {
            Console.WriteLine($"Processing payment for User: {request.UserId}, Amount: {request.Amount}, Method: {request.PaymentMethod}");

            var transactionId = Guid.NewGuid().ToString();
            var success = request.Amount > 0;

            var auditMessage = $"Processed payment: UserId={request.UserId}, Amount={request.Amount}, Method={request.PaymentMethod}, Success={success}, TransactionId={transactionId}";
            _bus.PubSub.Publish(auditMessage);

            return Task.FromResult(new Payment.PaymentResponse
            {
                Success = success,
                TransactionId = transactionId,
                Message = success ? "Payment processed successfully" : "Payment failed"
            });
        }
    }
}
