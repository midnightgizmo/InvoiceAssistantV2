using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceAssistantV2.Shared.Models.Database.User
{
	/// <summary>
	/// Holds the details for a type of payment method, e.g. Bank transfer
	/// </summary>
	public class PaymentMethod : IEditableObject
	{
		[Key]
        public int Id { get; set; }
		/// <summary>
		/// Foreign key link to <see cref="UserDetails"/>
		/// </summary>
		public int UserDetailsId { get; set; }
		
		/// <summary>
		/// The name of this payment method e.g. Bank transfer
		/// </summary>
		[Required]
		public string Name { get; set; } = null!;

		public List<PaymetDetail> PaymetDetails { get; set; } = null!;






		#region IEditableObject


		/// <summary>
		/// Creates a shallow copy of the <see cref="PaymentMethod"/> class
		/// </summary>
		/// <param name="CopyFrom">the object to copy from</param>
		/// <param name="CopyTo">the object to copy into</param>
		/// <returns>the copy</returns>
		private static PaymentMethod Copy(PaymentMethod CopyFrom, PaymentMethod CopyTo)
		{
			CopyTo.Id = CopyFrom.Id;
			CopyTo.Name = CopyFrom.Name;
			CopyTo.PaymetDetails = CopyFrom.PaymetDetails;
			CopyTo.UserDetailsId = CopyFrom.UserDetailsId;
			
			return CopyTo;

		}

		[NotMapped]
		public bool IsInEditMode { get; set; } = false;

		/// <summary>
		/// Used to store a shall copy of this instance of the <see cref="PaymentMethod"/> class
		/// when we are being edited.
		/// </summary>
		[NotMapped]
		private PaymentMethod? _PaymentMethodShallowCopy { get; set; } = null;
		public void BeginEdit()
		{
			this._PaymentMethodShallowCopy = PaymentMethod.Copy(this, new PaymentMethod());
			this.IsInEditMode = true;
		}

		/// <summary>
		/// We are happy with the changes we made and now want to commit
		/// them to memory
		/// </summary>
		public void EndEdit()
		{
			// no longer need the orignal
			this._PaymentMethodShallowCopy = null;
			this.IsInEditMode = false;
		}

		/// <summary>
		/// We want to restore the orignal values and
		/// forget any changes that were made from when
		/// BeginEdit was called
		/// </summary>
		public void CancelEdit()
		{
			if (this._PaymentMethodShallowCopy == null)
				return;

			// retore the orignal values
			PaymentMethod.Copy(this._PaymentMethodShallowCopy, this);

			// no longer need the copy
			this._PaymentMethodShallowCopy = null;

			this.IsInEditMode = false;
		}

		#endregion


	}
}