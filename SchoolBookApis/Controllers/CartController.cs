using SchoolBookShop.UOW;

namespace SchoolBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UnitOfWork _unitOfWork;

        public CartController(ICartRepository cartRepository,UserManager<ApplicationUser> userManager,
            UnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        [HttpGet("getcart/{id}")]
        public async Task<IActionResult> GetCartAsync(int id)
        {
            var response= await _cartRepository.GetCartAsync(id);
            if(!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPost("addcart")]
        public async Task<IActionResult> AddCartsAsync()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _cartRepository.AddCartAsync( await _userManager.GetUserAsync(User));
            if (!response.IsSuccess)
                return BadRequest(response);
            await _unitOfWork.Save();
            return Ok(response);
        }
        [HttpDelete("deletecart")]
        public async Task<IActionResult> DeleteCartAsync(int id)
        {
            var response =await _cartRepository.DeleteCartAsync(id);
            if (!response.IsSuccess)
                return BadRequest(response);
            await _unitOfWork.Save();
            return Ok(response);
        }
    }
}
