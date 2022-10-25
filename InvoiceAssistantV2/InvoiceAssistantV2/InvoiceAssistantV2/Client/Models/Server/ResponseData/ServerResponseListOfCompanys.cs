using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
    public class ServerResponseListOfCompanys : ServerResponseData
    {
        public new List<CompanyDetails> ReturnValue { get; set; }
    }
}
