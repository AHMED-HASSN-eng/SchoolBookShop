using SchoolBookShop.Dtos.Cart;
using SchoolBookShop.Models;

namespace SchoolBookShop.Repositories.Services
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplictionDbContext _context;
        private readonly IMapper _mapper;

        public CartRepository(ApplictionDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto> GetCartAsync(int id)
        {
            var cart=await _context.Carts.FindAsync(id);
            if(cart == null) 
                return new ResponseDto { IsSuccess = false, Message = "this cart you try to Get it is Not found", StatusCode = 400 };
            return new ResponseDto { IsSuccess = true,StatusCode=200,Model= cart};
        }
        public async Task<ResponseDto> AddCartAsync(ApplicationUser user)
        {
            CartDto dto = new()
            {
                UserId=user.Id
            };
            var cart= _mapper.Map<CartRepository>(dto);    
           await _context.AddAsync(cart);
            var result=_context.Entry(cart);
            if(result.State==EntityState.Added)
                return new ResponseDto { IsSuccess = true, StatusCode = 200, Model = cart };
            return new ResponseDto { IsSuccess = false, Message = "Failed to add this cart", StatusCode = 400 };

        }
        public async Task<ResponseDto> DeleteCartAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
                return new ResponseDto { IsSuccess = false, Message = "this cart you try to Delete it is Not found", StatusCode = 400 };
             _context.Remove(cart);
            var result = _context.Entry(cart);
            if (result.State == EntityState.Deleted)
                return new ResponseDto { IsSuccess = true, StatusCode = 200, Model = cart };
            return new ResponseDto { IsSuccess = false, Message = "Failed to Delete this cart", StatusCode = 400 };
        }


    }
}
