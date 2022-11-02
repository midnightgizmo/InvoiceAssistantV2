using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.Company;

namespace InvoiceAssistantV2.Server.ControllersLogic.Company
{
	public class FindCompanyControllerLogic
	{
		public ControllerLogicReturnValue Process(int CompanyId)
		{
			ControllerLogicReturnValue returnValue;

			// check the input to make sure it is a valid input
			returnValue = this.CheckForInputErrors(CompanyId);
			// if input is invalid
			if (returnValue.HasErrors)
				return returnValue;

			// try and find the Company Details from the passed in id
			returnValue = this.FindCompanyDetails(CompanyId);

			return returnValue;

        }

		/// <summary>
		/// Checks the input value to see if it is a valid value
		/// </summary>
		/// <param name="CompanyId">The value to check</param>
		/// <returns>HasErrors set to true if errors are found</returns>
		public ControllerLogicReturnValue CheckForInputErrors(int CompanyId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			if (CompanyId < 1)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid CompanyID. Company Id must be greater than zero");
			}

			return returnValue;
		}

		/// <summary>
		/// Finds the <see cref="CompanyDetails"/> using the passed in id
		/// </summary>
		/// <param name="companyId">The Id to use to find the Company Details in the database</param>
		/// <returns>returns the found company details or sets HasErrors to true if not found</returns>
		/// <exception cref="NotImplementedException"></exception>
        private ControllerLogicReturnValue FindCompanyDetails(int companyId)
        {
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
			// look in the database for the CompanyDetails using the companyID value
			InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext();
			CompanyDb companyDb = new CompanyDb(dbContext);
			CompanyDetails? companyDetails = companyDb.Select(companyId);

			if (companyDetails != null)
				returnValue.ReturnValue = companyDetails;
			else
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Company Details Not Found");
			}

			return returnValue;
        }

    }
}
