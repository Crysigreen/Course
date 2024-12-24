using Course.Data;
using Course.Models;
using Course.Services.Contarcts;

namespace Course.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        public PaymentService(ApplicationDbContext context) => _context = context;

        public IEnumerable<Payment> GetAllPayments() => _context.Payments.ToList();
        public Payment GetPaymentById(int id)
            => _context.Payments.Find(id);
        public void ProcessPayment(string userId, int courseId, decimal amount)
        {
            _context.Payments.Add(new Payment
            {
                UserId = userId,
                CourseId = courseId,
                Amount = amount,
                PaymentDate = DateTime.Now
            });
            _context.SaveChanges();
        }
        public void UpdatePayment(int id, Payment updatedPayment)
        {
            var payment = GetPaymentById(id);
            if (payment != null)
            {
                payment.Amount = updatedPayment.Amount;
                payment.PaymentDate = updatedPayment.PaymentDate;
                _context.SaveChanges();
            }
        }
        public void DeletePayment(int id)
        {
            var payment = GetPaymentById(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                _context.SaveChanges();
            }
        }
    }
}
