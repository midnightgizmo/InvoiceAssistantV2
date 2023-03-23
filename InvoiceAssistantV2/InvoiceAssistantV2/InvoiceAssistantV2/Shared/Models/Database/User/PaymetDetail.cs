using System.ComponentModel.DataAnnotations;

namespace InvoiceAssistantV2.Shared.Models.Database.User
{
	/// <summary>
	/// Represents a single payment detail for <see cref="PaymentMethod"/>
	/// </summary>
	public class PaymetDetail
	{
		[Key]
		public int Id { get; set; }

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
	}
}