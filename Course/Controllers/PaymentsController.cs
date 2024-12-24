using Course.Models;
using Course.Services.Contarcts;
using Microsoft.AspNetCore.Mvc;

namespace Course.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_paymentService.GetAllPayments());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var payment = _paymentService.GetPaymentById(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Payment payment)
        {
            _paymentService.ProcessPayment(payment.UserId, payment.CourseId, payment.Amount);
            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Payment payment)
        {
            _paymentService.UpdatePayment(id, payment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _paymentService.DeletePayment(id);
            return NoContent();
        }
    }
}
