using InvoiceAssistantV2.Client.ViewModels.Invoices;
using InvoiceAssistantV2.Client.ViewModels.Invoices.SearchResults;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using InvoiceAssistantV2.Shared.Models.Database.User;

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

		/// <summary>
		/// Will be set in ListInvoice.razor file when the user clicks the print button
		/// Will be accessed in the CreatePrintableInvoicePage.razor file
		/// </summary>
		public ListInvoiceRowViewModel? vmInvoiceSelectedToPrint { get; set; }

		/// <summary>
		/// Should be set from the UsersDetails.razor page. Will be set when a new payment method is made or
		/// a payment method is set to be edited. The page will then navigate to the PaymentDetails.razor
		/// page who will then access the use the below property for editing it.
		/// </summary>
		public PaymentMethod? PaymentMethodToEdit { get; set; }

	}
}
