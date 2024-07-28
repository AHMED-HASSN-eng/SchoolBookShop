
using Microsoft.AspNetCore.Mvc;

namespace SchoolBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository _cartItem;

        public CartItemController(ICartItemRepository cartItem)
        {
            _cartItem = cartItem;
        }
        [HttpGet("getcartitem/{id}")]
        public async Task<IActionResult> GetCartItemAsync(int id)
        {
            var response = await _cartItem.GetCartItemAsync(id);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("getallcartitemincart/{id}")]
        public async Task<IActionResult> GetAllCartitemsInCartAsync(int id)
        {
            var response = await _cartItem.GetAllItemsInCartAsync(id);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPost("addcartitrm")]
        public async Task<IActionResult> AddCartItemAsync(Models.CartItem cartItem)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _cartItem.AddItemInCartAsync(cartItem);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPut("updatecartitem")]
        public async Task<IActionResult> UpdateCartitemAsync(int id,Models.CartItem cartItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _cartItem.UpdateCartItemAsync(id,cartItem);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpDelete("deletecartitem/{id}")]
        public async Task<IActionResult> DeleteCartItemAsync(int id)
        {
            var response = await _cartItem.DeleteCartItemAsync(id);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
