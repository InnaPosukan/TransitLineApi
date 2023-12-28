using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TransitLine.DBContext;
using TransitLine.DTO;
using TransitLine.Models;
using TransitLine.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace TransitLine.Services
{
    public class DriverService : IDriverService
    {
        private readonly TransitLineContext _context;
        private readonly IUserService _userService;

        public DriverService(TransitLineContext context, IUserService userService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<IActionResult> AddTruckAsync(Truck truck, ClaimsPrincipal userClaims)
        {
            try
            {
                var requestingUserId = userClaims.FindFirstValue("userID");
                var user = _userService.GetRequestingUser(requestingUserId);

                if (user == null || user.Role != "Driver")
                {
                    return new BadRequestObjectResult("Only drivers can add trucks.");
                }

                truck.UserId = user.IdUser;
                truck.Capacity = truck.Capacity;
                truck.Height = truck.Height;
                truck.Model = truck.Model;
                truck.CarNumber = truck.CarNumber;

                _context.Trucks.Add(truck);
                await _context.SaveChangesAsync();

                return new OkObjectResult("Truck added successfully.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        public async Task<IActionResult> DeleteTruckAsync(int truckId, ClaimsPrincipal userClaims)
        {
            try
            {
                var userIdString = userClaims.FindFirstValue("userID");
                if (int.TryParse(userIdString, out int userId))
                {
                    var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == userId);

                    if (currentUser != null)
                    {
                        var truckToDelete = await _context.Trucks
                            .Include(t => t.User)
                            .FirstOrDefaultAsync(t => t.IdTruck == truckId);

                        if (truckToDelete == null)
                        {
                            return new NotFoundObjectResult($"Truck with ID {truckId} not found.");
                        }

                        _context.Trucks.Remove(truckToDelete);
                        await _context.SaveChangesAsync();

                        return new OkObjectResult("Truck deleted successfully.");
                    }
                    else
                    {
                        return new NotFoundObjectResult($"User with ID {userId} not found.");
                    }
                }
                else
                {
                    return new BadRequestObjectResult("Invalid user ID format.");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }




        public async Task<IActionResult> GetOrdersForDriver(ClaimsPrincipal userClaims)
        {
            try
            {
                var userIdClaim = userClaims.FindFirstValue("userID");

                if (userIdClaim == null)
                {
                    return new BadRequestObjectResult("Unable to retrieve user information from token");
                }

                string driverUserId = userIdClaim;

                var ordersForDriver = await _context.Orders
                    .Where(o => o.DriverUserId.ToString() == driverUserId)
                    .ToListAsync();

                if (ordersForDriver.Count == 0)
                {
                    return new NotFoundObjectResult($"No orders found for current driver with ID {driverUserId}");
                }

                return new OkObjectResult(ordersForDriver);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }


        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                var order = await _context.Orders
                    .Include(o => o.Deliveries)  // Include the Delivery related data
                    .Include(o => o.Payments)   // Include the Payment related data
                    .FirstOrDefaultAsync(o => o.IdOrder == orderId);

                if (order == null)
                {
                    return new NotFoundObjectResult($"Order with ID {orderId} not found");
                }

                var jsonResult = new OkObjectResult(JsonSerializer.Serialize(order, options));
                return jsonResult;
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }


        public async Task<IActionResult> UpdateDeliveryStatusAsync(int deliveryId, UpdateDeliveryStatusDTO request, ClaimsPrincipal userClaims)
        {
            try
            {
                var requestingUserId = userClaims.FindFirstValue("userID");

                if (!string.IsNullOrEmpty(requestingUserId))
                {
                    var driver = _userService.GetRequestingUser(requestingUserId);

                    if (driver != null && driver.Role == "Driver")
                    {
                        var order = await _context.Orders
                            .Include(o => o.Deliveries)
                            .Where(o => o.Deliveries.Any(d => d.IdDelivery == deliveryId) && o.DriverUserId == driver.IdUser)
                            .FirstOrDefaultAsync();

                        if (order != null)
                        {
                            var delivery = order.Deliveries.FirstOrDefault(d => d.IdDelivery == deliveryId);

                            if (delivery != null)
                            {
                                delivery.DeliveryStatus = request.DeliveryStatus;

                                if (request.DeliveryStatus.Equals("Delivered", StringComparison.OrdinalIgnoreCase))
                                {
                                    order.OrderStatus = "Delivered";
                                    _context.Deliveries.Remove(delivery);
                                }

                                await _context.SaveChangesAsync();

                                return new OkObjectResult("Delivery status successfully updated.");
                            }
                            else
                            {
                                return new BadRequestObjectResult("Delivery not found for the specified order.");
                            }
                        }
                        else
                        {
                            return new BadRequestObjectResult("Order not found or user does not have permission to update the status.");
                        }
                    }
                    else
                    {
                        return new BadRequestObjectResult("Invalid requesting user or user does not have the required role (Driver).");
                    }
                }

                return new BadRequestObjectResult("Invalid requesting user or user ID.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error updating delivery status: {ex.Message}");
            }
        }
    }
}