using InvoiceAssistantV2.Client.ViewModels.Invoices;
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

		/// <summary>
		/// A view model for the search for invoices page.
		/// Allows us to keep the users search parameters they enterd so if they navigate
		/// away from the search page and then go back to it, they don't have to re enter
		/// all the search parameters again.
		/// </summary>
		public SearchInvoiceVM? vmSearch { get; set; }
	}
}
