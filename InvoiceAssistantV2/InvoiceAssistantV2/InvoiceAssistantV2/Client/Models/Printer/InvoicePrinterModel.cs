using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Client.Pages.UserDetails;
using InvoiceAssistantV2.Client.ViewModels.Invoices.SearchResults;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using InvoiceAssistantV2.Shared.Models.Database.User;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;

namespace InvoiceAssistantV2.Client.Models.Printer
{
	public class InvoicePrinterModel
	{
		public static int MaxNumberOfPaymentRowsInvoiceAllows { get => 28; }

		public string NameOfPersonFillingOutInvoiceFirst { get; set; } = string.Empty;

		public string DateOfInvoice { get; set; } = string.Empty;
		public string ReferenceNumber { get; set; } = string.Empty;


		#region address from (person making out invoice)
		public string NameOfPersonFillingOutInvoiceSecond { get; set; } = string.Empty;
		public AddressPrinterModel AddressOfPersonFillingOutInvoice { get; set; } = new AddressPrinterModel();
		#endregion





		#region address to
		public string NameOfCompanyInvoiceMadeOutTo { get; set; } = string.Empty;
		public AddressPrinterModel AddressOfCompanyInvoiceMadeOutTo { get; set; } = new AddressPrinterModel();
		#endregion

		public List<PaymentOption> PaymentOptions { get; set; } = new List<PaymentOption>();

		/// <summary>
		/// How much to charge the customer and the payment break down
		/// </summary>
		public PaymentData PaymentData { get; set; } = new PaymentData();


		/// <summary>
		/// Convert the passed in invoice data into a <see cref="InvoicePrinterModel"/>
		/// </summary>
		/// <param name="modelToPopulate">the model to add the data to</param>
		/// <param name="invoice">The invoice data to be converted</param>
		/// <returns><see cref="InvoicePrinterModel"/> populated with data from the input</returns>
		public static async Task<InvoicePrinterModel> CreateModel(InvoicePrinterModel modelToPopulate, ApplicationSharedData sharedData,
													  HttpClient httpClient, AppSettings appSettings)
		{
			Task<List<InvoicePaymentBreakDown>>? taskInvoicePaymentsBreakDown =null;
			Task<UserDetails?>? userDetailsTask = null;
			Task<CompanyAddress?>? companyAddressTask = null;

			// one or more tasks will be added to this list that will will want to wait to complete
			List<Task> ListOfTasks = new List<Task>();

			if (modelToPopulate == null)
				modelToPopulate = new InvoicePrinterModel();

			InvoicePrinterModel model = modelToPopulate;

			// if what was sent in was null, return a blank model
			if (sharedData.vmInvoiceSelectedToPrint == null)
				return model;

			// if we don't have the payments asoshiated with the invoice, get them now
			if (sharedData.vmInvoiceSelectedToPrint.InvoiceData.InvoicePayments == null)
			{
				//sharedData.vmInvoiceSelectedToPrint.InvoiceData.InvoicePayments = await InvoicePrinterModel.LoadInoicePaymentsFromServer(sharedData.vmInvoiceSelectedToPrint.InvoiceData.Id, httpClient, appSettings);
				taskInvoicePaymentsBreakDown = InvoicePrinterModel.LoadInoicePaymentsFromServer(sharedData.vmInvoiceSelectedToPrint.InvoiceData.Id, httpClient, appSettings);
				ListOfTasks.Add(taskInvoicePaymentsBreakDown);
			}

			// if we don't have the users details, we need to go off to the server and get them.
			if (sharedData.UsersDetails == null)
			{
				//sharedData.UsersDetails = await InvoicePrinterModel.LoadUsersDetailsFromServer(httpClient, appSettings);
				userDetailsTask = InvoicePrinterModel.LoadUsersDetailsFromServer(httpClient, appSettings);
				ListOfTasks.Add(userDetailsTask);
			}

			// if we don't have the address the invoice is being made out to, we need to go off to theserver and get it
			if(sharedData.vmInvoiceSelectedToPrint.InvoiceData.AddressToMakeInvoiceOutTo == null)
			{
				companyAddressTask = InvoicePrinterModel.LoadAddressToMakeInvoiceOutToFromServer(httpClient, appSettings, sharedData.vmInvoiceSelectedToPrint.InvoiceData.AddressToMakeInvoiceOutToId);
				ListOfTasks.Add(companyAddressTask);
			}

			// we may have 3 tasks running in this method, we are going to let them run at the same time
			// instead of waiting for one to complete and then run the other one next.
			// this should speed up code exection. We used the ListOfTasks to see if any of them did run,
			// and if they did we are going to wait for them to complete.
			if (ListOfTasks.Any())
			{
				// wait for 1 or 2 tasks to complete
				await Task.WhenAll(ListOfTasks.ToArray());

				// if the InvoicePaymenBreakDown Task was ran, get its result
				if (taskInvoicePaymentsBreakDown != null)
					sharedData.vmInvoiceSelectedToPrint.InvoiceData.InvoicePayments = taskInvoicePaymentsBreakDown.Result;


				// if the UserDetails Task was ran, get its result
				if(userDetailsTask != null)
					sharedData.UsersDetails = userDetailsTask.Result;

				// if the companyAddressTask was ran, get its result
				if(companyAddressTask != null)
					sharedData.vmInvoiceSelectedToPrint.InvoiceData.AddressToMakeInvoiceOutTo = companyAddressTask.Result;
			}

			// if we could not get the users details, just return a blank model
			if (sharedData.UsersDetails == null)
				return model;

			

			await InvoicePrinterModel.GetDetailsOfPersonFillingOutInvoice(modelToPopulate, sharedData.vmInvoiceSelectedToPrint, sharedData.UsersDetails);
			await InvoicePrinterModel.GetDetailsOfPersonToMakeInvoiceOutTo(modelToPopulate, sharedData.vmInvoiceSelectedToPrint);
			await InvoicePrinterModel.GetInvoicePaymentDetails(modelToPopulate, sharedData.UsersDetails, httpClient, appSettings);
			await InvoicePrinterModel.GetInvoicePaymentBreakDown(modelToPopulate, sharedData.vmInvoiceSelectedToPrint);
			return model;

		}



