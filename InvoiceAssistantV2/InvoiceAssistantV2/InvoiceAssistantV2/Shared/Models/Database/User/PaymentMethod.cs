using System.ComponentModel.DataAnnotations;

namespace InvoiceAssistantV2.Shared.Models.Database.User
{
	/// <summary>
	/// Holds the details for a type of payment method, e.g. Bank transfer
	/// </summary>
	public class PaymentMethod
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
    }
}