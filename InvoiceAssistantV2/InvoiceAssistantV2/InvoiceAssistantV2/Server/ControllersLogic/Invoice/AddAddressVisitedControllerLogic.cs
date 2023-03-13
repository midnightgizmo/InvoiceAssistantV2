﻿using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class AddAddressVisitedControllerLogic
	{

		private HttpResponse _HttpResponse;

		public AddAddressVisitedControllerLogic(HttpResponse httpResponse)
		{
			this._HttpResponse = httpResponse;
		}

		public ControllerLogicReturnValue Process(int InvoiceId, int AddressId, int NoTimesVisited)
		{
			ControllerLogicReturnValue returnValue;

			returnValue = this.CheckInputValues(InvoiceId, AddressId, NoTimesVisited);
			if (returnValue.HasErrors)
				return returnValue;

			returnValue = this.AddAddressVisitedToDatabase(InvoiceId, AddressId, NoTimesVisited);

			return returnValue;
		}

		/// <summary>
		/// chekcs input values to make sure they are greater than zero
		/// </summary>
		/// <param name="invoiceId"></param>
		/// <param name="AddressId"></param>
		/// <param name="noTimesVisited"></param>
		/// <returns></returns>
		private ControllerLogicReturnValue CheckInputValues(int invoiceId, int AddressId, int noTimesVisited)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			if(invoiceId <= 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid InvoicID");
			}

			if (AddressId <= 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid AddressId");
			}

			if(AddressId <= 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid noTimesVisited");
			}

			if (returnValue.HasErrors)
				this._HttpResponse.StatusCode = 400;

			return returnValue;
		}

		/// <summary>
		/// Attempts to add the visited address with its no times visited to the database.
		/// If its allready found in the database, the data will not be added.
		/// </summary>
		/// <param name="invoiceId"></param>
		/// <param name="addressId"></param>
		/// <param name="noTimesVisited"></param>
		/// <returns></returns>
		private ControllerLogicReturnValue AddAddressVisitedToDatabase(int InvoiceId, int AddressId, int NoTimesVisited)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				PlacesVisitedForInvoiceDb dbPlacesVisited = new PlacesVisitedForInvoiceDb(dbContext);

				// check to see if there is allready an invoice with this address asigned to it
				PlacesVisitedForInvoice? PlaceVisitedModel = dbPlacesVisited.SelectPlacesVisitedForInvoice(InvoiceId, AddressId);

				// the place visisted allready exists on this invoice
				if(PlaceVisitedModel != null) 
				{ 
					// send back an error to say it allready exists and the data was not added.
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Address allready assigned to invoice, unable to add");

					if (returnValue.HasErrors)
						this._HttpResponse.StatusCode = 400;

					return returnValue;
				}

				// does not exist, so lets add it.
				PlaceVisitedModel = dbPlacesVisited.Insert(new PlacesVisitedForInvoice() 
				{ 
					CompanyAddressId= AddressId,
					InvoiceId= InvoiceId,
					NumberOfTimesVisited= NoTimesVisited
				});

				// if we were unabel to add it to the database (somthing went wrong)
				if(PlaceVisitedModel ==  null)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unknown erorr when adding to database");

					if (returnValue.HasErrors)
						this._HttpResponse.StatusCode = 500;
				}

				// was addded ok so set return value to PlaceVisitedModel
				returnValue.ReturnValue = PlaceVisitedModel;
				return returnValue;

			}

		}
	}
}
