using InvoiceModels =  InvoiceAssistantV2.Shared.Models.Database.Invoice;
using Microsoft.AspNetCore.Mvc;
using Database.DbInteractions;
using Database.Data;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class UpdateInvoiceMainDetailsControllerLogic
	{
		//this will be inishalized when Process method is called
		// used for setting the status code
		private HttpResponse _HttpResponse = null!;
		public ControllerLogicReturnValue Process(int Id, DateTime DateOfInvoice,string ReferenceNumber,
												  string Description,int PaymentTypeID, int AddressToMakeInvoiceOutToId,
												  DateTime? DateRecievedMoney, HttpResponse HttpServerResponse)
		{
			ControllerLogicReturnValue returnValue;
			InvoiceModels.Invoice InvoiceInputs;

			this._HttpResponse = HttpServerResponse;

			// check the supplied inputs for errors, if no errors, convert to an Invoice object
			returnValue = CheckInputParametersAndConvertToInvoiceObject(Id, DateOfInvoice, ReferenceNumber, Description,
																		PaymentTypeID, AddressToMakeInvoiceOutToId, DateRecievedMoney);
			// if there were any error in the supplied inputs
			if (returnValue.HasErrors)
			{
				// bad client request
				this._HttpResponse.StatusCode = 400;
				return returnValue;
			}

			// Get the input parameters as an Invoice object
			InvoiceInputs = (InvoiceModels.Invoice)returnValue.ReturnValue;

			// using the supplied id, check if the invoice exists in the database and return it if it exists
			returnValue = FindInvoiceInDatabase(Id);
			// if we could not find the invoice in the datbase
			if (returnValue.HasErrors)
			{
				// bad client request
				this._HttpResponse.StatusCode = 400;
				return returnValue;
			}

			
			// attempt to update the database with the new updates
			returnValue = UpdateInvoiceDetails(InvoiceInputs, (InvoiceModels.Invoice)returnValue.ReturnValue);
			
				
			return returnValue;
		}

		/// <summary>
		/// Checks the passed in parameters to see if they are valid arguments and converts them to an <see cref="InvoiceModels.Invoice"/> object
		/// </summary>
		/// <param name="InvoiceId"></param>
		/// <param name="dateOfInvoice"></param>
		/// <param name="referenceNumber"></param>
		/// <param name="description"></param>
		/// <param name="paymentTypeID"></param>
		/// <param name="addressToMakeInvoiceOutToId"></param>
		/// <param name="dateRecievedMoney"></param>
		/// <returns><see cref="InvoiceModels.Invoice"/> if sucsefulll, else error data</returns>
		private ControllerLogicReturnValue CheckInputParametersAndConvertToInvoiceObject(int InvoiceId, DateTime dateOfInvoice, string referenceNumber, string description, int paymentTypeID, int addressToMakeInvoiceOutToId, DateTime? dateRecievedMoney)
		{
			// the return value that will contain errors or an instance of Invoice
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
			// keeps track of if errors were found
			bool WereErrorsFound = false;
			// will hold all the input parameters
			InvoiceModels.Invoice InvoiceInputs = new InvoiceModels.Invoice();
			
			
			// used for connecting to the dtabase.
			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				

				// if we have an valid InvoiceId
				if (InvoiceId > 0)
					InvoiceInputs.Id = InvoiceId;
				// We have an Invalid invoice Id
				else
				{
					WereErrorsFound = true;
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Invalid Invoice Id");
				}

				// can't think of anything to check on DateOfInvoice
				InvoiceInputs.DateOfInvoice = dateOfInvoice;

				// if we have a valid reference number (not checked to see if it allready exists in database)
				string refNumber = referenceNumber.Trim();
				if (refNumber.Length > 0)
				{
					// gets invoice data from the database.
					InvoiceDb invoiceDb = new InvoiceDb(dbContext);

					// check if ref number allready exists in database. can't have 2 of the save reference numbers existing
					var FoundInvoices = invoiceDb.Select_CustomQuery(null,null,null,null,null,null, refNumber, null,null,null);
					
					// if we have found some invoices, it means the reference number we want to use
					// has allready been taken
					if(FoundInvoices.Any() && FoundInvoices[0].Id != InvoiceId) 
					{
						WereErrorsFound = true;
						returnValue.HasErrors = true;
						returnValue.Errors.Add("Reference number allready in use by another invoice");
					}
					// reference number is ok to use
					else
						InvoiceInputs.ReferenceNumber = refNumber;
				}
				else
				{
					WereErrorsFound = true;
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Invalid Reference Number");
				}


				string desc = description.Trim();
				// if we have a description
				if (desc.Length > 0)
					InvoiceInputs.Description = desc;
				// if we don't have a description
				else
				{
					WereErrorsFound = true;
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Invalid Invoice Description");
				}

				// we dont' have to have a payment type, but if we do, set it
				// (Note we are not checking at this stage if the payment type id exists in the datbaase)
				if (paymentTypeID > 0)
				{
					// check the payment id is valid (does it exist in the database)
					PaymentTypeDb paymentTypeDb = new PaymentTypeDb(dbContext);
					if(paymentTypeDb.Select(paymentTypeID) == null)
					{
						WereErrorsFound = true;
						returnValue.HasErrors = true;
						returnValue.Errors.Add("Payment Type not found");
					}
					// payment type id is ok
					else
						InvoiceInputs.PaymentTypeID = paymentTypeID;


				}


				// we don't have to have an address to make payment out to, but if we do, set it
				// (Note we are not checking at this stage if the address exists in the database)
				if (addressToMakeInvoiceOutToId > 0)
				{
					// check the address exists in the database
					CompanyAddressDb companyAddressDb = new CompanyAddressDb(dbContext);
					if(companyAddressDb.Select(addressToMakeInvoiceOutToId) == null)
					{
						WereErrorsFound = true;
						returnValue.HasErrors = true;
						returnValue.Errors.Add("Address not found");
					}
					// Address is found in the database so we can use it
					else
						InvoiceInputs.AddressToMakeInvoiceOutToId = addressToMakeInvoiceOutToId;
				}

				// we don't have to have a date recieved money, but if we do, set it
				if (dateRecievedMoney != null)
					InvoiceInputs.DateRecievedMoney = dateRecievedMoney;






				// if there were no errors, we can set the returnValue to the Invoice object we have created
				// and inishalized with the input parameters
				if (WereErrorsFound == false)
					returnValue.ReturnValue = InvoiceInputs;

				
			}// end if db context


			// return the invoice object or a list of errors
			return returnValue;
		}

		/// <summary>
		/// Trys to return the invoice from the database using the passed in id
		/// </summary>
		/// <param name="InvoiceId">Id of the invoice we are looking for in the database</param>
		/// <returns>an instance of invoice or error messages</returns>
		private ControllerLogicReturnValue FindInvoiceInDatabase(int InvoiceId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
			InvoiceModels.Invoice? FoundInvoice;
			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				InvoiceDb invoiceDb = new InvoiceDb(dbContext);

				// try and find the invoice in the database
				FoundInvoice = invoiceDb.SelectInvoice(InvoiceId);
			}

			// if we could not find the invoice in the database
			if(FoundInvoice == null)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Unable to find invoice in datbase");
			}
			// we found the invoice in the database
			else
				returnValue.ReturnValue = FoundInvoice;

			// return the invoice, or error information
			return returnValue;
		}

		/// <summary>
		/// Attempts to update an Invoice Row in the database with the new passed in inputs
		/// </summary>
		/// <param name="invoiceInputs">The updated Invoice data we want applied to the database</param>
		/// <param name="InvoiceFromDatabase">The invoice data from the database we want to apply updates to</param>
		/// <returns>An invoice object with the updated values or errors if anything goes wrong</returns>
		private ControllerLogicReturnValue UpdateInvoiceDetails(InvoiceModels.Invoice invoiceInputs, InvoiceModels.Invoice InvoiceFromDatabase)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			// copy the values that were supplied by the client into the invoice object we got from the database
			InvoiceFromDatabase.DateOfInvoice = invoiceInputs.DateOfInvoice;
			InvoiceFromDatabase.Description = invoiceInputs.Description;
			InvoiceFromDatabase.ReferenceNumber = invoiceInputs.ReferenceNumber;
			InvoiceFromDatabase.PaymentTypeID = invoiceInputs.PaymentTypeID;
			InvoiceFromDatabase.AddressToMakeInvoiceOutToId = invoiceInputs.AddressToMakeInvoiceOutToId;
			InvoiceFromDatabase.DateRecievedMoney = invoiceInputs.DateRecievedMoney;

			// update the invoice with the new values
			using (InvoiceAssistantDbContext dbContext= new InvoiceAssistantDbContext()) 
			{
				InvoiceDb invoiceDb = new InvoiceDb(dbContext);
				invoiceDb.Update(InvoiceFromDatabase);

				int AffectedRows = dbContext.SaveChanges();

				if(AffectedRows > 0)
				{
					returnValue.ReturnValue = InvoiceFromDatabase;
				}
				else
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to apply updates to Invoice from in database");
				}
			}

			return returnValue;
		}
	}
}
