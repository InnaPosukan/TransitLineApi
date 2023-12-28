using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using TransitLine.DBContext;
using TransitLine.DTO;
using TransitLine.Models;
using TransitLine.Tools;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;

namespace TransitLine.Services
{
    public class ManagementService : IManagementService
    {
        private readonly TransitLineContext _context;

        public ManagementService(TransitLineContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IActionResult> AddUser(User user, ClaimsPrincipal userClaims)
        {
            try
            {
                var requestingUserId = userClaims.FindFirstValue("userID");
                if (string.IsNullOrEmpty(requestingUserId))
                    return new ForbidResult();

                var requestingUser = await _context.Users.FirstOrDefaultAsync(u => u.IdUser.ToString() == requestingUserId);

                if (requestingUser == null)
                    return new ForbidResult();



                if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                    return new BadRequestObjectResult("Username already exists");

                user.Password = Password.hashPassword(user.Password);

                if (string.IsNullOrEmpty(user.Role))
                    return new BadRequestObjectResult("Role cannot be empty");

                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                return new OkObjectResult($"User is successfully registered with role: {user.Role}");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }



        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _context.Users
                    .Where(u => u.Role != "Admin")
                    .Select(u => new
                    {
                        u.IdUser,
                        u.Email,
                        u.Role,
                        u.LastName,
                        u.FirstName,
                        u.PhoneNumber
                    })
                    .ToList();

                var responseObject = new
                {
                    Users = users
                };

                return new OkObjectResult(responseObject);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<IActionResult> UpdateUser(int userId, User updatedUser)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(userId);

                if (existingUser == null)
                {
                    return new NotFoundObjectResult($"User with ID {userId} not found");
                }

                existingUser.Email = updatedUser.Email;
                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;

                await _context.SaveChangesAsync();

                return new OkObjectResult("User updated successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var userToDelete = await _context.Users.FindAsync(userId);

                if (userToDelete == null)
                {
                    return new NotFoundObjectResult($"User with ID {userId} not found");
                }

                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();

                return new OkObjectResult("User deleted successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<IActionResult> AssignDriverToOrder(AssignDriverDTO request)
        {
            try
            {
                if (!IsValidId(request.DriverUserId))
                {
                    return new BadRequestObjectResult("Invalid DriverUserId. Please provide a valid positive integer.");
                }

                // Validate if the Order ID is provided
                if (!IsValidId(request.IdOrder))
                {
                    return new BadRequestObjectResult("Invalid IdOrder. Please provide a valid positive integer.");
                }

                // Search for the order in the database
                var order = await _context.Orders.FindAsync(request.IdOrder);

                // If the order is not found, return a not found response
                if (order == null)
                {
                    return new NotFoundObjectResult($"Order with ID {request.IdOrder} not found");
                }

                // Update order details
                order.DriverUserId = request.DriverUserId;
                order.OrderStatus = "Transfer to delivery service";

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Create a new delivery entry
                var delivery = new Delivery
                {
                    IdOrder = order.IdOrder,
                    DeliveryStatus = "Getting ready to ship",
                    DepartureDate = DateTime.Now,
                    DestinationDate = DateTime.Now.AddDays(7)
                };

                _context.Deliveries.Add(delivery);
                await _context.SaveChangesAsync();

                return new OkObjectResult("Driver assigned successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }


        private bool IsValidId(int id)
        {
            return id > 0;
        }


        [HttpGet("GetAllDriversWithTrucks")]
        public IActionResult GetAllDriversWithTrucks()
        {
            try
            {
                var usersWithTrucks = _context.Users
                    .Where(u => u.Role == "Driver")
                    .Select(user => new
                    {
                        IdUser = user.IdUser, 
                        user.FirstName,
                        user.LastName,
                        user.PhoneNumber,
                        Trucks = _context.Trucks
                            .Where(truck => truck.UserId == user.IdUser)
                            .Select(truck => new
                            {
                                TruckId = truck.IdTruck,
                                TruckModel = truck.Model,
                                TruckNumber = truck.CarNumber,
                                TruckLength = truck.Length,
                                TruckHeight = truck.Height,
                                TruckCapacity = truck.Capacity
                            })
                            .ToList()
                    })
                    .ToList();

                var responseObject = new
                {
                    Users = usersWithTrucks
                };

                return new OkObjectResult(responseObject);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error rejecting order: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }

        public async Task<IActionResult> RejectOrder(Order order, ClaimsPrincipal userClaims)
        {
            try
            {
                if (order == null || order.IdOrder <= 0)
                    return new BadRequestObjectResult("Invalid order data in the request.");

                var existingOrder = await _context.Orders.FindAsync(order.IdOrder);

                if (existingOrder == null)
                    return new NotFoundObjectResult($"Order with ID {order.IdOrder} not found");

                var requestingUserId = userClaims.FindFirstValue("userID");
                if (string.IsNullOrEmpty(requestingUserId))
                    return new ForbidResult();

                var requestingUser = await _context.Users.FirstOrDefaultAsync(u => u.IdUser.ToString() == requestingUserId);

                if (requestingUser == null || (requestingUser.Role != "Admin" && requestingUser.Role != "Manager" && existingOrder.IdUser != requestingUser.IdUser))
                    return new ForbidResult("You do not have permission to reject this order");

                existingOrder.OrderStatus = "Rejected";
                await _context.SaveChangesAsync();

                return new OkObjectResult($"Order with ID {order.IdOrder} has been rejected. Order status updated to 'Rejected'.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error rejecting order: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }
        public IActionResult GetAllDeliveries()
        {
            try
            {
                var deliveries = _context.Deliveries
                    .Select(delivery => new
                    {
                        IdDelivery = delivery.IdDelivery,
                        IdOrder = delivery.IdOrder,
                        DeliveryStatus = delivery.DeliveryStatus,
                        DepartureDate = delivery.DepartureDate,
                        DestinationDate = delivery.DestinationDate,
                        TemperatureHumidities = delivery.TemperatureHumidities
                            .Select(th => new
                            {
                                IdTemperatureHumidity = th.IdTemperatureHumidity,
                                Temperature = th.Temperature,
                                Humidity = th.Humidity,
                                Timestamp = th.Timestamp,
                            })
                            .ToList()
                    })
                    .ToList();

                var responseObject = new
                {
                    Deliveries = deliveries
                };

                return new OkObjectResult(responseObject);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error retrieving deliveries: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }

    }
}
