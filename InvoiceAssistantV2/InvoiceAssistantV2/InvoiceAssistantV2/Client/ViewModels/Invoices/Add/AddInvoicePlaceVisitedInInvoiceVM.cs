using InvoiceAssistantV2.Shared.Models.Database.Company;

namespace InvoiceAssistantV2.Client.ViewModels.Invoices.Add
{
	/// <summary>
	/// A place the user has visited for a given invoice
	/// </summary>
	public class AddInvoicePlaceVisitedInInvoiceVM
	{
		public CompanyAddress PlaceVisited { get; set; }
		public int NumberOfTimesVisited { get; set; } = 0;
	}
}
