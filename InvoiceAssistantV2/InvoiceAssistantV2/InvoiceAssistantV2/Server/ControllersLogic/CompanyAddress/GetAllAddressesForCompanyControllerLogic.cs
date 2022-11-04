using Database.Data;
using Database.DbInteractions;

namespace InvoiceAssistantV2.Server.ControllersLogic.CompanyAddress
{
	public class GetAllAddressesForCompanyControllerLogic
	{
		public ControllerLogicReturnValue Process(int CompanyId)
		{
			ControllerLogicReturnValue returnValue;

			returnValue = this.FindAllAddressesForCompany(CompanyId);

			return returnValue;

        }

		/// <summary>
		/// Find all addresses assoshiated with the companyId passed in
		/// </summary>
		/// <param name="companyId"></param>
		/// <returns></returns>
		private ControllerLogicReturnValue FindAllAddressesForCompany(int companyId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				CompanyAddressDb addressessDb = new CompanyAddressDb(dbContext);

				returnValue.ReturnValue = addressessDb.SelectAllAddressForCompany(companyId);
			}

			return returnValue;
		}
	}
}
