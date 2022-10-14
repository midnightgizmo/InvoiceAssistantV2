using InvoiceAssistantV2.Shared.Models.Database.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAssistantV2.Shared.Models.Database.Invoice
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Unique reference number to this invoice
        /// </summary>
        [Required]
        public string ReferenceNumber { get; set; } = null!;
        /// <summary>
        /// A short description of the invoice for us (not sent to the customer)
        /// </summary>
        [Required]
        public string Description { get; set; } = null!;
        /// <summary>
        /// The date we want to appear on the invoice when sent to the customer
        /// </summary>
        public DateTime DateOfInvoice { get; set; }
        /// <summary>
        /// The date the Money from the customer went into the account
        /// </summary>
        public DateTime? DateRecievedMoney { get; set; }
        /// <summary>
        /// Total ammount the customer should pay us.
        /// </summary>
        public decimal TotalInvoiceAmmount { get; set; }

        public int PaymentTypeID { get; set; }
        public PaymentType? PaymentType { get; set; }

        public int AddressToMakeInvoiceOutToId { get; set; }
        /// <summary>
        /// The Address to make the Invoice out too.
        /// </summary>
        public CompanyAddress? AddressToMakeInvoiceOutTo{ get; set; }

        /// <summary>
        /// List of company address we visited for this invoice (used to cacualte milage)
        /// </summary>
        public IEnumerable<PlacesVisitedForInvoice> PlacesVisitedForInvoice { get; set; } = null!;

        /// <summary>
        /// List of payments that belong to this invoice
        /// </summary>
        public IEnumerable<InvoicePaymentBreakDown> InvoicePayments { get; set; } = null!;
    }
}
