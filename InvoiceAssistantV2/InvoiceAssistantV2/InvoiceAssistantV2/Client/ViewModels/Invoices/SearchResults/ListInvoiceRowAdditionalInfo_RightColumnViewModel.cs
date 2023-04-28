using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Client.ViewModels.Invoices.Add;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices.SearchResults
{
	public class ListInvoiceRowAdditionalInfo_RightColumnViewModel : AddInvoicePaymentBreakDown
	{
		/// <summary>
		/// The view model that creates this view model
		/// </summary>
		private ListInvoiceRowAdditionalInfoViewModel _Parent;

		/// <summary>
		/// will be set to true the first time <see cref="Inishalize"/> method has been called
		/// </summary>
		private bool _HasRowBeenInishalized = false;

		public ListInvoiceRowAdditionalInfo_RightColumnViewModel(ListInvoiceRowAdditionalInfoViewModel parent)
        {
			this._Parent = parent;
			this._HttpClient = this._Parent.Parent.httpClient;
			this._AppSettings = this._Parent.Parent.appSettings;
			this.IsVisable = true;
		}

		public async Task Inishalize()
		{
			// only allow this method to be called once
			if (this._HasRowBeenInishalized == true)
				return;

			// load all the payments for this invoice.
			this.ListOfPayments.Clear();
			this.ListOfPayments.AddRange(await this.LoadInoicePaymentsFromServer(this.Parent.Parent.InvoiceData.Id));

			// add the invoice payments we just recieved from the server to the InvoiceData Model
			// This will allow us to access it if/when we want to print the invoice
			if (this.Parent.Parent.InvoiceData.InvoicePayments == null)
				this.Parent.Parent.InvoiceData.InvoicePayments = new List<InvoicePaymentBreakDown>();
			else
				this.Parent.Parent.InvoiceData.InvoicePayments.Clear();

			this.Parent.Parent.InvoiceData.InvoicePayments.AddRange(this.ListOfPayments);




			// get the new blance after payment was removed
			this.Balance = this.CaculateSumOfAllPayments();
		}

		

		public ListInvoiceRowAdditionalInfoViewModel Parent { get => this._Parent;}

		/// <summary>
		/// Asks the server for all payments assoshiated with passed in Invoice Id
		/// </summary>
		/// <param name="InvoiceId">The Invoice payments will be assoshiated with</param>
		/// <returns>List of Payments assoshiated with Invoice</returns>
		private async Task<List<InvoicePaymentBreakDown>> LoadInoicePaymentsFromServer(int InvoiceId)
		{
			InvoiceCommunication ServerCommunication = new InvoiceCommunication(this._HttpClient,this._AppSettings);
			ServerResponseListOfInvoicePayments ServerResponse;
			
			ServerResponse = await ServerCommunication.ListAllPaymentsForInvoice(InvoiceId);

			if (ServerResponse.HasErrors == true)
				return new List<InvoicePaymentBreakDown>();
			else
				return ServerResponse.ReturnValue;

		}
	}
}
