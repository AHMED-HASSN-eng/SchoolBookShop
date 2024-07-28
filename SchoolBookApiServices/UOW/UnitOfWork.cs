namespace SchoolBookShop.UOW
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ApplictionDbContext _context;
        public UnitOfWork(ApplictionDbContext context)
        {
            _context = context;
        }
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly JwtOptions _jwtoptions;
        //private readonly IMapper _mapper;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        //public IAccountRepository AccountRepository { get; private set; }

        //public IBookRepository BookRepository { get; private set; }

        //public ICartItemRepository CartItemRepository { get; private set; }

        //public ICartRepository CartRepository { get; private set; }

        //public IOrderItemRepository OrderItemRepository { get; private set; }

        //public IOrderRepository OrderRepository { get; private set; }
        //public UnitOfWork(ApplictionDbContext context,IHttpContextAccessor httpContextAccessor,JwtOptions jwtoptions,
        //    IMapper mapper,UserManager<ApplicationUser>userManager,RoleManager<IdentityRole> roleManager)
        //{
        //    _context = context;
        //    _httpContextAccessor = httpContextAccessor;
        //    _jwtoptions = jwtoptions;
        //    _mapper = mapper;
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //    AccountRepository = new AccountRepository(_userManager,_jwtoptions,_roleManager,_mapper);
        //    BookRepository = new BookRepository(_context, _mapper, _httpContextAccessor, _userManager);
        //    CartItemRepository = new CartItemRepository(_context, _mapper);
        //    CartRepository = new CartRepository(_context,_mapper);
        //    OrderItemRepository = new OrderItemRepository(_context, _mapper);
        //    OrderRepository = new OrderRepository(_context, _mapper, _httpContextAccessor, _userManager);

        //}

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        //public void Dispose()
        //{
        //    _context.Dispose();
        //}
    }
}
