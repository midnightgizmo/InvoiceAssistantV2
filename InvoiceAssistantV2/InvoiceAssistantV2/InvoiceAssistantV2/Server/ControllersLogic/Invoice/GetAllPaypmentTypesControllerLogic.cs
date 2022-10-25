using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class GetAllPaypmentTypesControllerLogic
	{
        public ControllerLogicReturnValue Process()
        {
            ControllerLogicReturnValue DataToReturn = new ControllerLogicReturnValue();

            InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext();
            PaymentTypeDb paymentTypeDb = new PaymentTypeDb(dbContext);

            DataToReturn.ReturnValue = paymentTypeDb.SelectAll();

            return DataToReturn;
        }

    }
}
