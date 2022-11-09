using Database.Data;
using Database.DbInteractions;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAssistantV2.Server.ControllersLogic.CompanyAddress
{
	public class InsertCompanyAddressControllerLogic
	{
        public ControllerLogicReturnValue Process(Shared.Models.Database.Company.CompanyAddress companyAddress)
        {
            ControllerLogicReturnValue returnValue;

            // check the input we have been sent for errors
            returnValue = this.CheckForErrorsOnInput(companyAddress);
            // if input is invalid
            if (returnValue.HasErrors)
                return returnValue;

            // add the new company address to the database, and return the new company address
            returnValue = this.CreateNewCompanyAddress(companyAddress);

            return returnValue;

        }



        private ControllerLogicReturnValue CheckForErrorsOnInput(Shared.Models.Database.Company.CompanyAddress companyAddressDetails)
        {
            ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

            if (companyAddressDetails == null)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Company address details not found");
                return returnValue;
            }

            // if we don't have the company details id, we can't do the update
            if (companyAddressDetails.CompanyDetailsID < 1)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Company address details: CompanyDetailsID not found");
                return returnValue;
            }

            // if we have not recieved a Friendly Name
            if (companyAddressDetails.FriendlyName == null)
            {
                returnValue.HasErrors = true;
                returnValue.Errors.Add("Company address details: FriendlyName not found");
                return returnValue;
            }
            // remove any whitespace 
            companyAddressDetails.FriendlyName = companyAddressDetails.FriendlyName.Trim();
            if (companyAddressDetails.FriendlyName.Length == 0)
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


        private ControllerLogicReturnValue CreateNewCompanyAddress(Shared.Models.Database.Company.CompanyAddress companyAddress)
        {
            ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

            using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
            {
                CompanyAddressDb companyAddressDb = new CompanyAddressDb(dbContext);

                Shared.Models.Database.Company.CompanyAddress? newCompanyAddress;
                newCompanyAddress = companyAddressDb.AddNewCompanyAddress(companyAddress);

                if (newCompanyAddress == null)
                {
                    returnValue.HasErrors = true;
                    returnValue.Errors.Add("Unable to create new compnay Address");
                }
                else
                    returnValue.ReturnValue = newCompanyAddress;
            }

            return returnValue;
        }


    }
}
