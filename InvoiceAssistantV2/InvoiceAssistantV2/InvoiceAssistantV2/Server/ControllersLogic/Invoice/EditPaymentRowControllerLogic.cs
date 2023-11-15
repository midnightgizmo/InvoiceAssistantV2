using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Client.Pages.Invoices;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class EditPaymentRowControllerLogic
	{
		public ControllerLogicReturnValue Process(int InvoicePaymentId, string Description, decimal Ammount)
		{
			ControllerLogicReturnValue returnValue;

			returnValue = this.CheckInputForErrors(InvoicePaymentId, Description, Ammount);

			if (returnValue.HasErrors)
				return returnValue;

			returnValue = this.UpdatePaymentRow(InvoicePaymentId, Description, Ammount);

			return returnValue;
		}


		private ControllerLogicReturnValue CheckInputForErrors(int InvoicePaymentId, string Description, decimal Ammount)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			// remove any white space at begining and end of string
			string description = Description.Trim();

			// if we have an ammount less th an zero
			if (InvoicePaymentId < 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("InvoicePaymentId must be greater than zero");
			}

			// if we don't have a description
			if (description.Length < 1)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Missing Value Description");
			}

			// if we have an ammount less th an zero
			if (Ammount < 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Ammount must be zero or greater");
			}

			return returnValue;
		}

		private ControllerLogicReturnValue UpdatePaymentRow(int InvoicePaymentId, string Description, decimal Ammount)
		{


			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			// remove any white space at begining and end of string
			string description = Description.Trim();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				// make sure the payment exists
				InvoicePaymentBreakDownDb dbPaymentBreakDown = new InvoicePaymentBreakDownDb(dbContext);
				// find the payment row in the database 
				InvoicePaymentBreakDown? FoundPayment = dbPaymentBreakDown.Select(InvoicePaymentId);

				// if we could not find the payment in the database
				if (FoundPayment == null)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Invoice Payment not found");
					return returnValue;
				}

				// make sure the invoice exists (we will need this later on to update the total amount the invoice is for)
				InvoiceDb dbInvoice = new InvoiceDb(dbContext);
				Shared.Models.Database.Invoice.Invoice? FoundInvoice = dbInvoice.SelectInvoice(FoundPayment.InvoiceId);
				if (FoundInvoice == null) 
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("No Invoice found that is associated with invoice payment to be updated");
					return returnValue;
				}

				// if we get this far we have found the payment row 

				// update the payment in the database
				FoundPayment.Description = description;
				FoundPayment.Ammount = Ammount;
				FoundPayment = dbPaymentBreakDown.UpdatePayment(FoundPayment);

				if (FoundPayment == null)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to update payment");
					return returnValue;
				}
				// payment was sucsesfully added
				else
				{
					returnValue.ReturnValue = FoundPayment;

					// Sums up all the payments allocated to this invoice and updates
					// the TotalInvoiceAmmount value in the Invoice table

					// sum of all the payments allocated to this invoice
					FoundInvoice.TotalInvoiceAmmount = dbPaymentBreakDown.GetAllPaymentsForInvoice(FoundInvoice.Id).Sum(s => s.Ammount);
					// update the invoice total ammount in the database
					dbInvoice.Update(FoundInvoice);
					dbContext.SaveChanges();
				}


			}



			// if we get this far,we should have a new payemnt set in returnValue.Returnvalue;
			return returnValue;





		}


	}
}
