using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.User;

namespace InvoiceAssistantV2.Server.ControllersLogic.User.Payment
{
	public class AddPaymentDetailControllerLogic
	{
		private HttpResponse _HttpResponse = null!;

		public AddPaymentDetailControllerLogic(HttpResponse httpResponse)
		{
			this._HttpResponse = httpResponse;
		}

		public ControllerLogicReturnValue Process(int PaymentMethodId, string PaymentDetailKey, string? PaymentDetailsValue)
		{
			ControllerLogicReturnValue returnValue;

			returnValue = this.CheckInputsAndAddPaymentDetail(PaymentMethodId, PaymentDetailKey, PaymentDetailsValue);


			return returnValue;
		}

		private ControllerLogicReturnValue CheckInputsAndAddPaymentDetail(int paymentMethodId, string paymentDetailKey, string? paymentDetailsValue)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();
			string newPaymentDetailKey = paymentDetailKey.Trim();
			string newPaymentDetailValue = paymentDetailsValue == null ? string.Empty : paymentDetailsValue.Trim();

			// if we don't have a valid paymentMethodId
			if (paymentMethodId < 1)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid paymentMethodId");
			}

			// if we don't have a valid paymentDetailKey
			if(newPaymentDetailKey.Length == 0)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("paymentDetailKey is missing a value");
			}

			// if we have errors
			if (returnValue.HasErrors == true)
			{

				this._HttpResponse.StatusCode = 400;
				return returnValue;
			}

			// if we get this far, the input parameters are good enough to try and add the new payment detail

			return this.AddPaymentDetailToDatabase(paymentMethodId, newPaymentDetailKey, newPaymentDetailValue);
		}

		private ControllerLogicReturnValue AddPaymentDetailToDatabase(int paymentMethodId, string newPaymentDetailKey, string newPaymentDetailValue)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using (InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				PaymetDetail newPaymentDetail;
				PaymentMethodDb paymentMethodDb;
				PaymentMethod? foundPaymentMethod;
				PaymentDetailsDb paymentDetailsDb;

				// make sure we have a payment method to add the payment detail too
				paymentMethodDb = new PaymentMethodDb(dbContext);
				foundPaymentMethod = paymentMethodDb.SelectPaymentMethod(paymentMethodId);
				if(foundPaymentMethod == null) 
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Payment Method not found in database");
					this._HttpResponse.StatusCode = 400;
					return returnValue;
				}

				// if we get this far, we know there is a payment method that matched the passed in paymentMethodId

				// create a model for the data we want to add to the database
				newPaymentDetail = new PaymetDetail()
				{
					PaymentMethodId = paymentMethodId,
					Key = newPaymentDetailKey,
					Value = newPaymentDetailValue
				};
				paymentDetailsDb = new PaymentDetailsDb(dbContext);

				// add the new payment detail to the database
				bool wasSucsefull = paymentDetailsDb.InsertPaymentDetails(newPaymentDetail);
				if(wasSucsefull == true)
					returnValue.ReturnValue = newPaymentDetail;
				// we were unable to add the data to the database
				else
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("Failed to insert payment detail");
					this._HttpResponse.StatusCode = 500;
				}
			}
			// return eaither a new paymentDetail or an error indicating why it failed
			return returnValue;
		}
	}
}
