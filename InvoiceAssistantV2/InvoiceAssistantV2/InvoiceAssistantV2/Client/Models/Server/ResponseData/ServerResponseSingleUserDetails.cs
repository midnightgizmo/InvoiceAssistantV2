using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseSingleUserDetails : ServerResponseData
	{
		public new UserDetails ReturnValue { get; set; }
	}
}
