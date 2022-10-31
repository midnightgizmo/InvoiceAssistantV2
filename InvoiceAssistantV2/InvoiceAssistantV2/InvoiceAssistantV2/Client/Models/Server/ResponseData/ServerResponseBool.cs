using InvoiceAssistantV2.Shared.Models.Database.Company;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseBool : ServerResponseData
    {
        public new bool ReturnValue { get; set; }
    }
}
