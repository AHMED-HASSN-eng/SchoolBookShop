namespace SchoolBookShop.Dtos.Order
{
    public class OrderDto
    {
        public DateTime? Date { get; set; }
        public string? Status { get; set; }
        public DateTime? DeliverDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public String? ShippingMethod { get; set; }
        public String? ShippingAddress { get; set; }
        public int? ShippingCode { get; set; }
        public double ShippingCost { get; set; }
    }
}
