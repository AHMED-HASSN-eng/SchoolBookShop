using Microsoft.EntityFrameworkCore;
using SchoolBookShop.Dtos.Order;
using SchoolBookShop.Models;

namespace SchoolBookShop.Repositories.Services
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplictionDbContext _context;
        private readonly IMapper _mapper;

        public OrderItemRepository(ApplictionDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto> GetAllOrderIteminoOrderAsync(int orderid)
        {
            var order = await _context.OrderItems.Where(o=>o.OrderId== orderid).ToListAsync();
            if (order is null || order.Count() == 0)
                return new ResponseDto { IsSuccess = false, StatusCode = 400, Message = "this order not have any item" };
            var dto= _mapper.Map<OrderItemDto>(order);
            return new ResponseDto
            {
                IsSuccess= true,
                Model= dto,
                StatusCode=200
            };
        }
        public async Task<ResponseDto> GetOrderItemAsync(int id)
        {
            var order = await _context.OrderItems.Where(o => o.OrderId ==id).ToListAsync();
            if (order is null || order.Count() == 0)
                return new ResponseDto { IsSuccess = false, StatusCode = 400, Message = "this orderitem not exist" };
            var dto = _mapper.Map<OrderItemDto>(order);
            return new ResponseDto
            {
                IsSuccess = true,
                Model = dto,
                StatusCode = 200
            };
        }
        public async Task<ResponseDto> AddOrderItem(Models.OrderItem item)
        {
            if (! await _context.Orders.AnyAsync(o => o.Id == item.OrderId))
                return new ResponseDto { IsSuccess = false, StatusCode = 400, Message = "this order you try to add on it  not exist" };
            if (!await _context.Books.AnyAsync(o => o.Id == item.BookId))
                return new ResponseDto { IsSuccess = false, StatusCode = 400, Message = "this Book you try to add on it  not exist" };
             item.Order =  await _context.Orders.FindAsync( item.OrderId);
             item.Book = await _context.Books.FindAsync(item.BookId);
            await _context.AddAsync(item);
            var result= _context.Entry(item);
            if (result.State == EntityState.Added)
                return new ResponseDto
                {
                    StatusCode=200,
                    Model=item,
                    IsSuccess=true
                };
            return new ResponseDto 
            { IsSuccess = false, StatusCode = 400, Message = "the added book is failed" };

        }
       
        public async Task<ResponseDto> DeleteOrderItemAsync(int id)
        {
            var order=await _context.OrderItems.FindAsync(id);
            if(order is null)
                return new ResponseDto { IsSuccess = false, StatusCode = 400, Message = "this orderitem not exist" };
             _context.Remove(order);
            var result = _context.Entry(order);
            if (result.State == EntityState.Deleted)
                return new ResponseDto
                {
                    StatusCode = 200,
                    Model = order,
                    IsSuccess = true
                };
            return new ResponseDto
            { IsSuccess = false, StatusCode = 400, Message = "the Deleted book is failed" };
        }


    }
}
