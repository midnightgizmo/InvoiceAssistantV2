using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Server.ControllersLogic.User.Payment
{
	public class UpdatePaymentMethodNameControllerLogic
	{
		
		private HttpResponse _HttpResponse = null!;

        public UpdatePaymentMethodNameControllerLogic(HttpResponse httpResponse)
        {
            this._HttpResponse = httpResponse;
        }
        public ControllerLogicReturnValue Process(int PaymentMethodId,string PaymentMethodName)
		{
			ControllerLogicReturnValue returnValue;


			returnValue = this.UpdatePaymentMethodName(PaymentMethodId, PaymentMethodName);

			return returnValue;
		}

		private ControllerLogicReturnValue UpdatePaymentMethodName(int paymentMethodId, string paymentMethodName)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
			string newPaymentMethodName = paymentMethodName.Trim();

			// if we don't have a valid payment id
			if(paymentMethodId < 1) 
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid paymentMethodId");
			}
			// if we don't have a value for payment method name
			if(newPaymentMethodName.Length < 1)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("paymentMethodName is missing a value");
			}

			// if we have errors
			if(returnValue.HasErrors == true)
			{

				this._HttpResponse.StatusCode = 400;
				return returnValue;
			}

			// if we get this far, the input parameters are good enough to try and find the payment method

			return this.UpdateNameInDatabase(paymentMethodId, newPaymentMethodName);
			
		}

		private ControllerLogicReturnValue UpdateNameInDatabase(int paymentMethodId, string newPaymentMethodName)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				PaymentMethodDb paymentMethodDb = new PaymentMethodDb(dbContext);

				// try and find the payment method row in the database
				PaymentMethod? paymentMethod = paymentMethodDb.SelectPaymentMethod(paymentMethodId);

				// if we could not find the row in the database
				if (paymentMethod == null)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("paymentMethodId not found");

					this._HttpResponse.StatusCode = 400;
				}
				else
				{

					// if we get this far, we have found the row in the database

					// update the name in the database
					paymentMethod.Name = newPaymentMethodName;
					bool WasSucsefull = paymentMethodDb.UpdatePaymentMethod(paymentMethod);
					if(WasSucsefull == true)
						// set to true to say we have updated the payment method name
						returnValue.ReturnValue = true;
					else
					{
						returnValue.HasErrors = true;
						returnValue.Errors.Add("PaymentMethod: Unable to update row");
						this._HttpResponse.StatusCode = 400;
					}
				}
			}

			return returnValue;
		}
	}
}
