using Microsoft.AspNetCore.Mvc;
using TransitLine.Models;

namespace TransitLine.Interfaces
{
    public interface IPaymentService
    {
        Task<IActionResult> ProcessPayment(Payment payment);

    }

}
