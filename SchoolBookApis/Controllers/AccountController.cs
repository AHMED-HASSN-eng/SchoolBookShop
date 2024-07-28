using SchoolBookShop.UOW;

namespace SchoolBookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;

        public AccountController(IUnitOfWork unitOfWork,IAccountRepository accountRepository)
        {
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync(RegisterDto dto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            var user=await _accountRepository.RegisterAsync(dto);
            if(!user.IsAuthenticated) 
                return BadRequest(user.Message);
            return Ok(user.Token);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUserAsync(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _accountRepository.LoginAsync(dto);
            if (!user.IsAuthenticated)
                return BadRequest(user.Message);
            return Ok(user.Token);
        }
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleToUserAsync(AddRolesModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _accountRepository.AddRoleAsync(model);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(model);
        }
    }
}
