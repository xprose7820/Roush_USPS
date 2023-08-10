using RoushUSPS_App.Models;
using RoushUSPS_App.ViewModels.Address;
using X.PagedList;

namespace RoushUSPS_App.Services.Address
{
    public interface IAddressService
    {
        public Task<IEnumerable<AddressListDetailViewModel>> GetAllAsync();
        public Task<IEnumerable<AddressListDetailViewModel>> GetAllProcessedAddressesAsync();
        public Task<IPagedList<AddressListDetailViewModel>> GetAllProcessedAddressesAsync(int? addressPage);
        public Task<IEnumerable<AddressListDetailViewModel>> GetAllUnprocessedAddressesAsync();
        public Task<IPagedList<AddressListDetailViewModel>> GetAllUnprocessedAddressesAsync(int? addressPage);

		public Task<bool> CreateAsync(AddressCreateViewModel model);
        public Task<bool> DeleteByIdAsync(int id);
        public Task<bool> AdoptAddressAsIsAsync(int id);
        public Task<bool> AdoptUSPSVerifiedAddressAsync(int id);
        public Task<AddressDetailViewModel> GetByIdAsync(int id);
       
    }
}
