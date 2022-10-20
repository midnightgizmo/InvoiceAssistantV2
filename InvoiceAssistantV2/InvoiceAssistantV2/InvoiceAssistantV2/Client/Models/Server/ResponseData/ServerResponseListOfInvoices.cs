using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseListOfInvoices : ServerResponseData
	{
        public new List<Invoice> ReturnValue { get; set; }
    }
}
