
namespace SchoolBookShop.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string StudyYear { get; set; }
        public string Description { get; set; }
        public string CompanyName  { get; set; }
        public string Subject { get; set;}
        public DateTime Date { get; set; }
        public int Rate { get; set;}
        public double Price { get; set;}
        //public List<BookPhoto> Photos { get; set;}
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public virtual ICollection<BookPhoto>?BookPhotos { get; set; }=new LinkedList<BookPhoto>();
        public virtual ICollection<OrderItem>?OrderItems { get; set; }
        public virtual ICollection<CartItem>?CartItems { get; set; }
    }
}