		private static async Task<InvoicePrinterModel> GetDetailsOfPersonFillingOutInvoice(InvoicePrinterModel model, ListInvoiceRowViewModel invoice, UserDetails userDetails)
		{
			// Will need to get this from the server when its been implemented
			model.NameOfPersonFillingOutInvoiceFirst = userDetails.UsersName;
			// Will need to get this from the server when its been implemented
			model.NameOfPersonFillingOutInvoiceSecond = userDetails.UsersName;
			// Will need to get this from the server when its been implemented
			model.AddressOfPersonFillingOutInvoice.Line1 = userDetails.UserAddress.AddressLine1 ?? "";
			model.AddressOfPersonFillingOutInvoice.Line2 = userDetails.UserAddress.AddressLine2 ?? "";
			model.AddressOfPersonFillingOutInvoice.Line3 = userDetails.UserAddress.AddressLine3 ?? "";
			model.AddressOfPersonFillingOutInvoice.Line4 = userDetails.UserAddress.AddressLine4 ?? "";
			model.AddressOfPersonFillingOutInvoice.Line5 = userDetails.UserAddress.AddressLine5 ?? "";
			model.AddressOfPersonFillingOutInvoice.Line6 = userDetails.UserAddress.PostCode ?? "";

			model.DateOfInvoice = invoice.InvoiceData.DateOfInvoice.ToString("dd/MM/yyyy");
			model.ReferenceNumber = invoice.InvoiceData.ReferenceNumber;

			
			

			return model;
		}

		private static async Task GetDetailsOfPersonToMakeInvoiceOutTo(InvoicePrinterModel model, ListInvoiceRowViewModel invoice)
		{
			if (invoice.InvoiceData.AddressToMakeInvoiceOutTo == null)
				return;


			model.NameOfCompanyInvoiceMadeOutTo = invoice.InvoiceData.AddressToMakeInvoiceOutTo.CompanyDetails.CompanyName;

			model.AddressOfCompanyInvoiceMadeOutTo.Line1 = invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine1 ?? "";		
			model.AddressOfCompanyInvoiceMadeOutTo.Line2 = invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine2 ?? "";
			model.AddressOfCompanyInvoiceMadeOutTo.Line3 = invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine3 ?? "";
			model.AddressOfCompanyInvoiceMadeOutTo.Line4 = invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine4 ?? "";
			model.AddressOfCompanyInvoiceMadeOutTo.Line5 = invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine5 ?? "";
			model.AddressOfCompanyInvoiceMadeOutTo.Line6 = invoice.InvoiceData.AddressToMakeInvoiceOutTo.PostCode ?? "";
		}

		/// <summary>
		/// populate the invoicePrinterModel with the users details
		/// </summary>
		/// <param name="model"></param>
		/// <param name="sharedData"></param>
		/// <param name="httpClient"></param>
		/// <param name="appSettings"></param>
		/// <returns>true if sucsefull, else false</returns>
		private static async Task GetInvoicePaymentDetails(InvoicePrinterModel model, UserDetails userDetails, HttpClient httpClient, AppSettings appSettings)
		{
			// go through each payment option the user allows and add it to the invoice printer model
			foreach(PaymentMethod paymentMethod in userDetails.PaymentMethods)
			{
				PaymentOption paymentOption = new PaymentOption();

				paymentOption.paymentHeaderText = paymentMethod.Name;

				foreach(PaymetDetail paymentDetail in paymentMethod.PaymetDetails)
				{
					paymentOption.paymentData.Add(new PaymentOptionRow()
					{
						key = paymentDetail.Key,
						value = paymentDetail.Value ?? string.Empty
					});
				}

				model.PaymentOptions.Add(paymentOption);
			}
			
			
		}

