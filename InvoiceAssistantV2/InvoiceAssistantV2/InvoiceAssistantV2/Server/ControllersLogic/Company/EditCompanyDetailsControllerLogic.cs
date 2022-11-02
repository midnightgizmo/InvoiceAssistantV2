using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.Company;

namespace InvoiceAssistantV2.Server.ControllersLogic.Company
{
	public class EditCompanyDetailsControllerLogic
	{

		public ControllerLogicReturnValue Process(CompanyDetails companyDetails)
		{
			ControllerLogicReturnValue returnValue;

			// check the input we have been sent for errors
			returnValue = this.CheckForErrorsOnInput(companyDetails);
			// if there are errors, return from this method passing the errors we found
			if(returnValue.HasErrors == true)
				return returnValue;

			// updated the database with the passed in company details
			returnValue = this.UpdateCompanyDetails(companyDetails);

			return returnValue;

        }


		/// <summary>
		/// Checks the passed in input to make sure there are no errors.
		/// </summary>
		/// <param name="companyDetails"></param>
		/// <returns>HasErorrs set to true if errors, else faslse</returns>
		private ControllerLogicReturnValue CheckForErrorsOnInput(CompanyDetails companyDetails)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			if(companyDetails == null)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid Input");
				// no point doing any other checks as there is nothing hear to check
				return returnValue;
			}

			if(companyDetails.CompanyName == null)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Missing CompanyName");
                return returnValue;
            }
			else // we have a company name, lets remove any white spacing around it
				companyDetails.CompanyName = companyDetails.CompanyName.Trim();

            if (companyDetails.FriendlyName == null)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Missing FriendlyName");
                return returnValue;
            }
			else // we have a friendly name, lets remove any white spaceing around it
				companyDetails.FriendlyName = companyDetails.FriendlyName.Trim();

			// check company name has some data
			if(companyDetails.CompanyName.Length == 0)
			{
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Missing CompanyName");
                return returnValue;
            }

            // check if friendly name has some data
            if (companyDetails.FriendlyName.Length == 0)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Missing FriendlyName");
                return returnValue;
            }

			// return any errors with HasErrors set to true or if no erors
			// HasErorrs set to false
            return returnValue;
		}

		/// <summary>
		/// Update the database with the passed in details
		/// </summary>
		/// <param name="companyDetails"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
        private ControllerLogicReturnValue UpdateCompanyDetails(CompanyDetails companyDetails)
        {
            ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
			
			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				CompanyDb companyDb = new CompanyDb(dbContext);
				// update the company details in the database
				CompanyDetails? company = companyDb.EditCompanyDetails(companyDetails);

				// check if the details were updated in the databse
				if(company == null)
				{// details were not updated
					returnValue.HasErrors = true;
					returnValue.Errors.Add("unable to update");
				}
				else
				{// details were updated
					returnValue.ReturnValue = company;
				}
			}

			// return the updated company details, or any errors that occured
			return returnValue;
        }

    }
}
