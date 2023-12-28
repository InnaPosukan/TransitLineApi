using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransitLine.Dto;



namespace TransitLine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("createOrderWithCargoType")]
        [Authorize]
        public async Task<IActionResult> CreateOrderWithCargoType([FromBody] OrderDTO request)
        {
            var result = await _orderService.CreateOrderWithCargoType(request, ModelState, User);
            return result;
        }

        [HttpPut]
        [Route("updateOrder/{orderId}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderUpdateDTO request)
        {
            var result = await _orderService.UpdateOrder(orderId, request, ModelState);
            return result;
        }

        [HttpGet]
        [Route("getOrderById/{orderId}")]
        [Authorize]
        public IActionResult GetOrderById(int orderId)
        {
            var result = _orderService.GetOrderById(orderId);
            return result;
        }
        [HttpGet]
        [Route("getAllOrders")]
        public IActionResult GetAllOrders()
        {
            var result = _orderService.GetAllOrders();
            return result;
        }
        [HttpDelete]
        [Route("delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            return await _orderService.DeleteOrder(orderId);
        }
        [HttpGet("getOrdersByUserId/{userId}")]
        public IActionResult GetOrdersByUserId(int userId)
        {
            var result = _orderService.GetOrdersByUserId(userId);
            return result;
        }
    }
}