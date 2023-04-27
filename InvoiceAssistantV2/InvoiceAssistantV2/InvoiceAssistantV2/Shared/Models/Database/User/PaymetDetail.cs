using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceAssistantV2.Shared.Models.Database.User
{
	/// <summary>
	/// Represents a single payment detail for <see cref="PaymentMethod"/>
	/// </summary>
	public class PaymetDetail : IEditableObject
	{
		[Key]
		public int Id { get; set; }
		/// <summary>
		/// Foreign key link to <see cref="PaymentMethod"/>
		/// </summary>
		public int PaymentMethodId { get; set; }

		/// <summary>
		/// the key value of a payment detail. e.g. sort code
		/// </summary>
		[Required]
		public string Key { get; set; } = null!;
		/// <summary>
		/// The value part of a payment detail e.g. 10-11-12.
		/// Note: the Value part is allowed to be null;
		/// </summary>
		public string? Value { get; set; } = null!;





		#region IEditableObject


		/// <summary>
		/// Creates a copy of the <see cref="PaymetDetail"/> class
		/// </summary>
		/// <param name="CopyFrom">the object to copy from</param>
		/// <param name="CopyTo">the object to copy into</param>
		/// <returns>the copy</returns>
		private static PaymetDetail Copy(PaymetDetail CopyFrom, PaymetDetail CopyTo)
		{
			CopyTo.Id = CopyFrom.Id;
			CopyTo.Key = CopyFrom.Key;
			CopyTo.Value = CopyFrom.Value;
			CopyTo.PaymentMethodId = CopyFrom.PaymentMethodId;

			return CopyTo;

		}

		[NotMapped]
		public bool IsInEditMode { get; set; } = false;

		/// <summary>
		/// Used to store a copy of this instance of the <see cref="PaymetDetail"/> class
		/// when we are being edited.
		/// </summary>
		[NotMapped]
		private PaymetDetail? _PaymentDetailCopy { get; set; } = null;
		public void BeginEdit()
		{
			this._PaymentDetailCopy = PaymetDetail.Copy(this, new PaymetDetail());
			this.IsInEditMode = true;
		}

		/// <summary>
		/// We are happy with the changes we made and now want to commit
		/// them to memory
		/// </summary>
		public void EndEdit()
		{
			// no longer need the orignal
			this._PaymentDetailCopy = null;
			this.IsInEditMode = false;
		}

		/// <summary>
		/// We want to restore the orignal values and
		/// forget any changes that were made from when
		/// BeginEdit was called
		/// </summary>
		public void CancelEdit()
		{
			if (this._PaymentDetailCopy == null)
				return;

			// retore the orignal values
			PaymetDetail.Copy(this._PaymentDetailCopy, this);

			// no longer need the copy
			this._PaymentDetailCopy = null;

			this.IsInEditMode = false;
		}

		#endregion

	}
}