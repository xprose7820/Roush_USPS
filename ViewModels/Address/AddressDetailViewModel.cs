namespace RoushUSPS_App.ViewModels.Address
{
    public class AddressDetailViewModel
    {

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }
        
        
        public string ZipCode5 { get; set; }
        public string ZipCode4 { get; set; }
        public bool isProcessed { get; set; }
        public bool isVerified { get; set; }
    }
}
