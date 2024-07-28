namespace SchoolBookShop.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<ResponseDto> GetCartAsync(int id);
        Task<ResponseDto> AddCartAsync(ApplicationUser user);
        Task<ResponseDto> DeleteCartAsync(int id);
    }
}
