using Database.Data;
using Database.DbInteractions;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class GetInvoiceTotalPaymentAmountControllerLogic
	{
		public ControllerLogicReturnValue Process(int InvoiceId)
		{
			ControllerLogicReturnValue DataToReturn = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				InvoiceDb invoiceDb = new InvoiceDb(dbContext);

				Shared.Models.Database.Invoice.Invoice? foundInvoice = invoiceDb.SelectInvoice(InvoiceId);
				if(foundInvoice == null)
				{
					DataToReturn.HasErrors = true;
					DataToReturn.Errors.Add("Unable to find invoice");
					return DataToReturn;
				}
				// if we get this far, we have found the invoice
				DataToReturn.ReturnValue = foundInvoice.TotalInvoiceAmmount;
			}
			return DataToReturn;
		}
	}
}
