using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseListOfInvoicePayments : ServerResponseData
	{
		public new List<InvoicePaymentBreakDown> ReturnValue { get; set; }
	}
}
