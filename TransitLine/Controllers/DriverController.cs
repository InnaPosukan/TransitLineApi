using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransitLine.DTO;
using TransitLine.Interfaces;
using TransitLine.Models;
using System;
using System.Threading.Tasks;

namespace TransitLine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService ?? throw new ArgumentNullException(nameof(driverService));
        }

        [HttpPost("AddTruck")]
        public async Task<IActionResult> AddTruck([FromBody] Truck truck)
        {
            var userClaims = HttpContext.User;

            return await _driverService.AddTruckAsync(truck, userClaims);
        }

        [HttpDelete("DeleteTruck/{truckId}")]
        public async Task<IActionResult> DeleteTruck(int truckId)
        {
            var userClaims = HttpContext.User;

            return await _driverService.DeleteTruckAsync(truckId, userClaims);
        }


        [HttpGet("GetOrdersForDriver")]
        [Authorize]
        public async Task<IActionResult> GetOrdersForDriver()
        {
            return await _driverService.GetOrdersForDriver(User);
        }

        [HttpGet("GetOrderById/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            return await _driverService.GetOrderById(orderId);
        }


        [Authorize]
        [HttpPut]
        [Route("UpdateDeliveryStatus/{deliveryId}")]
        public async Task<IActionResult> UpdateDeliveryStatus(int deliveryId, [FromBody] UpdateDeliveryStatusDTO request)
        {
            var userClaims = HttpContext.User;

            return await _driverService.UpdateDeliveryStatusAsync(deliveryId, request, userClaims);
        }
    }
}
