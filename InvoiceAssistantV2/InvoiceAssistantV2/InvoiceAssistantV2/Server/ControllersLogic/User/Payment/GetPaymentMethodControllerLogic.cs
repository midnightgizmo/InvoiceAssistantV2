using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Server.ControllersLogic.User.Payment
{
	public class GetPaymentMethodControllerLogic
	{
		private HttpResponse _HttpResponse = null!;

		

		public GetPaymentMethodControllerLogic(HttpResponse httpResponse)
		{
			this._HttpResponse = httpResponse;
		}

		public ControllerLogicReturnValue Process(int PaymentMethodId)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			// get the payment method model using the supplied PaymentMethodId
			using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				PaymentMethodDb dbPaymentMethod = new PaymentMethodDb(dbContext);
				PaymentMethod? model;
				// go off to the database and find the payment method
				model = dbPaymentMethod.SelectPaymentMethod(PaymentMethodId);

				// could not find payment method in database
				if (model == null)
				{
					// indicate it model was not found
					this._HttpResponse.StatusCode = 404;
					returnValue.HasErrors = true;
					returnValue.Errors.Add("PaymentMethod could not be found");
				}
				// we found the payment method
				else
					returnValue.ReturnValue = model;
			}

			return returnValue;

		}
	}
}
