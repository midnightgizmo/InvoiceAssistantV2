using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseListOfPlacesVisitedForInvoice : ServerResponseData
	{
		public new List<PlacesVisitedForInvoice> ReturnValue { get; set; }
	}
}
