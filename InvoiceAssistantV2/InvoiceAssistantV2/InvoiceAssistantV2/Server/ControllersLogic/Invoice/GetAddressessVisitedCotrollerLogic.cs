using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class GetAddressessVisitedCotrollerLogic
	{

		public ControllerLogicReturnValue Process(int InvoiceId)
		{
			ControllerLogicReturnValue ReturnValue = new ControllerLogicReturnValue();

			InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext();
			PlacesVisitedForInvoiceDb placesVisitedDb = new PlacesVisitedForInvoiceDb(dbContext);

			List<PlacesVisitedForInvoice> PlacesVisitedList = placesVisitedDb.SelectAllPlacesVisitedForInvoice(InvoiceId);
			ReturnValue.ReturnValue= PlacesVisitedList;

			return ReturnValue;
		}
	}
}
