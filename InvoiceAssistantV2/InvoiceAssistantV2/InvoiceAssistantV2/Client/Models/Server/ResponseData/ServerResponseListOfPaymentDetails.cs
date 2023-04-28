using InvoiceAssistantV2.Client.Pages.UserDetails;
using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseListOfPaymentDetails : ServerResponseData
	{
		public new List<PaymetDetail> ReturnValue { get; set; }
	}
}
