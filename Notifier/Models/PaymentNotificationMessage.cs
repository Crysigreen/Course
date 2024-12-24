using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifier.Models
{
    public class PaymentNotificationMessage
    {
        public string UserId { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public double Amount { get; set; }
        public string Message { get; set; }
    }
}
