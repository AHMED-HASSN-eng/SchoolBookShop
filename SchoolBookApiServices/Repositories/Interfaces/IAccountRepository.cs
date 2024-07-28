namespace SchoolBookShop.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Authinformation> RegisterAsync(RegisterDto dto);
        Task<Authinformation> LoginAsync(LoginDto dto);
        Task<string> AddRoleAsync(AddRolesModel addrole);
    }
}
