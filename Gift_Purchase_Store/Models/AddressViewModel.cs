using System.ComponentModel.DataAnnotations;

namespace Gift_Purchase_Store.Models
{
    public class AddressViewModel
    {
        [Required]
        public string AddressLine1 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }

}
