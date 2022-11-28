using Database.Data;
using Database.DbInteractions;

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
				InvoicePaymentBreakDownDb dbPaymentBreadkDown = new InvoicePaymentBreakDownDb(dbContext);

				if (dbPaymentBreadkDown.RemovePayment(invoicePaymentId) == false)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to remove payment");
					returnValue.ReturnValue = false;
				}
				else
					returnValue.ReturnValue = true;
			}

			return returnValue;
		}
	}
}
