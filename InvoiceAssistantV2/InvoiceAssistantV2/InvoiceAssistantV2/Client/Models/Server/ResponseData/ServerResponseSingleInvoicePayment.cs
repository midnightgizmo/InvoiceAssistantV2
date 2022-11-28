using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseSingleInvoicePayment : ServerResponseData
	{
		public new InvoicePaymentBreakDown ReturnValue { get; set; }
	}
}
