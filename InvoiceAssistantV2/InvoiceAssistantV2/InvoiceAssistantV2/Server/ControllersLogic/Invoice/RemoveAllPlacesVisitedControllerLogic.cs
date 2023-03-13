using Database.Data;
using Database.DbInteractions;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class RemoveAllPlacesVisitedControllerLogic
	{
		private HttpResponse _HttpResponse;

		public RemoveAllPlacesVisitedControllerLogic(HttpResponse httpResponse)
		{
			this._HttpResponse = httpResponse;
		}

		public ControllerLogicReturnValue Process(int InvoiceId)
		{
			ControllerLogicReturnValue returnValue;

			returnValue = this.CheckInputValuesForErrors(InvoiceId);

			if (returnValue.HasErrors)
				return returnValue;

			returnValue = this.RemovePlacesVisited(InvoiceId);


			return returnValue;
		}

		private ControllerLogicReturnValue CheckInputValuesForErrors(int invoiceId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			if(invoiceId <=0) 
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid invoiceId");

				this._HttpResponse.StatusCode = 300;
			}

			return returnValue;
		}

		private ControllerLogicReturnValue RemovePlacesVisited(int invoiceId)
		{
			using (InvoiceAssistantDbContext dbContext= new InvoiceAssistantDbContext()) 
			{
				PlacesVisitedForInvoiceDb placeVisitedDb = new PlacesVisitedForInvoiceDb(dbContext);

				placeVisitedDb.DeletePlacesLinkedToInvoice(invoiceId);


			}

			return new ControllerLogicReturnValue() { ReturnValue = true };


		}
	}
}
