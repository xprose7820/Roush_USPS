using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoushUSPS_App.Data;
using RoushUSPS_App.Models;
using RoushUSPS_App.Services.AddressValidation;
using RoushUSPS_App.ViewModels.Address;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Claims;
using X.PagedList;

namespace RoushUSPS_App.Services.Address
{
	public class AddressService : IAddressService
	{
		private readonly string _userId;
		private ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IAddressValidationService _addressValidationService;

		public AddressService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IAddressValidationService addressValidationService)
		{
			var userClaims = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
			_userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(_userId))
			{
				throw new Exception("Attempted to build without user Id claim.");

			}
			_context = context;
			_userManager = userManager;
			_addressValidationService = addressValidationService;
		}

		public async Task<IEnumerable<AddressListDetailViewModel>> GetAllAsync()
		{
			List<AddressListDetailViewModel> details = await _context.Addresses.Where(entity => entity.UserId == _userId)
				.Select(g => new AddressListDetailViewModel
				{

					UserId = g.UserId,
					Address1 = g.Address1,
					Address2 = g.Address2,
					City = g.City,
					State = g.State,
					ZipCode4 = g.ZipCode4,
					ZipCode5 = g.ZipCode5,

					isProcessed = g.isProcessed,
					isVerified = g.isVerified,


				}).ToListAsync();
			return details;

		}

		public async Task<IEnumerable<AddressListDetailViewModel>> GetAllProcessedAddressesAsync()
		{
			List<AddressListDetailViewModel> details = await _context.Addresses.Where(entity => entity.UserId == _userId && entity.isProcessed == true)
				.Select(g => new AddressListDetailViewModel
				{

					Id = g.Id,
					UserId = g.UserId,
					Address1 = g.Address1,
					Address2 = g.Address2,
					City = g.City,
					State = g.State,
					ZipCode4 = g.ZipCode4,
					ZipCode5 = g.ZipCode5,

					isVerified = g.isVerified,


				}).ToListAsync();
			return details;

		}

		public async Task<IPagedList<AddressListDetailViewModel>> GetAllProcessedAddressesAsync(int? addressPage)
		{
			int addressPageNumber = (addressPage ?? 1);
			List<AddressListDetailViewModel> details = (await GetAllProcessedAddressesAsync()).ToList();
			IPagedList<AddressListDetailViewModel> pagedAddressDetails = details.ToPagedList(addressPageNumber, 5);
			return pagedAddressDetails;

		}

		public async Task<IEnumerable<AddressListDetailViewModel>> GetAllUnprocessedAddressesAsync()
		{
			List<AddressListDetailViewModel> details = await _context.Addresses.Where(entity => entity.UserId == _userId && entity.isProcessed == false)
				.Select(g => new AddressListDetailViewModel
				{
					Id = g.Id,

					UserId = g.UserId,
					Address1 = g.Address1,
					Address2 = g.Address2,
					City = g.City,
					State = g.State,
					ZipCode4 = g.ZipCode4,
					ZipCode5 = g.ZipCode5,

					isVerified = g.isVerified,


				}).ToListAsync();
			return details;

		}
		public async Task<IPagedList<AddressListDetailViewModel>> GetAllUnprocessedAddressesAsync(int? addressPage)
		{
			int addressPageNumber = (addressPage ?? 1);
			List<AddressListDetailViewModel> details = (await GetAllUnprocessedAddressesAsync()).ToList();
			IPagedList<AddressListDetailViewModel> pagedAddressDetails = details.ToPagedList(addressPageNumber, 5);
			return pagedAddressDetails;
		}
		public async Task<bool> CreateAsync(AddressCreateViewModel model)
		{
			AddressEntity entity = new AddressEntity
			{

				UserId = _userId,
				Address1 = model.Address1,
				Address2 = model.Address2,
				City = model.City,
				State = model.State,
				ZipCode4 = model.ZipCode4,
				ZipCode5 = model.ZipCode5,
				isProcessed = false,
				isVerified = false,
			};
			_context.Addresses.Add(entity);
			return (await _context.SaveChangesAsync()) > 0;

		}
		public async Task<AddressDetailViewModel> GetByIdAsync(int id)
		{
			AddressEntity entity = await _context.Addresses.Where(e => e.Id == id).FirstOrDefaultAsync();
			if (entity == null)
			{
				return null;
			}

			AddressDetailViewModel detail = new AddressDetailViewModel
			{
				Id = id,
				UserId = entity.UserId,
				Address1 = entity.Address1,
				Address2 = entity.Address2,
				City = entity.City,
				State = entity.State,
				ZipCode4 = entity.ZipCode4,
				ZipCode5 = entity.ZipCode5,
				isProcessed = entity.isProcessed,
				isVerified = entity.isVerified,

			};
			return detail;
		}
		public async Task<bool> DeleteByIdAsync(int id)
		{
			AddressEntity entity = await _context.Addresses.Where(e => e.Id == id).FirstOrDefaultAsync();
			if (entity == null)
			{
				return false;
			}
			_context.Addresses.Remove(entity);
			return (await _context.SaveChangesAsync()) > 0;
		}
		public async Task<bool> AdoptUSPSVerifiedAddressAsync(int id)
		{
			AddressEntity entity = await _context.Addresses.Where(e => e.Id == id).FirstOrDefaultAsync();
			AddressDetailViewModel detail = await GetByIdAsync(id);
			(bool isValid, AddressDetailViewModel model) = await _addressValidationService.VerifyAddressAsync(detail);

			if (isValid)
			{
				entity.Address1 = model.Address1;
				entity.Address2 = model.Address2;
				entity.City = model.City;
				entity.State = model.State;
				entity.ZipCode4 = model.ZipCode4;
				entity.ZipCode5 = model.ZipCode5;
				entity.isProcessed = true;
				entity.isVerified = true;

				return (await _context.SaveChangesAsync()) > 0;
			}
			return false;

		}

		public async Task<bool> AdoptAddressAsIsAsync(int id)
		{
			AddressEntity entity = await _context.Addresses.Where(e => e.Id == id).FirstOrDefaultAsync();


			entity.isProcessed = true;
			entity.isVerified = false;


			return (await _context.SaveChangesAsync()) > 0;



		}

	}
}
