using Database.Data;
using Database.DbInteractions;

namespace InvoiceAssistantV2.Server.ControllersLogic.Company
{
    public class ListOfCompanysControllerLogic
    {
        public ControllerLogicReturnValue Process()
        {
            ControllerLogicReturnValue DataToReturn = new ControllerLogicReturnValue();

            InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext();
            CompanyDb paymentTypeDb = new CompanyDb(dbContext);

            DataToReturn.ReturnValue = paymentTypeDb.SelectAllCompanies();

            return DataToReturn;
        }
    }
}
