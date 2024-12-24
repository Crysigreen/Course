using Course.Data;
using Course.Models;
using Course.Services.Contarcts;

namespace Course.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _context;
        public SubscriptionService(ApplicationDbContext context) => _context = context;

        public IEnumerable<Subscription> GetAllSubscriptions()
            => _context.Subscriptions.ToList();
        public Subscription GetSubscriptionById(int id)
            => _context.Subscriptions.Find(id);
        public void AddSubscription(Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();
        }
        public void UpdateSubscription(int id, Subscription updatedSubscription)
        {
            var subscription = GetSubscriptionById(id);
            if (subscription != null)
            {
                subscription.StartDate = updatedSubscription.StartDate;
                subscription.EndDate = updatedSubscription.EndDate;
                subscription.IsActive = updatedSubscription.IsActive;
                _context.SaveChanges();
            }
        }
        public void DeleteSubscription(int id)
        {
            var subscription = GetSubscriptionById(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
                _context.SaveChanges();
            }
        }
        public bool IsUserSubscribed(string userId, int courseId)
            => _context.Subscriptions.Any(s => s.UserId == userId && s.CourseId == courseId);
    }
}
