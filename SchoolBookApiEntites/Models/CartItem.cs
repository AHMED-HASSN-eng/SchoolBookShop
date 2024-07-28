namespace SchoolBookShop.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int? BookId { get; set; }

        public int? CartId { get; set; }

        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }
        [ForeignKey("CartId")]
        public virtual Cart? Cart { get; set; }
    }
}
