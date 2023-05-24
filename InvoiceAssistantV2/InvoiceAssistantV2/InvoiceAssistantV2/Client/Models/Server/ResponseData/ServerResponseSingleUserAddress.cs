using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseSingleUserAddress : ServerResponseData
	{
		public new UserAddress ReturnValue { get; set; }
	}
}
