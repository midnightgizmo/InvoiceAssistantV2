using System.ComponentModel.DataAnnotations;

namespace InvoiceAssistantV2.Shared.Models.Database.Invoice
{
    /// <summary>
    /// Payments assoshiated with <see cref="Invoice"/>
    /// </summary>
    public class InvoicePaymentBreakDown
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// The Invoice this payment is assoshiated with
        /// </summary>
        [Required]
        public int InvoiceId { get; set; }
        /// <summary>
        /// Payment description to show on the invoice
        /// </summary>
        [Required]
        public string Description { get; set; } = null!;
        /// <summary>
        /// the cost for this porition of the invoice
        /// </summary>
        public decimal Ammount { get; set; }
    }
}