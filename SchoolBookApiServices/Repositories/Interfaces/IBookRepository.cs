namespace SchoolBookShop.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<ResponseDto> GetBookAsync(int id);
        Task<ResponseDto> GetAllBookAsync();
        Task<ResponseDto> GetBookByPrice(int price);
        Task<ResponseDto> GetBookBySearch(string searchtext);
        Task<ResponseDto>AddBookAsync(AddBookDto dto);
        Task<ResponseDto> UpdateBook(int id,UpdateDto dto);
        Task<ResponseDto> DeleteBook(int id);
    }
}
