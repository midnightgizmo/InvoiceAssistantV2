using InvoiceAssistantV2.Shared.Models.Database.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAssistantV2.Shared.Models.Database.Invoice
{
    public class Invoice : IEditableObject
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

        public int? PaymentTypeID { get; set; }
        public PaymentType? PaymentType { get; set; }

        public int? AddressToMakeInvoiceOutToId { get; set; }
        /// <summary>
        /// The Address to make the Invoice out too.
        /// </summary>
        public CompanyAddress? AddressToMakeInvoiceOutTo{ get; set; }

        /// <summary>
        /// List of company address we visited for this invoice (used to cacualte milage)
        /// </summary>
        public List<PlacesVisitedForInvoice> PlacesVisitedForInvoice { get; set; } = null!;

        /// <summary>
        /// List of payments that belong to this invoice
        /// </summary>
        public List<InvoicePaymentBreakDown> InvoicePayments { get; set; } = null!;




        private Invoice _InvoiceCopy;


        /// <summary>
        /// Does NOT copy <see cref="PlacesVisitedForInvoice"/> or <see cref="InvoicePayments"/>
        /// </summary>
        /// <returns></returns>
		public static Invoice Copy(Invoice CopyFrom, Invoice CopyTo)
		{
            // make a copy of all the data except PlacesVisitedForInvoice and InvoicePayments (just
            // ignore those properties)

            CopyTo.Id = CopyFrom.Id;
            CopyTo.ReferenceNumber = CopyFrom.ReferenceNumber;
            CopyTo.Description = CopyFrom.Description;
            CopyTo.DateOfInvoice = CopyFrom.DateOfInvoice;
            CopyTo.DateRecievedMoney = CopyFrom.DateRecievedMoney;
            CopyTo.TotalInvoiceAmmount = CopyFrom.TotalInvoiceAmmount;
            CopyTo.PaymentTypeID = CopyFrom.PaymentTypeID;
            CopyTo.AddressToMakeInvoiceOutToId = CopyFrom.AddressToMakeInvoiceOutToId;
            CopyTo.PaymentType = CopyFrom.PaymentType;
            CopyTo.AddressToMakeInvoiceOutTo = CopyFrom.AddressToMakeInvoiceOutTo;

			return CopyTo;
		}

		public void BeginEdit()
		{
			this._InvoiceCopy = Invoice.Copy(this,new Invoice());
		}

		/// <summary>
		/// We are happy with the changes we made and now want to commit
		/// them to memory
		/// </summary>
		public void EndEdit()
		{
			// no longer need the orignal
			this._InvoiceCopy = null;
		}

		/// <summary>
		/// We want to restore the orignal values and
		/// forget any changes that were made from when
		/// BeginEdit was called
		/// </summary>
		public void CancelEdit()
		{
			// retore the orignal values
			Invoice.Copy(this._InvoiceCopy, this);

			// no longer need the copy
			this._InvoiceCopy = null;
		}
	}
}
