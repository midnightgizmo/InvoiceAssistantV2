using System.ComponentModel.DataAnnotations;

namespace InvoiceAssistantV2.Shared.Models.Database.Invoice
{
    /// <summary>
    /// Contains a payment type. These are how we were paid for the invoice, e.g. Cheque, bank transfer, etc
    /// </summary>
    public class PaymentType
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// The name given to this payment type (e.g. Cheque, Bank transfer etc)
        /// </summary>
        [Required]
        private string Name { get; set; } = null!;
    }
}