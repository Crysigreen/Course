using Microsoft.AspNetCore.SignalR;

namespace Courses.Hubs
{
    public class PaymentHub : Hub
    {
        public async Task SendPaymentNotification(object message)
        {
            await Clients.All.SendAsync("ReceivePaymentNotification", message);
        }
    }
}
