using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SchoolBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _order;

        public OrderController(IOrderRepository order)
        {
            _order = order;
        }
        [HttpGet("getorder/{id}")]
        public async Task< IActionResult> GetOrderAsync(int id) 
        {
            var response = await _order.GetOrder(id);
            if(!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("getuserorder")]
        public async Task<IActionResult> GetUserOrderAsync()
        {
            var response = await _order.GetCustomerOrders();
            if(!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPost("addorder")]
        public async Task<IActionResult> AddOrderAsync(OrderDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _order.AddOrder( dto);
            if(!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPut("updateorder")]
        public async Task<IActionResult> UpdateOrderAsync(int id,OrderDto dto)
        {
             if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _order.UpdateOrder(id, dto);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpDelete("deleteorder")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            var response=await _order.DeleteOrder(id);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
