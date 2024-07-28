namespace SchoolBookShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Status { get; set; }
        public DateTime? DeliverDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public String? ShippingMethod { get; set; }
        public String? ShippingAddress { get; set; }
        public int? ShippingCode { get; set; }
        public double ShippingCost { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser{ get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
