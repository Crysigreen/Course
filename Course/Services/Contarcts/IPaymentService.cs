using Course.Models;

namespace Course.Services.Contarcts
{
    public interface IPaymentService
    {
        IEnumerable<Payment> GetAllPayments();
        Payment GetPaymentById(int id);
        void ProcessPayment(string userId, int courseId, decimal amount);
        void UpdatePayment(int id, Payment payment);
        void DeletePayment(int id);
    }

}
