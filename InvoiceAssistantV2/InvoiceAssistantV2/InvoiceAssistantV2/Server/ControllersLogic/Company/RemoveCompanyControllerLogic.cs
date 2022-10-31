using Database.Data;
using Database.DbInteractions;

namespace InvoiceAssistantV2.Server.ControllersLogic.Company
{
	public class RemoveCompanyControllerLogic
	{
        public ControllerLogicReturnValue Process(int CompanyId)
        {
            ControllerLogicReturnValue DataToReturn = new ControllerLogicReturnValue();

            using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
            {

                // check to see if any invoices are linked to the passed in CompanyId.
                // We don't want to delete the compnay if it exists in any invoices, instead
                // we will just hide the compnay from the user.
                if (this.IsCompanyBeingUsed(CompanyId, dbContext) == true)
                {// one or more companys are linked to the company

                    // hide the company and any assoshiated addresses linked to it
                    DataToReturn.ReturnValue = this.HideCompany(CompanyId, dbContext);
                }
                else
                {// no invoices are linked to the company

                    // delete the company from the database and any asoshated addresses linked to it
                    DataToReturn.ReturnValue = this.DeleteCompany(CompanyId, dbContext);
                        
                }

            }

            return DataToReturn;
        }

        private bool IsCompanyBeingUsed(int CompanyId, InvoiceAssistantDbContext dbContext)
        {
            //var company=dbContext.CompanyDetails.Where(c => c.Id == CompanyId).FirstOrDefault();

            CompanyDb companyDb = new CompanyDb(dbContext);
            return companyDb.IsCompanyAddressLinkedToAnyInvoices(CompanyId);

            //try
            //{
            //    var result = (from i in dbContext.Invoices
            //                 join a in dbContext.CompanyAddress
            //                 on i.AddressToMakeInvoiceOutToId equals a.Id
            //                 join c in dbContext.CompanyDetails
            //                 on a.CompanyDetailsID equals c.Id
            //                 where c.Id == CompanyId
            //                  select i).FirstOrDefault();
            //}
            //catch(Exception e)
            //{
            //    int i = 0;
            //}

            //return true;
        }

        /// <summary>
        /// Hides the CompanyDetails and any assoshiated Addresses from the user
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="dbContext"></param>
        /// <returns>true if sucsefull, else false</returns>
        private bool HideCompany(int CompanyId, InvoiceAssistantDbContext dbContext)
        {
            CompanyAddressDb companyAddressDb = new CompanyAddressDb(dbContext);
            companyAddressDb.HideAllAddressesLinkedToCompany(CompanyId);

            CompanyDb companyDetailsDb = new CompanyDb(dbContext);
            return companyDetailsDb.HideCompany(CompanyId);
           
        }

        /// <summary>
        /// Deletes the Company and any assoshiated company addresses from the database
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="dbContext"></param>
        private bool DeleteCompany(int CompanyId, InvoiceAssistantDbContext dbContext)
        {
            CompanyDb companyDb = new CompanyDb(dbContext);
            return companyDb.DeleteCompany(CompanyId);
        }
    }
}
