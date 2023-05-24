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
	public class UserAddress : IEditableObject
	{
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Foreign key link to <see cref="UserDetails"/>
		/// </summary>
		public int UserDetailsId { get; set; }

		public string? AddressLine1 { get; set; }
		public string? AddressLine2 { get; set; }
		public string? AddressLine3 { get; set; }
		public string? AddressLine4 { get; set; }
		public string? AddressLine5 { get; set; }
		public string? PostCode { get; set; }



		/// <summary>
		/// Creates a shallow copy of the <see cref="UserAddress"/> class
		/// </summary>
		/// <param name="CopyFrom">the object to copy from</param>
		/// <param name="CopyTo">the object to copy into</param>
		/// <returns>the copy</returns>
		private static UserAddress Copy(UserAddress CopyFrom, UserAddress CopyTo)
		{
			CopyTo.Id = CopyFrom.Id;
			CopyTo.AddressLine1 = CopyFrom.AddressLine1;
			CopyTo.AddressLine2 = CopyFrom.AddressLine2;
			CopyTo.AddressLine3 = CopyFrom.AddressLine3;
			CopyTo.AddressLine4 = CopyFrom.AddressLine4;
			CopyTo.AddressLine5 = CopyFrom.AddressLine5;
			CopyTo.PostCode = CopyFrom.PostCode;
			CopyTo.UserDetailsId = CopyFrom.UserDetailsId;

			return CopyTo;

		}

		[NotMapped]
		public bool IsInEditMode { get; set; } = false;

		/// <summary>
		/// Used to store a shall copy of this instance of the <see cref="UserAddress"/> class
		/// when we are being edited.
		/// </summary>
		[NotMapped]
		private UserAddress? _UserDetailsShallowCopy { get; set; } = null;

		/// <summary>
		/// Set this moel into an edit state. Allows properties to be edited and then
		/// canceled by calling CancelEdit. To Confirm changes, call EndEdit
		/// </summary>
		public void BeginEdit()
		{
			this._UserDetailsShallowCopy = UserAddress.Copy(this, new UserAddress());
			this.IsInEditMode = true;
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
			UserAddress.Copy(this._UserDetailsShallowCopy, this);

			// no longer need the copy
			this._UserDetailsShallowCopy = null;

			this.IsInEditMode = false;
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
	}
}
