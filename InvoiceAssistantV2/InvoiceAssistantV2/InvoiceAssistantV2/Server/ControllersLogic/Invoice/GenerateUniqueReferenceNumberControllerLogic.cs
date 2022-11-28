using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class GenerateUniqueReferenceNumberControllerLogic
	{
		public ControllerLogicReturnValue Process(DateTime InvoiceDate)
		{
			ControllerLogicReturnValue returnValue;

			returnValue = this.GenerateReferenceNumber(InvoiceDate);

			return returnValue;
		}


		/// <summary>
		/// creates a unique 8 digit number
		/// </summary>
		/// <param name="invoiceDate"></param>
		/// <returns></returns>
		private ControllerLogicReturnValue GenerateReferenceNumber(DateTime invoiceDate)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
			List<Shared.Models.Database.Invoice.Invoice> FoundInvoices;

			// find all invoices that have been created on the given date
			FoundInvoices = this.SelectAllInvoicesOn(invoiceDate);

			// If there are no invoices on the given date
			if(FoundInvoices.Count == 0) 
			{
				// generate a reference number using the date and add "01" to the end of it
				returnValue.ReturnValue = this.CreateReferenceNumber(invoiceDate, 1);
			}
			else
			{
				int nextReferenceNumber = this.FindNextNumberToUseToGernerateReferenceNumber(FoundInvoices);
				returnValue.ReturnValue = this.CreateReferenceNumber(invoiceDate, nextReferenceNumber);
			}
			

			return returnValue;

		}



		/// <summary>
		/// Find all invoices that are held on the given date
		/// </summary>
		/// <param name="invoiceDate">The date to use to find invoices on</param>
		/// <returns></returns>
		private List<Shared.Models.Database.Invoice.Invoice> SelectAllInvoicesOn(DateTime invoiceDate)
		{
			List<Shared.Models.Database.Invoice.Invoice> ListOfInvoices;
			DateTime startOfDay, endOfDay;
			
			// we want to search from 12:00am to 11:59pm
			startOfDay = new DateTime(invoiceDate.Year,invoiceDate.Month,invoiceDate.Day,0,0,0);
			endOfDay = new DateTime(invoiceDate.Year, invoiceDate.Month, invoiceDate.Day, 23, 59, 59);

			using(InvoiceAssistantDbContext dbContext= new InvoiceAssistantDbContext()) 
			{
				InvoiceDb dbInvoice = new InvoiceDb(dbContext);
				// find all invoices in the database on the given date
				ListOfInvoices = dbInvoice.Select_CustomQuery(startOfDay, endOfDay,null,null,null,null,null,null,null,null);
			}

			return ListOfInvoices;
		}

		/// <summary>
		/// Creates an 8 digit number using the passed in data day + month + year + EndNumberToAppend
		/// </summary>
		/// <param name="date"></param>
		/// <param name="EndNumberToAppend"></param>
		/// <returns>8 digit number</returns>
		private string CreateReferenceNumber(DateTime date, int EndNumberToAppend)
		{
			// returns a 8 digit number as a string
			return date.ToString("ddMMyy") + EndNumberToAppend.ToString("00");
		}

		/// <summary>
		/// Looks through the passed in lists reference numbers (which all should be on the same date) 
		/// and finds the reference number whos last 2 digits are the highest. It then returns one number 
		/// higher
		/// </summary>
		/// <param name="foundInvoices"></param>
		/// <returns>A 2 digit number that can be used to help create a reference number</returns>
		private int FindNextNumberToUseToGernerateReferenceNumber(List<Shared.Models.Database.Invoice.Invoice> foundInvoices)
		{
			return foundInvoices.Count + 1;
		}
	}
}