		/// <summary>
		/// Get all the payments to list with a total that needs to be payed
		/// </summary>
		/// <param name="modelToPopulate"></param>
		/// <param name="vmInvoiceSelectedToPrint"></param>
		private static async Task GetInvoicePaymentBreakDown(InvoicePrinterModel modelToPopulate, ListInvoiceRowViewModel vmInvoiceSelectedToPrint)
		{

			// What we are doing hear is adding one blank payment to the array.
			// We do this because when we create an invoice, we want the first row in the payments section
			// to be empty. This allows the user to add any custom test they want (e.g. may be the date when the work was carried out)
			modelToPopulate.PaymentData.InvoicePaymentBreakDown.Add(new PaymentDataRow()
			{
				Ammount = string.Empty,
				Description = string.Empty
			});

			// Will hold the total invoice amount
			decimal totalInvoiceAmount = 0;
			foreach(InvoicePaymentBreakDown aPayment in vmInvoiceSelectedToPrint.InvoiceData.InvoicePayments)
			{
				// add up all the individual payments so we can get a total payment
				totalInvoiceAmount += aPayment.Ammount;

				// add this payment to the invoice printer model
				modelToPopulate.PaymentData.InvoicePaymentBreakDown.Add(new PaymentDataRow()
				{
					// if the Ammount containers decimal places, parse the number with 2 decimal places
					// if the Ammount does not have any decimal plces, parse the number with no decimal places
					Ammount = aPayment.Ammount.ToString(aPayment.Ammount % 1 == 0 ? "N0" : "N2"),
					Description = aPayment.Description
				});

			}

			// when creating the printable invoice, there is a set number of rows that must be avalable to populate.
			// so if the above payements don't total the number of rows needed, we will just create empty
			// blank rows for the rest of the rows
			while(modelToPopulate.PaymentData.InvoicePaymentBreakDown.Count < InvoicePrinterModel.MaxNumberOfPaymentRowsInvoiceAllows)
			{
				modelToPopulate.PaymentData.InvoicePaymentBreakDown.Add(new PaymentDataRow()
				{
					Ammount = string.Empty,
					Description = string.Empty
				});
			}

			// add the total invoice amount to the invoice pinter model
			modelToPopulate.PaymentData.TotalPayment = totalInvoiceAmount.ToString();
			
		}

		/// <summary>
		/// Get users details and payment details from the server
		/// </summary>
		/// <returns></returns>
		private static async Task<UserDetails?> LoadUsersDetailsFromServer(HttpClient httpClient, AppSettings appSettings)
		{
			

			UsersDetailsCommunication server = new UsersDetailsCommunication(httpClient, appSettings);
			ServerResponseSingleUserDetails response = await server.GetUserDetailsAsync();

			// if we could not get the users details from the server
			if (response.HasErrors == true)
				return null;
			// return the users details we got from the server
			else
				return response.ReturnValue;

		}

		/// <summary>
		/// Asks the server for all payments assoshiated with passed in Invoice Id
		/// </summary>
		/// <param name="InvoiceId">The Invoice payments will be assoshiated with</param>
		/// <returns>List of Payments assoshiated with Invoice</returns>
		private static async Task<List<InvoicePaymentBreakDown>> LoadInoicePaymentsFromServer(int InvoiceId, HttpClient httpClient, AppSettings appSettings)
		{
			InvoiceCommunication ServerCommunication = new InvoiceCommunication(httpClient, appSettings);
			ServerResponseListOfInvoicePayments ServerResponse;

			ServerResponse = await ServerCommunication.ListAllPaymentsForInvoice(InvoiceId);

			if (ServerResponse.HasErrors == true)
				return new List<InvoicePaymentBreakDown>();
			else
				return ServerResponse.ReturnValue;

		}

