using Database.Data;
using Database.DbInteractions;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class DeleteInvoiceControllerLogic
	{
		public ControllerLogicReturnValue Process(int InvoiceId)
		{
			ControllerLogicReturnValue returnValue;

			returnValue = this.CheckInputForErrors(InvoiceId);
			if (returnValue.HasErrors == true)
				return returnValue;

			returnValue = this.DeleteInvoiceFromDatabase(InvoiceId);

			return returnValue;
		}

		private ControllerLogicReturnValue CheckInputForErrors(int InvoiceId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			if(InvoiceId < 1)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("InvoiceId is an invalid value");
				returnValue.ReturnValue = false;
			}

			return returnValue;

		}

		private ControllerLogicReturnValue DeleteInvoiceFromDatabase(int InvoiceId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				InvoiceDb dbInvoice = new InvoiceDb(dbContext);

				// find the invoice we want to delete
				Shared.Models.Database.Invoice.Invoice? invoiceToDelete = dbInvoice.SelectInvoice(InvoiceId);

				// If the invoice does not exist (we can't delete what does not exists)
				if(invoiceToDelete == null)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to delete invoice. Invoice not found");
					returnValue.ReturnValue = false;
				}
				// we found the invoice, so now we need to delete it
				else
				{
					dbInvoice.Delete(invoiceToDelete);
					dbContext.SaveChanges();

					returnValue.ReturnValue = true;
				}
			}

			return returnValue;
		}
	}
}
