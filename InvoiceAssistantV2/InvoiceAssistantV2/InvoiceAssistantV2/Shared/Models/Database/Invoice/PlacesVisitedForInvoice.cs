using InvoiceAssistantV2.Shared.Models.Database.Company;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace InvoiceAssistantV2.Shared.Models.Database.Invoice
{
    /// <summary>
    /// This is a One to many to many to one relasionship: <see cref="CompanyAddress"/> to <see cref="PlacesVisitedForInvoice"/> to <see cref="Invoice"/>
    /// </summary>
    [Keyless]
    public class PlacesVisitedForInvoice : PlacesVisitedForInvoiceBase
	{
        /// <summary>
        /// foreign key for <see cref="Invoice"/>
        /// </summary>
        public int InvoiceId { get; set; }

		/// <summary>
		/// Keeps a note of the driving distance to the address for the given invoice.
		/// This allows future changes to CompanyAddress.DrivingDistanceToAddress while
		/// preserving the changing to previouse invoices that were created
		/// </summary>
		public int DrivingDistanceToAddress { get; set; }

		/// <summary>
		/// The invoice we are linking too
		/// </summary>
		[JsonIgnore]
		public Invoice Invoice { get; set; } = null!;


		/// <summary>
		/// The address we visited for the invoice
		/// </summary>
		[JsonIgnore]
		public CompanyAddress CompanyAddress { get; set; } = null!;

       

    }
}