using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Security.Claims;
using TransitLine.DBContext;
using TransitLine.Dto;
using TransitLine.Models;
using TransitLine.Services;

namespace TransitLine.Services
{
    public class OrderService : IOrderService
    {
        private readonly TransitLineContext _context;

        public OrderService(TransitLineContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> CreateOrderWithCargoType(OrderDTO request, ModelStateDictionary modelState, ClaimsPrincipal user)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (!modelState.IsValid)
                {
                    return new BadRequestObjectResult(modelState);
                }

                var userIdClaim = user.FindFirst("userID");

                if (userIdClaim == null)
                {
                    return new BadRequestObjectResult("User ID not found in the token");
                }

                var userId = Convert.ToInt32(userIdClaim.Value);

                var cargoType = new CargoType
                {
                    CargoWeight = request.CargoWeight,
                    NumberUnits = request.NumberUnits,
                };

                _context.CargoTypes.Add(cargoType);
                await _context.SaveChangesAsync();

                float distanceInKm = request.Distance;

                (float distanceConverted, string unitOfMeasurement) = DistanceConverterService.ConvertDistance(distanceInKm, request.UnitsOfMeasurement);

                var order = new Order
                {
                    IdUser = userId,
                    IdCargo = cargoType.IdCargo,
                    DepartureLocation = request.DepartureLocation,
                    DestinationLocation = request.DestinationLocation,
                    CreationDate = DateTime.Now,
                    OrderStatus = request.OrderStatus ?? "Pending",
                    DriverUserId = request.DriverUserId,
                    Distance = distanceConverted,
                    UnitsOfMeasurement = unitOfMeasurement,

                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return new OkObjectResult(new { cargoType.IdCargo, order.IdOrder, Distance = distanceConverted, OrderId = order.IdOrder });
            }
            catch (Exception ex)
            {
                if (transaction.GetDbTransaction() != null)
                {
                    transaction.Rollback();
                }
                return new BadRequestObjectResult($"Error creating cargo type and order: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
    }

        public async Task<IActionResult> UpdateOrder(int orderId, OrderUpdateDTO request, ModelStateDictionary modelState)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (!modelState.IsValid)
                {
                    return new BadRequestObjectResult(modelState);
                }

                var orderToUpdate = await _context.Orders.FindAsync(orderId);

                if (orderToUpdate == null)
                {
                    return new NotFoundObjectResult($"Order with ID {orderId} not found");
                }

                orderToUpdate.DepartureLocation = request.DepartureLocation ?? orderToUpdate.DepartureLocation;
                orderToUpdate.DestinationLocation = request.DestinationLocation ?? orderToUpdate.DestinationLocation;
                orderToUpdate.OrderStatus = request.OrderStatus ?? orderToUpdate.OrderStatus;
                orderToUpdate.DriverUserId = request.DriverUserId ?? orderToUpdate.DriverUserId;

                await _context.SaveChangesAsync();

                transaction.Commit();

                return new OkObjectResult($"Order with ID {orderId} updated successfully");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new BadRequestObjectResult($"Error updating order: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var orderToDelete = await _context.Orders.FindAsync(orderId);

                if (orderToDelete == null)
                {
                    return new NotFoundObjectResult($"Order with ID {orderId} not found");
                }

                _context.Orders.Remove(orderToDelete);
                var paymentsToDelete = _context.Payments.Where(p => p.IdOrder == orderId);
                _context.Payments.RemoveRange(paymentsToDelete);
                await _context.SaveChangesAsync();

                await _context.SaveChangesAsync();

                transaction.Commit();

                return new OkObjectResult($"Order with ID {orderId} deleted successfully");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new BadRequestObjectResult($"Error deleting order: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }
        public IActionResult GetOrderById(int orderId)
        {
            try
            {
                var orderWithCargoInfo = _context.Orders
                    .Where(o => o.IdOrder == orderId)
                    .Join(
                        _context.CargoTypes,
                        order => order.IdCargo,
                        cargoType => cargoType.IdCargo,
                        (order, cargoType) => new
                        {
                            order.IdOrder,
                            order.DepartureLocation,
                            order.DestinationLocation,
                            order.CreationDate,
                            cargoType.CargoWeight,
                            cargoType.NumberUnits,
                            order.Distance
                        }
                    )
                    .FirstOrDefault();

                if (orderWithCargoInfo == null)
                {
                    return new NotFoundObjectResult($"Order with ID {orderId} not found");
                }

                return new OkObjectResult(orderWithCargoInfo);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error retrieving order: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }
        public IActionResult GetAllOrders()
        {
            try
            {
                var allOrdersWithCargoInfo = _context.Orders
                    .Join(
                        _context.CargoTypes,
                        order => order.IdCargo,
                        cargoType => cargoType.IdCargo,
                        (order, cargoType) => new
                        {
                            order.IdOrder,
                            order.DepartureLocation,
                            order.DestinationLocation,
                            order.CreationDate,
                            cargoType.CargoWeight,
                            cargoType.NumberUnits,
                            order.Distance,
                            order.UnitsOfMeasurement,
                            order.OrderStatus,
                            order.DriverUserId
                        }
                    )
                    .ToList();

                if (allOrdersWithCargoInfo.Count == 0)
                {
                    return new NotFoundObjectResult("No orders found");
                }

                return new OkObjectResult(allOrdersWithCargoInfo);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error retrieving all orders: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }
        public IActionResult GetOrdersByUserId(int userId)
        {
            try
            {
                var ordersByUser = _context.Orders
                    .Where(o => o.IdUser == userId)
                    .Join(
                        _context.CargoTypes,
                        order => order.IdCargo,
                        cargoType => cargoType.IdCargo,
                        (order, cargoType) => new
                        {
                            order.IdOrder,
                            order.DepartureLocation,
                            order.DestinationLocation,
                            order.CreationDate,
                            cargoType.CargoWeight,
                            cargoType.NumberUnits,
                            order.Distance,
                            order.UnitsOfMeasurement,
                            order.OrderStatus,
                            order.DriverUserId,
                            Payments = _context.Payments.Where(p => p.IdOrder == order.IdOrder).ToList(),
                            Delivery = _context.Deliveries.FirstOrDefault(d => d.IdOrder == order.IdOrder)
                        }
                    )
                    .ToList();

                if (ordersByUser.Count == 0)
                {
                    return new NotFoundObjectResult($"No orders found for user with ID {userId}");
                }

                return new OkObjectResult(ordersByUser);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error retrieving orders for user: {ex.Message}, InnerException: {ex.InnerException?.Message}");
            }
        }

    }
}