		/// <summary>
		/// Get the address assosiated with the passed in Address ID. Also gets the addresses Company details (CompanyAddress.CompanyDetails)
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="appSettings"></param>
		/// <param name="AddressToMakeInvoiceOutToId">The id of the address we want to look for</param>
		/// <returns>null if CompanyAddress not found</returns>
		private static async Task<CompanyAddress?> LoadAddressToMakeInvoiceOutToFromServer(HttpClient httpClient, AppSettings appSettings, int? AddressToMakeInvoiceOutToId)
		{
			// if there is no address id, we can't find the address
			if(AddressToMakeInvoiceOutToId == null)
				return null;

			CompanyAddressCommunication serverCompanyAddress = new CompanyAddressCommunication(httpClient, appSettings);
			ServerResponseSingleCompanyAddress responseCompanyAddress;
			responseCompanyAddress = await serverCompanyAddress.GetCompanyAddress(AddressToMakeInvoiceOutToId.Value);

			if(responseCompanyAddress.HasErrors == true) 
				return null;

			// now get the company details assosicated with this address
			CompanyCommunication serverCompanyCommunication = new CompanyCommunication(httpClient, appSettings);
			ServerResponseSingleCompanyDetails responseCompanyDetails = await serverCompanyCommunication.GetSingleCompanyDetails(responseCompanyAddress.ReturnValue.CompanyDetailsID);

			// if we could not get the company details assoshiated with the company address
			if(responseCompanyDetails.HasErrors == true) 
				return null;

			// assign the company details to the company address
			responseCompanyAddress.ReturnValue.CompanyDetails = responseCompanyDetails.ReturnValue;
			
			return responseCompanyAddress.ReturnValue;
		}
	}


	public class AddressPrinterModel
	{
        public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string Line3 { get; set; }
		public string Line4 { get; set; }
		public string Line5 { get; set; }
		public string Line6 { get; set; }
	}

	public class PaymentOption
	{
		/// <summary>
		/// the payment type. e.g. "Bank Transfer"
		/// </summary>
		public string paymentHeaderText { get; set; } = string.Empty;
		/// <summary>
		/// Array of all the details related to bank transfer
		/// e.g.
		/// key: "Sort code" value: "12-34-56"
		/// key: "Account Name" value: "some bank name"
		/// key "Account Number" value: "12345678"
		/// </summary>
		public List<PaymentOptionRow> paymentData { get; set; } = new List<PaymentOptionRow>();
    
	}
	/// <summary>
	/// A key value pair that belongs to a <see cref="PaymentOption"/>
	/// </summary>
	public class PaymentOptionRow
	{
		public string key { get; set; } = string.Empty;
		public string value { get; set; } = string.Empty;
	}

	/// <summary>
	/// Holds all the payments to list on the invoice so the customer can see what they are being charged for
	/// </summary>
	public class PaymentData
	{
		/// <summary>
		/// The total invoice ammount (should be a sum of all the PaymentDatRow
		/// </summary>
		public string TotalPayment { get; set; } = string.Empty;
		/// <summary>
		/// break down of invoice payments. All of these put togeher should form a total invoice payment
		/// </summary>
		public List<PaymentDataRow> InvoicePaymentBreakDown { get; set; } = new List<PaymentDataRow>();
	}

	/// <summary>
	/// Single payment row. Each row can have a description and ammount.
	/// </summary>
	public class PaymentDataRow
	{
		public string Description { get; set; } = string.Empty;
		
		//public string Ammount { get; set; } = string.Empty;
		private string _Ammount = string.Empty;
		public string Ammount
		{
			get => this._Ammount;
			set
			{
				// only accept the incoming value is it meats the following criteria
				// It has only numbers which can include a . to all decimal numbers
				// no spaces
				// no a-z or any other weird chars


				// remove any spaces at beginning and end of string
				string newValue = value.Trim().TrimStart('£');
				// used the hold the value of the number converted from a string
				decimal convertedNumber = 0;
				
				// if the new value is an empty string. just set the ammount value to string.empty
				// if after remove spaces from begining and end of the incoming string there is nothing left)
				if (newValue.Length == 0)
					this._Ammount = string.Empty;
				// if the new value is a number
				// (check we have just a number and nothing else in the string)
				else if (decimal.TryParse(newValue, out convertedNumber) == true)
				{
					// look for a dot in the incoming value to see if we are looking for a decimal value
					int dotLocation = newValue.IndexOf('.');
					// if there is a dot in string
					if (dotLocation != -1)
					{
						// get the number before the dot
						string valueBeforeDot = newValue.Substring(0, dotLocation);
						// get the number after the dot
						string valueAfterDot = newValue.Substring(dotLocation + 1);
						
						// if we have more than 2 decimal places
						// (we only want to allow 2 decimal places because we are dealing with currency)
						if (valueAfterDot.Length > 2)
							// put the number back together (before dot + dot + 2 digits after the dot)
							newValue = valueBeforeDot + "." + valueAfterDot.Substring(0,2);
						// if we have less than 2 decimal places
						else
							newValue = valueBeforeDot + "." + valueAfterDot;
					}
					// add the pound sign to the begining of the number
					newValue = "£" + newValue;

					// set the new value 
					this._Ammount = newValue;

				}
				// user trying to put in a char that is not a number so we won't allowd it, reset back to 
				// what the value was before they tried to input this new letter.
				else
					this._Ammount = this._Ammount;
			}
		}
	}
}
