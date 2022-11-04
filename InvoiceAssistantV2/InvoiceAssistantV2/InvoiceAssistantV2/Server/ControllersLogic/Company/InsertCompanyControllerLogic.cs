using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.Company;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAssistantV2.Server.ControllersLogic.Company
{
	public class InsertCompanyControllerLogic
	{
        public ControllerLogicReturnValue Process(string FriendlyName, string CompanyName)
        {
            ControllerLogicReturnValue returnValue;

            // check the input to make sure it is a valid input
            returnValue = this.CheckForInputErrors(FriendlyName, CompanyName);
            // if input is invalid
            if (returnValue.HasErrors)
                return returnValue;

            // add the new company to the datbase, and return the new company deatils
            returnValue = this.CreateNewCompany(FriendlyName, CompanyName);

            return returnValue;

        }



        /// <summary>
        /// Checks passed in values to make sure they have a value set against them
        /// </summary>
        /// <param name="friendlyName"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        private ControllerLogicReturnValue CheckForInputErrors(string friendlyName, string companyName)
        {
            ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

            if(friendlyName == null || friendlyName.Trim().Length == 0)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("FriendlyName required");
            }
            if(companyName == null || companyName.Trim().Length == 0)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("CompanyName required");
            }

            return returnValue;
        }


        /// <summary>
        /// Adds a new company to the database based on the passed in values.
        /// </summary>
        /// <param name="friendlyName"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        private ControllerLogicReturnValue CreateNewCompany(string friendlyName, string companyName)
        {
            ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

            using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
            {
                CompanyDb companyDb = new CompanyDb(dbContext);

                CompanyDetails? newCompany = companyDb.AddNewCompany(friendlyName, companyName);
                // check to make sure the new company was created in the database
                if (newCompany == null)
                {// somthing went wrong, company not added to database
                    returnValue.HasErrors = true;
                    returnValue.Errors.Add("Unable to add new company");
                }
                else// all went ok, and company was added to the database
                    returnValue.ReturnValue = newCompany;
            }

            return returnValue;
        }
    }
}
