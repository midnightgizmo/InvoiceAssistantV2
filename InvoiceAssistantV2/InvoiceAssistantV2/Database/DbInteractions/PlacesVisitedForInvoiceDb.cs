using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
	public class PlacesVisitedForInvoiceDb
	{
		private Data.InvoiceAssistantDbContext _DbContext;
		public PlacesVisitedForInvoiceDb(Data.InvoiceAssistantDbContext dbContext)
		{
			_DbContext = dbContext;
		}

		

		

		public List<PlacesVisitedForInvoice> SelectAllPlacesVisitedForInvoice(int InvoiceId)
		{
			List<PlacesVisitedForInvoice> PlacesVisited;
			PlacesVisited = _DbContext.PlacesVisitedForInvoice.Where(i => i.InvoiceId== InvoiceId).ToList();

			return PlacesVisited;
		}

		public PlacesVisitedForInvoice? SelectPlacesVisitedForInvoice(int invoiceId, int addressId)
		{
			return _DbContext.PlacesVisitedForInvoice.Where(p => p.InvoiceId == invoiceId && p.CompanyAddressId == addressId).FirstOrDefault();
		}


		public PlacesVisitedForInvoice? Insert(PlacesVisitedForInvoice placesVisitedForInvoice)
		{
			_DbContext.PlacesVisitedForInvoice.Add(placesVisitedForInvoice);

			// insert the record and check if insert sucseeded.
			if (_DbContext.SaveChanges() > 0)
				return placesVisitedForInvoice;
			else
				return null;
		}

		/// <summary>
		/// Remove all visited addresses from passed in invoice id
		/// </summary>
		/// <param name="invoiceId">The invoice to remove addresses from</param>
		public void DeletePlacesLinkedToInvoice(int invoiceId)
		{
			var PlacesVisited = _DbContext.PlacesVisitedForInvoice.Where(i => i.InvoiceId == invoiceId).ToList();
			_DbContext.PlacesVisitedForInvoice.RemoveRange(PlacesVisited);
			_DbContext.SaveChanges();
		}

		/// <summary>
		/// Remove a single address visited from specified invoice
		/// </summary>
		/// <param name="invoiceId">Invoice to remove address from</param>
		/// <param name="addressId">Address to remove</param>
		/// <returns>true if row removed, else false</returns>
		public bool DeleteAddressLinkedToInvoice(int invoiceId, int addressId)
		{
			// find the row in the database we want to remove.
			var placesVisited = _DbContext.PlacesVisitedForInvoice.Where(i => i.InvoiceId == invoiceId && i.CompanyAddressId== addressId).ToList();
			// remove the row from the database.
			_DbContext.PlacesVisitedForInvoice.RemoveRange(placesVisited);
			if (_DbContext.SaveChanges() > 0)
				return true;
			else
				return false;
		}

	}
}
