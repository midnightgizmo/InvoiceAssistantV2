using Database.Data;
using Database.DbInteractions;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class RemoveVisitedAddressFromInvoiceControllerLogic
	{
		private HttpResponse _HttpResponse;
		public RemoveVisitedAddressFromInvoiceControllerLogic(HttpResponse httpResponse)
        {

			this._HttpResponse = httpResponse;
		}
		public ControllerLogicReturnValue Process(int InvoiceId, int AddressId)
		{
			ControllerLogicReturnValue returnValue;

			// check we have valid input values
			returnValue = this.CheckInputValuesForErrors(InvoiceId, AddressId);
			// if input values are wrong, return errors (300 response)
			if (returnValue.HasErrors == true)
				return returnValue;

			// remove visited address from invoice in database
			return this.RemoveVisitedAddressFromInvoice(InvoiceId, AddressId);

			
		}

		private ControllerLogicReturnValue CheckInputValuesForErrors(int invoiceId, int AddressId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			if (invoiceId <= 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid invoiceId");

				this._HttpResponse.StatusCode = 300;
			}
			if (AddressId <= 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid AddressId");

				this._HttpResponse.StatusCode = 300;
			}

			return returnValue;
		}

		private ControllerLogicReturnValue RemoveVisitedAddressFromInvoice(int invoiceId, int addressId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				PlacesVisitedForInvoiceDb placeVisitedDb = new PlacesVisitedForInvoiceDb(dbContext);
				
				// try and find/remove the address from the invoice
				bool wasSucsefull = placeVisitedDb.DeleteAddressLinkedToInvoice(invoiceId, addressId);
				// if we were unable to remove the row from the database
				if (wasSucsefull == false)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to remove address. It may not exist");
					returnValue.ReturnValue = false;
					this._HttpResponse.StatusCode = 300;
				}
				// All went well and row was removed from database
				else
				{
					returnValue.ReturnValue = true;
				}


			}

			return returnValue;
		}
	}
}
