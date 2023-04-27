using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseSinglePaymentMethod : ServerResponseData
	{
		public new PaymentMethod ReturnValue { get; set; }
	}
}
