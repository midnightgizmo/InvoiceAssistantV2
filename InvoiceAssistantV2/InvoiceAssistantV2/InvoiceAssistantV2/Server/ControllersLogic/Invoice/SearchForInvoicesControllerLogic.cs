using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Client.Models;
using InvoiceAssistantV2.Shared.Models;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class SearchForInvoicesControllerLogic
	{
        public ControllerLogicReturnValue Process(InvoiceSearchParameters invoiceSearchParameters)
        {
            ControllerLogicReturnValue DataToReturn = new ControllerLogicReturnValue();

            this.CheckInputValues(invoiceSearchParameters);

            InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext();
            InvoiceDb invoiceDb = new InvoiceDb(dbContext);
            DataToReturn.ReturnValue = invoiceDb.Select_CustomQuery(invoiceSearchParameters.StartDate, invoiceSearchParameters.EndDate,
                                         invoiceSearchParameters.StartAmmount, invoiceSearchParameters.EndAmmount,
                                         invoiceSearchParameters.DateRecievedMoneyStart, invoiceSearchParameters.DateRecievedMoneyEnd,
                                         invoiceSearchParameters.ReferenceNumber, invoiceSearchParameters.TypeOfPaymentId,
                                         invoiceSearchParameters.AddressToMakePaymentOutToId, invoiceSearchParameters.Description);

            
            return DataToReturn;
        }

        private void CheckInputValues(InvoiceSearchParameters invoiceSearchParameters)
        {
            // check values we have been sent and make sure they are ok

            if (invoiceSearchParameters.TypeOfPaymentId <= 0)
                invoiceSearchParameters.TypeOfPaymentId = null;

            if (invoiceSearchParameters.AddressToMakePaymentOutToId <= 0)
                invoiceSearchParameters.AddressToMakePaymentOutToId = null;
        }

    }
}
