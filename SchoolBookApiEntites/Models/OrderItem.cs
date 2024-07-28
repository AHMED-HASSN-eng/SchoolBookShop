namespace SchoolBookShop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int? BookId { get; set; }
        public int? OrderId { get; set; }
        [ForeignKey("BookId")]
        public Book? Book { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
    }
}
