using RoushUSPS_App.Models;
using RoushUSPS_App.ViewModels.Address;

namespace RoushUSPS_App.Services.AddressValidation
{
    public interface IAddressValidationService
    {
        public Task<(bool, AddressDetailViewModel)> VerifyAddressAsync(AddressDetailViewModel model);
    }
}
