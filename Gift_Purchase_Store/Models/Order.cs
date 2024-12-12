namespace Gift_Purchase_Store.Models
{
    public class Order //Domain Model
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public int? ShippingAddressId { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }

    }
}
