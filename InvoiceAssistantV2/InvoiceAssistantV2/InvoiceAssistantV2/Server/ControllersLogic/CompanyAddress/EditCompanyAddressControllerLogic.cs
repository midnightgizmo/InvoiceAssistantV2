
using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.Company;

namespace InvoiceAssistantV2.Server.ControllersLogic.CompanyAddress
{
    public class EditCompanyAddressControllerLogic
    {
        public ControllerLogicReturnValue Process(InvoiceAssistantV2.Shared.Models.Database.Company.CompanyAddress companyAddressDetails)
        {
            ControllerLogicReturnValue returnValue;

            // check the input we have been sent for errors
            returnValue = this.CheckForErrorsOnInput(companyAddressDetails);
            // if there are errors, return from this method passing the errors we found
            if (returnValue.HasErrors == true)
                return returnValue;

            // updated the database with the passed in company details
            returnValue = this.UpdateCompanyDetails(companyAddressDetails);

            return returnValue;

        }

        
        /// <summary>
        /// Checks the input to make sure all required fieilds are there.
        /// Removes any white space at begining and end of strings.
        /// </summary>
        /// <param name="companyAddressDetails"></param>
        /// <returns>HasErrors = true if errors are found</returns>
        private ControllerLogicReturnValue CheckForErrorsOnInput(Shared.Models.Database.Company.CompanyAddress companyAddressDetails)
        {
            ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

            if(companyAddressDetails == null)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Company address details not found");
                return returnValue;
            }

            // if we don't have the id, then we can't update the details
            if(companyAddressDetails.Id < 1)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Company address details: Id not found");
                return returnValue;
            }

            // if we don't have the company details id, we can't do the update
            if(companyAddressDetails.CompanyDetailsID < 1)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Company address details: Id not found");
                return returnValue;
            }

            // if we have not recieved a Friendly Name
            if(companyAddressDetails.FriendlyName == null)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Company address details: FriendlyName not found");
                return returnValue;
            }
            // remove any whitespace 
            companyAddressDetails.FriendlyName = companyAddressDetails.FriendlyName.Trim();
            if(companyAddressDetails.FriendlyName.Length == 0)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Company address details: FriendlyName not found");
                return returnValue;
            }

            // remove any white space at begining and end of address. if address is null, return null
            companyAddressDetails.AddressLine1 = companyAddressDetails.AddressLine1?.Trim();
            companyAddressDetails.AddressLine2 = companyAddressDetails.AddressLine2?.Trim();
            companyAddressDetails.AddressLine3 = companyAddressDetails.AddressLine3?.Trim();
            companyAddressDetails.AddressLine4 = companyAddressDetails.AddressLine4?.Trim();
            companyAddressDetails.AddressLine5 = companyAddressDetails.AddressLine5?.Trim();
            companyAddressDetails.PostCode = companyAddressDetails.PostCode?.Trim();
            
            
            return returnValue;
        }


        /// <summary>
        /// Updateds the database with the passed in Address details
        /// </summary>
        /// <param name="companyAddressDetails"></param>
        /// <returns></returns>
        private ControllerLogicReturnValue UpdateCompanyDetails(Shared.Models.Database.Company.CompanyAddress companyAddressDetails)
        {
            ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

            using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
            {
                CompanyAddressDb companyAddressDb = new CompanyAddressDb(dbContext);

                Shared.Models.Database.Company.CompanyAddress? updatedAddress = companyAddressDb.EditCompanyAddressDetails(companyAddressDetails);

                if(updatedAddress == null)
                {
                    returnValue.HasErrors = true;
                    returnValue.Errors.Add("Unable to updated Address Details");
                }
            }

            return returnValue;


        }
    }
}
