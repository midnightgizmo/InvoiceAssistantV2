using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAssistantV2.Server.ControllersLogic.Invoice
{
	public class InsertNewInvoiceControllerLogic
	{
		public ControllerLogicReturnValue Process(DateTime DateOfInvoice, string ReferenceNumber,
												  string Description, int AddressToMakeInvoiceOutToId,
												  PlacesVisitedForInvoiceBase[]? PlacesVisited)
		{
			ControllerLogicReturnValue returnValue;

			returnValue = this.CheckInputForErrors(DateOfInvoice, ReferenceNumber, Description,
													AddressToMakeInvoiceOutToId, PlacesVisited);
			if (returnValue.HasErrors == true)
				return returnValue;

			returnValue = this.CreateNewInvoice(DateOfInvoice, ReferenceNumber, Description,
													AddressToMakeInvoiceOutToId, PlacesVisited);

			return returnValue;
		}



		private ControllerLogicReturnValue CheckInputForErrors(DateTime dateOfInvoice, string referenceNumber, string description, int addressToMakeInvoiceOutToId, PlacesVisitedForInvoiceBase[]? PlacesVisited)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			referenceNumber = referenceNumber == null ? string.Empty : referenceNumber.Trim();
			if (referenceNumber.Length == 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("referenceNumber must contain data");
			}

			description = description == null ? string.Empty : description.Trim();
			if (description.Length == 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("description must contain data");
			}

			if(addressToMakeInvoiceOutToId < 1)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("addressToMakeInvoiceOutToId is invalid");
			}

			if(PlacesVisited != null)
			{
				foreach(PlacesVisitedForInvoiceBase placeVisited in PlacesVisited) 
				{
					if(placeVisited.NumberOfTimesVisited < 1 || placeVisited.CompanyAddressId < 1)
					{
						returnValue.HasErrors = true;
						returnValue.Errors.Add("PlacesVisited contains an invalid input");
						break;
					}
				}
			}
			
			return returnValue;
		}



		private ControllerLogicReturnValue CreateNewInvoice(DateTime dateOfInvoice, string referenceNumber, string description, int addressToMakeInvoiceOutToId, PlacesVisitedForInvoiceBase[]? PlacesVisited)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
			
			// create a new instance of the invoice class and all the input parameters too it
			InvoiceAssistantV2.Shared.Models.Database.Invoice.Invoice newInvoice = new Shared.Models.Database.Invoice.Invoice()
			{
				DateOfInvoice = dateOfInvoice,
				ReferenceNumber = referenceNumber,
				Description = description,
				AddressToMakeInvoiceOutToId= addressToMakeInvoiceOutToId
			};

			using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				InvoiceDb dbInvoice = new InvoiceDb(dbContext);

				dbInvoice.Insert(newInvoice);
				dbContext.SaveChanges();

				// if the new invoice was not added to the database
				if(newInvoice.Id < 1)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Unable to create invoice");
					return returnValue;
				}

				// see if there are any places visited for this invoice
				// this will allow working out of milage when running reports
				if (PlacesVisited != null)
				{
					CompanyDb dbCompany = new CompanyDb(dbContext);
					CompanyAddressDb dbCompanyAddress = new CompanyAddressDb(dbContext);

					// find the company that is linked to this invoice. We will need this when adding places visited in invoice
					CompanyDetails? CompanyLinkedToInvoice = dbCompany.Select_ByCompanyAddressId(newInvoice.AddressToMakeInvoiceOutToId.Value);

					// if we found the company details, we can add the places visisted to the invoice
					if (CompanyLinkedToInvoice != null)
					{
						newInvoice.PlacesVisitedForInvoice = new List<PlacesVisitedForInvoice>();

						// go through each place visited and add it to the invoice
						foreach (PlacesVisitedForInvoiceBase placeVisited in PlacesVisited)
						{

							// make sure the place visited belongs the the company
							// if it does not, do not add it to the invoice
							if (dbCompanyAddress.DoesCompanyAddressBelongToCompany(placeVisited.CompanyAddressId, CompanyLinkedToInvoice.Id) == false)
								continue;


							newInvoice.PlacesVisitedForInvoice.Add(new PlacesVisitedForInvoice()
							{
								NumberOfTimesVisited = placeVisited.NumberOfTimesVisited,
								InvoiceId = newInvoice.Id,
								CompanyAddressId = placeVisited.CompanyAddressId
							});

							// commit the changes to the database
							dbContext.SaveChanges();
						}
					}
				}

			}

			// set the newly created invoice as the return value
			returnValue.ReturnValue = newInvoice;

			return returnValue;

		}





	}
}
