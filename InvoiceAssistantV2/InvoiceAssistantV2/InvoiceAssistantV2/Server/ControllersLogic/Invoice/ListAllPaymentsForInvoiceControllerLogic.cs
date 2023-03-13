using Database.Data;
using Database.DbInteractions;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class ListAllPaymentsForInvoiceControllerLogic
	{

		public ControllerLogicReturnValue Process(int InvoiceId)
		{
			return this.GetAllPaymentsForInvoice(InvoiceId);
		}

		/// <summary>
		/// sets the ControllerLogicReturnValue.ReturnValue to a list of <see cref="InvoicePaymentBreakDownDb"/>
		/// that are assoshiated with the invoice
		/// </summary>
		/// <param name="InvoiceId"></param>
		/// <returns></returns>
		private ControllerLogicReturnValue GetAllPaymentsForInvoice(int InvoiceId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext  dbContext = new InvoiceAssistantDbContext())
			{
				InvoicePaymentBreakDownDb paymentsDb = new InvoicePaymentBreakDownDb(dbContext);

				returnValue.ReturnValue = paymentsDb.GetAllPaymentsForInvoice(InvoiceId);
			}

			return returnValue;
		}
	}
}
