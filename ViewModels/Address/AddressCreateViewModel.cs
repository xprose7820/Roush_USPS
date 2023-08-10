using System.ComponentModel.DataAnnotations;

namespace RoushUSPS_App.ViewModels.Address
{
    public class AddressCreateViewModel
    {
        public string? Address1 { get; set; }
        [Required]
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [StringLength(2)]
        public string State { get; set; }
        
        
        public string? ZipCode5 { get; set; }
        public string? ZipCode4 { get; set; }

    }
}
