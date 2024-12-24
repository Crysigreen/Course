using Grpc.Core;
using EasyNetQ;
using EasyNetQ.Serialization.SystemTextJson;
using System;
using System.Threading.Tasks;
using EasyNetQ.DI;



namespace GrpcService.Services
{
    public class PaymentServiceImpl : PaymentService.PaymentServiceBase
    {
        public override Task<PaymentResponse> ProcessPayment(PaymentRequest request, ServerCallContext context)
        {
            try
            {
                Console.WriteLine($"[x] Обработка платежа: UserId={request.UserId}, Amount={request.Amount}, PaymentMethod={request.PaymentMethod}");

                // Пример логики обработки платежа
                if (request.Amount <= 0)
                {
                    Console.WriteLine($"[!] Некорректная сумма платежа: {request.Amount}");
                    return Task.FromResult(new PaymentResponse
                    {
                        Success = false,
                        TransactionId = string.Empty,
                        Message = "Invalid payment amount"
                    });
                }

                var transactionId = GenerateTransactionId(request.UserId, request.Amount);

                Console.WriteLine($"[✓] Платеж успешно обработан. TransactionId={transactionId}");

                return Task.FromResult(new PaymentResponse
                {
                    Success = true,
                    TransactionId = transactionId,
                    Message = $"Payment processed successfully using {request.PaymentMethod}"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Ошибка при выполнении метода <<ProcessPayment>>: {ex.Message}");
                throw;
            }
        }

        private string GenerateTransactionId(string userId, double amount)
        {
            var combinedString = $"{userId}-{amount}-{DateTime.UtcNow.Ticks}";

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combinedString));

                return BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 16).ToUpper();
            }
        }
    }
}
