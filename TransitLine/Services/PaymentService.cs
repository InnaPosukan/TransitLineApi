using Microsoft.AspNetCore.Mvc;
using TransitLine.DBContext;
using TransitLine.Interfaces;
using TransitLine.Models;

namespace TransitLine.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly TransitLineContext _context;

        public PaymentService(TransitLineContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> ProcessPayment(Payment payment)
        {
            try
            {
                var newPayment = new Payment
                {
                   
                    IdOrder = payment.IdOrder,
                    Amount = payment.Amount,
                    PaymentStatus = "Not Paid",
                    PaymentMethod = payment.PaymentMethod,
                    Currency = payment.Currency,
                };

                _context.Payments.Add(newPayment);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { Id = newPayment.IdPayment,  newPayment.Amount });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling payment: {ex}");
                return new BadRequestObjectResult($"Error handling payment: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }
    }
}
