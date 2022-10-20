using InvoiceAssistantV2.Shared.Models;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices
{
	public class SearchInvoiceVM
	{
        public InvoiceSearchParameters FormModel { get; set; } = new InvoiceSearchParameters();


        public List<PaymentType> PaymentTypes { get; set; }
    }
}
