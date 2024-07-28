
namespace SchoolBookShop.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<ResponseDto> GetOrderItemAsync(int id);
        Task<ResponseDto> GetAllOrderIteminoOrderAsync(int orderid);
        Task<ResponseDto> AddOrderItem(OrderItem item);
        Task<ResponseDto> DeleteOrderItemAsync(int id);
    }
}
