using SchoolBookShop.Dtos.Cart;

namespace SchoolBookShop.Repositories.Services
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplictionDbContext _context;
        private readonly IMapper _mapper;

        public CartItemRepository(ApplictionDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
       public async Task<ResponseDto> GetCartItemAsync(int id)
        {
            var item= await _context.CartItems.FindAsync(id);
            if(item == null)return new ResponseDto { IsSuccess= false ,Message="this item not exist",StatusCode=400};
            var dto= _mapper.Map<CartItemDto>(item);
            return new ResponseDto
            {
                IsSuccess=true,
                StatusCode=200,
                Model=dto
            };
        }

        public async Task<ResponseDto> GetAllItemsInCartAsync(int cartid)
        {
            var items= await _context.CartItems.Where(x=>x.CartId==cartid).ToListAsync();   
            if(items is null || items.Count==0) 
                return new ResponseDto { IsSuccess = false, Message = "this cart is empety", StatusCode = 400 };
            var dto=_mapper.Map<List<CartItemDto>>(items);
            return new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                Model = dto
            };
        }

        public async Task<ResponseDto> AddItemInCartAsync(Models.CartItem item)
        {
            if(!await _context.Carts.AnyAsync(c=>c.Id==item.CartId))
                return new ResponseDto { IsSuccess = false, Message = "this cart you try to add to it is Not found", StatusCode = 400 };
            if (!await _context.CartItems.AnyAsync(c => c.Id == item.Id))
                return new ResponseDto { IsSuccess = false, Message = "this item you try to add it is Not found", StatusCode = 400 };
            item.Cart = await _context.Carts.FindAsync(item.CartId);
            item.Book=await _context.Books.FindAsync(item.BookId);
             await _context.AddAsync(item);
            var result = _context.Entry(item);
            if(result.State==EntityState.Added)
                return new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Model = item
                };
            return new ResponseDto { IsSuccess = false, Message = "Something Wrong While add this item to cart", StatusCode = 400 };

        }

        public async Task<ResponseDto> UpdateCartItemAsync(int id, Models.CartItem item)
        {
            var cartitem = await _context.CartItems.FindAsync(id);
            if (cartitem is null)
                return new ResponseDto { IsSuccess = false, Message = "this item you try to update it is Not found", StatusCode = 400 };
            //item.Id = cartitem.Id;
            //item.CartId = cartitem.CartId;
            //item.BookId = cartitem.BookId;
             _context.Entry(cartitem).CurrentValues.SetValues(item);
            var result =  _context.Entry(cartitem);
            if(result.State==EntityState.Modified)
                return new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Model = cartitem
                };
            return new ResponseDto { IsSuccess = false, Message = "Something Wrong While update this item to cart", StatusCode = 400 };

        }

        public async Task<ResponseDto> DeleteCartItemAsync(int id)
        {
            var cartitem = await _context.CartItems.FindAsync(id);
            if (cartitem is null)
                return new ResponseDto { IsSuccess = false, Message = "this item you try to Delete it is Not found", StatusCode = 400 };
             _context.CartItems.Remove(cartitem);
            var result = _context.Entry(cartitem);
            if (result.State == EntityState.Deleted)
                return new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Model = cartitem
                };
            return new ResponseDto { IsSuccess = false, Message = "Something Wrong While delete this item to cart", StatusCode = 400 };
        }
    }
}
