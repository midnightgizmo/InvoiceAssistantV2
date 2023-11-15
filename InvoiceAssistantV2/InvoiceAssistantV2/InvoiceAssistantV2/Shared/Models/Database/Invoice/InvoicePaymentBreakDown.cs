using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceAssistantV2.Shared.Models.Database.Invoice
{
    /// <summary>
    /// Payments assoshiated with <see cref="Invoice"/>
    /// </summary>
    public class InvoicePaymentBreakDown : IEditableObject
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







		private InvoicePaymentBreakDown _InvoicePaymentBreakDownCopy;

		[NotMapped]
		public bool isInEditState { get; set; } = false;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static InvoicePaymentBreakDown Copy(InvoicePaymentBreakDown CopyFrom, InvoicePaymentBreakDown CopyTo)
		{
			// make a copy of the data

			CopyTo.Id = CopyFrom.Id;
			CopyTo.InvoiceId = CopyFrom.InvoiceId;
			CopyTo.Description = CopyFrom.Description;
			CopyTo.Ammount = CopyFrom.Ammount;

			return CopyTo;
		}

		public void BeginEdit()
		{
			this.isInEditState = true;
			this._InvoicePaymentBreakDownCopy = InvoicePaymentBreakDown.Copy(this, new InvoicePaymentBreakDown());
		}

		/// <summary>
		/// We are happy with the changes we made and now want to commit
		/// them to memory
		/// </summary>
		public void EndEdit()
		{
			// no longer need the orignal
			this._InvoicePaymentBreakDownCopy = null;
			this.isInEditState = false;
		}

		/// <summary>
		/// We want to restore the orignal values and
		/// forget any changes that were made from when
		/// BeginEdit was called
		/// </summary>
		public void CancelEdit()
		{
			// retore the orignal values
			InvoicePaymentBreakDown.Copy(this._InvoicePaymentBreakDownCopy, this);

			// no longer need the copy
			this._InvoicePaymentBreakDownCopy = null;
			this.isInEditState = false;
		}




	}
}