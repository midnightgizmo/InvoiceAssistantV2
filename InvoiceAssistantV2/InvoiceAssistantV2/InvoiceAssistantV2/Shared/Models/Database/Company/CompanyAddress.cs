using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System.ComponentModel.DataAnnotations;

namespace InvoiceAssistantV2.Shared.Models.Database.Company
{
    /// <summary>
    /// An address that is linked to the <see cref="CompanyDetails"/>
    /// </summary>
    public class CompanyAddress
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Foreign key link to <see cref="CompanyDetails"/>
        /// </summary>
        public int CompanyDetailsID { get; set; }
        public CompanyDetails CompanyDetails { get; set; } = null!;
        /// <summary>
        /// FriendlyName given to the address we can use to display in the UI
        /// </summary>
        [Required]
        public string FriendlyName { get; set; } = null!;
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? AddressLine4 { get; set; }
        public string? AddressLine5 { get; set; }
        public string? PostCode { get; set; }
        // The driving distance from where we start to the companies address (used for cauclating milage cost)
        public int DrivingDistanceToAddress { get; set; } = 0;

        /// <summary>
        /// Needed for the One to many to many to one relationship with Invoices
        /// </summary>
        public IEnumerable<PlacesVisitedForInvoice> PlacesVisitedForInvoice { get; set; } = null!;

        /// <summary>
        /// When a user deletes a compnay address but it exists in one or more inoices. We won't be able
        /// to delete the addreess, so instead we hide it and set this bool value to true to indicate it has been
        /// deleted by the user
        /// </summary>
        public bool HasBeenDeleted { get; set; }
    }
}