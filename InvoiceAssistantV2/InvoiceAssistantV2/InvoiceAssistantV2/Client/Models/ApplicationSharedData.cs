using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Client.Models
{
	/// <summary>
	/// Can be used throughout the app to pass around information.
	/// Can be accessed by Depedence injection
	/// </summary>
	public class ApplicationSharedData
	{
		/// <summary>
		/// A list of invoices that have been found from the criteria the user provided
		/// on the SearchInvoice Page
		/// </summary>
		public List<Invoice> InvoicesFromSearchResults { get; set; } = new List<Invoice>();
	}
}
