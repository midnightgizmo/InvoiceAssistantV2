﻿

using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Client.ViewModels.Invoices.Add;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class AddPaymentControllerLogiccs
	{
		public ControllerLogicReturnValue Process(int InvoiceId, string Description, decimal Ammount)
		{
			ControllerLogicReturnValue returnValue;

			returnValue = this.CheckInputForErrors(InvoiceId, Description, Ammount);

			if (returnValue.HasErrors)
				return returnValue;

			returnValue = this.AddInvoicePayment(InvoiceId, Description, Ammount);

			return returnValue;
		}
		
		private ControllerLogicReturnValue CheckInputForErrors(int InvoiceId, string Description, decimal Ammount)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			// remove any white space at begining and end of string
			string description = Description.Trim();

			// if we have an ammount less th an zero
			if (InvoiceId < 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("InvoiceId must be greater than zero");
			}

			// if we don't have a description
			if (description.Length < 1)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Missing Value Description");
			}

			// if we have an ammount less th an zero
			if(Ammount < 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Ammount must be zero or greater");
			}

			return returnValue;
		}

		private ControllerLogicReturnValue AddInvoicePayment(int InvoiceId, string Description, decimal Ammount)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			// remove any white space at begining and end of string
			string description = Description.Trim();

			using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext()) 
			{
				// make sure the Invoice exists
				InvoiceDb dbInvoice = new InvoiceDb(dbContext);
				if(dbInvoice.SelectInvoice(InvoiceId) == null)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Invoice not found");
					return returnValue;
				}

				// if we get this far we have found the invoice 
				InvoicePaymentBreakDownDb dbPaymentBreakDown = new InvoicePaymentBreakDownDb(dbContext);
				InvoicePaymentBreakDown? newPayment;
				newPayment = dbPaymentBreakDown.AddPayment(InvoiceId, Description,Ammount);

				if (newPayment == null)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to add payment to invoice");
					return returnValue;
				}
				else
					returnValue.ReturnValue = newPayment;
			}


			// if we get this far,we should have a new payemnt set in returnValue.Returnvalue;
			return returnValue;
		}
	}
}
