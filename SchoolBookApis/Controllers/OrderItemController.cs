using Azure;

namespace SchoolBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItem;

        public OrderItemController(IOrderItemRepository orderItem)
        {
            _orderItem = orderItem;
        }
        [HttpGet("getorderitem/{id}")]
        public async Task<IActionResult> GetOrderItemAsync(int id)
        {
            var responde=await _orderItem.GetOrderItemAsync(id);
            if(!responde.IsSuccess) 
                return BadRequest(responde);
            return Ok(responde);
        }
        [HttpGet("getorderitemsinorder/{id}")]
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            var responde = await _orderItem.GetAllOrderIteminoOrderAsync(id);
            if (!responde.IsSuccess)
                return BadRequest(responde);
            return Ok(responde);
        }
        [HttpPost("addorderitem")]
        public async Task<IActionResult> AddOrderItemAsync(Models.OrderItem orderItem)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _orderItem.AddOrderItem(orderItem);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPut("deleteorderitem")]
        public async Task<IActionResult> DeleteOrderItemAsync(int id)
        {
            var response= await _orderItem.DeleteOrderItemAsync(id);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
