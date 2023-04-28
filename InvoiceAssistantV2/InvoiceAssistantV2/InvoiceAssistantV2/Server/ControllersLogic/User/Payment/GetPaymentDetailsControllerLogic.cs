using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Server.ControllersLogic.User.Payment
{
	public class GetPaymentDetailsControllerLogic
	{
		private HttpResponse _HttpResponse = null!;


		public GetPaymentDetailsControllerLogic(HttpResponse httpResponse)
		{
			this._HttpResponse = httpResponse;
		}

		public ControllerLogicReturnValue Process(int paymentMethodInfoId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			// get all payment details assoshiated with the passed in Paymemnt Method
			using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				List<PaymetDetail> ListOfPaymentDetails;
				PaymentDetailsDb dbPaymentDetails = new PaymentDetailsDb(dbContext);

				ListOfPaymentDetails = dbPaymentDetails.SelectAllPaymentDetailsRelatingToPaymentMethod(paymentMethodInfoId);

				returnValue.ReturnValue = ListOfPaymentDetails;
				return returnValue;
			}
		}
	}
}
