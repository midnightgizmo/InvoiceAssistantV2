using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAssistantV2.Shared.Models
{
    /// <summary>
    /// All the posible search parameters we can use when searching for invoices
    /// </summary>
	public class InvoiceSearchParameters
	{
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? StartAmmount { get; set; }
        public decimal? EndAmmount { get; set; }
        public DateTime? DateRecievedMoneyStart { get; set; }
        public DateTime? DateRecievedMoneyEnd { get; set; }
        public string? ReferenceNumber { get; set; }
        public int? TypeOfPaymentId { get; set; } 
        public int? AddressToMakePaymentOutToId { get; set; }
        public string? Description { get; set; }

    }
}
