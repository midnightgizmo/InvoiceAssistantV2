using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseSinglePlaceVisitedForInvoice : ServerResponseData
	{
		public new PlacesVisitedForInvoice ReturnValue { get; set; }
	}
}
