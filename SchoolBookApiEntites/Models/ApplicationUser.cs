using Microsoft.AspNetCore.Authentication.Cookies;

namespace SchoolBookShop.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Book>? Books { get; set; } = new List<Book>();
        public ICollection<Models.Order>? Orders { get; set; }
    }
}
