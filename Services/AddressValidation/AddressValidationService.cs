using RoushUSPS_App.Models;
using RoushUSPS_App.ViewModels.Address;
using System.Web;
using System.Xml.Linq;

namespace RoushUSPS_App.Services.AddressValidation
{
	public class AddressValidationService : IAddressValidationService
	{
		private readonly HttpClient _httpClient;
		public readonly string _uspsUserId = "16PURDU20J761";

		public AddressValidationService(IHttpClientFactory httpClient)
		{
			_httpClient = httpClient.CreateClient();
		}

		public async Task<(bool, AddressDetailViewModel)> VerifyAddressAsync(AddressDetailViewModel model)
		{
			XElement xmlRequest = new XElement("AddressValidateRequest",
				new XAttribute("USERID", _uspsUserId),
				new XElement("Revision", "1"),
				new XElement("Address",
					new XAttribute("ID", "0"),
					new XElement("Address1", model.Address1),
					new XElement("Address2", model.Address2),
					new XElement("City", model.City),
					new XElement("State", model.State),
					new XElement("Zip5", model.ZipCode5),
					new XElement("Zip4", model.ZipCode4)
					)
			);

			string requestBody = xmlRequest.ToString();

			string url = "https://secure.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + HttpUtility.UrlEncode(requestBody);

			HttpResponseMessage response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				string responseXml = await response.Content.ReadAsStringAsync();
				Console.WriteLine(responseXml);

				XElement xmlResponse = XElement.Parse(responseXml);
			
				XElement addressElement = xmlResponse.Element("Address");
				var addressElementXML = xmlResponse.Element("Address");
				if (addressElementXML == null)
				{
					return (false, null);
				}

				var dpvElement = addressElementXML.Element("DPVConfirmation");
				if (dpvElement == null)
				{
					return (false, null);
				}

				string dpvConfirmation = dpvElement.Value;
				if (dpvConfirmation == "Y" || dpvConfirmation == "D")
				{
					AddressDetailViewModel detail = new AddressDetailViewModel
					{
						Id = model.Id,
						Address1 = (string)addressElement.Element("Address1"),
						Address2 = (string)addressElement.Element("Address2"),
						City = (string)addressElement.Element("City"),
						State = (string)addressElement.Element("State"),
						ZipCode5 = (string)addressElement.Element("Zip5"),
						ZipCode4 = (string)addressElement.Element("Zip4")
					};
					return (true, detail);

				}
			}
			return (false, null);

		}

	}
}
