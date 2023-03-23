using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAssistantV2.Shared.Models.Database.User
{
	public class UserDetails
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
    }
}
