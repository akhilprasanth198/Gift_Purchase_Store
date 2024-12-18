﻿namespace Gift_Purchase_Store.Models
{
    public class ShippingAddress
    {
        public int ShippingAddressId { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? AddressLine1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
    }

}
