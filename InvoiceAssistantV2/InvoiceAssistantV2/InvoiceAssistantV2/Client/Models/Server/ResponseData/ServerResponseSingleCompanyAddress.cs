using InvoiceAssistantV2.Shared.Models.Database.Company;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseSingleCompanyAddress : ServerResponseData
    {
        public new CompanyAddress ReturnValue { get; set; }
    }
}
