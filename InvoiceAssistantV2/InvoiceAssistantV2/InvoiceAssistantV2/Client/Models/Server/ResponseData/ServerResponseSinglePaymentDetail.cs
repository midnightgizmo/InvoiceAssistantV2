using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseSinglePaymentDetail : ServerResponseData
	{
		public new PaymetDetail ReturnValue { get; set; }
	}
}
