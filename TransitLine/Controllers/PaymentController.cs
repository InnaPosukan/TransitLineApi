using Microsoft.AspNetCore.Mvc;
using TransitLine.DBContext;
using TransitLine.Interfaces;
using TransitLine.Models;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
    }

    [HttpPost("processPayment")]
    public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
    {
        return await _paymentService.ProcessPayment(payment);
    }
 
}
