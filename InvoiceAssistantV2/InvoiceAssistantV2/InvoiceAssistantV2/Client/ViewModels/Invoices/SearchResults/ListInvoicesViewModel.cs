

using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices.SearchResults
{
	public class ListInvoicesViewModel
	{
		private List<Invoice> _InvoicesToDisplay = new List<Invoice>();

		/// <summary>
		/// Insishalize the view model with the invoices obtained from search page results
		/// </summary>
		/// <param name="invoicesToDisplay"></param>
		public async Task Inishalize(List<Invoice> invoicesToDisplay)
		{
			this._InvoicesToDisplay.AddRange(invoicesToDisplay.ToArray());

			// create a view model for each invoice (this will be each row to list in the view
			foreach(Invoice anInvoice in this._InvoicesToDisplay)
				this.InvoiceRowsViewModel.Add(new ListInvoiceRowViewModel(anInvoice,this.httpClient,this.appSettings,this));

			Task[] tasksToWaitFor = new Task[2];
			// get all the types of payment that are possible e.g. chash, cheque, bank transfer
			tasksToWaitFor[0] = this.LoadAllPaymentTypesFromServer();
			// get all the companies
			tasksToWaitFor[1] = this.LoadAllCompanyDetailsFromServer();

			// wait for the above to tasks to complete
			await Task.WhenAll(tasksToWaitFor);
		}

		/// <summary>
		/// An event that gets called to request the UI call StateHasChanged method
		/// </summary>
		public event Action UiNeedsUpdating = delegate () { };
		/// <summary>
		/// Calls the <see cref="UiNeedsUpdating"/> event
		/// </summary>
		public void CallUiNeedsUpdatingEvent()
		{
			this.UiNeedsUpdating();
		}

		/// <summary>
		/// View model for each Invoice to display from the search results
		/// </summary>
		public List<ListInvoiceRowViewModel> InvoiceRowsViewModel { get; set; } = new List<ListInvoiceRowViewModel>();


		public async Task DeleteInvoice(ListInvoiceRowViewModel InvoiceRowViewModel)
		{
			// Ask the server to remove the invoice from the database
			InvoiceCommunication Server = new InvoiceCommunication(this.httpClient, this.appSettings);
			ServerResponseBool ServerResponse  = await Server.DeleteInvoice(InvoiceRowViewModel.InvoiceData.Id);

			// if there are no errors on the server
			// and the server was able to delete the invoice
			if(ServerResponse.HasErrors == false && ServerResponse.ReturnValue == true)
			{
				// remove the invoice from the client side list (this will remove it from the UI to the customer)
				InvoiceRowsViewModel.Remove(InvoiceRowViewModel);
			}
			else
			{// do somthing to indicate the invoice was not deleted.

			}

			return;

			
		}

		/// <summary>
		/// The row the user has selected to see more details regarding that selected invoice
		/// </summary>
		public ListInvoiceRowViewModel? SelectedRow { get; set; } = null;

		public AppSettings appSettings { get; set; }
		public HttpClient httpClient { get; set; }

		/// <summary>
		/// A list of all possible payment types
		/// </summary>
		public List<PaymentType> ListOfPaymentTypes { get; set; } = new List<PaymentType>();


		public List<CompanyDetails> ListOfCompanies { get; set; } = new List<CompanyDetails>();

		/// <summary>
		/// A place to store company address that have been fetched from the server.
		/// Each time a user changes the company an invoice belongs we will need to find
		/// all address that belong to that company. We should this this list to see if those addresses
		/// exist within this list first. If they do not, we will have to go off to the server to get them.
		/// The returned address will then be stored in this list.
		/// Note you will need to filter the address in this list by there company id to get the ones you want.
		/// </summary>
		public List<CompanyAddress> ListOfCompanyAddressess { get; set; } = new List<CompanyAddress>();

		/// <summary>
		/// Get a list of all payment types and store them in <see cref="ListOfPaymentTypes"/>
		/// </summary>
		/// <returns></returns>
		private async Task LoadAllPaymentTypesFromServer()
		{
			InvoiceCommunication server = new InvoiceCommunication(this.httpClient, this.appSettings);
			ServerResponsePaymentTypes serverResponse = await server.GetPaymentTypes();

			if(serverResponse.HasErrors == false)
				this.ListOfPaymentTypes.AddRange(serverResponse.ReturnValue);
		}

		/// <summary>
		/// Get a list of all the companys and store them in <see cref="ListOfCompanies"/>
		/// </summary>
		/// <returns></returns>
		private async Task LoadAllCompanyDetailsFromServer()
		{
			CompanyCommunication server = new CompanyCommunication(this.httpClient, this.appSettings);
			ServerResponseListOfCompanys serverResponse = await server.GetListOfCompanys();

			if(serverResponse.HasErrors == false)
				this.ListOfCompanies.AddRange(serverResponse.ReturnValue);
			
		}
	}
}
