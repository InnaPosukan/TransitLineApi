using Microsoft.AspNetCore.Mvc;
using TransitLine.DTO;
using TransitLine.Interfaces;
using TransitLine.Models;
using TransitLine.Services;


namespace TransitLine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementController : ControllerBase
    {
        private readonly IManagementService _managementService;

        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService ?? throw new ArgumentNullException(nameof(managementService));
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            return await _managementService.AddUser(user, HttpContext.User);
        }


        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            return _managementService.GetAllUsers();
        }
        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] User updatedUser)
        {
            return await _managementService.UpdateUser(userId, updatedUser);
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            return await _managementService.DeleteUser(userId);
        }

        [HttpPut("AssignDriverToOrder")]
        public async Task<IActionResult> AssignDriverToOrder([FromBody] AssignDriverDTO request)
        {
            return await _managementService.AssignDriverToOrder(request);
        }

        [HttpPut("RejectOrder")]
        public async Task<IActionResult> RejectOrder([FromBody] Order order)
        {
            return await _managementService.RejectOrder(order, HttpContext.User);
        }
        [HttpGet("GetAllDeliveries")]
        public IActionResult GetAllDeliveries()
        {
            return _managementService.GetAllDeliveries();
        }

        [HttpGet("GetAllDriversWithTrucks")]
        public IActionResult GetAllDriversWithCars()
        {
            return _managementService.GetAllDriversWithTrucks();
        }

    }
}
