using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAssistantV2.Shared.Models.Database.User
{
	public class UserDetails : IEditableObject
	{
		[Key]
        public int Id { get; set; }

		/// <summary>
		/// The name of the person who the invoice is from
		/// </summary>
		[Required]
		public string UsersName { get; set; } = null!;

		/// <summary>
		/// List of payments the user allows for when an invoice is paid
		/// </summary>
		public List<PaymentMethod> PaymentMethods { get; set; } = null!;



		/// <summary>
		/// Creates a shallow copy of the <see cref="UserDetails"/> class
		/// </summary>
		/// <param name="CopyFrom">the object to copy from</param>
		/// <param name="CopyTo">the object to copy into</param>
		/// <returns>the copy</returns>
		private static UserDetails Copy(UserDetails CopyFrom, UserDetails CopyTo)
		{
			CopyTo.Id = CopyFrom.Id;
			CopyTo.UsersName = CopyFrom.UsersName;
			CopyTo.PaymentMethods = CopyFrom.PaymentMethods;

			return CopyTo;
			
		}

		[NotMapped]
		public bool IsInEditMode { get; set; } = false;

		/// <summary>
		/// Used to store a shall copy of this instance of the <see cref="UserDetails"/> class
		/// when we are being edited.
		/// </summary>
		[NotMapped]
		private UserDetails? _UserDetailsShallowCopy { get; set; } = null;
		public void BeginEdit()
		{
			this._UserDetailsShallowCopy = UserDetails.Copy(this, new UserDetails());
			this.IsInEditMode = true;
		}

		/// <summary>
		/// We are happy with the changes we made and now want to commit
		/// them to memory
		/// </summary>
		public void EndEdit()
		{
			// no longer need the orignal
			this._UserDetailsShallowCopy = null;
			this.IsInEditMode = false;
		}

		/// <summary>
		/// We want to restore the orignal values and
		/// forget any changes that were made from when
		/// BeginEdit was called
		/// </summary>
		public void CancelEdit()
		{
			if (this._UserDetailsShallowCopy == null)
				return;

			// retore the orignal values
			UserDetails.Copy(this._UserDetailsShallowCopy, this);

			// no longer need the copy
			this._UserDetailsShallowCopy = null;

			this.IsInEditMode = false;
		}



	}
}
