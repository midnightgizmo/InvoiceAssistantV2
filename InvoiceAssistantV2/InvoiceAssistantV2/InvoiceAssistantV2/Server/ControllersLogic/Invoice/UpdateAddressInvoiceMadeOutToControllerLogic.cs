using Database.Data;
using Database.DbInteractions;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class UpdateAddressInvoiceMadeOutToControllerLogic
	{
		//this will be inishalized when Process method is called
		// used for setting the status code
		private HttpResponse _HttpResponse = null!;

		public ControllerLogicReturnValue Process(int InvoiceId, int? AddressToMakeInvoiceOutToId, HttpResponse HttpServerResponse)
		{
			ControllerLogicReturnValue returnValue;

			this._HttpResponse = HttpServerResponse;

			returnValue = this.CheckInputsForErrors(InvoiceId, AddressToMakeInvoiceOutToId);
			if (returnValue.HasErrors == true)
				return returnValue;

			// try and upate address the invoice is made out to
			returnValue = this.UpdateAddressToMakeInvoiceOutTo(InvoiceId, AddressToMakeInvoiceOutToId);

			// if no errors, returns true
			return returnValue;
		}

		private ControllerLogicReturnValue CheckInputsForErrors(int invoiceId, int? addressToMakeInvoiceOutToId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
			// if invoice id is less than 1. report an error message
			if(invoiceId <= 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid InvoiceId");
				this._HttpResponse.StatusCode = 300;
			}
			// if an address id has been supplied and its less than 1, report a error message
			if(addressToMakeInvoiceOutToId != null && addressToMakeInvoiceOutToId <= 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid AddressToMakeInvoiceOutToId");
				this._HttpResponse.StatusCode = 300;
			}

			return returnValue;
		}


		private ControllerLogicReturnValue UpdateAddressToMakeInvoiceOutTo(int invoiceId, int? addressToMakeInvoiceOutToId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				InvoiceDb invoiceDb = new InvoiceDb(dbContext);

				// try and find the invoice in the database (make sure it exists)
				Shared.Models.Database.Invoice.Invoice? InvoiceModel = invoiceDb.SelectInvoice(invoiceId);
				// if we could not find the invoice
				if(InvoiceModel == null)
				{
					// report the error
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Invoice not found");
					this._HttpResponse.StatusCode = 300;

					return returnValue;
				}

				// if we are setting an address (its not null),
				// make sure the address exists
				if (addressToMakeInvoiceOutToId != null)
				{
					// check the address we want to use exists.
					CompanyAddressDb companyAddressDb = new CompanyAddressDb(dbContext);
					Shared.Models.Database.Company.CompanyAddress? CompanAddressModel = companyAddressDb.Select(addressToMakeInvoiceOutToId ?? 0);

					// if we could not find the company address
					if(CompanAddressModel == null)
					{
						returnValue.HasErrors = true;
						returnValue.Errors.Add("Addess linked to AddressToMakeInvoiceOutToId not found");
						this._HttpResponse.StatusCode = 300;
						returnValue.ReturnValue = false;

						return returnValue;

					}
				}


				// updated the address invoice made out too.
				InvoiceModel.AddressToMakeInvoiceOutToId = addressToMakeInvoiceOutToId;
				dbContext.SaveChanges();

			}

			returnValue.ReturnValue = true;

			return returnValue;
		}
	}
}
