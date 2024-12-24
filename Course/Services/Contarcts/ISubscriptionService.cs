using Course.Models;

namespace Course.Services.Contarcts
{
    public interface ISubscriptionService
    {
        IEnumerable<Subscription> GetAllSubscriptions();
        Subscription GetSubscriptionById(int id);
        void AddSubscription(Subscription subscription);
        void UpdateSubscription(int id, Subscription subscription);
        void DeleteSubscription(int id);
        bool IsUserSubscribed(string userId, int courseId);
    }
}
