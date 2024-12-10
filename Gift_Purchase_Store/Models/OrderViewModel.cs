namespace Gift_Purchase_Store.Models
{
    public class OrderViewModel

    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public string PaymentMethod { get; set; }
        // New fields for address
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
