using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.Models.Server.ResponseData
{
	public class ServerResponsePaymentTypes : ServerResponseData
    {
        public new List<PaymentType> ReturnValue { get; set; }
    }
}
