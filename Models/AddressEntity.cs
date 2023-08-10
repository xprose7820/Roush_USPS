using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoushUSPS_App.Models
{
    public class AddressEntity
    {
        // based on api, how is it that only address2 is required, address2 can have multiple (cities, states, etc.)
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        // api is confusing
        // would be secondary(apt #, etc.)
        public string? Address1 { get; set; }
        [Required]
        // would be street address(121 main st)
        public string Address2 { get; set; }
        
        public string? City { get; set; }
        
        public string? State { get; set; }
        
        
        public string? ZipCode5 { get; set; }
        public string? ZipCode4 { get; set; }
        [Required]
        public bool isProcessed { get; set; }
        [Required]
        public bool isVerified { get; set; }

    }
}
