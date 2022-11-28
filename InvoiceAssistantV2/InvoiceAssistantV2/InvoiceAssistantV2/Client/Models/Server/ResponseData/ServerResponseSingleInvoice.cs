using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseSingleInvoice : ServerResponseData
	{
		public new Invoice ReturnValue { get; set; }
	}
}
