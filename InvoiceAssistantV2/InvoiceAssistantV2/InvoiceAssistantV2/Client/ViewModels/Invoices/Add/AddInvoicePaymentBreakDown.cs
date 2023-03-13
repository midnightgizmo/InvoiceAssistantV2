using InvoiceAssistantV2.Client.Classes.Server;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System.Runtime.InteropServices;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices.Add
{
	public class AddInvoicePaymentBreakDown
	{
		protected HttpClient _HttpClient;
		protected AppSettings _AppSettings;


		/// <summary>
		/// Determins if this view model should be visable
		/// </summary>
		public bool IsVisable { get; set; } = false;

		/// <summary>
		/// The Description to use when making a new payment to add to the invoice
		/// </summary>
		public string Description { get; set; } = string.Empty;
		/// <summary>
		/// The ammount in pounds to use when makeing a new payment to add to the invoice
		/// </summary>
		public decimal Ammount { get; set; } = 0;


		/// <summary>
		/// The sum of all payments held in <see cref="ListOfPayments"/>
		/// </summary>
		public decimal Balance { get; protected set; } = 0;

		public List<InvoicePaymentBreakDown> ListOfPayments { get; set; } = new List<InvoicePaymentBreakDown>();

		public void Inishazlie(HttpClient httpClient, AppSettings appSettings)
		{
			this._HttpClient = httpClient;
			this._AppSettings = appSettings;

			
		}


		public bool IsAddPaymentInProgress { get; set; } = false;
		/// <summary>
		/// Create a new payment and add it to the database. (uses values held in <see cref="Description"/> & <see cref="Ammount"/>
		/// </summary>
		/// <returns></returns>
		public async Task<bool> AddPayment(int InvoiceId)
		{
			// Takes the values stored in this.Description & this.Ammount to make
			// a new payment to add to the invoice

			if (this.IsAddPaymentInProgress == true)
				return false;


			this.IsAddPaymentInProgress = true;

			// get the description (with white space removed)
			string description = this.Description.Trim();
			// get the invoice ammount
			decimal ammount = this.Ammount;

			// check we have the data to create a new payment
			if (description.Length < 1 || ammount < 0) 
			{
				this.IsAddPaymentInProgress = false;
				// we are missing some data, so can't add the payment, return false
				return false;
			}

			// send the payment information off to the server to be added.
			InvoiceCommunication server = new InvoiceCommunication(this._HttpClient, this._AppSettings);
			ServerResponseSingleInvoicePayment response = await server.AddInvoicePayment(InvoiceId,description, ammount);

			// if the server was unable to add the payment
			if (response.HasErrors)
			{
				this.IsAddPaymentInProgress = false;
				return false;
			}

			// if we get this far, payment was added to the database
			this.ListOfPayments.Add(response.ReturnValue);

			// get the new blance after payment was added
			this.Balance = this.CaculateSumOfAllPayments();

			this.Description = string.Empty;
			this.Ammount = 0;

			this.IsAddPaymentInProgress = false;

			return true;
		}

		public async Task<bool> RemovePayment(int PaymentId)
		{
			// send the a request to the server to remove the payment from the invoice
			InvoiceCommunication server = new InvoiceCommunication(this._HttpClient, this._AppSettings);
			ServerResponseBool response = await server.RemovePaymentFromInvoice(PaymentId);

			// if the server was unable to remove the payment from the server
			if (response.HasErrors == true)
				return false;

			if (response.ReturnValue == false)
				return false;

			// if we get this far, the payment has been removed from the server
			InvoicePaymentBreakDown? payment = this.ListOfPayments.Where(p => p.Id == PaymentId).FirstOrDefault();

			if (payment != null)
				this.ListOfPayments.Remove(payment);

			// get the new blance after payment was removed
			this.Balance = this.CaculateSumOfAllPayments();

			return true;
		}

		protected decimal CaculateSumOfAllPayments()
		{
			if (this.ListOfPayments == null)
				return 0;

			return this.ListOfPayments.Sum(l => l.Ammount);
		}

		internal void Reset()
		{
			this.IsVisable = false;
			this.Description = string.Empty;
			this.Ammount = 0;
			this.Balance = 0;
			this.ListOfPayments.Clear();
		}
	}
}
