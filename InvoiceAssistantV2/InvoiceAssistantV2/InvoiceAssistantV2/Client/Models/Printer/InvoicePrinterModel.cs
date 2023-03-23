using InvoiceAssistantV2.Client.ViewModels.Invoices.SearchResults;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.Models.Printer
{
	public class InvoicePrinterModel
	{
		public string NameOfPersonFillingOutInvoiceFirst { get; set; } = string.Empty;

		public string DateOfInvoice { get; set; } = string.Empty;
		public string ReferenceNumber { get; set; } = string.Empty;


		#region address from (person making out invoice)
		public string NameOfPersonFillingOutInvoiceSecond { get; set; } = string.Empty;
		public List<string> AddressOfPersonFillingOutInvoice { get; set; } = new List<string>();
		#endregion





		#region address to
		public string NameOfCompanyInvoiceMadeOutTo { get; set; } = string.Empty;
		public List<string> AddressOfCompanyInvoiceMadeOutTo { get; set; } = new List<string>();
		#endregion

		public List<PaymentOption> PaymentOptions { get; set; } = new List<PaymentOption>();


		/// <summary>
		/// Convert the passed in invoice data into a <see cref="InvoicePrinterModel"/>
		/// </summary>
		/// <param name="modelToPopulate">the model to add the data to</param>
		/// <param name="invoice">The invoice data to be converted</param>
		/// <returns><see cref="InvoicePrinterModel"/> populated with data from the input</returns>
		public static InvoicePrinterModel CreateModel(InvoicePrinterModel modelToPopulate, ListInvoiceRowViewModel? invoice)
		{
			if (modelToPopulate == null)
				modelToPopulate = new InvoicePrinterModel();

			InvoicePrinterModel model = modelToPopulate;

			// if what was sent in was null, return a blank model
			if (invoice == null)
				return model;

			InvoicePrinterModel.GetDetailsOfPersonFillingOutInvoice(modelToPopulate, invoice);
			InvoicePrinterModel.GetDetailsOfPersonToMakeInvoiceOutTo(modelToPopulate, invoice);
			InvoicePrinterModel.GetInvoicePaymentDetails(modelToPopulate, invoice);

			return model;

		}

		

		private static InvoicePrinterModel GetDetailsOfPersonFillingOutInvoice(InvoicePrinterModel model, ListInvoiceRowViewModel invoice)
		{
			// Will need to get this from the server when its been implemented
			model.NameOfPersonFillingOutInvoiceFirst = "test";
			// Will need to get this from the server when its been implemented
			model.NameOfPersonFillingOutInvoiceSecond = "test";
			// Will need to get this from the server when its been implemented
			model.AddressOfPersonFillingOutInvoice.Add("Address line 1");
			model.AddressOfPersonFillingOutInvoice.Add("Address line 2");
			model.AddressOfPersonFillingOutInvoice.Add("Address line 3");
			model.AddressOfPersonFillingOutInvoice.Add("Address line 4");

			model.DateOfInvoice = invoice.InvoiceData.DateOfInvoice.ToString("dd/MM/yyyy");
			model.ReferenceNumber = invoice.InvoiceData.ReferenceNumber;

			

			return model;
		}

		private static void GetDetailsOfPersonToMakeInvoiceOutTo(InvoicePrinterModel model, ListInvoiceRowViewModel invoice)
		{
			if (invoice.InvoiceData.AddressToMakeInvoiceOutTo == null)
				return;


			model.NameOfCompanyInvoiceMadeOutTo = invoice.InvoiceData.AddressToMakeInvoiceOutTo.CompanyDetails.CompanyName;

			if (invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine1 != null)
				model.AddressOfCompanyInvoiceMadeOutTo.Add(invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine1);

			if (invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine2 != null)
				model.AddressOfCompanyInvoiceMadeOutTo.Add(invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine2);

			if (invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine3 != null)
				model.AddressOfCompanyInvoiceMadeOutTo.Add(invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine3);

			if (invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine4 != null)
				model.AddressOfCompanyInvoiceMadeOutTo.Add(invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine4);

			if (invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine5 != null)
				model.AddressOfCompanyInvoiceMadeOutTo.Add(invoice.InvoiceData.AddressToMakeInvoiceOutTo.AddressLine5);

			if (invoice.InvoiceData.AddressToMakeInvoiceOutTo.PostCode != null)
				model.AddressOfCompanyInvoiceMadeOutTo.Add(invoice.InvoiceData.AddressToMakeInvoiceOutTo.PostCode);
			
		}

		private static void GetInvoicePaymentDetails(InvoicePrinterModel model, ListInvoiceRowViewModel invoice)
		{
			// need to implement this
			throw new NotImplementedException();
		}
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
		public List<PaymentData> paymentData { get; set; } = new List<PaymentData>();
    
	}
	public class PaymentData
	{
		public string description { get; set; } = string.Empty;
		public string amount { get; set; } = string.Empty;
	}
}
