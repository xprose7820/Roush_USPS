using Microsoft.AspNetCore.Mvc;
using RoushUSPS_App.Models;
using RoushUSPS_App.Services.Address;
using RoushUSPS_App.Services.AddressValidation;
using RoushUSPS_App.ViewModels.Address;
using RoushUSPS_App.ViewModels.Login;

namespace RoushUSPS_App.Controllers
{
	public class AddressController : Controller
	{
		private IAddressService _addressService;
		private IAddressValidationService _addressValidationService;
		public AddressController(IAddressService addressService, IAddressValidationService addressValidationService)
		{
			_addressService = addressService;
			_addressValidationService = addressValidationService;
		}
		public static Dictionary<string, string> USStates = new Dictionary<string, string>
		{
			{ "AL", "Alabama" },
			{ "AK", "Alaska" },
			{ "AZ", "Arizona" },
			{ "AR", "Arkansas" },
			{ "CA", "California" },
			{ "CO", "Colorado" },
			{ "CT", "Connecticut" },
			{ "DE", "Delaware" },
			{ "FL", "Florida" },
			{ "GA", "Georgia" },
			{ "HI", "Hawaii" },
			{ "ID", "Idaho" },
			{ "IL", "Illinois" },
			{ "IN", "Indiana" },
			{ "IA", "Iowa" },
			{ "KS", "Kansas" },
			{ "KY", "Kentucky" },
			{ "LA", "Louisiana" },
			{ "ME", "Maine" },
			{ "MD", "Maryland" },
			{ "MA", "Massachusetts" },
			{ "MI", "Michigan" },
			{ "MN", "Minnesota" },
			{ "MS", "Mississippi" },
			{ "MO", "Missouri" },
			{ "MT", "Montana" },
			{ "NE", "Nebraska" },
			{ "NV", "Nevada" },
			{ "NH", "New Hampshire" },
			{ "NJ", "New Jersey" },
			{ "NM", "New Mexico" },
			{ "NY", "New York" },
			{ "NC", "North Carolina" },
			{ "ND", "North Dakota" },
			{ "OH", "Ohio" },
			{ "OK", "Oklahoma" },
			{ "OR", "Oregon" },
			{ "PA", "Pennsylvania" },
			{ "RI", "Rhode Island" },
			{ "SC", "South Carolina" },
			{ "SD", "South Dakota" },
			{ "TN", "Tennessee" },
			{ "TX", "Texas" },
			{ "UT", "Utah" },
			{ "VT", "Vermont" },
			{ "VA", "Virginia" },
			{ "WA", "Washington" },
			{ "WV", "West Virginia" },
			{ "WI", "Wisconsin" },
			{ "WY", "Wyoming" }
		};



		[HttpGet]
		public async Task<IActionResult> Index(int? addressPage)
		{
			//List<AddressListDetailViewModel> addressProcessedList = (await _addressService.GetAllProcessedAddressesAsync()).ToList();
			ViewBag.AddressProcessedList = await _addressService.GetAllProcessedAddressesAsync(addressPage);
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewBag.StateList = USStates;

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(AddressCreateViewModel model)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors);
				foreach (var error in errors)
				{
					// You might log the error, print it to the console, or add it to a list to return with the view
					System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
				}

				return View(model);
			}

			bool isCreated = await _addressService.CreateAsync(model);
			if (isCreated)
			{
				return RedirectToAction("GetAllUnprocessedAddresses", "Address");
			}
			else
			{
				return View(model);
			}



		}
		[HttpGet]
		public async Task<IActionResult> GetAllUnprocessedAddresses(int? addressPage)
		{
			//List<AddressListDetailViewModel> addressUnprocessedList = (await _addressService.GetAllUnprocessedAddressesAsync()).ToList();
			ViewBag.AddressUnprocessedList = await _addressService.GetAllUnprocessedAddressesAsync(addressPage);
			//return View("~/Views/Address/GetAllUnprocessedAddresses.cshtml");
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> VerifyAddress(int id)
		{
			AddressDetailViewModel detail = await _addressService.GetByIdAsync(id);
			(bool isAddressValid, AddressDetailViewModel uspsVerifiedAddress) = await _addressValidationService.VerifyAddressAsync(detail);
			if (isAddressValid)
			{
				ViewBag.AdoptAddressAsIs = await _addressService.GetByIdAsync(id);
				ViewBag.AdoptUSPSVerifiedAddress = uspsVerifiedAddress;
			}
			ViewBag.IsAddressValid = isAddressValid;
			return View("~/Views/Address/AddressVerificationResult.cshtml");
		}
		[HttpGet]
		public async Task<IActionResult> AdoptAddressAsIs(int id)
		{
			bool addressUpdated = await _addressService.AdoptAddressAsIsAsync(id);
			if (addressUpdated)
			{
				return RedirectToAction("Index", "Address");
			}
			return BadRequest("Failed to adopt the address.");
		}
		[HttpGet]
		public async Task<IActionResult> AdoptUSPSVerifiedAddress(int id)
		{
			bool addressUpdated = await _addressService.AdoptUSPSVerifiedAddressAsync(id);
			if (addressUpdated)
			{
				return RedirectToAction("Index", "Address");
			}
			return BadRequest("Failed to adopt the address.");

		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			bool addressDeleted = await _addressService.DeleteByIdAsync(id);
			if (!addressDeleted)
			{
				return BadRequest("Failed to delete address.");

			}
			else
			{
				return RedirectToAction("GetAllUnprocessedAddresses", "Address");
			}
		}

	}

}
