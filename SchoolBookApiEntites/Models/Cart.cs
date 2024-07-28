using System.Collections;

namespace SchoolBookShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }
    }
}
