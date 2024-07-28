namespace SchoolBookShop.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        Task<ResponseDto> GetCartItemAsync(int id);
        Task<ResponseDto> GetAllItemsInCartAsync(int cartid);
        Task<ResponseDto> AddItemInCartAsync(CartItem item);
        Task<ResponseDto> UpdateCartItemAsync(int id, CartItem item);
        Task<ResponseDto> DeleteCartItemAsync(int id);
    }
}
