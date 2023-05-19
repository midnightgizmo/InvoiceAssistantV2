using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAssistantV2.Shared.Models.Database.User
{
	public class UserAddress
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
	}
}
