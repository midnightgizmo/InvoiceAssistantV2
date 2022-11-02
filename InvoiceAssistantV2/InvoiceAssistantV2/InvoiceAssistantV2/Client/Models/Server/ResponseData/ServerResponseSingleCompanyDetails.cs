using InvoiceAssistantV2.Shared.Models.Database.Company;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponseSingleCompanyDetails : ServerResponseData
    {
        public new CompanyDetails ReturnValue { get; set; }
    }
}
