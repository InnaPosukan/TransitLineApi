using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using TransitLine.Dto;

public interface IOrderService
{
    Task<IActionResult> CreateOrderWithCargoType(OrderDTO request, ModelStateDictionary modelState, ClaimsPrincipal user);
    Task<IActionResult> UpdateOrder(int orderId, OrderUpdateDTO request, ModelStateDictionary modelState);
    Task<IActionResult> DeleteOrder(int orderId);
    IActionResult GetOrdersByUserId(int userId);

    IActionResult GetOrderById(int orderId);
    IActionResult GetAllOrders();

}