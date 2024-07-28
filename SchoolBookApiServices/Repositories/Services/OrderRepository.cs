namespace SchoolBookShop.Repositories.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplictionDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderRepository(ApplictionDbContext context,IMapper mapper,IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
           _userManager = userManager;
        }
        public async Task<ResponseDto> GetCustomerOrders()
        {
            var orders=await _context.Orders.ToListAsync();
            if(orders is null || orders.Count()==0) 
                return new ResponseDto { IsSuccess= false ,Message="this customer not have any orders",StatusCode=400};
            var dto= _mapper.Map<List<OrderDto>>(orders);
            return new ResponseDto { IsSuccess = true, StatusCode = 200, Model = dto };
        }

        public async Task<ResponseDto> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is null )
                return new ResponseDto { IsSuccess = false, Message = "this order not exist", StatusCode = 400 };
            var dto = _mapper.Map<OrderDto>(order);
            return new ResponseDto { IsSuccess = true, StatusCode = 200, Model = dto };
        }
        private async Task<ApplicationUser> GetUser()
        {
            var userclaim = _httpContextAccessor.HttpContext!.User;
            return await _userManager.GetUserAsync(userclaim);
        }
        public async Task<ResponseDto> AddOrder(OrderDto dto)
        {
            var user=await GetUser();
            if(user is null)
                return new ResponseDto { IsSuccess = false, Message = "this user unauthenticated", StatusCode = 400 };
            var order = _mapper.Map<Models.Order>(dto);
            order.UserId = user.Id;
            await _context.AddAsync(order);
            var result= _context.Entry(order);
            if(result.State==EntityState.Added)
                return new ResponseDto { IsSuccess = true, StatusCode = 200, Model = order };
            return new ResponseDto { IsSuccess = false, Message = "the added this order is failed", StatusCode = 400 };

        }
        public async Task<ResponseDto> UpdateOrder(int id, OrderDto dto)
        {
            Models.Order res = await _context.Orders.FindAsync(id);
            if(res is null )
                return new ResponseDto { IsSuccess = false, Message = "this order not exist", StatusCode = 400 };
           var order = _mapper.Map<Models.Order>(dto);
            order.Id=res.Id;
            order.UserId=res.UserId;
            _context.Entry(res).CurrentValues.SetValues(order);
            var result = _context.Entry(res);
            if(result.State==EntityState.Modified)
                return new ResponseDto { IsSuccess = true, StatusCode = 200, Model = res };
            return new ResponseDto { IsSuccess = false, Message = "the update this order is failed", StatusCode = 400 };
        }
        public async Task<ResponseDto> DeleteOrder(int id)
        {
            Models.Order res = await _context.Orders.FindAsync(id);
            if (res is null)
                return new ResponseDto { IsSuccess = false, Message = "this order not exist", StatusCode = 400 };
            var order=_mapper.Map<OrderDto>(res);
            _context.Remove(res);
            var result=_context.Entry(res);
            if(result.State==EntityState.Deleted)
                return new ResponseDto { IsSuccess = true, StatusCode = 200, Model = order };
            return new ResponseDto { IsSuccess = false, Message = "the delete this order is failed", StatusCode = 400 };

        }


    }
}
