using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
	public class InvoicePaymentBreakDownDb
	{
		private Data.InvoiceAssistantDbContext _DbContext;

		public InvoicePaymentBreakDownDb(Data.InvoiceAssistantDbContext dbContext)
		{
			this._DbContext = dbContext;
		}

		public InvoicePaymentBreakDown? Select(int PaymentId)
		{
			return this._DbContext.InvoicesPaymentBreakDown.Where(p => p.Id == PaymentId).FirstOrDefault();


		}

		/// <summary>
		/// Selects all the payments that have been allocated to the passed in InvoiceId
		/// </summary>
		/// <param name="InvoiceId"></param>
		/// <returns></returns>
		public List<InvoicePaymentBreakDown> GetAllPaymentsForInvoice(int InvoiceId)
		{
			return this._DbContext.InvoicesPaymentBreakDown.Where(i => i.InvoiceId == InvoiceId).ToList();
		}

		/// <summary>
		/// Adds a new Payment to the specified Invoice
		/// </summary>
		/// <param name="invoiceId">Invoice to add payment too</param>
		/// <param name="description">The text to assoshiate with the price</param>
		/// <param name="ammount">The price for this part of the invoice</param>
		/// <returns></returns>
		public InvoicePaymentBreakDown? AddPayment(int invoiceId, string description, decimal ammount)
		{
			InvoicePaymentBreakDown newPayment= new InvoicePaymentBreakDown();
			newPayment.InvoiceId = invoiceId;
			newPayment.Description = description;
			newPayment.Ammount= ammount;

			this._DbContext.InvoicesPaymentBreakDown.Add(newPayment);
			this._DbContext.SaveChanges();

			if(newPayment.Id > 0)
				return newPayment;
			else
				return null;
		}

		/// <summary>
		/// Removes the specified payment from an invoice
		/// </summary>
		/// <param name="PaymentId">The id of the payment to find and remove</param>
		/// <returns>true if sucsefull, else false</returns>
		public bool RemovePayment(int PaymentId)
		{
			// find the payment in the database we want to remove
			InvoicePaymentBreakDown? payment = this.Select(PaymentId);
			
			// if we could not find the payment
			if(payment == null)
				return false;

			this._DbContext.InvoicesPaymentBreakDown.Remove(payment);
			this._DbContext.SaveChanges();

			return true;

		}
	}
}
