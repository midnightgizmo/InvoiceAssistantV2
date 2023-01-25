using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Client.Pages.Invoices;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class RemovePaymentControllerLogic
	{
		internal ControllerLogicReturnValue Process(int invoicePaymentId)
		{
			ControllerLogicReturnValue returnValue;

			returnValue = this.CheckInputForErrors(invoicePaymentId);
			if (returnValue.HasErrors)
				return returnValue;

			returnValue = this.RemovePayment(invoicePaymentId);

			return returnValue;
		}

		private ControllerLogicReturnValue CheckInputForErrors(int invoicePaymentId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			if (invoicePaymentId < 1)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid value on invoicePaymentId");
				returnValue.ReturnValue = false;
			}
			else
				returnValue.ReturnValue = true;

			return returnValue;
		}

		private ControllerLogicReturnValue RemovePayment(int invoicePaymentId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				InvoicePaymentBreakDownDb dbPaymentBreakDown = new InvoicePaymentBreakDownDb(dbContext);
				// we need to find the payment so we can get its invoice id.
				InvoicePaymentBreakDown? payment = dbPaymentBreakDown.Select(invoicePaymentId);
				
				if (payment == null || dbPaymentBreakDown.RemovePayment(invoicePaymentId) == false)
				{// we were unable to remove the payment from the database (may be it does not exists)

					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to remove payment");
					returnValue.ReturnValue = false;
				}
				else
				{// payment removed
					


					// Sums up all the payments allocated to this invoice and updates
					// the TotalInvoiceAmmount value in the Invoice table

					// find the invoice in the database we want to add a payament too
					InvoiceDb dbInvoice = new InvoiceDb(dbContext);
					Shared.Models.Database.Invoice.Invoice? FoundInvoice = dbInvoice.SelectInvoice(payment.InvoiceId);

					// make sure we found the invoice
					if(FoundInvoice != null) 
					{ 
						// sum of all the payments allocated to this invoice
						FoundInvoice.TotalInvoiceAmmount = dbPaymentBreakDown.GetAllPaymentsForInvoice(FoundInvoice.Id).Sum(s => s.Ammount);
						// update the invoice total ammount in the database
						dbInvoice.Update(FoundInvoice);
						dbContext.SaveChanges();

						// set the return value to the ture to say payment was remove
						// and invoice total ammount has been updated sucsefully
						returnValue.ReturnValue = true;
					}
					// could not find the invoice
					else
					{
						returnValue.HasErrors = true;
						returnValue.Errors.Add("Payment was removed but Invoice total ammount was not updated");
						returnValue.ReturnValue = false;
					}


				}
			}

			return returnValue;
		}
	}
}
