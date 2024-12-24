using Course.Models;

using Microsoft.AspNetCore.Mvc;
using Course.Services.Contarcts;
using Course.AuditLogger;

namespace Course.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        // GET: api/Subscriptions
        [HttpGet]
        public IActionResult GetAll()
        {
            var subscriptions = _subscriptionService.GetAllSubscriptions();
            if (subscriptions == null || !subscriptions.Any())
            {
                return NotFound("No subscriptions found.");
            }

            return Ok(subscriptions);
        }

        // GET: api/Subscriptions/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var subscription = _subscriptionService.GetSubscriptionById(id);
            if (subscription == null)
            {
                return NotFound($"Subscription with ID {id} not found.");
            }

            return Ok(subscription);
        }

        // POST: api/Subscriptions
        [HttpPost]
        public IActionResult Create([FromBody] Subscription subscription)
        {
            if (subscription == null)
            {
                return BadRequest("Subscription data is required.");
            }

            _subscriptionService.AddSubscription(subscription);

            using (var auditProducer = new AuditProducer())
            {
                auditProducer.SendAuditMessage($"Subscription created: UserId={subscription.UserId}, CourseId={subscription.CourseId}");
            }

            return CreatedAtAction(nameof(GetById), new { id = subscription.Id }, subscription);
        }

        // PUT: api/Subscriptions/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Subscription updatedSubscription)
        {
            if (updatedSubscription == null)
            {
                return BadRequest("Updated subscription data is required.");
            }

            var subscription = _subscriptionService.GetSubscriptionById(id);
            if (subscription == null)
            {
                return NotFound($"Subscription with ID {id} not found.");
            }

            _subscriptionService.UpdateSubscription(id, updatedSubscription);
            return NoContent(); // 204 No Content
        }

        // DELETE: api/Subscriptions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var subscription = _subscriptionService.GetSubscriptionById(id);
            if (subscription == null)
            {
                return NotFound($"Subscription with ID {id} not found.");
            }

            _subscriptionService.DeleteSubscription(id);
            return NoContent(); // 204 No Content
        }
    }
}
