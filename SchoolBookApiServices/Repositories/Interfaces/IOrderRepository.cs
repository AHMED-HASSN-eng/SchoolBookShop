namespace SchoolBookShop.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<ResponseDto> GetCustomerOrders();
        Task<ResponseDto> GetOrder(int id);
        Task<ResponseDto> AddOrder(OrderDto order);
        Task<ResponseDto> UpdateOrder(int id, OrderDto order);
        Task<ResponseDto> DeleteOrder(int id);
    }
}
