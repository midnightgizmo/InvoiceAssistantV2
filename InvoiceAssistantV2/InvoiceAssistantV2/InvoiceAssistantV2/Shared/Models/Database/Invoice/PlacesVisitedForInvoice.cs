using InvoiceAssistantV2.Shared.Models.Database.Company;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAssistantV2.Shared.Models.Database.Invoice
{
    /// <summary>
    /// This is a One to many to many to one relasionship: <see cref="CompanyAddress"/> to <see cref="PlacesVisitedForInvoice"/> to <see cref="Invoice"/>
    /// </summary>
    [Keyless]
    public class PlacesVisitedForInvoice
    {
        /// <summary>
        /// foreign key for <see cref="Invoice"/>
        /// </summary>
        public int InvoiceId { get; set; }
        /// <summary>
        /// The invoice we are linking too
        /// </summary>
        public Invoice Invoice { get; set; } = null!;

        /// <summary>
        /// foreign key for <see cref="CompanyAddress"/>
        /// </summary>
        public int CompanyAddressId { get; set; }
        /// <summary>
        /// The address we visited for the invoice
        /// </summary>
        public CompanyAddress CompanyAddress { get; set; } = null!;

        /// <summary>
        /// The Number of times we visited the <see cref="CompanyAddress"/> for this <see cref="Invoice"/>
        /// </summary>
        public int NumberOfTimesVisited { get; set; }

    }
}