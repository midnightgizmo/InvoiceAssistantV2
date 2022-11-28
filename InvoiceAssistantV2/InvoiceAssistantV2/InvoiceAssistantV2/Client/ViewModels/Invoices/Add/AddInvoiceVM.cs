using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices.Add
{
	public class AddInvoiceVM
	{
		private HttpClient _HttpClient;
		private AppSettings _AppSettings;
		public AddInvoiceMainDetailsVM MainDetails { get; set; } = new AddInvoiceMainDetailsVM();
		public AddInvoicePaymentBreakDown PaymentBreakDown { get; set; } = new AddInvoicePaymentBreakDown();


		public Task Inishazlie(HttpClient httpClient, AppSettings appSettings)
		{
			this._HttpClient = httpClient;
			this._AppSettings = appSettings;

			this.PaymentBreakDown.Inishazlie(this._HttpClient, this._AppSettings);

			return this.MainDetails.Inishazlie(this._HttpClient, this._AppSettings);
		}

		/// <summary>
		/// Checks the details on MainDetails view model are ok and submits them to the server to
		/// add a new invoices. on suscesfull new invoice the MainDetails ViewModel will be locked
		/// and the PaymentBreakDown view model will be unlocked to allow the user to add payments
		/// to the invoice.
		/// </summary>
		public async Task SubmitMainInvoiceDetailsToServer()
		{
			bool wasSucsefull = await this.MainDetails.SubmitInvoiceDetailsToServer();
			if (wasSucsefull == true)
			{
				this.MainDetails.HasMainDetailsBeenAddedToDataBase = true;
				this.MainDetails.IsSubmitButtonDisabled = true;
				this.PaymentBreakDown.IsVisable = true;
			}



			return;
		}

		private bool _IsDeleteInvoiceBeingSubmitted = false;
		public async Task DeleteInvoice()
		{
			// if there is no invoice to delete
			if (this.MainDetails.HasMainDetailsBeenAddedToDataBase == false)
				return;

			// if we are allreay waiting for a response for the invoice to be delete
			// (user may have clicked the button more than once)
			if (this._IsDeleteInvoiceBeingSubmitted == true)
				return;

			InvoiceCommunication server = new InvoiceCommunication(this._HttpClient, this._AppSettings);
			ServerResponseBool serverResponse = await server.DeleteInvoice(this.MainDetails.CreatedInvoice.Id);

			// if an error occured, the invoice was not deleted
			if (serverResponse.HasErrors == true || serverResponse.ReturnValue == false)
				return;

			// if we get this far, the invoice has been deleted.
			this.MainDetails.Reset();
			this.PaymentBreakDown.Reset();

			return;

		}


		public async Task NewInvoice()
		{
			// if a new invoice has not yet been created.
			if (this.MainDetails.HasMainDetailsBeenAddedToDataBase == false)
				return;

			this.MainDetails.Reset();
			this.PaymentBreakDown.Reset();
		}


	}
}
