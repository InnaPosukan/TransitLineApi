using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TransitLine.DTO;
using TransitLine.Models;
using System.Threading.Tasks;

namespace TransitLine.Interfaces
{
    public interface IDriverService
    {
        Task<IActionResult> AddTruckAsync(Truck truck, ClaimsPrincipal userClaims);
        Task<IActionResult> DeleteTruckAsync(int truckId, ClaimsPrincipal userClaims);
        Task<IActionResult> GetOrdersForDriver(ClaimsPrincipal userClaims);
        Task<IActionResult> GetOrderById(int orderId);
        Task<IActionResult> UpdateDeliveryStatusAsync(int deliveryId, UpdateDeliveryStatusDTO request, ClaimsPrincipal userClaims);
    }
}
