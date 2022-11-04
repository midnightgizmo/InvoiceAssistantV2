using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.Company;

namespace InvoiceAssistantV2.Server.ControllersLogic.CompanyAddress
{
	public class FindCompanyAddressControllerLogic
	{
        public ControllerLogicReturnValue Process(int CompanyAddressId)
        {
            ControllerLogicReturnValue returnValue;

            // check the input to make sure it is a valid input
            returnValue = this.CheckForInputErrors(CompanyAddressId);
            // if input is invalid
            if (returnValue.HasErrors)
                return returnValue;

            // try and find the Company Address Details from the passed in id
            returnValue = this.FindCompanyDetails(CompanyAddressId);

            return returnValue;

        }

        /// <summary>
		/// Checks the input value to see if it is a valid value
		/// </summary>
		/// <param name="CompanyId">The value to check</param>
		/// <returns>HasErrors set to true if errors are found</returns>
		public ControllerLogicReturnValue CheckForInputErrors(int CompanyAddressId)
        {
            ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

            if (CompanyAddressId < 1)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Invalid CompanyAddressId. Company Address Id must be greater than zero");
            }

            return returnValue;
        }


        /// <summary>
		/// Finds the <see cref="CompanyDetails"/> using the passed in id
		/// </summary>
		/// <param name="CompanyAddressId">The Id to use to find the Company Address Details in the database</param>
		/// <returns>returns the found company address details or sets HasErrors to true if not found</returns>
        private ControllerLogicReturnValue FindCompanyDetails(int CompanyAddressId)
        {
            ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
            // look in the database for the Company Address using the Company Address Id value
            using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
            {
                CompanyAddressDb companyAddressDb = new CompanyAddressDb(dbContext);
                InvoiceAssistantV2.Shared.Models.Database.Company.CompanyAddress? companyAddressDetails = companyAddressDb.Select(CompanyAddressId);

                if (companyAddressDetails != null)
                    returnValue.ReturnValue = companyAddressDetails;
                else
                {
                    returnValue.HasErrors = true;
                    returnValue.Errors.Add("Company Address Details Not Found");
                }
            }
            return returnValue;
        }


    }
}
