using InvoiceAssistantV2.Client.Models.Server.ResponseData;
using InvoiceAssistantV2.Shared.Models.Database.Company;

namespace InvoiceAssistantV2.Client.Models.Server
{
    public class ServerResponseListOfCompanyAddresses : ServerResponseData
    {
        public new List<CompanyAddress> ReturnValue { get; set; }
    }
}
