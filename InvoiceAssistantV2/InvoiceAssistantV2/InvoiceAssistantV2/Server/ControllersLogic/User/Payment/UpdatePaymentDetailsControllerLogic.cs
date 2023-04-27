using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.User;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace InvoiceAssistantV2.Server.ControllersLogic.User.Payment
{
	public class UpdatePaymentDetailsControllerLogic
	{
		private HttpResponse _HttpResponse = null!;

		private int _PaymentDetailsId;
		private string _PaymentDetailsKey;
		private string _PaymentDetailsValue;


		public UpdatePaymentDetailsControllerLogic(HttpResponse httpResponse)
		{
			this._HttpResponse = httpResponse;
		}

		public ControllerLogicReturnValue Process(int PaymentDetailsId, string PaymentDetailKey, string? PaymentDetailsValue)
		{
			ControllerLogicReturnValue returnValue;

			// check the input values are valid
			returnValue = this.CheckInputsAndAddPaymentDetail(PaymentDetailsId, PaymentDetailKey, PaymentDetailsValue);
			if (returnValue.HasErrors)
				return returnValue;

			// try and update the payment method and return the updated values if sucsefull
			returnValue = this.UpdatePaymentDetailsInDatabase();

			return returnValue;
		}



		/// <summary>
		/// Checks the passed in invput values are valid values and if they are, copies them to the classes private veriables
		/// </summary>
		/// <param name="paymenDetailsId"></param>
		/// <param name="paymentDetailKey"></param>
		/// <param name="paymentDetailsValue"></param>
		/// <returns>any errors that might have occured</returns>
		private ControllerLogicReturnValue CheckInputsAndAddPaymentDetail(int paymenDetailsId, string paymentDetailKey, string? paymentDetailsValue)
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			// paymentDetailsId check
			if (paymenDetailsId < 1)
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid paymentMethodId");
			}
			else
				this._PaymentDetailsId = paymenDetailsId;

			//paymentDetailKey check
			this._PaymentDetailsKey = paymentDetailKey.Trim();
			if(this._PaymentDetailsKey.Length == 0 ) 
			{
				returnValue.HasErrors = true;
				returnValue.Errors.Add("Invalid paymentDetailKey");
			}

			//paymentDetailsValue check
			this._PaymentDetailsValue = paymentDetailsValue == null ? string.Empty :  paymentDetailsValue.Trim();

			if(returnValue.HasErrors) 
				this._HttpResponse.StatusCode = 400;

			return returnValue;
		}


		private ControllerLogicReturnValue UpdatePaymentDetailsInDatabase()
		{
			ControllerLogicReturnValue returnValue = new ControllerLogicReturnValue();

			using(InvoiceAssistantDbContext dbContext = new InvoiceAssistantDbContext())
			{
				PaymentDetailsDb dbPaymentDetails = new PaymentDetailsDb(dbContext);
				PaymetDetail? model;

				model = dbPaymentDetails.SelectPaymentDetail(this._PaymentDetailsId);

				// if we could not find the row in the database
				if (model == null)
				{
					returnValue.HasErrors = true;
					returnValue.Errors.Add("paymentMethodId not found");

					this._HttpResponse.StatusCode = 400;
				}
				// we found the model in the database
				else
				{
					// add the new values to the model
					model.Key = this._PaymentDetailsKey;
					model.Value = this._PaymentDetailsValue;

					bool WasSucsefull = dbPaymentDetails.UpdatePaymentDetails(model);
					if (WasSucsefull == false)
					{
						returnValue.HasErrors = true;
						returnValue.Errors.Add("PaymentDetails: upable to update row");
						this._HttpResponse.StatusCode = 400;
					}
					else
					{
						returnValue.ReturnValue = model;
					}
				}
	
				
			}

			return returnValue;
		}
	}
}
