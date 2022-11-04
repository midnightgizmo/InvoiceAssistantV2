using Database.Data;
using Database.DbInteractions;

namespace InvoiceAssistantV2.Server.ControllersLogic.CompanyAddress
{
	public class RemoveCompanyAddressControllerLogic
	{

		public ControllerLogicReturnValue Process(int CompanyAddressId)
		{
			ControllerLogicReturnValue DataToReturn = new ControllerLogicReturnValue();


			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
                // check if the company address is linked to any invoices.
                // We don't want to delete the company address if it is linked to any invoices, instead
                // we will just hide the compnay from the user.
                if (this.IsCompanyAddressBeingUsed(CompanyAddressId, dbContext) == true)
                {// one or more invoices are linked to the company address

                    // hide the company address
                    DataToReturn.ReturnValue = this.HideCompanyAddress(CompanyAddressId, dbContext);
                }
                else
                {// no invoices are linked to the company address

                    // delete the company address from the database
                    DataToReturn.ReturnValue = this.DeleteCompanyAddress(CompanyAddressId, dbContext);

                }
            }

                return DataToReturn;

        }

        /// <summary>
        /// Checks if the Company Address is linked to one or more invoices
        /// </summary>
        /// <param name="companyAddressId"></param>
        /// <param name="dbContext"></param>
        /// <returns>true if linekd to one or more invoice, else false</returns>
        private bool IsCompanyAddressBeingUsed(int companyAddressId, InvoiceAssistantDbContext dbContext)
        {
            CompanyAddressDb companyAddressDb = new CompanyAddressDb(dbContext);
            return companyAddressDb.IsCompanyAddressLinkedToAnyInvoices(companyAddressId);
        }

        /// <summary>
        /// Hides the company address so the user can no longer see it
        /// </summary>
        /// <param name="companyAddressId"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        private bool HideCompanyAddress(int companyAddressId, InvoiceAssistantDbContext dbContext)
        {
            CompanyAddressDb companyAddressDb = new CompanyAddressDb(dbContext);
            return companyAddressDb.HideCompanyAddress(companyAddressId);
        }

        private bool DeleteCompanyAddress(int companyAddressId, InvoiceAssistantDbContext dbContext)
        {
            CompanyAddressDb companyAddressDb = new CompanyAddressDb(dbContext);
            return companyAddressDb.DeleteCompanyAddress(companyAddressId);
        }
    }
}
