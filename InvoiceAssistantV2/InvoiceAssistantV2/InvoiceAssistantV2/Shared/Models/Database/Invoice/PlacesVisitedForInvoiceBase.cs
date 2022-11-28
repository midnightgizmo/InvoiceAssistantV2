using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAssistantV2.Shared.Models.Database.Invoice
{
	/// <summary>
	/// Used when the client sends data back to the server.
	/// This class also gets inherited by <see cref="PlacesVisitedForInvoice"/>
	/// </summary>
	public class PlacesVisitedForInvoiceBase
	{
		/// <summary>
		/// foreign key for <see cref="CompanyAddress"/>
		/// </summary>
		public int CompanyAddressId { get; set; }


		/// <summary>
		/// The Number of times we visited the <see cref="CompanyAddress"/> for this <see cref="Invoice"/>
		/// </summary>
		public int NumberOfTimesVisited { get; set; }
	}
}
